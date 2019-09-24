using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pecunia.Exceptions;
using Pecunia.Entities;
using Pecunia.DataAccessLayer;
using System.Text.RegularExpressions;
using Capgemini.Pecunia.Entities;
using Capgemini.Pecunia.Contracts.BLContracts;
using Capgemini.Pecunia.Contracts.DALContracts;
using Capgemini.Pecunia.DataAccessLayer;

namespace Capgemini.Pecunia.BusinessLayer
{
    /// <summary>
    /// Contains data access layer methods for inserting, updating, deleting Employees from Employees collection.
    /// </summary>
    public class EmployeeBL : BLBase<Employee>, IEmployeeBL, IDisposable
    {
        //fields
        EmployeeDALBase employeeDAL;

        /// <summary>
        /// Constructor.
        /// </summary>
        public EmployeeBL()
        {
            this.employeeDAL = new EmployeeDAL();
        }

        /// <summary>
        /// Validations on data before adding or updating.
        /// </summary>
        /// <param name="entityObject">Represents object to be validated.</param>
        /// <returns>Returns a boolean value, that indicates whether the data is valid or not.</returns>
        protected async override Task<bool> Validate(Employee entityObject)
        {
            //Create string builder
            StringBuilder sb = new StringBuilder();
            bool valid = await base.Validate(entityObject);

            //Email is Unique
            var existingObject = await GetEmployeeByEmailBL(entityObject.Email);
            if (existingObject != null && existingObject?.EmployeeID != entityObject.EmployeeID)
            {
                valid = false;
                sb.Append(Environment.NewLine + $"Email {entityObject.Email} already exists");
            }

            if (valid == false)
                throw new PecuniaException(sb.ToString());
            return valid;
        }

        /// <summary>
        /// Adds new Employee to Employees collection.
        /// </summary>
        /// <param name="newEmployee">Contains the Employee details to be added.</param>
        /// <returns>Determinates whether the new Employee is added.</returns>
        public async Task<bool> AddEmployeeBL(Employee newEmployee)
        {
            bool EmployeeAdded = false;
            try
            {
                if (await Validate(newEmployee))
                {
                    await Task.Run(() =>
                    {
                        this.employeeDAL.AddEmployeeDAL(newEmployee);
                        EmployeeAdded = true;
                        Serialize();
                    });
                }
            }
            catch (PecuniaException ex)
            {
                throw new EmployeeAddedException(ex.Message);
            }
            return EmployeeAdded;
        }

        /// <summary>
        /// Gets all Employees from the collection.
        /// </summary>
        /// <returns>Returns list of all Employees.</returns>
        public async Task<List<Employee>> GetAllEmployeesBL()
        {
            List<Employee> EmployeesList = null;
            try
            {
                await Task.Run(() =>
                {
                    EmployeesList = employeeDAL.GetAllEmployeesDAL();
                });
            }
            catch (PecuniaException ex)
            {
                throw new EmployeeListException(ex.Message);
            }
            return EmployeesList;
        }

        /// <summary>
        /// Gets Employee based on EmployeeID.
        /// </summary>
        /// <param name="searchEmployeeID">Represents EmployeeID to search.</param>
        /// <returns>Returns Employee object.</returns>
        public async Task<Employee> GetEmployeeByEmployeeIDBL(Guid searchEmployeeID)
        {
            Employee matchingEmployee = null;
            try
            {
                await Task.Run(() =>
                {
                    matchingEmployee = employeeDAL.GetEmployeeByEmployeeIDDAL(searchEmployeeID);
                });
            }
            catch (PecuniaException ex)
            {
                throw new InvalidEmployeeException(ex.Message);
            }
            return matchingEmployee;
        }

        /// <summary>
        /// Gets Employee based on EmployeeName.
        /// </summary>
        /// <param name="EmployeeName">Represents EmployeeName to search.</param>
        /// <returns>Returns Employee object.</returns>
        public async Task<List<Employee>> GetEmployeesByNameBL(string EmployeeName)
        {
            List<Employee> matchingEmployees = new List<Employee>();
            try
            {
                await Task.Run(() =>
                {
                    matchingEmployees = employeeDAL.GetEmployeesByNameDAL(EmployeeName);
                });
            }
            catch (PecuniaException ex)
            {
                throw new InvalidEmployeeException(ex.Message);
            }
            return matchingEmployees;
        }

