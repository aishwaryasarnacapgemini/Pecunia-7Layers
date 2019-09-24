using Capgemini.Pecunia.Helpers.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capgemini.Pecunia.Entities
{
    /// <summary>
    /// Interface for Employee Entity
    /// </summary>
    public interface IEmployee
    {
        Guid EmployeeID { get; set; }
        string EmployeeName { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        string Mobile { get; set; }
        DateTime CreationDateTime { get; set; }
        DateTime LastModifiedDateTime { get; set; }
    }

    /// <summary>
    /// Represents Employee
    /// </summary>
    public class Employee:IEmployee, IUser
    {
        /* Auto-Implemented Properties */
        [Required("Employee ID can't be blank.")]
        public Guid EmployeeID { get; set; }

        [Required("Employee Name can't be blank.")]
        [RegExp(@"^(\w{2,40})$", "Employee Name should contain only 2 to 40 characters.")]
        public string EmployeeName { get; set; }

        [Required("Email can't be blank.")]
        [RegExp(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", "Email is invalid.")]
        public string Email { get; set; }

        [Required("Password can't be blank.")]
        [RegExp(@"((?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,15})", "Password should be 6 to 15 characters with at least one digit, one uppercase letter, one lower case letter.")]
        public string Password { get; set; }

        [Required("Mobile number can't be blank.")]
        [RegExp(@"^([9]{1})([234789]{1})([0-9]{8})$", "Mobile number should be of 10 digits.")]
        public string Mobile { get; set; }

        public DateTime CreationDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }


        /* Constructor */
        public Employee()
        {
            EmployeeID = default(Guid);
            EmployeeName = null;
            Email = null;
            Password = null;
            Mobile = null;
            CreationDateTime = default(DateTime);
            LastModifiedDateTime = default(DateTime);
        }
    }
}
