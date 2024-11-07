using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

using var db = new BloggingContext();

// Note: This sample requires the database to be created before running.
Console.WriteLine($"Database path: {db.DbPath}.");

// Create
Console.WriteLine("Inserting a new blog");
db.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
db.SaveChanges();

// Read
Console.WriteLine("Querying for a blog");
var blog = db.Blogs
    .OrderBy(b => b.BlogId)
    .First();

// Update
Console.WriteLine("Updating the blog and adding a post");
blog.Url = "https://devblogs.microsoft.com/dotnet";
blog.Posts.Add(
    new Post { Title = "Hello World", Content = "I wrote an app using EF Core!" });
db.SaveChanges();

// Delete
Console.WriteLine("Delete the blog");
db.Remove(blog);
db.SaveChanges();

//seedTasks(db);

Console.WriteLine("\n------------------------------------\nTasks and todos from the database:\n------------------------------------");

using (BloggingContext context = new())
{
    var tasks = context.Tasks.Include(task => task.Todos);
    foreach (var task in tasks)
    {
        Console.WriteLine($"Task: {task.Name}");
        foreach (var todo in task.Todos)
        {
            Console.WriteLine($"- Todo: {todo.Name}");
        }
    }
}

Console.ReadLine();



// Methods
static void seedTasks(BloggingContext db)
{
    Task produceSoftwareTask = new Task
    {
        Name = "Produce software",
        Todos = new List<Todo>()
        {
            new Todo { Name = "Write code", IsComplete = false },
            new Todo { Name = "Compile source", IsComplete = false },
            new Todo { Name = "Test program", IsComplete = false }
        }
    };

    Task brewCoffeeTask = new Task
    {
        Name = "Brew Coffee",
        Todos = new List<Todo>()
        {
            new Todo { Name = "Pour water", IsComplete = false },
            new Todo { Name = "Pour coffee", IsComplete = false },
            new Todo { Name = "Turn on", IsComplete = false }
        }
    };

    db.Tasks.Add(produceSoftwareTask);
    db.Tasks.Add(brewCoffeeTask);

    db.SaveChanges();
    Console.WriteLine("\nTasks and todos have been added to the database.");
}