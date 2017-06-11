using System;
using System.IO;

namespace AddressProcessing.CSV
{
    /*
        1) List three to five key concerns with this implementation that you would discuss with the junior developer. 

        Please leave the rest of this file as it is so we can discuss your concerns during the next stage of the interview process.
        
        1) This class does too much, breaking the Single Responsability Principle (SRP). 
           This is immediately obvious just from the name used for this class. 
           It would be cleaner and more maintainable to split this class into two CSVReader and CSVWriter classes.
        2) This class should implement the IDisposable interface to clean up the file stream/s (_readerStream and _writerStream).
           This will enable it to be used within a Using statement, preventing the users of it from forgetting to call the Close method.
        3) There are two Read public methods which look almost identical (breaks DRY, too much code duplication), 
           but crucially one of them assigns values to input parameters, which won't be visible to the caller.
           In addition this method does no validation on the string returned from the ReadLine private method.
        4) Write methods uses expensive string concatenation (since strings are inmutable a new copy will be created each time 
           that the string concatenation operator + is used). Would be better to use StringBuffer.
        5) Some public methods are missing input/arguments validation for some arguments (e.g. Open, Write). 
           For instance in the Open method, the fileName argument could be null or not corresponding to an existing
           file for reading. This will still throw an exception from the .NET System.IO methods but we could do argument validation ourselves beforehand.
           Also in the Write method, the input columns array could be null.
        6) Separator character is defined and used in various methods, could be made a field of this class.
           Also the outPut variable in Write method has inconsistent casing on its name.
    */

    public class CSVReaderWriterForAnnotation
    {
        private StreamReader _readerStream = null;
        private StreamWriter _writerStream = null;

        [Flags]
        public enum Mode { Read = 1, Write = 2 };

        public void Open(string fileName, Mode mode)
        {
            if (mode == Mode.Read)
            {
                _readerStream = File.OpenText(fileName);
            }
            else if (mode == Mode.Write)
            {
                FileInfo fileInfo = new FileInfo(fileName);
                _writerStream = fileInfo.CreateText();
            }
            else
            {
                throw new Exception("Unknown file mode for " + fileName);
            }
        }

        public void Write(params string[] columns)
        {
            string outPut = "";

            for (int i = 0; i < columns.Length; i++)
            {
                outPut += columns[i];
                if ((columns.Length - 1) != i)
                {
                    outPut += "\t";
                }
            }

            WriteLine(outPut);
        }

        public bool Read(string column1, string column2)
        {
            const int FIRST_COLUMN = 0;
            const int SECOND_COLUMN = 1;

            string line;
            string[] columns;

            char[] separator = { '\t' };

            line = ReadLine();
            columns = line.Split(separator);

            if (columns.Length == 0)
            {
                column1 = null;
                column2 = null;

                return false;
            }
            else
            {
                column1 = columns[FIRST_COLUMN];
                column2 = columns[SECOND_COLUMN];

                return true;
            }
        }

        public bool Read(out string column1, out string column2)
        {
            const int FIRST_COLUMN = 0;
            const int SECOND_COLUMN = 1;

            string line;
            string[] columns;

            char[] separator = { '\t' };

            line = ReadLine();

            if (line == null)
            {
                column1 = null;
                column2 = null;

                return false;
            }

            columns = line.Split(separator);

            if (columns.Length == 0)
            {
                column1 = null;
                column2 = null;

                return false;
            } 
            else
            {
                column1 = columns[FIRST_COLUMN];
                column2 = columns[SECOND_COLUMN];

                return true;
            }
        }

        private void WriteLine(string line)
        {
            _writerStream.WriteLine(line);
        }

        private string ReadLine()
        {
            return _readerStream.ReadLine();
        }

        public void Close()
        {
            if (_writerStream != null)
            {
                _writerStream.Close();
            }

            if (_readerStream != null)
            {
                _readerStream.Close();
            }
        }
    }
}
