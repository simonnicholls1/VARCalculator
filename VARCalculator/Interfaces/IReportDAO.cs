using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyLogicSearch.Interfaces
{
    interface IReportDAO<T>
        where T: class
    {
        List<T> GetData();
        List<T> GetFilteredData(string filter);
    }
}
