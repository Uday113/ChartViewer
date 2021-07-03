
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace DataAccessLayer
{
    public static class DataAccessCommon
    {
        private const string defaultConnectionString = "default";

        public static DataSet GetDataFromStoredProc(string storedProcedureName, string[] tableNames, params object[] inputParameterValues)
        {
            DataSet dsResults = new DataSet();
            Database dDatabase = DatabaseFactory.CreateDatabase(defaultConnectionString);
            System.Data.Common.DbCommand dcCommand = dDatabase.GetSqlStringCommand(storedProcedureName);
            dcCommand.CommandType = CommandType.StoredProcedure;
            dDatabase.AssignParameters(dcCommand, inputParameterValues);
            dcCommand.CommandTimeout = 120;
            dDatabase.LoadDataSet(dcCommand, dsResults, tableNames);
            return dsResults;
        }

        public static int UpdateOrInsertData(string storedProcedureName, params object[] parameterValues)
        {
            return DatabaseFactory.CreateDatabase(defaultConnectionString).ExecuteNonQuery(storedProcedureName, parameterValues);
        }
    }
}
