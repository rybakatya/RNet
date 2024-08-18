namespace RapidNet.Logging
{
    /// <summary>
    /// Used to determine the severity level when logging text.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// just used to  display information. White text.
        /// </summary>
        Info,

        /// <summary>
        /// used  to display a warning. Yellow text.
        /// </summary>
        Warning,

        /// <summary>
        /// displays an error. Red text.
        /// </summary>
        Error,

        /// <summary>
        /// displays an exception. Red text.
        /// </summary>
        Exception
    }
}



