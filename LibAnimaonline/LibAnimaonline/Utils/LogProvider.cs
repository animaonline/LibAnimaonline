/*
LibAnimaonline - A set of useful cross platform helper classes to use with .NET, written in C#
Copyright (C) 2007-2014  Roman Alifanov - animaonline@gmail.com - http://www.animaonline.com

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see http://www.gnu.org/licenses/
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Animaonline.Utils
{
    [DebuggerStepThrough]
    public class LogProvider
    {
        #region Public Constructors

        public LogProvider(LogReceiver receiver)
        {
            Receiver = receiver;
        }

        public LogProvider(Action<LogEntry> onReceive)
        {
            Receiver = new LogReceiver(onReceive);
        }

        #endregion

        #region Public Properties

        public LogReceiver Receiver { get; set; }

        #endregion

        #region Public Methods

        #region StackTrace

        public void StackTrace(LogEntryType entryType, string value, string tag = null, Exception exception = null)
        {
            var logEntry = new LogEntry(entryType, value, tag, exception);

            var capturedStackTrace = new StackTrace(1); //1: skip this method.

            logEntry.StackTrace = capturedStackTrace;

            Log(logEntry);
        }

        #endregion

        #region Log

        public void Log(LogEntry logEntry)
        {
            Receiver.SignalReceive(logEntry);
        }

        public void Log(LogEntryType entryType, string value, string tag = null, Exception exception = null)
        {
            var logEntry = new LogEntry(entryType, value, tag, exception);

            Receiver.SignalReceive(logEntry);
        }

        #endregion

        #region Debug

        public void Debug(LogEntry logEntry)
        {
            Log(logEntry);
        }

        public void Debug(string value, string tag = null, Exception exception = null)
        {
            Log(LogEntryType.DEBUG, value, tag, exception);
        }

        #endregion

        #region Info

        public void Info(LogEntry logEntry)
        {
            Log(logEntry);
        }

        public void Info(string value, string tag = null, Exception exception = null)
        {
            Log(LogEntryType.INFO, value, tag, exception);
        }

        #endregion

        #region Warn

        public void Warn(LogEntry logEntry)
        {
            Log(logEntry);
        }

        public void Warn(string value, string tag = null, Exception exception = null)
        {
            Log(LogEntryType.WARN, value, tag, exception);
        }

        #endregion

        #region Error

        public void Error(LogEntry logEntry)
        {
            Log(logEntry);
        }

        public void Error(string value, string tag = null, Exception exception = null)
        {
            Log(LogEntryType.ERROR, value, tag, exception);
        }

        #endregion

        #region Fatal

        public void Fatal(LogEntry logEntry)
        {
            Log(logEntry);
        }

        public void Fatal(string value, string tag = null, Exception exception = null)
        {
            Log(LogEntryType.DEBUG, value, tag, exception);
        }

        #endregion

        #endregion

        #region Child Classes

        public enum LogEntryType
        {
            DEBUG,
            INFO,
            WARN,
            ERROR,
            FATAL,
            UNKNOWN
        }

        public class LogReceiver
        {
            #region Public Constructors

            public LogReceiver(Action<LogEntry> onReceive)
            {
                if (onReceive == null)
                    throw new ArgumentException("No action subscriber provided.");

                this.OnReceive = onReceive;
            }

            public LogReceiver(List<Action<LogEntry>> onReceiveActions)
            {
                if (onReceiveActions.Any(a => a == null))
                    throw new ArgumentException("Blank actions are not supported.");

                this.OnReceiveActions = onReceiveActions;
            }

            #endregion

            #region Public Fields

            public Action<LogEntry> OnReceive { get; set; }
            public List<Action<LogEntry>> OnReceiveActions { get; set; }

            #endregion

            #region Private Methods

            private readonly object _locker = new object();

            public void SignalReceive(LogEntry logEntry)
            {
                lock (_locker)
                {
                    if (this.OnReceive != null)
                        this.OnReceive(logEntry);

                    if (this.OnReceiveActions != null)
                        this.OnReceiveActions.ForEach(action => action(logEntry));
                }
            }

            #endregion
        }

        public struct LogEntry
        {
            #region Public Constructors

            public LogEntry(LogEntryType entryType, string value, string tag = null, Exception error = null)
                : this()
            {
                this.TimeStamp = DateTime.Now;

                this.EntryType = entryType;

                this.Value = value;

                if (!string.IsNullOrEmpty(tag))
                    this.Tag = tag;

                if (error != null)
                    this.Error = error;
            }

            #endregion

            #region Public Properties

            public LogEntryType EntryType { get; set; }
            public string Value { get; set; }
            public DateTime TimeStamp { get; set; }
            public string Tag { get; set; }
            public Exception Error { get; set; }
            public StackTrace StackTrace { get; set; }

            public bool HasError
            {
                get { return Error != null; }
            }

            public bool HasStackTrace
            {
                get { return this.StackTrace != null; }
            }

            #endregion

            #region Overridden Methods

            public override string ToString()
            {
                if (!string.IsNullOrEmpty(this.Value) && this.EntryType != LogEntryType.UNKNOWN && string.IsNullOrEmpty(this.Tag))
                    return string.Format("{0} [{1}] - {2}", this.TimeStamp, this.EntryType, this.Value);
                if (!string.IsNullOrEmpty(this.Value) && this.EntryType != LogEntryType.UNKNOWN && !string.IsNullOrEmpty(this.Tag))
                    return string.Format("{0} [{1} ({2})] - {3}", this.TimeStamp, this.EntryType, this.Tag, this.Value);

                return base.ToString();
            }

            #endregion
        }

        #endregion
    }
}