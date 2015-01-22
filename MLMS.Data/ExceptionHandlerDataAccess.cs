
namespace MLMS.Data
{
    using System;
    using Microsoft.Practices.EnterpriseLibrary.Data;

    /// <summary>
    /// Handles inserting exception details into the database.
    /// </summary>
    public static class ExceptionHandlerDataAccess
    {
        /// <summary>
        /// Writes the exception to the database
        /// </summary>
        /// <param name="ex">The exception object</param>
        public static void Write(Exception ex, int? hostID)
        {
            var database = DatabaseFactory.CreateDatabase();
            using (var connection = database.CreateConnection())
            {
                connection.Open();
                var transaction = connection.BeginTransaction();

                database.ExecuteNonQuery(
                        transaction,
                        "uspEventLogInsert",
                        Environment.MachineName,
                        hostID,
                        null,
                        ex.TargetSite.Name,
                        ex.Message,
                        ex.ToString());

                transaction.Commit();
                connection.Close();
            }
        }

        /// <summary>
        /// Writes the exception to the database
        /// </summary>
        /// <param name="ex">The exception object</param>
        /// <param name="className">The class name</param>
        public static void Write(Exception ex, string className, int? hostID)
        {
            var database = DatabaseFactory.CreateDatabase();
            using (var connection = database.CreateConnection())
            {
                connection.Open();
                var transaction = connection.BeginTransaction();

                database.ExecuteNonQuery(
                        transaction,
                        "uspEventLogInsert",
                        Environment.MachineName,
                        hostID,
                        className,
                        ex.TargetSite.Name,
                        ex.Message,
                        ex.ToString());

                transaction.Commit();
                connection.Close();
            }
        }

        public static void Write(string description, string message)
        {
            var database = DatabaseFactory.CreateDatabase();
            using (var connection = database.CreateConnection())
            {
                connection.Open();
                var transaction = connection.BeginTransaction();

                database.ExecuteNonQuery(
                        transaction,
                        "uspEventLogInsert",
                        Environment.MachineName,
                        null,
                        null,
                        null,
                        description,
                        message);

                transaction.Commit();
                connection.Close();
            }
        }
    }
}
