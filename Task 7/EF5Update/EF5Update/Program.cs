using System;
using System.Data.Entity.Migrations;
using static EF5Update.EF_ProjectEntities;

namespace EF5Update
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Insert: Press 1\nDelete: Press 2\nRead: Press 3\nUpdate: Press 4");
            string a = Console.ReadLine();

            EF_ProjectEntities db = new EF_ProjectEntities();
            Employee emp = new Employee();

            if (a == "1") {
                Insert(); 
            }
            else if (a == "2")
            {
                Delete();
            }
            else if (a == "3")
            {
                Read();
            }
            else if (a == "4")
            {
                Update();
            }



           void Insert()
            {
                Console.WriteLine("Give your Email:");
                string Email = Console.ReadLine();

                Console.WriteLine("Give your FirstName:");
                string FirstName = Console.ReadLine();

                Console.WriteLine("Give your LastName:");
                string LastName = Console.ReadLine();

                emp.employee_id = Guid.NewGuid().ToString();
                emp.email = Email;
                emp.first_name = FirstName;
                emp.last_name = LastName;
                emp.hire_date = DateTime.UtcNow;

                db.Employees.Add(emp);
                db.SaveChanges();
            };

            void Delete()
            {
                Console.WriteLine("Give the EmployeeID you want to delete:");
                string empID = Console.ReadLine();

                    if (empID != null)
                    {
                    try
                    {
                        var Employee = db.Employees.Find(empID);
                        db.Employees.Remove(Employee);


                        db.SaveChanges();
                    }
                    catch(Exception ex) 
                    {
                        Console.WriteLine(ex.Message);
                    }
                    }
                else
                {
                    Console.WriteLine("No ID provided...");
                }
                 
            }

            void Update()
            {
                Console.WriteLine("Give the EmployeeID you want to Update:");
                string empID = Console.ReadLine();

                if (empID != null)
                {
                    try
                    {
                        var Employee = db.Employees.Find(empID);
                        
                        Console.WriteLine($"Employees: {Employee}");

                        Console.WriteLine("Give your new Email:");
                        string Email = Console.ReadLine();

                        Console.WriteLine("Give your new FirstName:");
                        string FirstName = Console.ReadLine();

                        Console.WriteLine("Give your new LastName:");
                        string LastName = Console.ReadLine();
                        Employee.email = Email;
                        Employee.first_name = FirstName;
                        Employee.last_name = LastName;

                        db.Employees.AddOrUpdate(Employee);


                        db.SaveChanges();
                    }
                    catch(Exception e) 
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                else 
                {
                    Console.WriteLine("No ID provided...");
                }
            }

            void Read()
            {
                Console.WriteLine("Give the EmployeeID you want to Read:");
                string empID = Console.ReadLine();
                if (empID != null)
                {
                    var Employee = db.Employees.Find(empID);

                    Console.WriteLine("Employees Details:");
                    Console.WriteLine($"Employee Email: {Employee.email}");
                    Console.WriteLine($"Employee First Name: {Employee.first_name}");
                    Console.WriteLine($"Employee Last Name: {Employee.last_name}");
                    Console.WriteLine($"Employee Hiring Data: {Employee.hire_date}");
                }
                
            }



        }
    }
}
