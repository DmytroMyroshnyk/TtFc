using System.IO;
using System.Xml;
using System.Xml.Linq;
using TtFc.Models;
using static System.Net.Mime.MediaTypeNames;

namespace TtFc
{

    public static class XMLmanageLibrary
    {
        private const string Author = "Author";
        private const string Name = "Name";
        private const string Pages = "Pages";
        private const string Book = "Book";

        public static List<Book> ReadXmlFile(Stream file)
        {
            var load = XElement.Load(file);
            var books = new List<Book>();


            foreach (XElement book in load.Elements(Book))
            {
                books.Add(new Book
                {
                    Author = book.Element(Author)?.Value,
                    BookName = book.Element(Name)?.Value,
                    Pages = int.Parse(book.Element(Pages)?.Value)
                });
            }
            return books;
        }
        public static List<Book> ReadXmlFile(string fileName)
        {
            if(string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));
            }
            var load = XElement.Load(fileName);
            var books = new List<Book>();


            foreach (XElement book in load.Elements(Book))
            {
                books.Add(new Book
                {
                    Author = book.Element(Author)?.Value,
                    BookName = book.Element(Name)?.Value,
                    Pages = int.Parse(book.Element(Pages)?.Value)
                });
            }
            return books;
        }
        public static void AddBook(Stream file, List<Book> books)
        {
            file.Seek(0, SeekOrigin.Begin);
            var load = XElement.Load(file);
            foreach (var book in books)
            {
                if (string.IsNullOrWhiteSpace(book.Author) || string.IsNullOrWhiteSpace(book.BookName))
                {
                    continue;
                }
                XElement newBook = new XElement(Book,
                    new XElement(Author, book.Author),
                    new XElement(Name, book.BookName),
                    new XElement(Pages, book.Pages)
                );
                var contains =
                    from el in load.Elements(Book)
                    where (string)el.Element(Name)! == book.BookName && (string)el.Element(Author)! == book.Author
                    select el;
                if(!contains.Any())
                {
                    load.Add(newBook);
                }
            }
            file.SetLength(0);
            load.Save(file);
            file.Flush();
            file.Seek(0, SeekOrigin.Begin);
        }
        public static void AddBook(string fileName, List<Book> books)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));
            }
            var load = XElement.Load(fileName);
            foreach (var book in books)
            {
                if (string.IsNullOrWhiteSpace(book.Author) || string.IsNullOrWhiteSpace(book.BookName))
                {
                    continue;
                }
                XElement newBook = new XElement(Book,
                    new XElement(Author, book.Author),
                    new XElement(Name, book.BookName),
                    new XElement(Pages, book.Pages)
                );
                var contains =
                    from el in load.Elements(Book)
                    where (string)el.Element(Name)! == book.BookName && (string)el.Element(Author)! == book.Author
                    select el;
                if (!contains.Any())
                {
                    load.Add(newBook);
                }
            }
            load.Save(fileName);
        }
        public static void SortXmlFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));
            }
            var load = XElement.Load(fileName);
            var sorted = load.Elements(Book)
                .OrderBy(el => (string?)el.Element(Author))
                .ThenBy(el => (string?)el.Element(Name))
                .ToList();
            var sortedBooks = new XElement("Books", sorted);
            sortedBooks.Save(fileName);
        }

        public static  List<Book> SearchBook(string fileName, string pattern = null)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));
            }
            var load = XElement.Load(fileName);
            return load.Elements(Book)
                .Where(x => ((string?)x.Element(Name))?.Contains(pattern, StringComparison.OrdinalIgnoreCase) ?? false)
                .Select(book => new Book
                {
                    Author = book.Element(Author)?.Value,
                    BookName = book.Element(Name)?.Value,
                    Pages = int.Parse(book.Element(Pages)?.Value)
                })
                .ToList();
        }
        public static void SaveTheListOfBooks(string fileName, List<Book> books)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));
            }
            XElement booksXml = new XElement("Books");
            foreach (var book in books)
            {
                XElement newBook = new XElement(Book,
                    new XElement(Author, book.Author),
                    new XElement(Name, book.BookName),
                    new XElement(Pages, book.Pages)
                );
                booksXml.Add(newBook);
            }
            XDocument doc = new XDocument(booksXml);
            doc.Save(fileName);
        }
        

    }
}
