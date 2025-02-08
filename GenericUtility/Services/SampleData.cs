using GenericUtility.Models;
using System.Collections.Generic;

namespace GenericUtility.Services
{
    public static class SampleData
    {
        public static List<TutorialsVM> GetTutorials()
        {
            return new List<TutorialsVM>
            {
                new TutorialsVM
                {
                    Id = 1,
                    Name = "Python",
                    Description = "Learn Python programming.",
                    Introduction = "Python is a high-level, interpreted programming language.",
                    Variables = "Variables are used to store data in Python.",
                    Syntax = "Python syntax is simple and easy to learn.",
                    Examples = "Example: x = 5",
                    Exercises = "Exercise: Write a program to add two numbers."
                },
                new TutorialsVM
                {
                    Id = 2,
                    Name = "Python",
                    Description = "Python data types.",
                    Introduction = "Python supports various data types.",
                    Variables = "In Python, variables can hold different types of data.",
                    Syntax = "Example of data types in Python.",
                    Examples = "Example: x = 10, y = 'Hello'",
                    Exercises = "Exercise: Write a program to demonstrate different data types in Python."
                },
                new TutorialsVM
                {
                    Id = 3,
                    Name = "C#",
                    Description = "Learn C# programming.",
                    Introduction = "C# is a modern, object-oriented programming language.",
                    Variables = "Variables are used to store data in C#.",
                    Syntax = "C# syntax is based on the C family of languages.",
                    Examples = "Example: int x = 5;",
                    Exercises = "Exercise: Write a program to add two numbers in C#."
                },
                new TutorialsVM
                {
                    Id = 4,
                    Name = "C#",
                    Description = "C# data types.",
                    Introduction = "C# supports various data types.",
                    Variables = "In C#, variables can hold different types of data.",
                    Syntax = "Example of data types in C#.",
                    Examples = "Example: int x = 10; string y = 'Hello';",
                    Exercises = "Exercise: Write a program to demonstrate different data types in C#."
                },
                new TutorialsVM
                {
                    Id = 5,
                    Name = "JavaScript",
                    Description = "Learn JavaScript programming.",
                    Introduction = "JavaScript is a versatile, high-level programming language.",
                    Variables = "Variables are used to store data in JavaScript.",
                    Syntax = "JavaScript syntax is lightweight and interpreted.",
                    Examples = "Example: var x = 5;",
                    Exercises = "Exercise: Write a program to add two numbers in JavaScript."
                },
                new TutorialsVM
                {
                    Id = 6,
                    Name = "JavaScript",
                    Description = "JavaScript data types.",
                    Introduction = "JavaScript supports various data types.",
                    Variables = "In JavaScript, variables can hold different types of data.",
                    Syntax = "Example of data types in JavaScript.",
                    Examples = "Example: var x = 10; var y = 'Hello';",
                    Exercises = "Exercise: Write a program to demonstrate different data types in JavaScript."
                },
                // Add more tutorials as needed to demonstrate pagination
            };
        }
    }
}
