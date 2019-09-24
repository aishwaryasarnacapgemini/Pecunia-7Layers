using Capgemini.Pecunia.Entities;
using Capgemini.Pecunia.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capgemini.Pecunia.PresentationLayer
{
    public static class CommonData
    {
        public static IUser CurrentUser { get; set; }
        public static UserType CurrentUserType { get; set; }
    }
}
