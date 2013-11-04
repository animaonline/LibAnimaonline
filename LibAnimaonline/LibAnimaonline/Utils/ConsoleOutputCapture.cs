using System;
using System.IO;

namespace Animaonline.Utils
{
    /// <summary>
    /// Used for capturing console output.
    /// </summary>
    public static class ConsoleOutputCapture
    {
        public static void Start()
        {
            _coutStream = new MemoryStream();
            _coutWriter = new StreamWriter(_coutStream);

            if (_originalOutStream == null)
                _originalOutStream = Console.Out;

            //Redirect Console's output stream to our stream.
            Console.SetOut(_coutWriter);

            //capture console output till Dispose has been called.
        }

        public static string Stop()
        {
            if (_coutWriter != null && _coutStream != null)
            {
                //clear all buffers.
                _coutWriter.Flush();

                var coutString = _coutWriter.Encoding.GetString(_coutStream.ToArray());

                Console.SetOut(_originalOutStream);

                //dispose
                _coutStream.Dispose();
                _coutWriter.Dispose();

                return coutString;
            }

            return string.Empty;
        }

        /* fields */
        static MemoryStream _coutStream;
        static StreamWriter _coutWriter;
        private static TextWriter _originalOutStream;
    }
}
