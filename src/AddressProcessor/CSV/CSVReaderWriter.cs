using System;

namespace AddressProcessing.CSV
{
    /*
        2) Refactor this class into clean, elegant, rock-solid & well performing code, without over-engineering.
           Assume this code is in production and backwards compatibility must be maintained.
    */

    /// <summary>
    /// Normally we'd make this kind of I/O operations async using async/await task-based
    /// methods to avoid blocking on I/O.
    /// </summary>
    public class CSVReaderWriter : IDisposable
    {
        private CSVReader _CSVReader;
        private CSVWriter _CSVWriter;

        [Flags]
        public enum Mode { Read = 1, Write = 2 };
    
        public CSVReaderWriter()
        {
            _CSVReader = new CSVReader();
            _CSVWriter = new CSVWriter();
        }

        /// <summary>
        /// Opens the given file for reading or writing.
        /// </summary>
        /// <param name="fileName">The fileName</param>
        /// <param name="mode">The mode to open the file file (Read or Write) </param>
        public void Open(string fileName, Mode mode)
        {
            if (mode == Mode.Read)
            {
                _CSVReader.Open(fileName);
            }
            else if (mode == Mode.Write)
            {
                _CSVWriter.Open(fileName);
            }
            else
            {
                // We keep this for backwards compatibility but this means
                // that the mode can never be used as an enum flags, e.g. to both 
                // read and write the file
                throw new Exception("Unknown file mode for " + fileName);
            }
        }

        /// <summary>
        /// Writes the given columns to the file.
        /// </summary>
        /// <param name="columns">The input columns</param>
        public void Write(params string[] columns)
        {
            if (columns == null)
                throw new ArgumentNullException(nameof(columns));

            if (!_CSVWriter.IsOpened)
                throw new Exception("Not opened for writing!");

            _CSVWriter.Write(columns);
        }

        // For backwards compatibility I've kept this method in case
        // there are any callers out there using it just to check its
        // result. Ideally we'd get rid of it completely and move callers
        // to using the next method instead.
        public bool Read(string column1, string column2)
        {
            return Read(out column1, out column2);
        }

        /// <summary>
        /// Returns true if it read a line, and in this case column1
        /// and column2 will contain the tab separated lines content.
        /// Otherwise it returns false.
        /// </summary>
        /// <param name="column1"></param>
        /// <param name="column2"></param>
        /// <returns></returns>
        public bool Read(out string column1, out string column2)
        {
            if (!_CSVReader.IsOpened)
                throw new Exception("Not opened for reading!");

            return _CSVReader.Read(out column1, out column2);
        }

        /// <summary>
        /// Closes this reader/writer.
        /// </summary>
        public void Close()
        {
            if (_CSVReader != null)
            {
                _CSVReader.Dispose();
            }

            if (_CSVWriter != null)
            {
                _CSVWriter.Dispose();
            }
        }

        public void Dispose()
        {
            Close();
        }
    }
}
