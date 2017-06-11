using System;
using System.IO;
using System.Text;

namespace AddressProcessing.CSV
{
    public class CSVWriter : IDisposable
    {
        private StreamWriter _writerStream = null;
        private const char DEFAULT_SEPARATOR = '\t';
        private char _separator;
        public bool IsOpened { get; private set; }

        // This won't break backwards compatibility
        public CSVWriter(char separator = DEFAULT_SEPARATOR)
        {
            _separator = separator;
        }

        public void Open(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            _writerStream = fileInfo.CreateText();
            IsOpened = true;
        }

        public void Write(params string[] columns)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < columns.Length; i++)
            {
                sb.Append(columns[i]);
                if ((columns.Length - 1) != i)
                {
                    sb.Append(_separator);
                }
            }

            _writerStream.WriteLine(sb.ToString()); // WriteLineAsync
        }

        public void Dispose()
        {
            if (_writerStream != null)
            {
                _writerStream.Close();
                IsOpened = false;
            }
        }
    }
}
