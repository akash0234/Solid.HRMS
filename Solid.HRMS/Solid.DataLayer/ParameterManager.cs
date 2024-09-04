using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.DataLayer
{
   
    public class ParameterManager
    {
        private readonly List<SqlParameter> _parameters;

        public ParameterManager()
        {
            _parameters = new List<SqlParameter>();
        }

        public void AddParameter(string name, object value)
        {
            var parameter = new SqlParameter
            {
                ParameterName = name,
                Value = value ?? DBNull.Value
            };
            _parameters.Add(parameter);
        }

        public SqlParameter[] GetParameters()
        {
            return _parameters.ToArray();
        }
    }
}
