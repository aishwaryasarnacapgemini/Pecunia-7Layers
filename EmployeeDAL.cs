using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pecunia.Entities;
using Pecunia.Exceptions;
using System.Data.Common;
using Newtonsoft.Json;
using System.IO;
using Capgemini.Pecunia.Contracts.DALContracts;
using Capgemini.Pecunia.Entities;
using Capgemini.Pecunia.Helpers;


namespace Capgemini.Pecunia.DataAccessLayer
{
    /// <summary>
    /// Contains data access layer methods for inserting, updating, deleting Employees from Employees collection.
    /// </summary>
    public class EmployeeDAL : EmployeeDALBase, IDisposable
    {
        /// <summary>
        /// Adds new Employee to Employees collection.
        /// </summary>
        /// <param name="newEmployee">Contains the Employee details to be added.</param>
        /// <returns>Determinates whether the new Employee is added.</returns>
        public override bool AddEmployeeDAL(Employee newEmployee)
        {
            bool employeeAdded = false;
            try
            {
                newEmployee.EmployeeID = Guid.NewGuid();
                newEmployee.CreationDateTime = DateTime.Now;
                newEmployee.LastModifiedDateTime = DateTime.Now;
                EmployeeList.Add(newEmployee);
                employeeAdded = true;
            }
            catch (PecuniaException)
            {
                throw new EmployeeAddedException("Employee not added");
            }
            return employeeAdded;
        }

        /// <summary>
        /// Gets all Employees from the collection.
        /// </summary>
        /// <returns>Returns list of all Employees.</returns>
        public override List<Employee> GetAllEmployeesDAL()
        {
            try
            {
                return EmployeeList;
            }
            catch (PecuniaException)
            {

                throw new EmployeeListException("Employee List cannot be displayed.");
            }
           
        }

        /// <summary>
        /// Gets Employee based on EmployeeID.
        /// </summary>
        /// <param name="searchEmployeeID">Represents EmployeeID to search.</param>
        /// <returns>Returns Employee object.</returns>
        public override Employee GetEmployeeByEmployeeIDDAL(Guid searchEmployeeID)
        {
            Employee matchingEmployee = null;
            try
            {
                //Find Employee based on searchEmployeeID
                matchingEmployee = EmployeeList.Find(
                    (item) => { return item.EmployeeID == searchEmployeeID; }
                );
            }
            catch (PecuniaException)
            {
                throw new InvalidEmployeeException("Employee not found.");
            }
            return matchingEmployee;
        }

        /// <summary>
        /// Gets Employee based on EmployeeName.
        /// </summary>
        /// <param name="EmployeeName">Represents EmployeeName to search.</param>
        /// <returns>Returns Employee object.</returns>
        public override List<Employee> GetEmployeesByNameDAL(string EmployeeName)
        {
            List<Employee> matchingEmployees = new List<Employee>();
            try
            {
                //Find All Employees based on EmployeeName
                matchingEmployees = EmployeeList.FindAll(
                    (item) => { return item.EmployeeName.Equals(EmployeeName, StringComparison.OrdinalIgnoreCase); }
                );
            }
            catch (PecuniaException)
            {
                throw new InvalidEmployeeException("Employee not found.");
            }
            return matchingEmployees;
        }

        /// <summary>
        /// Gets Employee based on email.
        /// </summary>
        /// <param name="email">Represents Employee's Email Address.</param>
        /// <returns>Returns Employee object.</returns>
        public override Employee GetEmployeeByEmailDAL(string email)
        {
            Employee matchingEmployee = null;
            try
            {
                //Find Employee based on Email and Password
                matchingEmployee = EmployeeList.Find(
                    (item) => { return item.Email.Equals(email); }
                );
            }
            catch (PecuniaException)
            {
                throw new InvalidEmployeeException("Employee not found.");
            }
            return matchingEmployee;
        }

        /// <summary>
        /// Gets Employee based on Email and Password.
        /// </summary>
        /// <param name="email">Represents Employee's Email Address.</param>
        /// <param name="password">Represents Employee's Password.</param>
        /// <returns>Returns Employee object.</returns>
        public override Employee GetEmployeeByEmailAndPasswordDAL(string email, string password)
        {
            Employee matchingEmployee = null;
            try
            {
                //Find Employee based on Email and Password
                matchingEmployee = EmployeeList.Find(
                    (item) => { return item.Email.Equals(email) && item.Password.Equals(password); }
                );
            }
            catch (PecuniaException)
            {
                throw new InvalidEmployeeException("Employee not found");
            }
            return matchingEmployee;
        }

        /// <summary>
        /// Updates Employee based on EmployeeID.
        /// </summary>
        /// <param name="updateEmployee">Represents Employee details including EmployeeID, EmployeeName etc.</param>
        /// <returns>Determinates whether the existing Employee is updated.</returns>
        public override bool UpdateEmployeeDAL(Employee updateEmployee)
        {
            bool EmployeeUpdated = false;
            try
            {
                //Find Employee based on EmployeeID
                Employee matchingEmployee = GetEmployeeByEmployeeIDDAL(updateEmployee.EmployeeID);

                if (matchingEmployee != null)
                {
                    //Update Employee details
                    ReflectionHelpers.CopyProperties(updateEmployee, matchingEmployee, new List<string>() { "EmployeeName", "Email" });
                    matchingEmployee.LastModifiedDateTime = DateTime.Now;

                    EmployeeUpdated = true;
                }
            }
            catch (PecuniaException)
            {
                throw new EmployeeUpdateException("Employee could not be updated.");
            }
            return EmployeeUpdated;
        }

        /// <summary>
        /// Deletes Employee based on EmployeeID.
        /// </summary>
        /// <param name="deleteEmployeeID">Represents EmployeeID to delete.</param>
        /// <returns>Determinates whether the existing Employee is updated.</returns>
        public override bool DeleteEmployeeDAL(Guid deleteEmployeeID)
        {
            bool EmployeeDeleted = false;
            try
            {
                //Find Employee based on searchEmployeeID
                Employee matchingEmployee = EmployeeList.Find(
                    (item) => { return item.EmployeeID == deleteEmployeeID; }
                );

                if (matchingEmployee != null)
                {
                    //Delete Employee from the collection
                    EmployeeList.Remove(matchingEmployee);
                    EmployeeDeleted = true;
                }
            }
            catch (PecuniaException)
            {
                throw new EmployeeDeletedException("Employee could not be deleteds");
            }
            return EmployeeDeleted;
        }

        /// <summary>
        /// Updates Employee's password based on EmployeeID.
        /// </summary>
        /// <param name="updateEmployee">Represents Employee details including EmployeeID, Password.</param>
        /// <returns>Determinates whether the existing Employee's password is updated.</returns>
        public override bool UpdateEmployeePasswordDAL(Employee updateEmployee)
        {
            bool passwordUpdated = false;
            try
            {
                //Find Employee based on EmployeeID
                Employee matchingEmployee = GetEmployeeByEmployeeIDDAL(updateEmployee.EmployeeID);

                if (matchingEmployee != null)
                {
                    //Update Employee details
                    ReflectionHelpers.CopyProperties(updateEmployee, matchingEmployee, new List<string>() { "Password" });
                    matchingEmployee.LastModifiedDateTime = DateTime.Now;

                    passwordUpdated = true;
                }
            }
            catch (PecuniaException)
            {
                throw new EmployeeUpdateException("Employee password could not be updated.");
            }
            return passwordUpdated;
        }

        /// <summary>
        /// Clears unmanaged resources such as db connections or file streams.
        /// </summary>
        public void Dispose()
        {
            //No unmanaged resources currently
        }
    }
}
