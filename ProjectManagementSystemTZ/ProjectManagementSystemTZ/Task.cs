using System;
using System.Collections.Generic;

public class Task
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public User AssignedTo { get; set; }

    // Новое свойство для хранения логов
    public List<string> ChangeLogs { get; set; }

    public Task(int id, string title, string description)
    {
        Id = id;
        Title = title;
        Description = description;
        Status = "To do";
        ChangeLogs = new List<string>();
    }

    // Метод для добавления логов изменений
    public void AddChangeLog(string log)
    {
        ChangeLogs.Add(log);
    }
}

