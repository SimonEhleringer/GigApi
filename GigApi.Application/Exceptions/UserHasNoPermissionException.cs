using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApi.Application.Exceptions
{
    public class UserHasNoPermissionException : Exception
    {
        public UserHasNoPermissionException(string message = "Der Benutzer hat keine Rechte für diese Aktion.") : base(message)
        {   
        }
    }
}
