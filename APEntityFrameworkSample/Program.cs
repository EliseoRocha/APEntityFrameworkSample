using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APEntityFrameworkSample
{
    class Program
    {
        static void Main(string[] args)
        {
            APEntities context = new APEntities();
#if DEBUG //Pre-processor directive
            context.Database.Log = Console.Write;
#endif

            //Print a list of Vendors and any invoices
            //LINQ queries return IQueryable by default
            //LINQ Query Syntax
            //List<Vendors> allVendors = (from v in context.Vendors
            //                            join inv in context.Invoices on 
            //                                v.VendorID equals inv.VendorID
            //                            orderby v.VendorName ascending
            //                           select v).ToList();

            //LINQ Method Syntax
            List<Vendors> allVendors = context.Vendors
                .OrderBy(v => v.VendorName)
                .Include(v => v.Invoices)
                .ToList(); //Include all invoices

            foreach (Vendors currentVendor in allVendors)
            {
                Console.WriteLine(currentVendor.VendorName);
                foreach  (Invoices i in currentVendor.Invoices)
                {
                    Console.WriteLine($"\t{i.InvoiceNumber}");
                }
            }

            //Get a list of vendors in California
            List<Vendors> caVendors = (from v in context.Vendors
                                       where v.VendorState == "CA"
                                       select v).ToList();
            //Prints out a block of whitespace
            Console.WriteLine("\n\n\n");
            foreach (var currVendor in caVendors)
            {
                Console.WriteLine(currVendor.VendorName);
            }

            //Pull back a single Vendor
            Vendors singleVen = (from v in context.Vendors
                                 where v.VendorName == "IBM"
                                 select v).SingleOrDefault();
            //FirstOrDefault()
            //Return first record or null if no records

            //Pretend this has vendors in it
            List<Vendors> testList = new List<Vendors>();
            List<Vendors> caVendors2 = (from v in testList
                                       where v.VendorState == "CA"
                                       select v).ToList();

            //Get list of vendor names and phones
            //SELECT VendorName, VendorPhone FROM Vendors
            var contactInfo = (from v in context.Vendors
                              select new VendorContact
                              {
                                  VendorName = v.VendorName,
                                  VendorPhone = v.VendorPhone
                              }).ToList();

            foreach (var v in contactInfo)
            {
                Console.WriteLine(v.VendorName);
            }

            Console.ReadKey();
        }
    }

    class VendorContact
    {
        public string VendorName { get; set; }

        public string VendorPhone { get; set; }
    }
}
