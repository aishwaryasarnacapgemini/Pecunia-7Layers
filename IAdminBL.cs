using Capgemini.Pecunia.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capgemini.Pecunia.Contracts.BLContracts
{
    public interface IAdminBL : IDisposable
    {
        Task<Admin> GetAdminByEmailAndPasswordBL(string email, string password);
        Task<bool> UpdateAdminBL(Admin updateAdmin);
        Task<bool> UpdateAdminPasswordBL(Admin updateAdmin);
    }
}
