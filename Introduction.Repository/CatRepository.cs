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

        public async Task<List<Cat>?> GetCatsAsync(CatFilter catFilter, Paging paging, Sorting sorting)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                StringBuilder sql = new("SELECT * " +
                    "FROM \"Cat\" " +
                    "WHERE 1=1 ");

                if (!string.IsNullOrEmpty(catFilter.Name))
                    sql.Append("AND \"Name\" ILIKE @name ");

                if (catFilter.AgeAbove.HasValue)
                    sql.Append("AND \"Age\" > @ageAbove ");

                if (catFilter.AgeBelow.HasValue)
                    sql.Append("AND \"Age\" < @ageBelow ");

                if (!string.IsNullOrEmpty(catFilter.Color))
                    sql.Append("AND \"Color\" LIKE @color ");

                if (catFilter.ArrivalDateAfter.HasValue)
                    sql.Append("AND \"ArrivalDate\" > @arrivalDateAfter ");

                if (catFilter.ArrivalDateBefore.HasValue)
                    sql.Append("AND \"ArrivalDate\" < @arrivalDateBefore ");

                sql.Append($" ORDER BY \"{sorting.SortBy}\" ");
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
                    Cat cat = new()
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("Id")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        Age = reader.GetInt32(reader.GetOrdinal("Age")),
                        Color = reader.GetString(reader.GetOrdinal("Color")),
                        ArrivalDate = reader.IsDBNull(reader.GetOrdinal("ArrivalDate")) ?
                               null : reader.GetFieldValue<DateOnly>(reader.GetOrdinal("ArrivalDate")),
                        CatShelterId = reader.IsDBNull(reader.GetOrdinal("CatShelterId")) ? null : reader.GetGuid(reader.GetOrdinal("CatShelterId"))
                    };
                    cats.Add(cat);
                }
                return cats;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Cat?> GetCatByIdAsync(Guid id)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                string sql =
                    "SELECT * " +
                    "FROM \"Cat\" " +
                    "WHERE \"Id\" = @id ";

                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("id", NpgsqlDbType.Uuid, id);

                using var reader = await cmd.ExecuteReaderAsync();
                CatShelter catShelter = new();
                if (await reader.ReadAsync())
                {
                    Cat cat = new()
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("Id")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        Age = reader.GetInt32(reader.GetOrdinal("Age")),
                        Color = reader.GetString(reader.GetOrdinal("Color")),
                        ArrivalDate = reader.IsDBNull(reader.GetOrdinal("ArrivalDate")) ?
                                                   null : reader.GetFieldValue<DateOnly>(reader.GetOrdinal("ArrivalDate")),
                        CatShelterId = reader.IsDBNull(reader.GetOrdinal("CatShelterId")) ? null : reader.GetGuid(reader.GetOrdinal("CatShelterId"))
                    };
                    catShelter.Cats.Add(cat);
                    return cat;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> InsertCatAsync(CatAddModel catAddModel)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                Guid catShelterId = Guid.NewGuid();
                conn.Open();
                using var cmd = new NpgsqlCommand(
                    "INSERT INTO \"Cat\" (\"Id\", \"Name\", \"Age\", \"Color\", \"ArrivalDate\") VALUES (@id, @name, @age, @color, @arrivalDate)", conn);
                cmd.Parameters.AddWithValue("id", NpgsqlDbType.Uuid, catShelterId);
                cmd.Parameters.AddWithValue("name", catAddModel.Name);
                cmd.Parameters.AddWithValue("age", catAddModel.Age);
                cmd.Parameters.AddWithValue("color", catAddModel.Color);
                cmd.Parameters.AddWithValue("arrivalDate", catAddModel.ArrivalDate ?? (object)DBNull.Value);

                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                if (rowsAffected <= 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateCatByIdAsync(Guid id, CatUpdateModel catUpdateModel)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                StringBuilder sql = new();
                sql.Append("UPDATE \"Cat\" SET ");
                var parameters = new List<NpgsqlParameter>();
                List<string> setClauses = new List<string>();

                var updateModelProperties = typeof(CatUpdateModel).GetProperties();

                foreach (var property in updateModelProperties)
                {
                    var newValue = property.GetValue(catUpdateModel);

                    if (newValue != null)
                    {
                        var parameterName = $"@{property.Name}";
                        setClauses.Add($"\"{property.Name}\" = {parameterName}");
                        parameters.Add(new NpgsqlParameter(parameterName, newValue));
                    }
                }

                if (setClauses.Count > 0)
                {
                    sql.Append(string.Join(", ", setClauses) + " WHERE \"Id\" = @id");
                    parameters.Add(new NpgsqlParameter("@id", id));

                    using var cmd = new NpgsqlCommand(sql.ToString(), conn);
                    cmd.Parameters.AddRange(parameters.ToArray());

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected == 0)
                    {
                        return false;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteCatByIdAsync(Guid id)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                using var cmd = new NpgsqlCommand("DELETE FROM \"Cat\" WHERE \"Id\" = @id", conn);

                cmd.Parameters.AddWithValue("id", NpgsqlDbType.Uuid, id);

                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                if (rowsAffected <= 0)
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
