using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Console;
using Capgemini.Pecunia.BusinessLayer;
using Capgemini.Pecunia.Entities;
using Capgemini.Pecunia.Exceptions;
using Capgemini.Pecunia.Contracts.BLContracts;

namespace Capgemini.Pecunia.PresentationLayer
{
    public static class AdminPresentation
    {
        /// <summary>
        /// Menu for Admin User
        /// </summary>
        /// <returns></returns>
        public static async Task<int> AdminUserMenu()
        {
            int choice = -2;
            using (IEmployeeBL employeeBL = new EmployeeBL())
            {
                do
                {
                    //Get and display list of system users.
                    List<Employee> employees = await employeeBL.GetAllEmployeesBL();
                    WriteLine("\n***************ADMIN***********\n");
                    WriteLine("EMPLOYEES:");
                    if (employees != null && employees?.Count > 0)
                    {
                        WriteLine("#\tName\tEmail\tMobile\tCreated\tModified");
                        int serial = 0;
                        foreach (var employee in employees)
                        {
                            serial++;
                            WriteLine($"{serial}\t{employee.EmployeeName}\t{employee.Email}\t{employee.Mobile}\t{employee.CreationDateTime}\t{employee.LastModifiedDateTime}");
                        }
                    }

                    //Menu
                    WriteLine("\n1. Add Employee");
                    WriteLine("2. Update Employee");
                    WriteLine("3. Delete Employee");
                    WriteLine("-----------------------");
                    WriteLine("4. Change Password");
                    WriteLine("0. Logout");
                    WriteLine("-1. Exit");
                    Write("Choice: ");

                    //Accept and check choice
                    bool isValidChoice = int.TryParse(ReadLine(), out choice);
                    if (isValidChoice)
                    {
                        switch (choice)
                        {
                            case 1: await AddEmployee(); break;
                            case 2: await UpdateEmployee(); break;
                            case 3: await DeleteEmployee(); break;

                            case 4: await ChangeAdminPassword(); break;
                            case 0: break;
                            case -1: break;
                            default: WriteLine("Invalid Choice"); break;
                        }
                    }
                    else
                    {
                        choice = -2;
                    }
                } while (choice != 0 && choice != -1);
            }
            return choice;
        }

        /// <summary>
        /// Adds Employee.
        /// </summary>
        /// <returns></returns>
        public static async Task AddEmployee()
        {
            try
            {
                //Read inputs
                Employee employee = new Employee();
                Write("Name: ");
                employee.EmployeeName = ReadLine();
                Write("Email: ");
                employee.Email = ReadLine();
                Write("Password: ");
                employee.Password = ReadLine();
                Write("Mobile: ");
                employee.Mobile = ReadLine();

                //Invoke AddEmployeeBL method to add
                using (IEmployeeBL employeeBL = new EmployeeBL())
                {
                    bool isAdded = await employeeBL.AddEmployeeBL(employee);
                    if (isAdded)
                    {
                        WriteLine("Employee Added");
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Updates Employee.
        /// </summary>
        /// <returns></returns>
        public static async Task UpdateEmployee()
        {
            try
            {
                using (IEmployeeBL employeeBL = new EmployeeBL())
                {
                    //Read Sl.No
                    Write("Employee #: ");
                    bool isNumberValid = int.TryParse(ReadLine(), out int serial);
                    if (isNumberValid)
                    {
                        serial--;
                        List<Employee> employees = await employeeBL.GetAllEmployeesBL();
                        if (serial <= employees.Count - 1)
                        {
                            //Read inputs
                            Employee employee = employees[serial];
                            Write("Name: ");
                            employee.EmployeeName = ReadLine();
                            Write("Email: ");
                            employee.Email = ReadLine();
                            Write("Mobile: ");
                            employee.Mobile = ReadLine();

                            //Invoke UpdateemployeeBL method to update
                            bool isUpdated = await employeeBL.UpdateEmployeeBL(employee);
                            if (isUpdated)
                            {
                                WriteLine("Employee Updated");
                            }
                        }
                        else
                        {
                            WriteLine($"Invalid Employee #.\nPlease enter a number between 1 to {employees.Count}");
                        }
                    }
                    else
                    {
                        WriteLine($"Invalid number.");
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Delete Employee.
        /// </summary>
        /// <returns></returns>
        public static async Task DeleteEmployee()
        {
            try
            {
                using (IEmployeeBL employeeBL = new EmployeeBL())
                {
                    //Read Sl.No
                    Write("Employee #: ");
                    bool isNumberValid = int.TryParse(ReadLine(), out int serial);
                    if (isNumberValid)
                    {
                        serial--;
                        List<Employee> employees = await employeeBL.GetAllEmployeesBL();
                        if (serial <= employees.Count - 1)
                        {
                            //Confirmation
                            Employee employee = employees[serial];
                            Write("Are you sure? (Y/N): ");
                            string confirmation = ReadLine();

                            if (confirmation.Equals("Y", StringComparison.OrdinalIgnoreCase))
                            {
                                //Invoke DeleteemployeeBL method to delete
                                bool isDeleted = await employeeBL.DeleteEmployeeBL(employee.EmployeeID);
                                if (isDeleted)
                                {
                                    WriteLine("Employee Deleted");
                                }
                            }
                        }
                        else
                        {
                            WriteLine($"Invalid Employee #.\nPlease enter a number between 1 to {employees.Count}");
                        }
                    }
                    else
                    {
                        WriteLine($"Invalid number.");
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Updates Admin's Password.
        /// </summary>
        /// <returns></returns>
        public static async Task ChangeAdminPassword()
        {
            try
            {
                using (IAdminBL adminBL = new AdminBL())
                {
                    //Read Current Password
                    Write("Current Password: ");
                    string currentPassword = ReadLine();

                    Admin existingAdmin = await adminBL.GetAdminByEmailAndPasswordBL(CommonData.CurrentUser.Email, currentPassword);

                    if (existingAdmin != null)
                    {
                        //Read inputs
                        Write("New Password: ");
                        string newPassword = ReadLine();
                        Write("Confirm Password: ");
                        string confirmPassword = ReadLine();

                        if (newPassword.Equals(confirmPassword))
                        {
                            existingAdmin.Password = newPassword;

                            //Invoke UpdateemployeeBL method to update
                            bool isUpdated = await adminBL.UpdateAdminPasswordBL(existingAdmin);
                            if (isUpdated)
                            {
                                WriteLine("Admin Password Updated");
                            }
                        }
                        else
                        {
                            WriteLine($"New Password and Confirm Password doesn't match");
                        }
                    }
                    else
                    {
                        WriteLine($"Current Password doesn't match.");
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                WriteLine(ex.Message);
            }
        }
    }
}


