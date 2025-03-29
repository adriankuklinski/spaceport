using Spaceport.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spaceport.Domain.Interfaces;

public interface IContextService
{
    Task<IEnumerable<WorkItem>> GetAllContextsAsync();
    Task<WorkItem?> GetContextByIdAsync(Guid id);
    Task<WorkItem> CreateContextAsync(WorkItem workItem);
    Task UpdateContextAsync(WorkItem workItem);
    Task DeleteContextAsync(Guid id);
    Task<WorkItem?> SwitchContextAsync(Guid id);
    Task<WorkItem?> GetCurrentContextAsync();
}