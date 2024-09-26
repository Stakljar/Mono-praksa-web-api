using Introduction.Common;
using Introduction.Model;
using Introduction.Repository.Common;
using Npgsql;
using NpgsqlTypes;
using System.Text;

namespace Introduction.Repository
{
    public class CatRepository : ICatRepository
    {
        private readonly string connString = "Host=localhost;Username=postgres;Password=12345;Database=cat_shelter_system";

        public async Task<List<Cat>> GetCatsAsync(CatFilter catFilter, Paging paging, Sorting sorting)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            StringBuilder sql = new("SELECT c.\"Id\" AS CatId, c.\"Name\" AS CatName, c.\"Age\" AS CatAge, c.\"Color\" AS CatColor, " +
                "c.\"ArrivalDate\" AS CatArrivalDate, c.\"CatShelterId\" AS CatShelterId, cs.\"Id\" AS ShelterId, cs.\"Name\" AS ShelterName, " +
                "cs.\"Location\" AS ShelterLocation, cs.\"EstablishedAt\" AS ShelterEstablishedAt " +
                "FROM \"Cat\" c " +
                "LEFT JOIN \"CatShelter\" cs ON cs.\"Id\" = c.\"CatShelterId\" " +
                "WHERE 1=1 ");

            if (!string.IsNullOrEmpty(catFilter.Name))
                sql.Append("AND c.\"Name\" ILIKE @name ");

            if (catFilter.AgeAbove.HasValue)
                sql.Append("AND c.\"Age\" > @ageAbove ");

            if (catFilter.AgeBelow.HasValue)
                sql.Append("AND c.\"Age\" < @ageBelow ");

            if (!string.IsNullOrEmpty(catFilter.Color))
                sql.Append("AND c.\"Color\" LIKE @color ");

            if (catFilter.ArrivalDateAfter.HasValue)
                sql.Append("AND c.\"ArrivalDate\" > @arrivalDateAfter ");

            if (catFilter.ArrivalDateBefore.HasValue)
                sql.Append("AND c.\"ArrivalDate\" < @arrivalDateBefore ");

            sql.Append($"ORDER BY c.\"{sorting.SortBy}\" ");
            sql.Append(sorting.IsAscending ? " ASC " : " DESC ");

            var offset = (paging.PageNumber - 1) * paging.PageSize;
            sql.Append("LIMIT @pageSize OFFSET @offset ");

            using var cmd = new NpgsqlCommand(sql.ToString(), conn);
            cmd.Parameters.AddWithValue("@name", $"{catFilter.Name}%");
            cmd.Parameters.AddWithValue("@ageAbove", catFilter.AgeAbove ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ageBelow", catFilter.AgeBelow ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@color", catFilter.Color);
            cmd.Parameters.AddWithValue("@arrivalDateAfter", catFilter.ArrivalDateAfter ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@arrivalDateBefore", catFilter.ArrivalDateBefore ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@pageSize", paging.PageSize);
            cmd.Parameters.AddWithValue("@offset", offset);

