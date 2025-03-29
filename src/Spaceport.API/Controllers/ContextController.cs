using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Spaceport.Domain.Interfaces;
using Spaceport.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spaceport.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContextController : ControllerBase
{
    private readonly IContextService _contextService;
    private readonly ILogger<ContextController> _logger;

    public ContextController(IContextService contextService, ILogger<ContextController> logger)
    {
        _contextService = contextService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkItem>>> GetAllContexts()
    {
        try
        {
            var contexts = await _contextService.GetAllContextsAsync();
            return Ok(contexts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all contexts");
            return StatusCode(500, "An error occurred while retrieving contexts");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WorkItem>> GetContext(Guid id)
    {
        try
        {
            var context = await _contextService.GetContextByIdAsync(id);
            if (context == null)
            {
                return NotFound();
            }
            return Ok(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving context {ContextId}", id);
            return StatusCode(500, "An error occurred while retrieving the context");
        }
    }

    [HttpPost]
    public async Task<ActionResult<WorkItem>> CreateContext(WorkItem workItem)
    {
        try
        {
            var createdContext = await _contextService.CreateContextAsync(workItem);
            return CreatedAtAction(nameof(GetContext), new { id = createdContext.Id }, createdContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating context");
            return StatusCode(500, "An error occurred while creating the context");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContext(Guid id, WorkItem workItem)
    {
        if (id != workItem.Id)
        {
            return BadRequest("Context ID in URL must match the ID in the body");
        }

        try
        {
            await _contextService.UpdateContextAsync(workItem);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating context {ContextId}", id);
            return StatusCode(500, "An error occurred while updating the context");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContext(Guid id)
    {
        try
        {
            await _contextService.DeleteContextAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting context {ContextId}", id);
            return StatusCode(500, "An error occurred while deleting the context");
        }
    }

    [HttpPost("{id}/switch")]
    public async Task<ActionResult<WorkItem>> SwitchContext(Guid id)
    {
        try
        {
            var newContext = await _contextService.SwitchContextAsync(id);
            if (newContext == null)
            {
                return NotFound();
            }
            return Ok(newContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error switching to context {ContextId}", id);
            return StatusCode(500, "An error occurred while switching context");
        }
    }

    [HttpGet("current")]
    public async Task<ActionResult<WorkItem>> GetCurrentContext()
    {
        try
        {
            var currentContext = await _contextService.GetCurrentContextAsync();
            if (currentContext == null)
            {
                return NotFound("No active context found");
            }
            return Ok(currentContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving current context");
            return StatusCode(500, "An error occurred while retrieving the current context");
        }
    }
}