using System.IO;
using System.Xml.Linq;
using TtFc;

namespace Tests
{
    public class UnitTest
    {
        [Fact]
        public void AddNewStream()
        {
            string xml = @"<Books>
                <Book><Name>The Great</Name><Author>Amardilio</Author><Pages>321</Pages></Book>
            </Books>";
            var tempPath = Path.GetTempFileName();
            File.WriteAllText(tempPath, xml);

            using (FileStream stream = new FileStream(tempPath, FileMode.Open, FileAccess.ReadWrite))
            {
                stream.Seek(0, SeekOrigin.Begin);
                var books = new List<TtFc.Models.Book>
                {
                new TtFc.Models.Book { Author = "S. King", BookName = "It", Pages = 150 },
                new TtFc.Models.Book { Author = "A. Sapkovski", BookName = "The Witcher", Pages = 200 },
                new TtFc.Models.Book { Author = "A. J. M.", BookName = "On The Ground", Pages = 200 },
                new TtFc.Models.Book { Author = "DD", BookName = "The B", Pages = 200 }
                };

                XMLmanageLibrary.AddBook(stream, books);

                var test = XMLmanageLibrary.ReadXmlFile(stream);

                Assert.NotNull(test);
                Assert.True(test.Count > 0);
                var book = test.FirstOrDefault(a => a.Author == "S. King");
                Assert.NotNull(book);
                Assert.Equal("It", book.BookName);
                Assert.Equal(150, book.Pages);
                Assert.NotNull(test);
                Assert.True(test.Count == 5);
            }
        }
        [Fact]
        public void AddNewFile()
        {
            string xml = @"<Books>
                <Book><Name>The Great</Name><Author>Amardilio</Author><Pages>321</Pages></Book>
            </Books>";
            var tempPath = Path.GetTempFileName();
            File.WriteAllText(tempPath, xml);

            var books = new List<TtFc.Models.Book>
                {
                new TtFc.Models.Book { Author = "S. King", BookName = "It", Pages = 150 },
                new TtFc.Models.Book { Author = "A. Sapkovski", BookName = "The Witcher", Pages = 200 },
                new TtFc.Models.Book { Author = "A. J. M.", BookName = "On The Ground", Pages = 200 },
                new TtFc.Models.Book { Author = "DD", BookName = "The B", Pages = 200 }
                };
            XMLmanageLibrary.AddBook(tempPath, books);
            var test = XMLmanageLibrary.ReadXmlFile(tempPath);

            Assert.NotNull(test);
            Assert.True(test.Count > 0);
            var book = test.FirstOrDefault(a => a.Author == "S. King");
            Assert.NotNull(book);
            Assert.Equal("It", book.BookName);
            Assert.Equal(150, book.Pages);
            Assert.NotNull(test);
            Assert.True(test.Count == 5);

        }
        [Fact]
        public void Sort()
        {
            string xml = @"<Books>
                <Book><Name>CC</Name><Author>A</Author><Pages>300</Pages></Book>
                <Book><Name>BB</Name><Author>A</Author><Pages>200</Pages></Book>
                <Book><Name>AA</Name><Author>C</Author><Pages>300</Pages></Book>
            </Books>";
            var tempPath = Path.GetTempFileName();
            File.WriteAllText(tempPath, xml);

            XMLmanageLibrary.SortXmlFile(tempPath);

            var result = XElement.Load(tempPath);
            var names = result.Elements("Book")
                              .Select(x => (string)x.Element("Name"))
                              .ToList();
            var expectedOrder = new List<string> { "BB", "CC", "AA" };
            Assert.Equal(expectedOrder, names);
        }
        [Fact]
        public void Search()
        {
            string xml = @"<Books>
                <Book><Name>The Man</Name><Author>A</Author><Pages>300</Pages></Book>
                <Book><Name>The Witcher</Name><Author>A</Author><Pages>200</Pages></Book>
                <Book><Name>Gwienn</Name><Author>C</Author><Pages>300</Pages></Book>
            </Books>";
            var tempPath = Path.GetTempFileName();

            File.WriteAllText(tempPath, xml);

            var result = XMLmanageLibrary.SearchBook(tempPath, "he");
            Assert.NotNull(result);
            Assert.True(result.Count > 0);
            Assert.Equal(2, result.Count);
        }
        [Fact]
        public void SaveTheListOfBooks()
        {
            var books = new List<TtFc.Models.Book>
            {
                new TtFc.Models.Book { Author = "S. King", BookName = "It", Pages = 150 },
                new TtFc.Models.Book { Author = "A. Sapkovski", BookName = "The Witcher", Pages = 200 },
                new TtFc.Models.Book { Author = "A. J. M.", BookName = "On The Ground", Pages = 200 }
            };
            var guid = Guid.NewGuid().ToString() + ".xml";
            XMLmanageLibrary.SaveTheListOfBooks(guid, books);
            var test = XMLmanageLibrary.ReadXmlFile(guid);
            Assert.NotNull(test);
            Assert.True(test.Count == 3);
            Assert.Equal("S. King", test[0].Author);
            Assert.Equal("It", test[0].BookName);
            Assert.Equal(150, test[0].Pages);
            Assert.Equal("A. J. M.", test[2].Author);
            Assert.Equal("On The Ground", test[2].BookName);
            Assert.Equal(200, test[2].Pages);
        }
    }
}