            List<Cat> cats = [];
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                CatShelter? catShelter = null;
                if (!reader.IsDBNull(reader.GetOrdinal("ShelterId")))
                {
                    catShelter = new()
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("ShelterId")),
                        Name = reader.GetString(reader.GetOrdinal("ShelterName")),
                        Location = reader.GetString(reader.GetOrdinal("ShelterLocation")),
                        EstablishedAt = reader.GetFieldValue<DateOnly>(reader.GetOrdinal("ShelterEstablishedAt")),
                        Cats = null
                    };
                }
                Cat cat = new()
                {
                    Id = reader.GetGuid(reader.GetOrdinal("CatId")),
                    Name = reader.GetString(reader.GetOrdinal("CatName")),
                    Age = reader.GetInt32(reader.GetOrdinal("CatAge")),
                    Color = reader.GetString(reader.GetOrdinal("CatColor")),
                    ArrivalDate = reader.IsDBNull(reader.GetOrdinal("CatArrivalDate")) ?
                            null : reader.GetFieldValue<DateOnly>(reader.GetOrdinal("CatArrivalDate")),
                    CatShelterId = reader.IsDBNull(reader.GetOrdinal("CatShelterId")) ? null : reader.GetGuid(reader.GetOrdinal("CatShelterId")),
                    CatShelter = catShelter
                };
                cats.Add(cat);
            }
            return cats;
        }

        public async Task<Cat?> GetCatByIdAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            string sql =
                "SELECT c.\"Id\" AS CatId, c.\"Name\" AS CatName, c.\"Age\" AS CatAge, c.\"Color\" AS CatColor, " +
                "c.\"ArrivalDate\" AS CatArrivalDate, c.\"CatShelterId\" AS CatShelterId, cs.\"Id\" AS ShelterId, cs.\"Name\" AS ShelterName, " +
                "cs.\"Location\" AS ShelterLocation, cs.\"EstablishedAt\" AS ShelterEstablishedAt " +
                "FROM \"Cat\" c " +
                "LEFT JOIN \"CatShelter\" cs ON cs.\"Id\" = c.\"CatShelterId\" " +
                "WHERE c.\"Id\" = @id ";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", NpgsqlDbType.Uuid, id);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                CatShelter catShelter = null;
                if (!reader.IsDBNull(reader.GetOrdinal("ShelterId")))
                {
                    catShelter = new()
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("ShelterId")),
                        Name = reader.GetString(reader.GetOrdinal("ShelterName")),
                        Location = reader.GetString(reader.GetOrdinal("ShelterLocation")),
                        EstablishedAt = reader.GetFieldValue<DateOnly>(reader.GetOrdinal("ShelterEstablishedAt")),
                        Cats = null
                    };
                }
                Cat cat = new()
                {
                    Id = reader.GetGuid(reader.GetOrdinal("CatId")),
                    Name = reader.GetString(reader.GetOrdinal("CatName")),
                    Age = reader.GetInt32(reader.GetOrdinal("CatAge")),
                    Color = reader.GetString(reader.GetOrdinal("CatColor")),
                    ArrivalDate = reader.IsDBNull(reader.GetOrdinal("CatArrivalDate")) ?
                            null : reader.GetFieldValue<DateOnly>(reader.GetOrdinal("CatArrivalDate")),
                    CatShelterId = reader.IsDBNull(reader.GetOrdinal("CatShelterId")) ? null : reader.GetGuid(reader.GetOrdinal("CatShelterId")),
                    CatShelter = catShelter
                };
                return cat;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> InsertCatAsync(Cat cat)
        {
            using var conn = new NpgsqlConnection(connString);
            Guid catShelterId = Guid.NewGuid();
            conn.Open();

            string sql = "INSERT INTO \"Cat\" (\"Id\", \"Name\", \"Age\", \"Color\", \"ArrivalDate\", \"CatShelterId\") " +
                "VALUES (@id, @name, @age, @color, @arrivalDate, @shelterId)";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", NpgsqlDbType.Uuid, catShelterId);
            cmd.Parameters.AddWithValue("name", cat.Name ?? "");
            cmd.Parameters.AddWithValue("age", cat.Age);
            cmd.Parameters.AddWithValue("color", cat.Color ?? "");
            cmd.Parameters.AddWithValue("arrivalDate", cat.ArrivalDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("shelterId", cat.CatShelterId ?? (object)DBNull.Value);

            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            if (rowsAffected <= 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateCatByIdAsync(Cat cat)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            StringBuilder sql = new("UPDATE \"Cat\" SET ");

            if (!string.IsNullOrEmpty(cat.Name))
                sql.Append("\"Name\" = @name, ");

            if (cat.Age.HasValue)
                sql.Append("\"Age\" = @age, ");

            if (!string.IsNullOrEmpty(cat.Color))
                sql.Append("\"Color\" = @color, ");

            if (cat.ArrivalDate.HasValue)
                sql.Append("\"ArrivalDate\" = @arrivalDate, ");

            if (cat.CatShelterId.HasValue)
                sql.Append("\"CatShelterId\" = @catShelterId, ");

            if (sql[sql.Length - 2] == ',')
            {
                sql.Remove(sql.Length - 2, 1);
            }
                
            sql.Append("WHERE \"Id\" = @id ");
            using var cmd = new NpgsqlCommand(sql.ToString(), conn);

            cmd.Parameters.AddWithValue("@name", cat.Name ?? "");
            cmd.Parameters.AddWithValue("@age", cat.Age ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@color", cat.Color ?? "");
            cmd.Parameters.AddWithValue("@arrivalDate", NpgsqlDbType.Date, cat.ArrivalDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@catShelterId", cat.CatShelterId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@id", cat.Id);

            int rowsAffected = await cmd.ExecuteNonQueryAsync();

            if (rowsAffected == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteCatByIdAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM \"Cat\" WHERE \"Id\" = @id ", conn);

            cmd.Parameters.AddWithValue("id", NpgsqlDbType.Uuid, id);

            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            if (rowsAffected <= 0)
            {
                return false;
            }
            return true;
        }
    }
}
