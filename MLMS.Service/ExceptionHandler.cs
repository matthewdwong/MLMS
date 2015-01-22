namespace MLMS.Service
{
    using System;
    using Data;

    /// <summary>
    /// Handles system error and exception handling.
    /// </summary>
    public static class ExceptionHandler
    {
        /// <summary>
        /// Writes the exception details to the database
        /// </summary>
        /// <param name="ex">The exception object</param>
        public static void Write(Exception ex)
        {
            ExceptionHandlerDataAccess.Write(ex, null);
        }

        /// <summary>
        /// Writes the exception details to the database
        /// </summary>
        /// <param name="ex">The exception object</param>
        /// <param name="hostID">Host identifier</param>
        public static void Write(Exception ex, int hostID)
        {
            ExceptionHandlerDataAccess.Write(ex, hostID);
        }

        /// <summary>
        /// Writes the exception details to the database
        /// </summary>
        /// <param name="ex">The exception object</param>
        /// <param name="className">The name of the class</param>
        public static void Write(Exception ex, string className, int hostID)
        {
            ExceptionHandlerDataAccess.Write(ex, className, hostID);
        }

        /// <summary>
        /// Writes the exception details to the database
        /// </summary>
        /// <param name="ex">The exception object</param>
        /// <param name="className">The name of the class</param>
        public static void Write(Exception ex, string className)
        {
            ExceptionHandlerDataAccess.Write(ex, className, null);
        }
        
        /// <summary>
        /// Writes the exception details to the database
        /// </summary>
        /// <param name="ex">The exception object</param>
        /// <param name="className">The name of the class</param>
        public static void Write(string description, string message)
        {
            ExceptionHandlerDataAccess.Write(description, message);
        }
    }
}
