using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreTodo.Services
{
    public interface ITodoItemService
    {
        Task<bool> AddItemAsync(TodoItem newItem, IdentityUser user);
        Task<TodoItem[]> GetIncompleteItemsAsync(IdentityUser user);
            

        Task<bool> MarkDoneAsync(Guid id, IdentityUser user);
        Task<bool> DeleteAsync(Guid id, IdentityUser user);
    
    }
    
}