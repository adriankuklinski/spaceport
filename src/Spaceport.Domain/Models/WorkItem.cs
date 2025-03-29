using System;
using System.Collections.Generic;

namespace Spaceport.Domain.Models;

public class WorkItem
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public WorkItemType Type { get; set; }
    public WorkItemState State { get; set; }
    public List<WorkItemLink> Links { get; set; } = new();
    public List<Note> Notes { get; set; } = new();
    public List<Event> Events { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public enum WorkItemType
{
    Epic,
    Feature,
    UserStory,
    Task,
    Bug
}

public enum WorkItemState
{
    New,
    Active,
    Resolved,
    Closed
}

public class WorkItemLink
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Url { get; set; }
    public LinkType Type { get; set; }
}

public enum LinkType
{
    Repository,
    PullRequest,
    Branch,
    Commit,
    Documentation,
    Other
}

public class Note
{
    public Guid Id { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class Event
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public EventType Type { get; set; }
    public DateTime OccurredAt { get; set; }
}

public enum EventType
{
    Created,
    Updated,
    Commented,
    StateChanged,
    BranchCreated,
    CommitCreated,
    PullRequestCreated,
    PullRequestMerged
}