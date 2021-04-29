using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace LinqConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            // Use the following connection string.
            Northwnd db = new Northwnd(@"c:\linqtest6\northwnd.mdf");

            // Create the new Customer object.
            Customer newCust = new Customer();
            newCust.CompanyName = "AdventureWorks Cafe";
            newCust.CustomerID = "ADVCA";


            // Add the customer to the Customers table.
            db.Customers.InsertOnSubmit(newCust);

            Console.WriteLine("\nCustomers matching CA before insert");

            foreach (var c in db.Customers.Where(cust => cust.CustomerID.Contains("CA")))
            {
                Console.WriteLine("{0}, {1}, {2}",
                    c.CustomerID, c.CompanyName, c.Orders.Count);
            }

            // Query for specific customer.
            // you will retrieve a Customer object and modify one of its properties
            // First() returns one object rather than a collection.
            var existingCust =
                (from c in db.Customers
                 where c.CustomerID == "ALFKI"
                 select c)
                .First();

            // Change the contact name of the customer.
            existingCust.ContactName = "New Contact";


            // Access the first element in the Orders collection.
            Order ord0 = existingCust.Orders[0];

            // Access the first element in the OrderDetails collection.
            OrderDetail detail0 = ord0.OrderDetails[0];

            // Display the order to be deleted.
            Console.WriteLine
                ("The Order Detail to be deleted is: OrderID = {0}, ProductID = {1}",
                detail0.OrderID, detail0.ProductID);

            // Mark the Order Detail row for deletion from the database.
            db.OrderDetails.DeleteOnSubmit(detail0);


            db.SubmitChanges();

            // show the before and after effects of submitting the changes
            Console.WriteLine("\nCustomers matching CA after update");
            foreach (var c in db.Customers.Where(cust =>
                cust.CustomerID.Contains("CA")))
            {
                Console.WriteLine("{0}, {1}, {2}",
                    c.CustomerID, c.CompanyName, c.Orders.Count);
            }

            // Keep the console window open after activity stops.
            Console.ReadLine();
        }
    }















    // By strongly typing the DataContext object, you do not need calls to GetTable
    // You can use strongly typed tables in all your queries when you use the strongly typed DataContext object.

    // Walkthrough Two:
    public class Northwind : DataContext
    {
        // Table<T> abstracts database details per table/data type.
        public Table<Customer> Customers;
        public Table<Order> Orders;

        public Northwind(string connection) : base(connection) { }
    }

    // Create a class and map it to a database table
    [Table(Name = "Customers")]
    public class Customer
    {
        // You designate _CustomerID and _City fields for private storage. 
        // LINQ to SQL can then store and retrieve values directly, instead of using public accessors that might include business logic
        private string _CustomerID;
        [Column(IsPrimaryKey = true, Storage = "_CustomerID")]
        public string CustomerID
        {
            get
            {
                return this._CustomerID;
            }
            set
            {
                this._CustomerID = value;
            }
        }
        private string _City;
        [Column(Storage = "_City")]
        public string City
        {
            get
            {
                return this._City;
            }
            set
            {
                this._City = value;
            }
        }

        // Walkthrough Two:
        // Here we are defining the relationship between the Customer and the Order class
        // Adding this annotation does enable you to easily navigate objects in either direction
        private EntitySet<Order> _Orders;
        public Customer()
        {
            this._Orders = new EntitySet<Order>();
        }

        [Association(Storage = "_Orders", OtherKey = "CustomerID")]
        public EntitySet<Order> Orders
        {
            get { return this._Orders; }
            set { this._Orders.Assign(value); }
        }
    }





    // Walkthrough Two:
    [Table(Name = "Orders")]
    public class Order
    {
        // Here we which indicates that Order.Customer relates as a foreign key to Customer.CustomerID
        private int _OrderID = 0;
        private string _CustomerID;
        private EntityRef<Customer> _Customer;
        public Order() { this._Customer = new EntityRef<Customer>(); }

        [Column(Storage = "_OrderID", DbType = "Int NOT NULL IDENTITY",
        IsPrimaryKey = true, IsDbGenerated = true)]
        public int OrderID
        {
            get { return this._OrderID; }
            // No need to specify a setter because IsDBGenerated is
            // true.
        }

        [Column(Storage = "_CustomerID", DbType = "NChar(5)")]
        public string CustomerID
        {
            get { return this._CustomerID; }
            set { this._CustomerID = value; }
        }

        [Association(Storage = "_Customer", ThisKey = "CustomerID")]
        public Customer Customer
        {
            get { return this._Customer.Entity; }
            set { this._Customer.Entity = value; }
        }
    }
}
