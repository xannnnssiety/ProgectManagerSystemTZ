using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class ProjectManagementSystem
{
    private List<User> users = new List<User>();
    private List<Task> tasks = new List<Task>();
    private string usersFilePath = "users.json";
    private string tasksFilePath = "tasks.json";

    public ProjectManagementSystem()
    {
        LoadData();
    }

    private void LoadData()
    {
        if (File.Exists(usersFilePath))
        {
            var userData = File.ReadAllText(usersFilePath);
            users = JsonConvert.DeserializeObject<List<User>>(userData);
        }

        if (File.Exists(tasksFilePath))
        {
            var taskData = File.ReadAllText(tasksFilePath);
            tasks = JsonConvert.DeserializeObject<List<Task>>(taskData);
        }
    }

    private void SaveData()
    {
        var userData = JsonConvert.SerializeObject(users, Formatting.Indented);
        File.WriteAllText(usersFilePath, userData);

        var taskData = JsonConvert.SerializeObject(tasks, Formatting.Indented);
        File.WriteAllText(tasksFilePath, taskData);
    }

    public void RegisterUser(string username, string password, UserRole role)
    {
        if (users.Exists(u => u.Username == username))
        {
            Console.WriteLine("Пользователь с таким именем уже существует.");
            return;
        }

        var user = new User(username, password, role);
        users.Add(user);
        SaveData();
        Console.WriteLine("Пользователь зарегистрирован.");
    }

    public User Authenticate(string username, string password)
    {
        var user = users.Find(u => u.Username == username && u.Password == password);
        if (user != null)
        {
            Console.WriteLine($"Добро пожаловать, {user.Username}!");
        }
        else
        {
            Console.WriteLine("Неверные учетные данные.");
        }
        return user;
    }

    public void CreateTask(int taskId, string title, string description, User manager)
    {
        if (manager.Role != UserRole.Manager)
        {
            Console.WriteLine("Только управляющие могут создавать задачи.");
            return;
        }

        if (tasks.Exists(t => t.Id == taskId))
        {
            Console.WriteLine("Задача с таким ID уже существует.");
            return;
        }

        var task = new Task(taskId, title, description);
        tasks.Add(task);
        SaveData();
        Console.WriteLine("Задача успешно создана.");
    }


    public void AssignTask(int taskId, string employeeUsername, User manager)
    {
        if (manager.Role != UserRole.Manager)
        {
            Console.WriteLine("Только управляющие могут назначать задачи.");
            return;
        }

        var task = tasks.Find(t => t.Id == taskId);
        var employee = users.Find(u => u.Username == employeeUsername && u.Role == UserRole.Employee);

        if (task == null || employee == null)
        {
            Console.WriteLine("Неверный ID задачи или имя сотрудника.");
            return;
        }

        task.AssignedTo = employee;
        SaveData();
        Console.WriteLine($"Задача {taskId} назначена {employeeUsername}.");
    }

    public void ViewTasks(User user)
    {
        var userTasks = tasks.FindAll(t => t.AssignedTo == user);
        if (userTasks.Count == 0)
        {
            Console.WriteLine("На вас не назначены задачи.");
            return;
        }

        foreach (var task in userTasks)
        {
            Console.WriteLine($"ID задачи: {task.Id}, Название: {task.Title}, Статус: {task.Status}");
        }
    }

    public void ViewTaskLogs(int taskId, User manager)
    {
        if (manager.Role != UserRole.Manager)
        {
            Console.WriteLine("Только управляющие могут просматривать логи.");
            return;
        }

        var task = tasks.Find(t => t.Id == taskId);
        if (task == null)
        {
            Console.WriteLine("Задача с таким ID не найдена.");
            return;
        }

        if (task.ChangeLogs.Count == 0)
        {
            Console.WriteLine("Нет логов изменений для этой задачи.");
            return;
        }

        Console.WriteLine($"Логи изменений для задачи [{task.Title}]:");
        foreach (var log in task.ChangeLogs)
        {
            Console.WriteLine(log);
        }
    }


    public void UpdateTaskStatus(int taskId, string newStatus, User user)
    {
        var task = tasks.Find(t => t.Id == taskId);
        if (task == null)
        {
            Console.WriteLine("Задача с таким ID не найдена.");
            return;
        }

        task.Status = newStatus;

        // Добавляем лог изменения состояния задачи
        string log = $"[{DateTime.Now}] - [{user.Username}] изменил статус задачи [{task.Title}] на [{newStatus}]";
        task.AddChangeLog(log);

        SaveData();
        Console.WriteLine($"Статус задачи с ID {taskId} был обновлен на '{newStatus}'.");
    }


    public void ViewAllTasks()
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("Нет задач для отображения.");
            return;
        }

        foreach (var task in tasks)
        {
            string assignedTo = task.AssignedTo != null ? task.AssignedTo.Username : "Не назначено";
            Console.WriteLine($"ID задачи: {task.Id}, Название: {task.Title}, Статус: {task.Status}, Назначено: {assignedTo}");
        }
    }

    public void DeleteTask(int taskId, User manager)
    {
        if (manager.Role != UserRole.Manager)
        {
            Console.WriteLine("Только управляющие могут удалять задачи.");
            return;
        }

        var task = tasks.Find(t => t.Id == taskId);
        if (task == null)
        {
            Console.WriteLine("Задача с таким ID не найдена.");
            return;
        }

        tasks.Remove(task);
        SaveData();
        Console.WriteLine($"Задача с ID {taskId} была удалена.");
    }




}
