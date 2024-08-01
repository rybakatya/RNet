using System;


namespace RapidNetworkLibrary.Logging
{
    internal class Logger
    {
        internal static void Log(LogLevel level, string msg)
        {
            msg = "[LOGGER]: " + msg;
            switch (level)
            {
                case LogLevel.Info:
#if UNITY
                    UnityEngine.Debug.Log(msg);
#else
                    Console.WriteLine(msg);
#endif
                    break;

                case LogLevel.Warning:
#if UNITY
                    UnityEngine.Debug.LogWarning(msg);
#else
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(msg);
                    Console.ForegroundColor = ConsoleColor.White;
#endif
                    break;

                case LogLevel.Error:
#if UNITY
                    UnityEngine.Debug.LogError(msg);
#else
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(msg);
                    Console.ForegroundColor = ConsoleColor.White;
#endif
                    break;

                case LogLevel.Exception:
                    throw new Exception(msg);

            }
        }
    }
}



