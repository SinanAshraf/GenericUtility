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
                    Name = "Python",
                    Content = "Python is a high-level, interpreted programming language.",
                },
                new TutorialsVM
                {
                    Name = "Python",
                    Content = "Python supports various data types.",
                },
                new TutorialsVM
                {
                    Name = "C#",
                    Content = "C# is a modern, object-oriented programming language.",
                },
                new TutorialsVM
                {
                    Name = "C#",
                    Content = "C# supports various data types.",
                },
                new TutorialsVM
                {
                    Name = "JavaScript",
                    Content = "JavaScript is a versatile, high-level programming language.",
                },
                new TutorialsVM
                {
                    Name = "JavaScript",
                    Content = "JavaScript supports various data types.",
                },
                // Add more tutorials as needed to demonstrate pagination
            };
        }
    }
}
