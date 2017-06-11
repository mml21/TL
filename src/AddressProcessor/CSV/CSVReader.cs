using System;
using System.IO;

namespace AddressProcessing.CSV
{
    public class CSVReader : IDisposable
    {
        private StreamReader _readerStream = null;
        private const char DEFAULT_SEPARATOR = '\t';
        private char _separator;
        public bool IsOpened { get; private set; }

        // This won't break backwards compatibility
        public CSVReader(char separator = DEFAULT_SEPARATOR)
        {
            _separator = separator;
        }

        public void Open(string fileName)
        {
            _readerStream = File.OpenText(fileName);
            IsOpened = true;
        }

        public bool Read(out string column1, out string column2)
        {
            string line;
            string[] columns;

            line = _readerStream.ReadLine(); // ReadLineAsync

            if (string.IsNullOrWhiteSpace(line))
            {
                column1 = null;
                column2 = null;

                return false;
            }

            columns = line.Split(_separator);

            if (columns.Length >= 2)
            {
                column1 = columns[0];
                column2 = columns[1];

                return true;
            }
            else
            {
                column1 = null;
                column2 = null;

                return false;
            }
        }

        public void Dispose()
        {
            if (_readerStream != null)
            {
                _readerStream.Close();
                IsOpened = false;
            }
        }
    }
}
