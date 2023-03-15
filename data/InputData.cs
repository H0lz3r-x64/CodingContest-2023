using System.Collections.Generic;
using System.IO;
using com.knapp.CodingContest.data;
using com.knapp.CodingContest.core;
using System;
using System.Linq;

namespace com.knapp.CodingContest
{
    /// <summary>
    /// Container class for all input into the solution
    /// </summary>
    public abstract class InputData
    {

        protected readonly Dictionary<string, Product> products = new Dictionary<string, Product>();

        protected readonly Dictionary<string, Warehouse> warehouses = new Dictionary<string, Warehouse>();

        protected readonly Dictionary<string, Customer> customers = new Dictionary<string, Customer>();

        protected readonly List<OrderLine> orderLines = new List<OrderLine>();

        /// <summary>
        /// a collection of all warehouses
        /// </summary>
        /// <returns>a collection of all warehouses</returns>
        public IReadOnlyList<Warehouse> GetWarehouses() => warehouses.Values.ToList();

        /// <summary>
        /// a list of all order-lines
        /// </summary>
        /// <returns>a list of all order-lines</returns>
        public IReadOnlyList<OrderLine> GetOrderLines() => orderLines.AsReadOnly();

        public virtual void Load()
        {
            ReadProducts(Path.Combine(Settings.DataPath, Settings.ProductFile));
            ReadCustomers(Path.Combine(Settings.DataPath, Settings.CustomerFile));
            ReadWarehouses(Path.Combine(Settings.DataPath, Settings.WarehouseFile));
            ReadWarehouseStocks(Path.Combine(Settings.DataPath, Settings.WarehouseStockFile));
            ReadOrderLines(Path.Combine(Settings.DataPath, Settings.OrderLineFile));
        }

        protected abstract Product NewProduct(string code, int size);

        private void ReadProducts(string fullFilename)
        {
            using (StreamReader streamReader = new StreamReader(fullFilename))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line)
                        && !line.StartsWith("#"))
                    {
                        string[] fields = line.Split(new[] { ';' });

                        string productCode = fields[0].Trim();

                        int size = int.Parse(fields[1]);

                        products.Add(productCode, NewProduct(productCode, size));
                    }
                }
            }

            System.Console.Out.WriteLine($"+++ loaded: products, {products.Count} entries");
        }

        protected abstract Warehouse NewWarehouse(string code, Position position);

        private void ReadWarehouses(string fullFilename)
        {
            using (StreamReader streamReader = new StreamReader(fullFilename))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line)
                        && !line.StartsWith("#"))
                    {
                        string[] fields = line.Split(new[] { ';' });

                        string code = fields[0].Trim();
                        int x = int.Parse(fields[1].Trim());
                        int y = int.Parse(fields[2].Trim());


                        warehouses.Add( code, NewWarehouse(code, new Position(x,y)) );
                    }
                }
            }


            System.Console.Out.WriteLine($"+++ loaded: warehouses, {warehouses.Count} entries");
        }


        protected abstract void AddWarehouseStock(Warehouse warehouse, Product product, int stock);

        private void ReadWarehouseStocks(string fullFilename)
        {
            using (StreamReader streamReader = new StreamReader(fullFilename))
            {
                // whcode;prodcode;stock;
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line)
                        && !line.StartsWith("#"))
                    {
                        string[] fields = line.Split(new[] { ';' });

                        string whCode = fields[0].Trim();
                        string productCode = fields[1].Trim();
                        int stock = int.Parse(fields[2].Trim());

                        AddWarehouseStock(warehouses[whCode], products[productCode], stock);
                    }
                }
            }
        }

        public abstract Customer NewCustomer(string Code, Position position);

        private void ReadCustomers( string fullFilename )
        {
            using (StreamReader streamReader = new StreamReader(fullFilename))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line)
                        && !line.StartsWith("#"))
                    {
                        string[] fields = line.Split(new[] { ';' });

                        string code = fields[0].Trim();
                        int x = int.Parse(fields[1].Trim());
                        int y = int.Parse(fields[2].Trim());

                        customers.Add(code, NewCustomer(code, new Position(x, y)));
                    }
                }
            }


            System.Console.Out.WriteLine($"+++ loaded: customers, {customers.Count} entries");
        }

        public abstract OrderLine NewOrderLine( Customer customer, Product product );

        private void ReadOrderLines(string fullFilename )
        {
            using (StreamReader streamReader = new StreamReader(fullFilename))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line)
                        && !line.StartsWith("#"))
                    {
                        string[] fields = line.Split(new[] { ';' });
                        var customerCode = fields[0].Trim();
                        var productCode = fields[1].Trim();

                        orderLines.Add(NewOrderLine(customers[customerCode], products[productCode]));
                    }
                }
            }

            System.Console.Out.WriteLine($"+++ loaded: order-lines, {orderLines.Count} entries");
        }
    }
}
