using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using Capgemini.Pecunia.BusinessLayer;
using Capgemini.Pecunia.Exceptions;
using Capgemini.Pecunia.Helpers;
using Capgemini.Pecunia.Entities;
using Capgemini.Pecunia.Contracts.BLContracts;

namespace Capgemini.Pecunia.PresentationLayer
{
    class Program
    {
        /// <summary>
        /// Entry Point
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns></returns>
        static async Task Main(string[] args)
        {
            try
            {
                int internalChoice = -2;
                WriteLine("===============PECUNIA BANKING SYSTEM=========================");

                do
                {
                    //Invoke Login Screen
                    (UserType userType, IUser currentUser) = await ShowLoginScreen();

                    //Set current user details into CommonData (global data)
                    CommonData.CurrentUser = currentUser;
                    CommonData.CurrentUserType = userType;

                    //Invoke User's Menu
                    if (userType == UserType.Admin)
                    {
                        internalChoice = await AdminPresentation.AdminUserMenu();
                    }
                    else if (userType == UserType.Employee)
                    {
                        internalChoice = await EmployeePresentation.EmployeeMenu();
                    }
                    
                } while (internalChoice != -1);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                WriteLine(ex.Message);
            }

            WriteLine("Thank you!");
            ReadKey();
        }

        /// <summary>
        /// Login (based on Email and Password)
        /// </summary>
        /// <returns></returns>
        static async Task<(UserType, IUser)> ShowLoginScreen()
        {
            //Read inputs
            string email, password;
            WriteLine("=====LOGIN=========");
            Write("Email: ");
            email = ReadLine();
            Write("Password: ");
            password = ReadLine();

            using (IAdminBL adminBL = new AdminBL())
            {
                //Invoke GetAdminByEmailAndPasswordBL for checking email and password of Admin
                Admin admin = await adminBL.GetAdminByEmailAndPasswordBL(email, password);
                if (admin != null)
                {
                    return (UserType.Admin, admin);
                }
            }

            using (IEmployeeBL EmployeeBL = new EmployeeBL())
            {
                //Invoke GetEmployeeByEmailAndPasswordBL for checking email and password of Employee
                Employee Employee = await EmployeeBL.GetEmployeeByEmailAndPasswordBL(email, password);
                if (Employee != null)
                {
                    return (UserType.Employee, Employee);
                }
            }

            WriteLine("Invalid Email or Password. Please try again...");
            return (UserType.Anonymous, null);
        }
    }
}



