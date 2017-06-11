using System;
using System.IO;
using System.Text;

namespace AddressProcessing.CSV
{
    public class CSVWriter : IDisposable
    {
        private string _fileName;
        private StreamWriter _writerStream = null;
        private const char DEFAULT_SEPARATOR = '\t';

        public void Open(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            _writerStream = fileInfo.CreateText();
        }

        public void Write(params string[] columns)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < columns.Length; i++)
            {
                sb.Append(columns[i]);
                if ((columns.Length - 1) != i)
                {
                    sb.Append(DEFAULT_SEPARATOR);
                }
            }

            _writerStream.WriteLine(sb.ToString());
        }

        public void Dispose()
        {
            if (_writerStream != null)
            {
                _writerStream.Close();
            }
        }
    }
}
