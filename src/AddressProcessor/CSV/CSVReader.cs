using System;
using System.IO;

namespace AddressProcessing.CSV
{
    public class CSVReader : IDisposable
    {
        private string _fileName;
        private StreamReader _readerStream = null;
        private const char DEFAULT_SEPARATOR = '\t';

        public void Open(string fileName)
        {
            _readerStream = File.OpenText(fileName);
        }

        public bool Read(out string column1, out string column2)
        {
            string line;
            string[] columns;

            line = _readerStream.ReadLine();

            if (string.IsNullOrWhiteSpace(line))
            {
                column1 = null;
                column2 = null;

                return false;
            }

            columns = line.Split(DEFAULT_SEPARATOR);

            if (columns.Length == 0)
            {
                column1 = null;
                column2 = null;

                return false;
            }
            else
            {
                column1 = columns[0];
                column2 = columns[1];

                return true;
            }
        }

        public void Dispose()
        {
            if (_readerStream != null)
            {
                _readerStream.Close();
            }
        }
    }
}
