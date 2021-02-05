using UnityEngine;
using UnityEngine.UI;

namespace DigitalTwin.Utils {

    /// <summary>
    /// Basic logging class that can log to the editor console, the screen and a file. Supports logging levels. Set the logging levels for the console, screen and file logs using
    /// Log.consoleLogLevel, Log.screenLogLevel and Log.fileLogLevel. Set the log file name using Log.File. 
    /// </summary>
    public static class Log {
        public static string File {
            get; set;
        }

        public static LogLevel consoleLogLevel = LogLevel.INFO;
        public static LogLevel screenLogLevel = LogLevel.NONE;
        public static LogLevel fileLogLevel = LogLevel.NONE;

        /// <summary>
        /// Set the logging level for all logging operations.
        /// </summary>
        /// <param name="logLevel"></param>
        public static void SetLogLevel(LogLevel logLevel) {
            consoleLogLevel = logLevel;
            screenLogLevel = logLevel;
            fileLogLevel = logLevel;
        }

        /// <summary>
        /// Log an error message.
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message) {
            if(consoleLogLevel >= LogLevel.ERROR) {
                Debug.LogError(message);
            }
            if(screenLogLevel >= LogLevel.ERROR) {
                LogToScreen("ERROR: " + message);
            }
            if(fileLogLevel >= LogLevel.ERROR) {
                LogToFile("ERROR: " + message);
            }
        }

        /// <summary>
        /// Log a warning message.
        /// </summary>
        /// <param name="message"></param>
        public static void Warning(string message) {
            if(consoleLogLevel >= LogLevel.WARNING) {
                Debug.LogWarning(message);
            }
            if(screenLogLevel >= LogLevel.WARNING) {
                LogToScreen("WARNING: " + message);
            }
            if(fileLogLevel >= LogLevel.WARNING) {
                LogToFile("WARNING: " + message);
            }
        }

        /// <summary>
        /// Log an info message.
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message) {
            if(consoleLogLevel >= LogLevel.INFO) {
                Debug.Log(message);
            }
            if(screenLogLevel >= LogLevel.INFO) {
                LogToScreen("INFO: " + message);
            }
            if(fileLogLevel >= LogLevel.INFO) {
                LogToFile("INFO: " + message);
            }
        }

        private static Text messageText;

        private static void LogToScreen(string message) {
            if(messageText == null) {
                var panel = GameObject.Find("Message Panel");
                if(panel == null) {
                    Debug.LogWarning("No canvas in scene; cannot log to screen.");
                    return;
                }
                messageText = panel.GetComponent<RectTransform>().Find("Text").GetComponent<Text>();
            }

            messageText.text = message;
        }

        private static void LogToFile(string message) {
            try {
                System.IO.File.AppendAllText(File, message + "\n");                    
            } catch(System.Exception e) {
                Debug.LogException(e);
            }
        }
    }

    public enum LogLevel {
        /// <summary>
        /// Log nothing.
        /// </summary>
        NONE,
        /// <summary>
        /// Log errors.
        /// </summary>
        ERROR,
        /// <summary>
        /// Log warnings and errors.
        /// </summary>
        WARNING,
        /// <summary>
        /// Log info messages, warnings and errors.
        /// </summary>
        INFO
    }
}
