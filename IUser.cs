using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capgemini.Pecunia.Entities
{
    public interface IUser
    {
        string Email { get; set; }
        string Password { get; set; }
    }
}
