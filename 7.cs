using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Data;

namespace TaskApp
{
    class Task
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Term { get; set; }

    }
    class Program
    {
        static List<Task> tasks = new List<Task>();
        static string datebook = "datebook.json";
        static void Main(string[] args)
        {
            LoadTasksFromDatebook();

            while (true)
            {
                Console.WriteLine("--------------------------------");
                Console.WriteLine("Выберите действие: ");
                Console.WriteLine("1. Просмотреть все задачи");
                Console.WriteLine("2. Просмотреть задачи на сегодня");
                Console.WriteLine("3. Просмотреть задачи на завтра");
                Console.WriteLine("4. Просмотреть задачи на неделю");
                Console.WriteLine("5. Просмотреть предстоящие задачи");
                Console.WriteLine("6. Просмотреть прошедшие задачи");
                Console.WriteLine("7. Добавить новую задачу");
                Console.WriteLine("8. Удалить задачу");
                Console.WriteLine("9. Редактировать задачу");
                Console.WriteLine("0. Выйти из программы");
                Console.WriteLine("--------------------------------");

                Console.WriteLine("Ваш выбор: ");
                int number = int.Parse(Console.ReadLine());

                switch (number)
                {
                    case 1:
                        ViewAllTasks();
                        break;
                    case 2:
                        ViewTodosByDate(DateTime.Today);
                        break;
                    case 3:
                        ViewTodosByDate(DateTime.Today.AddDays(1));
                        break;
                    case 4:
                        ViewTodosByDate(DateTime.Today.AddDays(7));
                        break;
                    case 5:
                        ViewUpcomingTasks();
                        break;
                    case 6:
                        ViewCompletedTasks();
                        break;
                    case 7:
                        AddTask();
                        break;
                    case 8:
                        RemoveTask();
                        break;
                    case 9:
                        EditTask();
                        break;
                    case 0:
                        SaveTasksToDatebook();
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Неправильный ввод. Попробуйте еще раз");
                        break;
                }
            }

            static void LoadTasksFromDatebook()
            {
                if (File.Exists(datebook))
                {
                    string json = File.ReadAllText(datebook);
                    tasks = JsonConvert.DeserializeObject<List<Task>>(json);
                }
            }

            static int ParseIntInput(string input)
            {
                int value;
                while (!int.TryParse(input, out value))
                {
                    Console.WriteLine("Неверный формат ввода. Попробуйте еще раз.");
                    Console.Write("Ваш выбор: ");
                    input = Console.ReadLine();
                }
                return value;
            }

            static void ViewAllTasks()
            {
                Console.WriteLine("Список всех задач: ");

                bool found = false;
                foreach (Task task in tasks)
                {
                    Console.WriteLine($"Название задачи: {task.Title}");
                    Console.WriteLine($"Описание задачи: {task.Description}");
                    Console.WriteLine($"Дата выполнения задачи: {task.Term.ToShortDateString()}");
                    Console.WriteLine("--------------------------------");
                    found = true;
                }

                if (found == false)
                {
                    Console.WriteLine("Задачи отсутствуют");
                }
            }

            static void ViewTodosByDate(DateTime date)
            {
                Console.WriteLine($"Список всех задач до {date.ToShortDateString()}: ");

                bool found = false;
                foreach (Task task in tasks)
                {
                    if (task.Term.Date <= date.Date)
                    {
                        Console.WriteLine($"Название задачи: {task.Title}");
                        Console.WriteLine($"Описание задачи: {task.Description}");
                        Console.WriteLine($"Дата выполнения задачи: {task.Term.ToShortDateString()}");
                        Console.WriteLine("--------------------------------");
                        found = true;
                    }
                }

                if (found == false)
                {
                    Console.WriteLine("Задачи отсутствуют");
                }
            }


            static void ViewUpcomingTasks()
            {
                Console.WriteLine("Список предстоящих задач: ");
                bool found = false;

                foreach (Task task in tasks)
                {
                    if (task.Term.Date >= DateTime.Today)
                    {
                        Console.WriteLine($"Название задачи: {task.Title}");
                        Console.WriteLine($"Описание задачи: {task.Description}");
                        Console.WriteLine($"Дата выполнения задачи: {task.Term.ToShortDateString()}");
                        Console.WriteLine("--------------------------------");
                        found = true;
                    }
                }

                if (found == false)
                {
                    Console.WriteLine("Задачи отсутствуют");
                }
            }

            static void ViewCompletedTasks()
            {
                Console.WriteLine("Список прошедших задач: ");
                bool found = false;

                foreach (Task task in tasks)
                {
                    if (task.Term.Date < DateTime.Today)
                    {
                        Console.WriteLine($"Название задачи: {task.Title}");
                        Console.WriteLine($"Описание задачи: {task.Description}");
                        Console.WriteLine($"Дата выполнения задачи: {task.Term.ToShortDateString()}");
                        Console.WriteLine("--------------------------------");
                        found = true;
                    }
                }

                if (found == false)
                {
                    Console.WriteLine("Задачи отсутствуют");
                }
            }


            static void AddTask()
            {
                Console.WriteLine("Введите название задачи: ");
                string title = Console.ReadLine();

                Console.WriteLine("Введите описание задачи: ");
                string description = Console.ReadLine();

                Console.WriteLine("Введите дату выполнения задачи (в формате ДД.ММ.ГГГГ): ");
                DateTime term = ParseTerm(Console.ReadLine());

                tasks.Add(new Task { Title = title, Description = description, Term = term });

                Console.WriteLine("Задача добавлена");
            }

            static void RemoveTask()
            {
                Console.WriteLine("Введите индекс задачи: ");
                int index = ParseIntInput(Console.ReadLine());

                if (index >= 0 && index < tasks.Count)
                {
                    tasks.RemoveAt(index);
                    Console.WriteLine("Задача успешно удалена");
                }
                else
                {
                    Console.WriteLine("Индекс введен неккоректно");
                }
            }

            static void EditTask()
            {
                Console.WriteLine("Введите индекс задачи, которую хотите отредактировать: ");
                int index = ParseIntInput(Console.ReadLine());

                if (index >= 0 && index < tasks.Count)
                {
                    Task task = tasks[index];

                    Console.WriteLine($"Текущее название задачи: {task.Title}");
                    Console.WriteLine("Введите новое название задачи (Если хотите оставить без изменений, нажмите Enter): ");
                    string newTitle = Console.ReadLine();
                    if (newTitle != "")
                    {
                        task.Title = newTitle;
                    }

                    Console.WriteLine($"Текущее название задачи: {task.Description}");
                    Console.WriteLine("Введите новое описание задачи (Если хотите оставить без изменений, нажмите Enter): ");
                    string newDescription = Console.ReadLine();
                    if (newDescription != "")
                    {
                        task.Description = newDescription;
                    }

                    Console.WriteLine($"Текущая дата выполнения задачи: {task.Term.ToShortDateString()}");
                    Console.WriteLine("Введите новцю дату выполнения задачи (Если хотите оставить без изменений, нажмите Enter): ");
                    string newTerm = Console.ReadLine();
                    if (newTerm != "")
                    {
                        task.Term = ParseTerm(newTerm);
                    }

                    Console.WriteLine("Задача успешно отредактирована");
                }
                else
                {
                    Console.WriteLine("Индекс введен неккоректно");
                }
            }

            static DateTime ParseTerm(string input)
            {
                DateTime term;
                while (!DateTime.TryParse(input, out term))
                {
                    Console.WriteLine("Неправильный ввод. Попробуйте еще раз");
                    Console.Write("Введите дату выполнения задачи (в формате ДД.ММ.ГГГГ): ");
                    input = Console.ReadLine();
                }
                return term;
            }


            static void SaveTasksToDatebook()
            {
                string json = JsonConvert.SerializeObject(tasks);
                File.WriteAllText(datebook, json);

            }
        }
    }
}