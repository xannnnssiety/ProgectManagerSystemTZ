using System;

public class Program
{
    public static void Main(string[] args)
    {
        var system = new ProjectManagementSystem();

        while (true)
        {
            Console.WriteLine("1. Регистрация");
            Console.WriteLine("2. Вход");
            Console.WriteLine("3. Выход");
            Console.Write("Выберите опцию: ");
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    Console.Write("Введите имя пользователя: ");
                    var username = Console.ReadLine();
                    Console.Write("Введите пароль: ");
                    var password = Console.ReadLine();
                    Console.Write("Введите роль (Manager/Employee): ");
                    var roleInput = Console.ReadLine();
                    UserRole role = roleInput.ToLower() == "manager" ? UserRole.Manager : UserRole.Employee;
                    system.RegisterUser(username, password, role);
                    break;
                case "2":
                    Console.Write("Введите имя пользователя: ");
                    username = Console.ReadLine();
                    Console.Write("Введите пароль: ");
                    password = Console.ReadLine();
                    var user = system.Authenticate(username, password);
                    if (user != null)
                    {
                        if (user.Role == UserRole.Manager)
                            ManagerMenu(system, user);
                        else
                            EmployeeMenu(system, user);
                    }
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Неверная опция.");
                    break;
            }
        }
    }

    private static void ManagerMenu(ProjectManagementSystem system, User manager)
    {
        while (true)
        {
            Console.WriteLine("1. Создать задачу");
            Console.WriteLine("2. Назначить задачу");
            Console.WriteLine("3. Просмотреть все задачи");
            Console.WriteLine("4. Удалить задачу");
            Console.WriteLine("5. Просмотреть логи задачи");
            Console.WriteLine("6. Выйти");
            Console.Write("Выберите опцию: ");
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    Console.Write("Введите ID задачи: ");
                    int taskId;
                    while (!int.TryParse(Console.ReadLine(), out taskId))
                    {
                        Console.WriteLine("Пожалуйста, введите числовое значение для ID задачи.");
                        Console.Write("Введите ID задачи: ");
                    }

                    Console.Write("Введите название задачи: ");
                    var title = Console.ReadLine();
                    Console.Write("Введите описание задачи: ");
                    var description = Console.ReadLine();
                    system.CreateTask(taskId, title, description, manager);
                    break;
                case "2":
                    Console.Write("Введите ID задачи: ");
                    while (!int.TryParse(Console.ReadLine(), out taskId))
                    {
                        Console.WriteLine("Пожалуйста, введите числовое значение для ID задачи.");
                        Console.Write("Введите ID задачи: ");
                    }

                    Console.Write("Введите имя пользователя сотрудника: ");
                    var employeeUsername = Console.ReadLine();
                    system.AssignTask(taskId, employeeUsername, manager);
                    break;
                case "3":
                    system.ViewAllTasks();
                    break;
                case "4":
                    Console.Write("Введите ID задачи для удаления: ");
                    while (!int.TryParse(Console.ReadLine(), out taskId))
                    {
                        Console.WriteLine("Пожалуйста, введите числовое значение для ID задачи.");
                        Console.Write("Введите ID задачи для удаления: ");
                    }

                    system.DeleteTask(taskId, manager);
                    break;
                case "5":
                    Console.Write("Введите ID задачи для просмотра логов: ");
                    while (!int.TryParse(Console.ReadLine(), out taskId))
                    {
                        Console.WriteLine("Пожалуйста, введите числовое значение для ID задачи.");
                        Console.Write("Введите ID задачи для просмотра логов: ");
                    }

                    system.ViewTaskLogs(taskId, manager);
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Неверная опция.");
                    break;
            }
        }
    }



    private static void EmployeeMenu(ProjectManagementSystem system, User employee)
    {
        while (true)
        {
            Console.WriteLine("1. Просмотреть задачи");
            Console.WriteLine("2. Обновить статус задачи");
            Console.WriteLine("3. Выйти");
            Console.Write("Выберите опцию: ");
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    system.ViewTasks(employee);
                    break;
                case "2":
                    Console.Write("Введите ID задачи: ");
                    int taskId;
                    while (!int.TryParse(Console.ReadLine(), out taskId))
                    {
                        Console.WriteLine("Пожалуйста, введите числовое значение для ID задачи.");
                        Console.Write("Введите ID задачи: ");
                    }

                    Console.Write("Введите новый статус задачи: ");
                    var status = Console.ReadLine();
                    system.UpdateTaskStatus(taskId, status, employee);
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Неверная опция.");
                    break;
            }
        }
    }
}

