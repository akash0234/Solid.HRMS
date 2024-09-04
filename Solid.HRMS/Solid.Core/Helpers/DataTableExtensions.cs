using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Solid.Core.Helpers
{

    public static class DataTableExtensions
    {
        public static List<T> ConvertToList<T>(this DataTable dataTable) where T : new()
        {
            var list = new List<T>();

            // Get the properties of the model type
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (DataRow row in dataTable.Rows)
            {
                var item = new T();

                foreach (var property in properties)
                {
                    // Check if the DataTable contains a column with the same name as the property
                    if (dataTable.Columns.Contains(property.Name))
                    {
                        var value = row[property.Name];

                        if (value != DBNull.Value)
                        {
                            // Convert the value to the type of the property
                            var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                            // Convert value to the target type, handling nullable types
                            var convertedValue = Convert.ChangeType(value, targetType);

                            // Set the value to the property
                            property.SetValue(item, convertedValue, null);
                        }
                        else
                        {
                            // Handle nullable properties where value is DBNull
                            if (property.PropertyType.IsGenericType &&
                                property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                property.SetValue(item, null);
                            }
                        }
                    }
                }

                list.Add(item);
            }

            return list;
        }
    }


}
