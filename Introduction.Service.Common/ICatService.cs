﻿using Introduction.Model;

namespace Introduction.Service.Common
{
    public interface ICatService
    {
        Task<List<Cat>?> GetCatsAsync(string name = "", int? age = null, string color = "",
            DateOnly? arrivalDateAfter = null, DateOnly? arrivalDateBefore = null);

        Task<Cat?> GetCatAsync(Guid id);

        Task<bool> PostCatAsync(CatAddModel catAddModel);

        Task<bool> PutCatAsync(Guid id, CatUpdateModel catUpdateModel);

        Task<bool> DeleteCatAsync(Guid id);
    }
}
