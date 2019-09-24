using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Console;
using Capgemini.Pecunia.BusinessLayer;
using Capgemini.Pecunia.Entities;
using Capgemini.Pecunia.Exceptions;
using Capgemini.Pecunia.Contracts.BLContracts;
using Pecunia.Exceptions;

namespace Capgemini.Pecunia.PresentationLayer
{
    public static class EmployeePresentation
    {
        /// <summary>
        /// Menu for Employee
        /// </summary>
        /// <returns></returns>
        public static async Task<int> EmployeeMenu()
        {
            int choice = -2;

            do
            {
                //Menu
                WriteLine("\n***************EMPLOYEE***********");
                WriteLine("1. View Customers");
                WriteLine("2. Add Customer");
                WriteLine("3. Update Customer");
                WriteLine("4. Delete Customer");
                WriteLine("-----------------------");
                WriteLine("5. View Accounts");
                WriteLine("6. Add Account");
                WriteLine("7. Update Account");
                WriteLine("8. Delete Account");
                WriteLine("-----------------------");
                WriteLine("9. Change Password");
                WriteLine("0. Logout");
                WriteLine("-1. Exit");
                Write("Choice: ");

                //Accept and check choice
                bool isValidChoice = int.TryParse(ReadLine(), out choice);
                if (isValidChoice)
                {
                    switch (choice)
                    {
                        case 1: await ViewCustomers(); break;
                        case 2: await AddCustomer(); break;
                        case 3: await UpdateCustomer(); break;
                        case 4: await DeleteCustomer(); break;

                        case 5: await ViewAccounts(); break;
                        case 6: await AddAccount(); break;
                        case 7: await UpdateAccount(); break;
                        case 8: await DeleteAccount(); break;

                        case 9: await ChangeEmployeePassword(); break;
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
            return choice;
        }


        #region Customer

        /// <summary>
        /// Displays list of Customers.
        /// </summary>
        /// <returns></returns>
        public static async Task ViewCustomers()
        {
            try
            {
                using (ICustomerBL customerBL = new CustomerBL())
                {
                    //Get and display list of system users.
                    List<Customer> customers = await customerBL.GetAllCustomersBL();
                    WriteLine("CUSTOMERS:");
                    if (customers != null && customers?.Count > 0)
                    {
                        WriteLine("#\tName\tMobile\tEmail\tCreated\tModified");
                        int serial = 0;
                        foreach (var customer in customers)
                        {
                            serial++;
                            WriteLine($"{serial}\t{customer.CustomerName}\t{customer.CustomerMobile}\t{customer.Email}\t{customer.CreationDateTime}\t{customer.LastModifiedDateTime}");
                        }
                    }
                }
            }
            catch (PecuniaException ex)
            {
                ExceptionLogger.LogException(ex);
                WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Adds Customer.
        /// </summary>
        /// <returns></returns>
        public static async Task AddCustomer()
        {
            try
            {
                //Read inputs
                Customer customer = new Customer();
                Write("Name: ");
                customer.CustomerName = ReadLine();
                Write("Mobile: ");
                customer.CustomerMobile = ReadLine();
                Write("Email: ");
                customer.Email = ReadLine();
                Write("Gender: ");
                customer.Gender = ReadLine();
                Write("Date of Birth: ");
                customer.DOB = ReadLine();
                Write("Aadhar Number: ");
                customer.AadhaarNumber = ReadLine();
                Write("PAN number: ");
                customer.PanNumber = ReadLine();


                //Invoke AddCustomerBL method to add
                using (ICustomerBL customerBL = new CustomerBL())
                {
                    bool isAdded = await customerBL.AddCustomerBL(customer);
                    if (isAdded)
                    {
                        WriteLine("Customer Added");
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
        /// Updates Customer Details.
        /// </summary>
        /// <returns></returns>
        public static async Task UpdateCustomer()
        {
            try
            {
                using (ICustomerBL supplierBL = new CustomerBL())
                {
                    //Read Sl.No
                    Write("Customer #: ");
                    bool isNumberValid = int.TryParse(ReadLine(), out int serial);
                    if (isNumberValid)
                    {
                        serial--;
                        List<Customer> suppliers = await supplierBL.GetAllCustomersBL();
                        if (serial <= suppliers.Count - 1)
                        {
                            //Read inputs
                            Customer supplier = suppliers[serial];
                            Write("Name: ");
                            supplier.CustomerName = ReadLine();
                            Write("Mobile: ");
                            supplier.CustomerMobile = ReadLine();
                            Write("Email: ");
                            supplier.Email = ReadLine();

                            //Invoke UpdateSupplierBL method to update
                            bool isUpdated = await supplierBL.UpdateCustomerBL(supplier);
                            if (isUpdated)
                            {
                                WriteLine("Customer Updated");
                            }
                        }
                        else
                        {
                            WriteLine($"Invalid Customer #.\nPlease enter a number between 1 to {suppliers.Count}");
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
        /// Delete Customer.
        /// </summary>
        /// <returns></returns>
        public static async Task DeleteCustomer()
        {
            try
            {
                using (ICustomerBL supplierBL = new CustomerBL())
                {
                    //Read Sl.No
                    Write("Customer #: ");
                    bool isNumberValid = int.TryParse(ReadLine(), out int serial);
                    if (isNumberValid)
                    {
                        serial--;
                        List<Customer> suppliers = await supplierBL.GetAllCustomersBL();
                        if (serial <= suppliers.Count - 1)
                        {
                            //Confirmation
                            Customer supplier = suppliers[serial];
                            Write("Are you sure? (Y/N): ");
                            string confirmation = ReadLine();

                            if (confirmation.Equals("Y", StringComparison.OrdinalIgnoreCase))
                            {
                                //Invoke DeleteCustomerBL method to delete
                                bool isDeleted = await supplierBL.DeleteSupplierBL(supplier.CustomerID);
                                if (isDeleted)
                                {
                                    WriteLine("Customer Deleted");
                                }
                            }
                        }
                        else
                        {
                            WriteLine($"Invalid Customer #.\nPlease enter a number between 1 to {suppliers.Count}");
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

        #endregion



        #region Account

        /// <summary>
        /// Displays list of Account.
        /// </summary>
        /// <returns></returns>
        public static async Task ViewAccounts()
        {
            try
            {
                using (IAccountBL distributorBL = new AccountBL())
                {
                    //Get and display list of accounts.
                    List<Account> distributors = await distributorBL.GetAllAccountBL();
                    WriteLine("ACCOUNTS:");
                    if (distributors != null && distributors?.Count > 0)
                    {
                        WriteLine("#\tName\tMobile\tEmail\tCreated\tModified");
                        int serial = 0;
                        foreach (var distributor in distributors)
                        {
                            serial++;
                            WriteLine($"{serial}\t{distributor.DistributorName}\t{distributor.DistributorMobile}\t{distributor.Email}\t{distributor.CreationDateTime}\t{distributor.LastModifiedDateTime}");
                        }
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
        /// Adds Account.
        /// </summary>
        /// <returns></returns>
        public static async Task AddAccount()
        {
            try
            {
                //Read inputs
                Account distributor = new Account();
                Write("Name: ");
                distributor.DistributorName = ReadLine();
                Write("Mobile: ");
                distributor.DistributorMobile = ReadLine();
                Write("Email: ");
                distributor.Email = ReadLine();
                Write("Password: ");
                distributor.Password = ReadLine();

                //Invoke AddDistributorBL method to add
                using (IAccountBL distributorBL = new AccountBL())
                {
                    bool isAdded = await distributorBL.AddAccountBL(distributor);
                    if (isAdded)
                    {
                        WriteLine("Account Added");
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
        /// Updates Account.
        /// </summary>
        /// <returns></returns>
        public static async Task UpdateAccount()
        {
            try
            {
                using (IAccountBL distributorBL = new AccountBL())
                {
                    //Read Sl.No
                    Write("Account #: ");
                    bool isNumberValid = int.TryParse(ReadLine(), out int serial);
                    if (isNumberValid)
                    {
                        serial--;
                        List<Account> distributors = await distributorBL.GetAllAccountsBL();
                        if (serial <= distributors.Count - 1)
                        {
                            //Read inputs
                            Account distributor = distributors[serial];
                            Write("Name: ");
                            distributor.DistributorName = ReadLine();
                            Write("Mobile: ");
                            distributor.DistributorMobile = ReadLine();
                            Write("Email: ");
                            distributor.Email = ReadLine();

                            //Invoke UpdateDistributorBL method to update
                            bool isUpdated = await distributorBL.UpdateAccountBL(distributor);
                            if (isUpdated)
                            {
                                WriteLine("Account Updated");
                            }
                        }
                        else
                        {
                            WriteLine($"Invalid Account #.\nPlease enter a number between 1 to {distributors.Count}");
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
        /// Delete Account.
        /// </summary>
        /// <returns></returns>
        public static async Task DeleteAccount()
        {
            try
            {
                using (IAccountBL distributorBL = new AccountBL())
                {
                    //Read Sl.No
                    Write("Account #: ");
                    bool isNumberValid = int.TryParse(ReadLine(), out int serial);
                    if (isNumberValid)
                    {
                        serial--;
                        List<Account> distributors = await distributorBL.GetAllAccountsBL();
                        if (serial <= distributors.Count - 1)
                        {
                            //Confirmation
                            Account distributor = distributors[serial];
                            Write("Are you sure? (Y/N): ");
                            string confirmation = ReadLine();

                            if (confirmation.Equals("Y", StringComparison.OrdinalIgnoreCase))
                            {
                                //Invoke DeleteDistributorBL method to delete
                                bool isDeleted = await distributorBL.DeleteAccountBL(distributor.DistributorID);
                                if (isDeleted)
                                {
                                    WriteLine("Account Deleted");
                                }
                            }
                        }
                        else
                        {
                            WriteLine($"Invalid Account #.\nPlease enter a number between 1 to {distributors.Count}");
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

        #endregion


        /// <summary>
        /// Updates Employee's Password.
        /// </summary>
        /// <returns></returns>
        public static async Task ChangeEmployeePassword()
        {
            try
            {
                using (IEmployeeBL employeeBL = new EmployeeBL())
                {
                    //Read Current Password
                    Write("Current Password: ");
                    string currentPassword = ReadLine();

                    Employee existingEmployee = await employeeBL.GetEmployeeByEmailAndPasswordBL(CommonData.CurrentUser.Email, currentPassword);

                    if (existingEmployee != null)
                    {
                        //Read inputs
                        Write("New Password: ");
                        string newPassword = ReadLine();
                        Write("Confirm Password: ");
                        string confirmPassword = ReadLine();

                        if (newPassword.Equals(confirmPassword))
                        {
                            existingEmployee.Password = newPassword;

                            //Invoke UpdateEmployeeBL method to update
                            bool isUpdated = await employeeBL.UpdateEmployeePasswordBL(existingEmployee);
                            if (isUpdated)
                            {
                                WriteLine("Employee Password Updated");
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