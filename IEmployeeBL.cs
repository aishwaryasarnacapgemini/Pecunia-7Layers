using Capgemini.Pecunia.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capgemini.Pecunia.Contracts.BLContracts
{
    public interface IEmployeeBL : IDisposable
    {
        Task<bool> AddEmployeeBL(Employee newEmployee);
        Task<List<Employee>> GetAllEmployeesBL();
        Task<Employee> GetEmployeeByEmployeeIDBL(Guid searchEmployeeID);
        Task<List<Employee>> GetEmployeesByNameBL(string supplierName);
        Task<Employee> GetEmployeeByEmailBL(string email);
        Task<Employee> GetEmployeeByEmailAndPasswordBL(string email, string password);
        Task<bool> UpdateEmployeeBL(Employee updateEmployee);
        Task<bool> UpdateEmployeePasswordBL(Employee updateEmployee);
        Task<bool> DeleteEmployeeBL(Guid deleteEmployeeID);
    }
}
