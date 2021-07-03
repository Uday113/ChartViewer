using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserContract;
using DataAccessLayer;

namespace UserModule
{
    public class UserProcessor : IUserModule
    {
        public string ValidateUser(string emailId)
        {
            string result = "FAIL";

            var dataset = DataAccessCommon.GetDataFromStoredProc("uspValidateUser", new[] { "ValidationResult" }, emailId);

            if (dataset != null && dataset.Tables.Count > 0)
            {
                result = Convert.ToString(dataset.Tables[0].Rows[0][0]);
            }           

            return result;
           
        }

        public void UpdateLoginFailure(string emailId)
        {
            DataAccessCommon.UpdateOrInsertData("uspUpdateLoginFailure", emailId);
        }
    }
}
