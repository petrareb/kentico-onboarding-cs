using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MyOnboardingApp.Contracts.Models;
using MyOnboardingApp.Contracts.Validation;

namespace MyOnboardingApp.Contracts.Services
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public interface IRetrieveItemService
    {
        Task<IEnumerable<TodoListItem>> GetAllItemsAsync();

        Task<IResolvedItem<TodoListItem>> GetItemByIdAsync(Guid id);
    }
}
