using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeContracts
{
    public interface IHomeModule
    {
        #region Methods

        List<object> GetChartData();

        #endregion
    }
}
