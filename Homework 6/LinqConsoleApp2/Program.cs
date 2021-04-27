﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace LinqConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            // Use a connection string.
            DataContext db = new DataContext
                (@"c:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\northwnd.mdf");
            // I have tried:
            // changing various different file paths
            // changing the database on and offline
            // attaching to visual studio through server explorer and data connections
            // changing permissions on my SMSS folder. Led to this error
            // trying to attach the database to visual studios. (requires version 852, I'm running 904?)
           

            // Get a typed table to run queries.
            Table<Customer> Customers = db.GetTable<Customer>();

            // Here we are wrtiting a query find which customers in the database Customers table are located in London.
            // However this does not actually perform the query it simply describes it. Known as deferred execution

            // Attach the log to show generated SQL.
            db.Log = Console.Out;

            // Query for customers in London.
            IQueryable<Customer> custQuery =
                from cust in Customers
                where cust.City == "London"
                select cust;

            // Here we actually execute the command
            // When we hit the foreach statement a SQL command is executed against the database and objects are materialized.
            foreach (Customer cust in custQuery)
            {
                Console.WriteLine("ID={0}, City={1}", cust.CustomerID,
                    cust.City);
            }

            // Prevent console window from closing.
            Console.ReadLine();
        }
    }

    // Talk about this first

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
    }
}