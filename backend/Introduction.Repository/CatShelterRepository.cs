using Introduction.Common;
using Introduction.Model;
using Introduction.Repository.Common;
using Npgsql;
using NpgsqlTypes;
using System.Text;

namespace Introduction.Repository
{
    public class CatShelterRepository : ICatShelterRepository
    {
        private readonly string connString = "Host=localhost;Username=postgres;Password=12345;Database=cat_shelter_system";

        public async Task<List<CatShelter>> GetCatSheltersAsync(CatShelterFilter catShelterFilter, CatFilter catFilter, Paging paging, Sorting sorting)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            StringBuilder sql = new("SELECT cs.\"Id\" AS ShelterId, cs.\"Name\" AS ShelterName, cs.\"Location\" AS ShelterLocation, " +
                "cs.\"EstablishedAt\" AS ShelterEstablishedAt, c.\"Id\" AS CatId, c.\"Name\" AS CatName, c.\"Age\" AS CatAge, c.\"Color\" AS CatColor, " +
                "c.\"ArrivalDate\" AS CatArrivalDate, c.\"CatShelterId\" AS CatShelterId " +
                "FROM \"CatShelter\" cs " +
                "LEFT JOIN \"Cat\" c ON cs.\"Id\" = c.\"CatShelterId\" " +
                "WHERE 1=1 ");

            if (!string.IsNullOrEmpty(catShelterFilter.Name))
                sql.Append("AND cs.\"Name\" ILIKE @shelterName ");

            if (!string.IsNullOrEmpty(catShelterFilter.Location))
                sql.Append("AND cs.\"Location\" ILIKE @shelterLocation ");

            if (catShelterFilter.EstablishedAtAfter.HasValue)
                sql.Append("AND cs.\"EstablishedAt\" > @shelterEstablishedAtAfter ");

            if (catShelterFilter.EstablishedAtBefore.HasValue)
                sql.Append("AND cs.\"EstablishedAt\" < @shelterEstablishedAtBefore ");


            if (!string.IsNullOrEmpty(catFilter.Name))
                sql.Append("AND c.\"Name\" ILIKE @catName ");

            if (catFilter.AgeAbove.HasValue)
                sql.Append("AND c.\"Age\" > @catAgeAbove ");

            if (catFilter.AgeBelow.HasValue)
                sql.Append("AND c.\"Age\" < @catAgeBelow ");

            if (!string.IsNullOrEmpty(catFilter.Color))
                sql.Append("AND c.\"Color\" LIKE @catColor ");

            if (catFilter.ArrivalDateAfter.HasValue)
                sql.Append("AND c.\"ArrivalDate\" > @catArrivalDateAfter ");

            if (catFilter.ArrivalDateBefore.HasValue)
                sql.Append("AND c.\"ArrivalDate\" < @catArrivalDateBefore ");

            sql.Append($" ORDER BY cs.\"{sorting.SortBy}\" ");
            sql.Append(sorting.IsAscending ? " ASC " : " DESC ");

            var offset = (paging.PageNumber - 1) * paging.PageSize;
            sql.Append("LIMIT @pageSize OFFSET @offset ");

            using var cmd = new NpgsqlCommand(sql.ToString(), conn);
            cmd.Parameters.AddWithValue("@shelterName", $"%{catShelterFilter.Name}%");
            cmd.Parameters.AddWithValue("@shelterLocation", catShelterFilter.Location ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@shelterEstablishedAtAfter", catShelterFilter.EstablishedAtAfter ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@shelterEstablishedAtBefore", catShelterFilter.EstablishedAtBefore ?? (object)DBNull.Value);

            cmd.Parameters.AddWithValue("@catName", $"%{catFilter.Name}%");
            cmd.Parameters.AddWithValue("@catAgeAbove", catFilter.AgeAbove ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@catAgeBelow", catFilter.AgeBelow ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@catColor", catFilter.Color ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@catArrivalDateAfter", catFilter.ArrivalDateAfter ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@catArrivalDateBefore", catFilter.ArrivalDateBefore ?? (object)DBNull.Value);

            cmd.Parameters.AddWithValue("@pageSize", paging.PageSize);
            cmd.Parameters.AddWithValue("@offset", offset);

