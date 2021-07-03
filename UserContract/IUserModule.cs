using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserContract
{
    public interface IUserModule
    {
        string ValidateUser(string emailId);
        void UpdateLoginFailure(string emailId);
    }
}