        /// <summary>
        /// Gets Employee based on Email and Password.
        /// </summary>
        /// <param name="email">Represents Employee's Email Address.</param>
        /// <returns>Returns Employee object.</returns>
        public async Task<Employee> GetEmployeeByEmailBL(string email)
        {
            Employee matchingEmployee = null;
            try
            {
                await Task.Run(() =>
                {
                    matchingEmployee = employeeDAL.GetEmployeeByEmailDAL(email);
                });
            }
            catch (PecuniaException ex)
            {
                throw new InvalidEmployeeException(ex.Message);
            }
            return matchingEmployee;
        }

        /// <summary>
        /// Gets Employee based on Password.
        /// </summary>
        /// <param name="email">Represents Employee's Email Address.</param>
        /// <param name="password">Represents Employee's Password.</param>
        /// <returns>Returns Employee object.</returns>
        public async Task<Employee> GetEmployeeByEmailAndPasswordBL(string email, string password)
        {
            Employee matchingEmployee = null;
            try
            {
                await Task.Run(() =>
                {
                    matchingEmployee = employeeDAL.GetEmployeeByEmailAndPasswordDAL(email, password);
                });
            }
            catch (PecuniaException ex)
            {
                throw new InvalidEmployeeException(ex.Message);
            }
            return matchingEmployee;
        }

        /// <summary>
        /// Updates Employee based on EmployeeID.
        /// </summary>
        /// <param name="updateEmployee">Represents Employee details including EmployeeID, EmployeeName etc.</param>
        /// <returns>Determinates whether the existing Employee is updated.</returns>
        public async Task<bool> UpdateEmployeeBL(Employee updateEmployee)
        {
            bool EmployeeUpdated = false;
            try
            {
                if ((await Validate(updateEmployee)) && (await GetEmployeeByEmployeeIDBL(updateEmployee.EmployeeID)) != null)
                {
                    this.employeeDAL.UpdateEmployeeDAL(updateEmployee);
                    EmployeeUpdated = true;
                    Serialize();
                }
            }
            catch (PecuniaException ex)
            {
                throw new EmployeeUpdateException(ex.Message);
            }
            return EmployeeUpdated;
        }

        /// <summary>
        /// Deletes Employee based on EmployeeID.
        /// </summary>
        /// <param name="deleteEmployeeID">Represents EmployeeID to delete.</param>
        /// <returns>Determinates whether the existing Employee is updated.</returns>
        public async Task<bool> DeleteEmployeeBL(Guid deleteEmployeeID)
        {
            bool EmployeeDeleted = false;
            try
            {
                await Task.Run(() =>
                {
                    EmployeeDeleted = employeeDAL.DeleteEmployeeDAL(deleteEmployeeID);
                    Serialize();
                });
            }
            catch (PecuniaException ex)
            {
                throw new EmployeeDeletedException(ex.Message);
            }
            return EmployeeDeleted;
        }

        /// <summary>
        /// Updates Employee's password based on EmployeeID.
        /// </summary>
        /// <param name="updateEmployee">Represents Employee details including EmployeeID, Password.</param>
        /// <returns>Determinates whether the existing Employee's password is updated.</returns>
        public async Task<bool> UpdateEmployeePasswordBL(Employee updateEmployee)
        {
            bool passwordUpdated = false;
            try
            {
                if ((await Validate(updateEmployee)) && (await GetEmployeeByEmployeeIDBL(updateEmployee.EmployeeID)) != null)
                {
                    this.employeeDAL.UpdateEmployeePasswordDAL(updateEmployee);
                    passwordUpdated = true;
                    Serialize();
                }
            }
            catch (PecuniaException ex) 
            {
                throw new EmployeeUpdateException(ex.Message);
            }
            return passwordUpdated;
        }

        /// <summary>
        /// Disposes DAL object(s).
        /// </summary>
        public void Dispose()
        {
            ((EmployeeDAL)employeeDAL).Dispose();
        }

        /// <summary>
        /// Invokes Serialize method of DAL.
        /// </summary>
        public void Serialize()
        {
            try
            {
                EmployeeDAL.Serialize();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///Invokes Deserialize method of DAL.
        /// </summary>
        public void Deserialize()
        {
            try
            {
                EmployeeDAL.Deserialize();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
