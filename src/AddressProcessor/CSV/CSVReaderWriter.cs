using System;

namespace AddressProcessing.CSV
{
    /*
        2) Refactor this class into clean, elegant, rock-solid & well performing code, without over-engineering.
           Assume this code is in production and backwards compatibility must be maintained.
    */

    public class CSVReaderWriter
    {
        private CSVReader _csvReader;
        private CSVWriter _csvWriter;

        [Flags]
        public enum Mode { Read = 1, Write = 2 };
    
        public CSVReaderWriter()
        {
            _csvReader = new CSVReader();
            _csvWriter = new CSVWriter();
        }

        public void Open(string fileName, Mode mode)
        {
            if (mode == Mode.Read)
            {
                _csvReader.Open(fileName);
            }
            else if (mode == Mode.Write)
            {
                _csvWriter.Open(fileName);
            }
            else
            {
                // We keep this for backwards compatibility but this means
                // that the mode can never be used as an enum flags, e.g. to both 
                // read and write the file
                throw new Exception("Unknown file mode for " + fileName);
            }
        }

        public void Write(params string[] columns)
        {
            if (columns == null)
                throw new ArgumentNullException(nameof(columns));

            // TODO: Check that file is opened for writting
            _csvWriter.Write(columns);
        }

        // For backwards compatibility I've kept this method in case
        // there are any callers out there using it just to check the
        // result of this method. Ideally we'd get rid of it completely
        // and move them to using the next method instead.
        public bool Read(string column1, string column2)
        {
            return Read(out column1, out column2);
        }

        public bool Read(out string column1, out string column2)
        {
            // TODO: Check that file is opened for reading
            return _csvReader.Read(out column1, out column2);
        }

        public void Close()
        {
            if (_csvReader != null)
            {
                _csvReader.Dispose();
            }

            if (_csvWriter != null)
            {
                _csvWriter.Dispose();
            }
        }
    }
}
