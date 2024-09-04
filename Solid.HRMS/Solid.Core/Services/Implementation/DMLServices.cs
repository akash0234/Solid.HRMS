using Microsoft.Data.SqlClient;
using Solid.Core.Services.Repository;
using Solid.DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.Core.Services.Implementation
{
    public class DMLServices :IDMLServices
    {
        private readonly DataService dataService;
        private readonly ParameterManager paramManager;

        public DMLServices(string connectionString)
        {
            dataService = new DataService(connectionString);
            paramManager = new ParameterManager();
        }

        
        public async Task<int> ExecuteStoredProcedureNonQueryAsync<T>(string storedProcedure, T requestModel, string outputParam = "")
        {
            // Convert requestModel properties to SqlParameter array
            var parameters = ConvertToSqlParameters(requestModel, outputParam);

            // Call the asynchronous method to get the DataTable
            int rowsEffected = await dataService.ExecuteStoredProcedureAsync(storedProcedure, parameters);

            return rowsEffected;
        }
        public async Task<DataTable> GetDataTable<T>(string storedProcedure, T requestModel)
        {
            // Convert requestModel properties to SqlParameter array
            var parameters = ConvertToSqlParameters(requestModel);

            // Call the asynchronous method to get the DataTable
            DataTable dataTable = await dataService.GetDataTableFromStoredProcedureAsync(storedProcedure, parameters);

            return dataTable;
        }
        public async Task<DataSet> GetDataSet<T>(string storedProcedure, T requestModel)
        {
            // Convert requestModel properties to SqlParameter array
            var parameters = ConvertToSqlParameters(requestModel);

            // Call the asynchronous method to get the DataSet
            DataSet dataSet = await dataService.GetDataSetFromStoredProcedureAsync(storedProcedure, parameters);

            return dataSet;
        }


        private SqlParameter[] ConvertToSqlParameters<T>(T requestModel,string outputParam = "")
        {
            var parameters = new List<SqlParameter>();

            // Use reflection to get properties of the requestModel
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(requestModel);
                var parameter = new SqlParameter($"@{property.Name}", value ?? DBNull.Value)
                {
                    SqlDbType = GetSqlDbType(property.PropertyType)
                };
                parameters.Add(parameter);
            }
            if (!string.IsNullOrEmpty(outputParam))
            {
                // Output parameter for the new AccessLog ID
                var newAccessLogIdParam = new SqlParameter($"@{outputParam}", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                parameters.Add(newAccessLogIdParam);
            }
            return parameters.ToArray();
        }

        private SqlDbType GetSqlDbType(Type type)
        {
            // Map .NET types to SQL types
            if (type == typeof(int)) return SqlDbType.Int;
            if (type == typeof(long)) return SqlDbType.BigInt;
            if (type == typeof(string)) return SqlDbType.NVarChar;
            if (type == typeof(DateTime)) return SqlDbType.DateTime;
            if (type == typeof(bool)) return SqlDbType.Bit;
            if (type == typeof(byte[])) return SqlDbType.VarBinary;
            // Add other type mappings as needed

            throw new NotSupportedException($"Type {type.Name} is not supported.");
        }
    }
}
