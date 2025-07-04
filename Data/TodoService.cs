using System.Collections.Concurrent;

namespace TodoApp.Data;

public class TodoItem
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public bool IsComplete { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}

public class TodoService
{
    private readonly ConcurrentDictionary<int, TodoItem> _todos = new();
    private int _nextId = 1;

    public Task<List<TodoItem>> GetTodosAsync() => 
        Task.FromResult(_todos.Values.OrderBy(t => t.CreatedDate).ToList());

    public Task AddTodoAsync(string title)
    {
        var todo = new TodoItem
        {
            Id = _nextId++,
            Title = title
        };
        _todos[todo.Id] = todo;
        return Task.CompletedTask;
    }

    public Task UpdateTodoAsync(TodoItem updatedTodo)
    {
        if (_todos.TryGetValue(updatedTodo.Id, out var existing))
        {
            existing.Title = updatedTodo.Title;
            existing.IsComplete = updatedTodo.IsComplete;
        }
        return Task.CompletedTask;
    }

    public Task DeleteTodoAsync(int id)
    {
        _todos.TryRemove(id, out _);
        return Task.CompletedTask;
    }
}