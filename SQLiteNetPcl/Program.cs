using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteNetPcl
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] words = { "The", "quick", "brown", "fox", "jumps", "over", "the", "lazy", "dog." };

            var unreadablePhrase = string.Concat(words);
            System.Console.WriteLine(unreadablePhrase);

            var readablePhrase = string.Join(" ", words);
            System.Console.WriteLine(readablePhrase);
            var db = new SQLiteConnection(".\\NetPCL.db");

            db.CreateTable<Stock>();
            db.CreateTable<Valuation>();

            //db.Execute("DELETE FROM Stock;");
            //db.Execute("DELETE FROM Valuation;");

            //AddStock(db, "Alpha");
            //AddStock(db, "Beta");
            //AddStock(db, "Chi");
            //AddStock(db, "Delta");
            //AddStock(db, "Epsilon");
            //AddStock(db, "Phi");

            //AddStock(db, "Athos");
            //AddStock(db, "Porthos");
            //AddStock(db, "Aramis");

            Console.WriteLine();

            UpdateStock(db, "Alpha", "Alphas");

            var query = db.Table<Stock>().Where(v => v.Symbol.StartsWith("A"));

            foreach (var stock in query)
                Console.WriteLine("Stock: " + stock.Symbol);

            DeleteStock(db, "Alphas");

            Console.WriteLine();

            query = db.Table<Stock>().Where(v => v.Symbol.StartsWith("A"));

            foreach (var stock in query)
                Console.WriteLine("Stock: " + stock.Symbol);

            db.Close();

            Console.ReadKey();
        }
        public static void AddStock(SQLiteConnection db, string symbol)
        {
            var stock = new Stock()
            {
                Symbol = symbol
            };
            db.Insert(stock);
            Console.WriteLine("{0} == {1}", stock.Symbol, stock.Id);
        }

        public static void UpdateStock(SQLiteConnection db, string oldSymbol, string newSymbol)
        {
            var actuel = db.Table<Stock>().Where(v => v.Symbol == oldSymbol).FirstOrDefault();
            if (actuel != null)
            {
                actuel.Symbol = newSymbol;
                db.Update(actuel);
            }
        }
        public static void DeleteStock(SQLiteConnection db, string symbol)
        {
            var actuel = db.Table<Stock>().Where(v => v.Symbol == symbol).FirstOrDefault();
            if (actuel != null)
            {
                db.Delete(actuel);
            }
        }
    }

    public class Stock
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Symbol { get; set; }
    }

    public class Valuation
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Indexed]
        public int StockId { get; set; }
        public DateTime Time { get; set; }
        public decimal Price { get; set; }
    }
}
