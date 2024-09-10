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

        public async Task<List<Cat>?> GetCatsAsync(string name = "", int? age = null, string color = "",
            DateOnly? arrivalDateAfter = null, DateOnly? arrivalDateBefore = null)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                StringBuilder sql = new();
                sql.Append("SELECT c.\"Id\" AS CatId, c.\"Name\" AS CatName, c.\"Age\" AS CatAge, c.\"Color\" AS CatColor, c.\"ArrivalDate\" AS CatArrivalDate, " +
                    "cs.\"Id\" AS ShelterId, cs.\"Name\" AS ShelterName, cs.\"Location\" AS ShelterLocation, cs.\"CreatedAt\" AS ShelterCreatedAt " +
                    "FROM \"Cat\" c " +
                    "LEFT JOIN \"CatShelter\" cs ON cs.\"Id\" = c.\"CatShelterId\" " +
                    "WHERE 1=1 ");

                var parameters = new List<NpgsqlParameter>();
                var filterDict = new Dictionary<string, object?>
                {
                    { "name", !string.IsNullOrEmpty(name) ? name : null },
                    { "age", age.HasValue ? age : null },
                    { "color", !string.IsNullOrEmpty(color) ? color : null },
                    { "ArrivalDateAfter", arrivalDateAfter.HasValue ? arrivalDateAfter.Value : null },
                    { "ArrivalDateBefore", arrivalDateBefore.HasValue ? arrivalDateBefore.Value : null }
                };

                foreach (var (key, value) in filterDict)
                {
                    if (value != null)
                    {
                        sql.Append(key switch
                        {
                            "name" => " AND c.\"Name\" = @name",
                            "age" => " AND c.\"Age\" = @age",
                            "color" => " AND c.\"Color\" = @color",
                            "ArrivalDateAfter" => " AND c.\"ArrivalDate\" > @ArrivalDateAfter",
                            "ArrivalDateBefore" => " AND c.\"ArrivalDate\" < @ArrivalDateBefore",
                            _ => throw new ArgumentException("Invalid filter key")
                        });

                        parameters.Add(new NpgsqlParameter($"@{key}", value));
                    }
                }

                using var cmd = new NpgsqlCommand(sql.ToString(), conn);
                cmd.Parameters.AddRange(parameters.ToArray());

                List<Cat> cats = [];
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    Cat cat = new()
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("CatId")),
                        Name = reader.GetString(reader.GetOrdinal("CatName")),
                        Age = reader.GetInt32(reader.GetOrdinal("CatAge")),
                        Color = reader.GetString(reader.GetOrdinal("CatColor")),
                        ArrivalDate = reader.IsDBNull(reader.GetOrdinal("CatArrivalDate")) ?
                               null : reader.GetFieldValue<DateOnly>(reader.GetOrdinal("CatArrivalDate")),
                        CatShelterid = reader.IsDBNull(reader.GetOrdinal("ShelterId")) ? null : reader.GetGuid(reader.GetOrdinal("ShelterId"))
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
                    "SELECT c.\"Id\" AS CatId, c.\"Name\" AS CatName, c.\"Age\" AS CatAge, c.\"Color\" AS CatColor, c.\"ArrivalDate\" AS CatArrivalDate, " +
                    "cs.\"Id\" AS ShelterId, cs.\"Name\" AS ShelterName, cs.\"Location\" AS ShelterLocation, cs.\"CreatedAt\" AS ShelterCreatedAt " +
                    "FROM \"Cat\" c " +
                    "LEFT JOIN \"CatShelter\" cs ON cs.\"Id\" = c.\"CatShelterId\" " +
                    "WHERE c.\"Id\" = @id ";

                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("id", NpgsqlDbType.Uuid, id);

                using var reader = await cmd.ExecuteReaderAsync();
                CatShelter catShelter = new();
                if (await reader.ReadAsync())
                {
                    Cat cat = new()
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("CatId")),
                        Name = reader.GetString(reader.GetOrdinal("CatName")),
                        Age = reader.GetInt32(reader.GetOrdinal("CatAge")),
                        Color = reader.GetString(reader.GetOrdinal("CatColor")),
                        ArrivalDate = reader.IsDBNull(reader.GetOrdinal("CatArrivalDate")) ?
                                                   null : reader.GetFieldValue<DateOnly>(reader.GetOrdinal("CatArrivalDate")),
                        CatShelterid = reader.IsDBNull(reader.GetOrdinal("ShelterId")) ? null : reader.GetGuid(reader.GetOrdinal("ShelterId"))
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