            var catShelters = new Dictionary<Guid, CatShelter>();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var shelterId = reader.GetGuid(reader.GetOrdinal("ShelterId"));
                if (!catShelters.ContainsKey(shelterId))
                {
                    var catShelter = new CatShelter
                    {
                        Id = shelterId,
                        Name = reader.GetString(reader.GetOrdinal("ShelterName")),
                        Location = reader.GetString(reader.GetOrdinal("ShelterLocation")),
                        EstablishedAt = reader.GetFieldValue<DateOnly>(reader.GetOrdinal("ShelterEstablishedAt")),
                        Cats = []
                    };
                    catShelters[shelterId] = catShelter;
                }
                var catId = reader["CatId"];
                if (catId != DBNull.Value)
                {
                    Cat cat = new()
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("CatId")),
                        Name = reader.GetString(reader.GetOrdinal("CatName")),
                        Age = reader.GetInt32(reader.GetOrdinal("CatAge")),
                        Color = reader.GetString(reader.GetOrdinal("CatColor")),
                        ArrivalDate = reader.IsDBNull(reader.GetOrdinal("CatArrivalDate")) ?
                            null : reader.GetFieldValue<DateOnly>(reader.GetOrdinal("CatArrivalDate")),
                        CatShelterId = reader.GetGuid(reader.GetOrdinal("CatShelterId")),
                    };
                    catShelters[shelterId].Cats.Add(cat);
                }
            }
            return [.. catShelters.Values];
        }

        public async Task<List<CatShelter>> GetCatSheltersWithoutCatsAsync()
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            string sql = "SELECT * FROM \"CatShelter\" ";

            using var cmd = new NpgsqlCommand(sql.ToString(), conn);

            List<CatShelter> catShelters = [];
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var catShelter = new CatShelter
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Location = reader.GetString(reader.GetOrdinal("Location")),
                    EstablishedAt = reader.GetFieldValue<DateOnly>(reader.GetOrdinal("EstablishedAt")),
                    Cats = null
                };
                catShelters.Add(catShelter);
                    
            }
            return catShelters;
        }

        public async Task<CatShelter?> GetCatShelterByIdAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            string sql =
                "SELECT cs.\"Id\" AS ShelterId, cs.\"Name\" AS ShelterName, cs.\"Location\" AS ShelterLocation, cs.\"EstablishedAt\" AS ShelterEstablishedAt, " +
                "c.\"Id\" AS CatId, c.\"Name\" AS CatName, c.\"Age\" AS CatAge, c.\"Color\" AS CatColor, c.\"ArrivalDate\" AS CatArrivalDate, c.\"CatShelterId\" AS CatShelterId " +
                "FROM \"CatShelter\" cs " +
                "LEFT JOIN \"Cat\" c ON cs.\"Id\" = c.\"CatShelterId\" " +
                "WHERE cs.\"Id\" = @id ";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", NpgsqlDbType.Uuid, id);

            using var reader = await cmd.ExecuteReaderAsync();
            CatShelter? catShelter = null;
            while (await reader.ReadAsync())
            {
                catShelter ??= new CatShelter
                {
                    Id = reader.GetGuid(reader.GetOrdinal("ShelterId")),
                    Name = reader.GetString(reader.GetOrdinal("ShelterName")),
                    Location = reader.GetString(reader.GetOrdinal("ShelterLocation")),
                    EstablishedAt = reader.GetFieldValue<DateOnly>(reader.GetOrdinal("ShelterEstablishedAt")),
                };

                if (!reader.IsDBNull(reader.GetOrdinal("CatId")))
                {
                    Cat cat = new()
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("CatId")),
                        Name = reader.GetString(reader.GetOrdinal("CatName")),
                        Age = reader.GetInt32(reader.GetOrdinal("CatAge")),
                        Color = reader.GetString(reader.GetOrdinal("CatColor")),
                        ArrivalDate = reader.IsDBNull(reader.GetOrdinal("CatArrivalDate")) ?
                            null : reader.GetFieldValue<DateOnly>(reader.GetOrdinal("CatArrivalDate")),
                        CatShelterId = reader.GetGuid(reader.GetOrdinal("CatShelterId"))
                    };
                    catShelter.Cats.Add(cat);
                }
            }
            return catShelter;
        }

        public async Task<bool> InsertCatShelterAsync(CatShelter catShelter)
        {
            using var conn = new NpgsqlConnection(connString);
            Guid catShelterId = Guid.NewGuid();
            conn.Open();
            using var cmd = new NpgsqlCommand(
                "INSERT INTO \"CatShelter\" (\"Id\", \"Name\", \"Location\", \"EstablishedAt\") VALUES (@id, @name, @location, @establishedAt)", conn);
            cmd.Parameters.AddWithValue("id", NpgsqlDbType.Uuid, catShelterId);
            cmd.Parameters.AddWithValue("name", catShelter.Name ?? "");
            cmd.Parameters.AddWithValue("location", catShelter.Location ?? "");
            cmd.Parameters.AddWithValue("establishedAt", catShelter.EstablishedAt ?? DateOnly.FromDateTime(DateTime.Now));

            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            if (rowsAffected <= 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateCatShelterByIdAsync(CatShelter catShelter)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            StringBuilder sql = new("UPDATE \"CatShelter\" SET ");

            if (!string.IsNullOrEmpty(catShelter.Name))
                sql.Append("\"Name\" = @name, ");

            if (!string.IsNullOrEmpty(catShelter.Location))
                sql.Append("\"Location\" = @location, ");

            if (catShelter.EstablishedAt.HasValue)
                sql.Append("\"EstablishedAt\" = @establishedAt, ");


            if (sql[sql.Length - 2] == ',')
            {
                sql.Remove(sql.Length - 2, 1);
            }

            sql.Append(" WHERE \"Id\" = @id");

            using var cmd = new NpgsqlCommand(sql.ToString(), conn);
            cmd.Parameters.AddWithValue("@name", catShelter.Name ?? "");
            cmd.Parameters.AddWithValue("@location", catShelter.Location ?? "");
            cmd.Parameters.AddWithValue("@establishedAt", catShelter.EstablishedAt ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@id", catShelter.Id);

            int rowsAffected = await cmd.ExecuteNonQueryAsync();

            if (rowsAffected == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteCatShelterByIdAsync(Guid id)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM \"CatShelter\" WHERE \"Id\" = @id", conn);

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
