using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using Microsoft.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Solid.DataLayer
{
    public class DataService
    {
        private readonly DatabaseConnection _databaseConnection;
        public DataService(string connectionString)
        {
            _databaseConnection = new DatabaseConnection(connectionString);

        }
        public bool CheckConnection()
        {
            try
            {
                var connection = _databaseConnection.GetConnection();
                connection.Open();
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<int> ExecuteStoredProcedureAsync(string storedProcedureName, SqlParameter[] parameters)
        {
            var connection = (SqlConnection)_databaseConnection.GetConnection();
            SqlTransaction transaction = null;
            try
            {
                await connection.OpenAsync();
                transaction = connection.BeginTransaction();

                using (var command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);
                    command.Transaction = transaction; // Associate the transaction with the command

                    var result = await command.ExecuteNonQueryAsync();
                    await transaction.CommitAsync(); // Commit the transaction if successful

                    // Retrieve the output parameter value
                    var outputParam = command.Parameters
                        .Cast<SqlParameter>()
                        .FirstOrDefault(p => p.Direction == ParameterDirection.Output);
                    int newInsertedID = 0;
                    if (outputParam != null)
                    {
                        newInsertedID = (int)outputParam.Value;

                    }



                    return newInsertedID;
                }
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    try
                    {
                        await transaction.RollbackAsync(); // Rollback the transaction if an error occurs
                    }
                    catch
                    {
                        // Handle any errors that occur during rollback
                    }
                }
                throw new Exception("An error occurred while executing the stored procedure.", ex);
            }
            finally
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }


        public async Task<DataTable> GetDataTableFromStoredProcedureAsync(string storedProcedureName, SqlParameter[] parameters)
        {
            DataTable dataTable = new DataTable();

            try
            {
                await using (var connection = (SqlConnection)_databaseConnection.GetConnection())
                {
                    await connection.OpenAsync();

                    // Begin the transaction
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var command = new SqlCommand(storedProcedureName, connection, transaction))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddRange(parameters);

                                using (var dataAdapter = new SqlDataAdapter(command))
                                {
                                    // Fill the DataTable with the results from the stored procedure
                                    await Task.Run(() => dataAdapter.Fill(dataTable));
                                }
                            }

                            // Commit the transaction if everything is successful
                            await transaction.CommitAsync();
                        }
                        catch
                        {
                            // Roll back the transaction in case of an error
                            await transaction.RollbackAsync();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception as needed (e.g., logging)
                throw new Exception("An error occurred while executing the stored procedure.", ex);
            }

            return dataTable;
        }



        public async Task<DataSet> GetDataSetFromStoredProcedureAsync(string storedProcedureName, SqlParameter[] parameters)
        {
            DataSet dataSet = new DataSet();

            try
            {
                await using (var connection = (SqlConnection)_databaseConnection.GetConnection())
                {
                    await connection.OpenAsync();

                    // Begin the transaction
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var command = new SqlCommand(storedProcedureName, connection, transaction))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddRange(parameters);

                                using (var dataAdapter = new SqlDataAdapter(command))
                                {
                                    // Fill the DataSet with the results from the stored procedure
                                    await Task.Run(() => dataAdapter.Fill(dataSet));
                                }
                            }

                            // Commit the transaction if everything is successful
                            await transaction.CommitAsync();
                        }
                        catch
                        {
                            // Roll back the transaction in case of an error
                            await transaction.RollbackAsync();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception as needed (e.g., logging)
                throw new Exception("An error occurred while executing the stored procedure.", ex);
            }

            return dataSet;
        }

    }
}
