using NUnit.Framework;
using AddressProcessing.CSV;
using System;

namespace Csv.Tests
{
    [TestFixture]
    public class CSVReaderWriterTests
    {
        private CSVReaderWriter CSVReaderWriter;
        private const string TestInputFile = @"test_data\contacts.csv";
        private const string TestOutputFile = @"test_data\output.csv";

        [SetUp]
        public void SetUp()
        {
            CSVReaderWriter = new CSVReaderWriter();
        }

        [TearDown]
        public void TearDown()
        {
            CSVReaderWriter.Close();
        }
        
        [Test]
        public void TryOpenReadMode_NullFileName_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => CSVReaderWriter.Open(null, CSVReaderWriter.Mode.Read));
        }

        [Test]
        public void TryOpenWriteMode_NullFileName_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => CSVReaderWriter.Open(null, CSVReaderWriter.Mode.Write));
        }

        [Test]
        public void TryOpenReadMode_EmptyFileName_Throws()
        {
            Assert.Throws<ArgumentException>(() => CSVReaderWriter.Open(string.Empty, CSVReaderWriter.Mode.Read));
        }

        [Test]
        public void TryOpenWriteMode_EmptyFileName_Throws()
        {
            Assert.Throws<ArgumentException>(() => CSVReaderWriter.Open(string.Empty, CSVReaderWriter.Mode.Write));
        }

        [Test]
        public void TryOpenReadMode_InexstingFileName_Throws()
        {
            Assert.Throws<System.IO.FileNotFoundException>(() => 
                CSVReaderWriter.Open(@"test_data\notexists2.csv", CSVReaderWriter.Mode.Read));
        }

        [Test]
        public void TryOpen_ReadWriteFlags_Throws()
        {
            var fileName = @"test_data\notexists.csv";
            var exception = Assert.Throws<Exception>(() =>
                CSVReaderWriter.Open(fileName, CSVReaderWriter.Mode.Read | CSVReaderWriter.Mode.Write));
            Assert.AreEqual(string.Format("Unknown file mode for {0}", fileName), exception.Message);
        }

        [Test]
        public void CanReadFromFile()
        {
            CSVReaderWriter.Open(TestInputFile, CSVReaderWriter.Mode.Read);
            string column1, column2;

            CSVReaderWriter.Read(out column1, out column2);

            Assert.IsNotNullOrEmpty(column1);
            Assert.IsNotNullOrEmpty(column2);
            Assert.That(column1.Equals("Shelby Macias"));
            Assert.That(column2.Equals("3027 Lorem St.|Kokomo|Hertfordshire|L9T 3D5|England"));

            CSVReaderWriter.Read(out column1, out column2);

            Assert.IsNotNullOrEmpty(column1);
            Assert.IsNotNullOrEmpty(column2);
            Assert.That(column1.Equals("Porter Coffey"));
            Assert.That(column2.Equals("Ap #827-9064 Sapien. Rd.|Palo Alto|Fl.|HM0G 0YR|Scotland"));
        }

        [Test]
        public void Read_UsingWrongMethod_DoesNotWork()
        {
            CSVReaderWriter.Open(TestInputFile, CSVReaderWriter.Mode.Read);
            string column1 = string.Empty;
            string column2 = string.Empty;

            CSVReaderWriter.Read(column1, column2);

            Assert.IsEmpty(column1);
            Assert.IsEmpty(column2);
        }

        [Test]
        public void WriteToFile_AndThenReadWrittenContent()
        {
            CSVReaderWriter.Open(TestOutputFile, CSVReaderWriter.Mode.Write);

            var columns = new string[] { "Column1", "Column2" };
            CSVReaderWriter.Write(columns);

            CSVReaderWriter.Close();

            CSVReaderWriter.Open(TestOutputFile, CSVReaderWriter.Mode.Read);

            string column1, column2;
            CSVReaderWriter.Read(out column1, out column2);

            Assert.IsNotNullOrEmpty(column1);
            Assert.IsNotNullOrEmpty(column2);
            Assert.That(column1.Equals(columns[0]));
            Assert.That(column2.Equals(columns[1]));
        }

        [Test]
        public void OpenForReading_ThenTryToWrite_Throws()
        {
            CSVReaderWriter.Open(TestInputFile, CSVReaderWriter.Mode.Read);

            var columns = new string[] { "Column1", "Column2" };

            var exception = Assert.Throws<NullReferenceException>(() => CSVReaderWriter.Write(columns));
        }

        [Test]
        public void OpenForWritting_ThenTryToRead_Throws()
        {
            CSVReaderWriter.Open(TestOutputFile, CSVReaderWriter.Mode.Write);

            string column1 = string.Empty;
            string column2 = string.Empty;

            Assert.Throws<NullReferenceException>(() => CSVReaderWriter.Read(column1, column2));
            Assert.Throws<NullReferenceException>(() => CSVReaderWriter.Read(out column1, out column2));
        }
    }
}
