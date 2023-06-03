using Newtonsoft.Json;
using System;

namespace StudentTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Student Program!");

            // Get student information
            Console.Write("Enter student name: ");
            string name = Console.ReadLine();

            Console.Write("Enter email: ");
            string email = Console.ReadLine();

            Console.Write("Enter phone number: ");
            string phoneNumber = Console.ReadLine();

            // Display available courses
            Console.WriteLine("\nAvailable Courses:");
            Course[] courses = GetCourses();
            for (int i = 0; i < courses.Length; i++)
            {
                Console.WriteLine($"[{i + 1}] {courses[i].Name}");
            }

            // Select a course
            Console.Write("\nEnter the number of the course you want to select: ");
            int courseIndex = int.Parse(Console.ReadLine()) - 1;

            if (courseIndex >= 0 && courseIndex < courses.Length)
            {
                Course selectedCourse = courses[courseIndex];
                Console.WriteLine("\nSelected Course:");
                Console.WriteLine($"Name: {selectedCourse.Name}");
                Console.WriteLine($"Duration: {selectedCourse.Duration}");
                Console.WriteLine($"Instructor: {selectedCourse.Instructor}");
                Console.WriteLine($"Description: {selectedCourse.Description}");

                // Create student object
                Student student = new Student();
                student.Name = name;
                student.Email = email;
                student.PhoneNumber = phoneNumber;
                student.Course = selectedCourse;


                string asd = JsonConvert.SerializeObject(selectedCourse);
                
                
                Model1 db = new Model1();
                StudentTBL s = new StudentTBL();

                s.student_id = Guid.NewGuid().ToString();
                s.name = name;
                s.email = email;
                s.phone_number = phoneNumber;
                s.course = asd;

                

                db.StudentTBLs.Add(s);
                db.SaveChanges();


                /*{
                    Name = name,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    Course = selectedCourse
                };*/

                // Serialize student object as JSON
                string json = JsonConvert.SerializeObject(student);
                Console.WriteLine("\nStudent Information:");
                Console.WriteLine(json);
            }
            else
            {
                Console.WriteLine("Invalid course selection.");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        static Course[] GetCourses()
        {
            // Define and return an array of courses
            Course[] courses = new Course[]
            {
                new Course
                {
                    Name = "Introduction to Programming",
                    Duration = "4 weeks",
                    Instructor = "John Smith",
                    Description = "Learn the basics of programming using C#."
                },
                new Course
                {
                    Name = "Web Development Fundamentals",
                    Duration = "6 weeks",
                    Instructor = "Jane Doe",
                    Description = "Get started with web development using HTML, CSS, and JavaScript."
                },
                new Course
                {
                    Name = "Data Science Essentials",
                    Duration = "8 weeks",
                    Instructor = "David Johnson",
                    Description = "Explore the fundamental concepts of data science and machine learning."
                }
            };

            return courses;
        }
    }

    class Student
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Course Course { get; set; }
    }

    class Course
    {
        public string Name { get; set; }
        public string Duration { get; set; }
        public string Instructor { get; set; }
        public string Description { get; set; }
    }
}
