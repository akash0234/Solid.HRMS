using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.Core.Services.Repository
{
    public interface IDMLServices
    {
        Task<int> ExecuteStoredProcedureNonQueryAsync<T>(string storedProcedure, T requestModel,string outputParam = "");
        Task<DataTable> GetDataTable<T>(string storeProcedure, T requestModel);
        Task<DataSet> GetDataSet<T>(string storeProcedure, T requestModel);

    }
}
