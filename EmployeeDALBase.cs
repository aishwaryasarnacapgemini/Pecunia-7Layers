using Capgemini.Pecunia.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capgemini.Pecunia.Contracts.DALContracts
{
    /// <summary>
    /// This abstract class acts as a base for EmployeeDAL class
    /// </summary>
    public abstract class EmployeeDALBase
    {
        //Collection of Employees
        protected static List<Employee> EmployeeList = new List<Employee>();
        private static string fileName = "Employees.json";

        //Methods for CRUD operations
        public abstract bool AddEmployeeDAL(Employee newEmployee);
        public abstract List<Employee> GetAllEmployeesDAL();
        public abstract Employee GetEmployeeByEmployeeIDDAL(Guid searchEmployeeID);
        public abstract List<Employee> GetEmployeesByNameDAL(string EmployeeName);
        public abstract Employee GetEmployeeByEmailDAL(string email);
        public abstract Employee GetEmployeeByEmailAndPasswordDAL(string email, string password);
        public abstract bool UpdateEmployeeDAL(Employee updateEmployee);
        public abstract bool UpdateEmployeePasswordDAL(Employee updateEmployee);
        public abstract bool DeleteEmployeeDAL(Guid deleteEmployeeID);

        /// <summary>
        /// Writes collection to the file in JSON format.
        /// </summary>
        public static void Serialize()
        {
            string serializedJson = JsonConvert.SerializeObject(EmployeeList);
            using (StreamWriter streamWriter = new StreamWriter(fileName))
            {
                streamWriter.Write(serializedJson);
                streamWriter.Close();
            }
        }

        /// <summary>
        /// Reads collection from the file in JSON format.
        /// </summary>
        public static void Deserialize()
        {
            string fileContent = string.Empty;
            if (!File.Exists(fileName))
                File.Create(fileName).Close();

            using (StreamReader streamReader = new StreamReader(fileName))
            {
                fileContent = streamReader.ReadToEnd();
                streamReader.Close();
                var EmployeeListFromFile = JsonConvert.DeserializeObject<List<Employee>>(fileContent);
                if (EmployeeListFromFile != null)
                {
                    EmployeeList = EmployeeListFromFile;
                }
            }
        }

        /// <summary>
        /// Static Constructor.
        /// </summary>
        static EmployeeDALBase()
        {
            Deserialize();
        }
    }
}
