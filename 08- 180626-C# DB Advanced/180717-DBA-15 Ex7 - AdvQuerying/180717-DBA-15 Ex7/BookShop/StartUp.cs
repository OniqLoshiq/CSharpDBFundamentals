namespace BookShop
{
    using BookShop.Data;
    using BookShop.Initializer;
    using BookShop.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                //00-BookShopDatabase
                //DbInitializer.ResetDatabase(db);

                //01-AgeRestriction
                //string command = Console.ReadLine();
                //string result = GetBooksByAgeRestriction(db, command);

                //02-GoldenBooks
                //string result = GetGoldenBooks(db);

                //03-BooksByPrice
                //string result = GetBooksByPrice(db);

                //04-NotReleasedIn
                //int year = int.Parse(Console.ReadLine());B
                //string result = GetBooksNotRealeasedIn(db, year);

                //05-BookTitlesByCategory
                //string input = Console.ReadLine();
                //string result = GetBooksByCategory(db, input);

                //06-ReleasedBeforeDate
                //string date = Console.ReadLine();
                //string result = GetBooksReleasedBefore(db, date);

                //07-AuthorSearch
                //string input = Console.ReadLine();
                //string result = GetAuthorNamesEndingIn(db, input);

                //08-BookSearch
                //string input = Console.ReadLine();
                //string result = GetBookTitlesContaining(db, input);

                //09-BookSearchByAuthor
                //string input = Console.ReadLine();
                //string result = GetBooksByAuthor(db, input);

                //10-CountBooks
                //int lengthCheck = int.Parse(Console.ReadLine());
                //int result = CountBooks(db, lengthCheck);

                //11-TotalBookCopies
                //string result = CountCopiesByAuthor(db);

                //12-ProfitByCategory
                //string result = GetTotalProfitByCategory(db);

                //13-MostRecentBooks
                //string result = GetMostRecentBooks(db);

                //14-IncreasePrices
                //IncreasePrices(db);

                //15-RemoveBooks
                int result = RemoveBooks(db);

                Console.WriteLine(result);
            }
        }

        //01
        public static string GetBooksByAgeRestriction(BookShopContext db, string command)
        {
            var ageRestriction = (AgeRestriction)Enum.Parse(typeof(AgeRestriction), command, true);

            var books = db.Books.Where(b => b.AgeRestriction == ageRestriction)
                                .OrderBy(b => b.Title)
                                .Select(b => b.Title).ToArray();

            return string.Join(Environment.NewLine, books);
        }

        //02
        public static string GetGoldenBooks(BookShopContext db)
        {
            var books = db.Books.Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                                .OrderBy(b => b.BookId)
                                .Select(b => b.Title)
                                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        //03
        public static string GetBooksByPrice(BookShopContext db)
        {
            var books = db.Books.Where(b => b.Price > 40)
                                 .OrderByDescending(b => b.Price)
                                 .Select(b => $"{b.Title} - ${b.Price:f2}")
                                 .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        //04
        public static string GetBooksNotRealeasedIn(BookShopContext db, int year)
        {
            var books = db.Books.Where(b => b.ReleaseDate.Value.Year != year)
                                .OrderBy(b => b.BookId)
                                .Select(b => b.Title);


            return string.Join(Environment.NewLine, books);
        }

        //05
        public static string GetBooksByCategory(BookShopContext db, string input)
        {
            string[] categories = input.ToLower().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToArray();

            var books = db.Books.Where(b => b.BookCategories.Any(c => categories.Contains(c.Category.Name.ToLower())))
                                .Select(b => b.Title)
                                .OrderBy(b => b)
                                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        //06
        public static string GetBooksReleasedBefore(BookShopContext db, string date)
        {
            var beforeDate = DateTime.ParseExact(date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            var books = db.Books.Where(b => b.ReleaseDate < beforeDate)
                                .OrderByDescending(b => b.ReleaseDate)
                                .Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:f2}");

            return string.Join(Environment.NewLine, books);
        }

        //07
        public static string GetAuthorNamesEndingIn(BookShopContext db, string input)
        {
            var authors = db.Authors.Where(a => EF.Functions.Like(a.FirstName, $"%{input}"))
                                    .OrderBy(a => a.FirstName)
                                    .ThenBy(a => a.LastName)
                                    .Select(a => $"{a.FirstName} {a.LastName}");

            return string.Join(Environment.NewLine, authors);
        }
        
        //08
        public static string GetBookTitlesContaining(BookShopContext db, string input)
        {
            var books = db.Books.Where(b => b.Title.IndexOf(input, StringComparison.InvariantCultureIgnoreCase) >= 0)
                                .OrderBy(b => b.Title)
                                .Select(b => b.Title)
                                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        //09
        public static string GetBooksByAuthor(BookShopContext db, string input)
        {
            var books = db.Books.Where(b => EF.Functions.Like(b.Author.LastName, $"{input}%"))
                                .OrderBy(b => b.BookId)
                                .Select(b => $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})");

            return string.Join(Environment.NewLine, books);
        }

        //10
        public static int CountBooks(BookShopContext db, int lengthCheck)
        {
            var booksCount = db.Books.Where(b => b.Title.Length > lengthCheck).Count();

            return booksCount;
        }

        //11
        public static string CountCopiesByAuthor(BookShopContext db)
        {
            var result = db.Authors.Select(a => 
                                           new
                                           {
                                               a.FirstName,
                                               a.LastName,
                                               TotalCopies = a.Books.Sum(b => b.Copies)
                                           })
                                    .OrderByDescending(b => b.TotalCopies)
                                    .Select(a => $"{a.FirstName} {a.LastName} - {a.TotalCopies}");
                                 

            return string.Join(Environment.NewLine, result);
        }

        //12
        public static string GetTotalProfitByCategory(BookShopContext db)
        {
            var result = db.Categories.Select(c => new
                                             {
                                                 c.Name,
                                                 TotalProfit = c.CategoryBooks
                                                               .Select(x => x.Book.Price * x.Book.Copies).Sum()
                                                                         
                                              })
                                      .OrderByDescending(c => c.TotalProfit)
                                      .ThenBy(c => c.Name)
                                      .Select(c => $"{c.Name} ${c.TotalProfit}");


            return string.Join(Environment.NewLine, result);
        }

        //13
        public static string GetMostRecentBooks(BookShopContext db)
        {
            var results = db.Categories.OrderBy(c => c.Name)
                                       .Select(c => new
                                                   {
                                                        c.Name,
                                                        RecentBooks = c.CategoryBooks.Select(b => new
                                                                                                  {
                                                                                                      Title = b.Book.Title,
                                                                                                      ReleaseDate = b.Book.ReleaseDate
                                                                                                  })
                                                                                     .OrderByDescending(b => b.ReleaseDate)
                                                                                     .Take(3)
                                                   }
                                              )
                                        .ToArray();

            var sb = new StringBuilder();

            foreach (var result in results)
            {
                sb.AppendLine($"--{result.Name}");

                foreach (var b in result.RecentBooks)
                {
                    sb.AppendLine($"{b.Title} ({b.ReleaseDate.Value.Year})");
                }
            }
            
            return sb.ToString().Trim();
        }

        //14
        public static void IncreasePrices(BookShopContext db)
        {
            var books = db.Books.Where(b => b.ReleaseDate.Value.Year < 2010).ToArray();

            foreach (var book in books)
            {
                book.Price += 5m;
            }

            db.SaveChanges();
        }
        
        //15
        public static int RemoveBooks(BookShopContext db)
        {
            var booksToDelete = db.Books.Where(b => b.Copies < 4200).ToArray();

            int numDeletedBooks = booksToDelete.Length;

            db.RemoveRange(booksToDelete);

            db.SaveChanges();

            return numDeletedBooks;
        }
    }
}
