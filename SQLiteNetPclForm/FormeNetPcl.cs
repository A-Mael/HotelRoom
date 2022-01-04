using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQLiteNetPclForm
{
    public partial class FormeNetPcl : Form
    {
        public FormeNetPcl()
        {//intit app component
            InitializeComponent();
        }

        private void FormeNetPcl_Load(object sender, EventArgs e)
        {
            //   connection to and SQLite database
            var db = new SQLiteConnection(".\\HotelDb.db");

            db.Execute("DROP TABLE IF EXISTS Reservation;");
            db.CreateTable<Reservation>();
            db.Execute("DROP TABLE IF EXISTS Hotel;");
            db.CreateTable<Hotel>();
            db.Execute("DROP TABLE IF EXISTS Guest;");
            db.CreateTable<Guest>();
            db.Execute("DROP TABLE IF EXISTS Room;");
            db.CreateTable<Room>();
            
            // populate Hotel table 
            AddHotel(db, 101, "The Blue Moon", "London");
            AddHotel(db, 102, "Atlantide", "London");
            AddHotel(db, 201, "Blank", "New York");
            AddHotel(db, 202, "Spark", "New York");
            AddHotel(db, 401, "Memory", "Minneapolis");

            // populate Guest table 
            AddGuest(db, "Mack Simmer", "10 E 42 St, New York, NY");
            AddGuest(db, "Betty Sery", "110 Elm St, Garden City, NY");
            AddGuest(db, "Diane Clison", "40-40 172 St, Flushing, NY");
            AddGuest(db, "Sally Smith", "1052 E 85 St, Brooklyn, NY");
            AddGuest(db, "Robin Cury", "3010 Broadway, New York, NY");
            AddGuest(db, "Georges Paris", "2510 Utica Ave, Minneapolis, MN");
            AddGuest(db, "Larie Cohen", "41 Arschloch Rd, Grenewich, CT");
            AddGuest(db, "Barth Jones", "54 Morris Ave, Stillwater, MN");

            // populate Room table 

            // The Blue Moon
            AddRoom(db, 201, 101, "Single", 30);
            AddRoom(db, 202, 101, "Double", 40);
            AddRoom(db, 203, 101, "Single", 30);
            AddRoom(db, 204, 101, "Double", 40);
            AddRoom(db, 205, 101, "Double", 40);
            AddRoom(db, 206, 101, "Suite", 100);
            // Atlantide
            AddRoom(db, 201, 102, "Single", 25);
            AddRoom(db, 202, 102, "Double", 30);
            AddRoom(db, 203, 102, "Single", 25);
            AddRoom(db, 204, 102, "Double", 30);
            AddRoom(db, 205, 102, "Double", 30);
            AddRoom(db, 206, 102, "Suite", 80);
            // Blank
            AddRoom(db, 201, 201, "Single", 80);
            AddRoom(db, 202, 201, "Double", 100);
            AddRoom(db, 203, 201, "Single", 80);
            AddRoom(db, 204, 201, "Double", 100);
            AddRoom(db, 205, 201, "Double", 100);
            AddRoom(db, 206, 201, "Suite", 300);

            // populate reservation table 
            AddResevation(db, 101, 1, 201, 1, 0, "2023-02-02", "2023-02-04");
            AddResevation(db, 101, 3, 204, 2, 1, "2023-02-05", "2023-02-10");
            AddResevation(db, 101, 4, 206, 2, 0, "2023-02-22", "2023-02-24");
            AddResevation(db, 201, 6, 205, 2, 0, "2023-03-20", "2023-03-24");
            AddResevation(db, 201, 2, 203, 2, 1, "2023-03-17", "2023-03-20");
            AddResevation(db, 201, 5, 201, 3, 0, "2023-03-18", "2023-03-24");

            db.Close();
        }

        
        public void AddResevation(SQLiteConnection db, int hotelId, int guestId, int roomId, short adults, 
                                    short children, string startDate, string endDate)
        {
            var reserve = new Reservation()
            {
                HotelId = hotelId,
                GuestId = guestId,
                RoomId = roomId,
                Adults = adults,
                Children = children,
                StartDate = startDate,
                EndDate = endDate,

            };
            db.Insert(reserve);
        }

        public void AddHotel(SQLiteConnection db, int hotelId, string hotelName, string city)
        {
            var newHotel = new Hotel()
            {
                HotelId = hotelId,
                HotelName = hotelName,
                City = city,
            };
            db.Insert(newHotel);
        }

        public void AddGuest (SQLiteConnection db, string guestName, string guestAddress)
        {
            var newGuest = new Guest()
            {
                GuestName = guestName,
                GuestAddress = guestAddress,
            };
            db.Insert(newGuest);
        }

        public void AddRoom(SQLiteConnection db, int roomId, int hotelId, string type, decimal price)
        {
            var newRoom = new Room()
            {
                RoomId = roomId,
                HotelId = hotelId,
                Type = type,
                Price = price,
            };
            db.Insert(newRoom);
        }
        private void btnChargeDataGrid_Click(object sender, EventArgs e)
        {
            var db = new SQLiteConnection(".\\HotelDb.db"); // connect sqlite database

            dataGridView1.DataSource = db.Table<Reservation>().ToList();
            // dataGridView1.DataSource = (db.Query<Reservation>("SELECT * FROM Reservation WHERE EndDate LIKE  '2023-03-24' ")).ToList();

            db.Close();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
    public class Reservation
    {
        [Indexed]
        public int HotelId { get; set; }
        [Indexed]
        public int RoomId { get; set; }
        [Indexed]
        public int GuestId { get; set; }

        public short Adults { get; set; }
        public short Children { get; set; }

        [PrimaryKey]
        public string StartDate { get; set; }
        public string EndDate { get; set; }

    }

    public class Hotel
    {
        [PrimaryKey]
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public string City { get; set; }
        //[Indexed]
        //public int StockId { get; set; }
        //public DateTime Time { get; set; }
        //public decimal Price { get; set; }
    }

    public class Guest
    {
        [PrimaryKey, AutoIncrement]
        public int GuestId { get; set; }
        public string GuestName { get; set; }
        public string GuestAddress { get; set; }

    }

    public class Room
    {
        [Indexed]
        public int RoomId { get; set; }
        [Indexed]
        public int HotelId { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }

    }
}
