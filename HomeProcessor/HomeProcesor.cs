using DataAccessLayer;
using HomeContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace HomeModule
{
    public class HomeProcesor : IHomeModule
    {

        public List<object> GetChartData()
        {
            var dataset = DataAccessCommon.GetDataFromStoredProc("uspGetStatewiseConfirmedCases", new[] { "StatewiseCases" });
            List<object> iData = new List<object>();
            //Looping and extracting each DataColumn to List<Object>    
            foreach (DataColumn dc in dataset.Tables[0].Columns)
            {
                List<object> x;
                x = (from DataRow drr in dataset.Tables[0].Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            return iData;
        }

    }
}
