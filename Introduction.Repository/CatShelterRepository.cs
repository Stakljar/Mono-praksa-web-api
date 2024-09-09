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

        public async Task<List<CatShelter>?> GetCatSheltersAsync(string name = "", string location = "",
            DateOnly? createdAtDateAfter = null, DateOnly? createdAtDateBefore = null)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                StringBuilder sql = new();
                sql.Append("SELECT cs.\"Id\" AS ShelterId, cs.\"Name\" AS ShelterName, cs.\"Location\" AS ShelterLocation, cs.\"CreatedAt\" AS ShelterCreatedAt, " +
                "c.\"Id\" AS CatId, c.\"Name\" AS CatName, c.\"Age\" AS CatAge, c.\"Color\" AS CatColor, c.\"ArrivalDate\" AS CatArrivalDate " +
                "FROM \"CatShelter\" cs " +
                "LEFT JOIN \"Cat\" c ON cs.\"Id\" = c.\"CatShelterId\" " +
                "WHERE 1=1 ");

                var parameters = new List<NpgsqlParameter>();
                var filterDict = new Dictionary<string, object?>
                {
                    { "name", !string.IsNullOrEmpty(name) ? name : null },
                    { "location", !string.IsNullOrEmpty(location) ? location : null },
                    { "createdAtDateAfter", createdAtDateAfter.HasValue ? createdAtDateAfter.Value : null },
                    { "createdAtDateBefore", createdAtDateBefore.HasValue ? createdAtDateBefore.Value : null }
                };

                foreach (var (key, value) in filterDict)
                {
                    if (value != null)
                    {
                        sql.Append(key switch
                        {
                            "name" => " AND cs.\"Name\" = @name",
                            "location" => " AND cs.\"Location\" = @location",
                            "createdAtDateAfter" => " AND \"CreatedAt\" > @createdAtDateAfter",
                            "createdAtDateBefore" => " AND \"CreatedAt\" < @createdAtDateBefore",
                            _ => throw new ArgumentException("Invalid filter key")
                        });

                        parameters.Add(new NpgsqlParameter($"@{key}", value));
                    }
                }

                using var cmd = new NpgsqlCommand(sql.ToString(), conn);
                cmd.Parameters.AddRange(parameters.ToArray());

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
                            CreatedAt = reader.GetFieldValue<DateOnly>(reader.GetOrdinal("ShelterCreatedAt")),
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
                        };
                        catShelters[shelterId].Cats.Add(cat);
                    }
                }
                return [.. catShelters.Values];
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<CatShelter?> GetCatShelterByIdAsync(Guid id)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                string sql =
                    "SELECT cs.\"Id\" AS ShelterId, cs.\"Name\" AS ShelterName, cs.\"Location\" AS ShelterLocation, cs.\"CreatedAt\" AS ShelterCreatedAt, " +
                    "c.\"Id\" AS CatId, c.\"Name\" AS CatName, c.\"Age\" AS CatAge, c.\"Color\" AS CatColor, c.\"ArrivalDate\" AS CatArrivalDate " +
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
                        CreatedAt = reader.GetFieldValue<DateOnly>(reader.GetOrdinal("ShelterCreatedAt")),
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
                        };
                        catShelter.Cats.Add(cat);
                    }
                }
                return catShelter;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> InsertCatShelterAsync(CatShelterAddModel catShelterAddModel)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                Guid catShelterId = Guid.NewGuid();
                conn.Open();
                using var cmd = new NpgsqlCommand(
                    "INSERT INTO \"CatShelter\" (\"Id\", \"Name\", \"Location\", \"CreatedAt\") VALUES (@id, @name, @location, @createdAt)", conn);
                cmd.Parameters.AddWithValue("id", NpgsqlDbType.Uuid, catShelterId);
                cmd.Parameters.AddWithValue("name", catShelterAddModel.Name);
                cmd.Parameters.AddWithValue("location", catShelterAddModel.Location);
                cmd.Parameters.AddWithValue("createdAt", catShelterAddModel.CreatedAt ?? new DateOnly());

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

        public async Task<bool> UpdateCatShelterByIdAsync(Guid id, CatShelterUpdateModel catShelterUpdateModel)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                StringBuilder sql = new();
                sql.Append("UPDATE \"CatShelter\" SET ");
                var parameters = new List<NpgsqlParameter>();
                List<string> setClauses = [];

                var updateModelProperties = typeof(CatShelterUpdateModel).GetProperties();

                foreach (var property in updateModelProperties)
                {
                    var newValue = property.GetValue(catShelterUpdateModel);

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

        public async Task<bool> DeleteCatShelterByIdAsync(Guid id)
        {
            try
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
            catch (Exception)
            {
                return false;
            }
        }
    }
}
