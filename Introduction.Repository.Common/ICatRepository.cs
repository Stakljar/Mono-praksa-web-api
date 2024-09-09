﻿using Introduction.Model;

namespace Introduction.Repository.Common
{
    public interface ICatRepository
    {
        Task<List<Cat>?> GetCatsAsync(string name = "", int? age = null, string color = "",
            DateOnly? ArrivalDateAfter = null, DateOnly? ArrivalDateBefore = null);

        Task<Cat?> GetCatByIdAsync(Guid id);

        Task<bool> InsertCatAsync(CatAddModel catAddModel);

        Task<bool> UpdateCatByIdAsync(Guid id, CatUpdateModel catUpdateModel);

        Task<bool> DeleteCatByIdAsync(Guid id);
    }
}
