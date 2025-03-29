using Microsoft.Extensions.Logging;
using Spaceport.Domain.Interfaces;
using Spaceport.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spaceport.Infrastructure.Services;

public class MockContextService : IContextService
{
    private readonly List<WorkItem> _contexts = new();
    private Guid? _currentContextId;
    private readonly ILogger<MockContextService> _logger;

    public MockContextService(ILogger<MockContextService> logger)
    {
        _logger = logger;
        
        // Add some mock data
        _contexts.Add(new WorkItem
        {
            Id = Guid.NewGuid(),
            Title = "Implement Spaceport API",
            Description = "Create the core API for the Cosmos system",
            Type = WorkItemType.UserStory,
            State = WorkItemState.Active,
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            Links = new List<WorkItemLink>
            {
                new WorkItemLink
                {
                    Id = Guid.NewGuid(),
                    Title = "Spaceport Repository",
                    Url = "https://github.com/user/spaceport",
                    Type = LinkType.Repository
                }
            }
        });
        
        _contexts.Add(new WorkItem
        {
            Id = Guid.NewGuid(),
            Title = "Design Database Schema",
            Description = "Create the database schema for the Cosmos system",
            Type = WorkItemType.Task,
            State = WorkItemState.New,
            CreatedAt = DateTime.UtcNow.AddDays(-2)
        });
    }

    public Task<IEnumerable<WorkItem>> GetAllContextsAsync()
    {
        _logger.LogInformation("Getting all contexts");
        return Task.FromResult<IEnumerable<WorkItem>>(_contexts);
    }

    public Task<WorkItem?> GetContextByIdAsync(Guid id)
    {
        _logger.LogInformation("Getting context {ContextId}", id);
        var context = _contexts.FirstOrDefault(c => c.Id == id);
        return Task.FromResult(context);
    }

    public Task<WorkItem> CreateContextAsync(WorkItem workItem)
    {
        _logger.LogInformation("Creating new context");
        
        workItem.Id = Guid.NewGuid();
        workItem.CreatedAt = DateTime.UtcNow;
        
        _contexts.Add(workItem);
        return Task.FromResult(workItem);
    }

    public Task UpdateContextAsync(WorkItem workItem)
    {
        _logger.LogInformation("Updating context {ContextId}", workItem.Id);
        
        var existingContext = _contexts.FirstOrDefault(c => c.Id == workItem.Id);
        if (existingContext == null)
        {
            throw new KeyNotFoundException($"Context with ID {workItem.Id} not found");
        }
        
        var index = _contexts.IndexOf(existingContext);
        workItem.UpdatedAt = DateTime.UtcNow;
        _contexts[index] = workItem;
        
        return Task.CompletedTask;
    }

    public Task DeleteContextAsync(Guid id)
    {
        _logger.LogInformation("Deleting context {ContextId}", id);
        
        var existingContext = _contexts.FirstOrDefault(c => c.Id == id);
        if (existingContext == null)
        {
            throw new KeyNotFoundException($"Context with ID {id} not found");
        }
        
        _contexts.Remove(existingContext);
        
        if (_currentContextId == id)
        {
            _currentContextId = null;
        }
        
        return Task.CompletedTask;
    }

    public Task<WorkItem?> SwitchContextAsync(Guid id)
    {
        _logger.LogInformation("Switching to context {ContextId}", id);
        
        var context = _contexts.FirstOrDefault(c => c.Id == id);
        if (context == null)
        {
            return Task.FromResult<WorkItem?>(null);
        }
        
        _currentContextId = id;
        return Task.FromResult<WorkItem?>(context);
    }

    public Task<WorkItem?> GetCurrentContextAsync()
    {
        _logger.LogInformation("Getting current context");
        
        if (_currentContextId == null)
        {
            return Task.FromResult<WorkItem?>(null);
        }
        
        var context = _contexts.FirstOrDefault(c => c.Id == _currentContextId);
        return Task.FromResult(context);
    }
}