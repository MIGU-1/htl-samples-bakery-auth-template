using System;
using System.Collections.Generic;
using System.Linq;
using Bakery.Core.Entities;
using Utils;

namespace Bakery.ImportConsole
{
    public class ImportController
    {
        public static IEnumerable<Product> ReadFromCsv()
        {
            var csvProd = "Products.csv".ReadStringMatrixFromCsv(true);
            var csvOrderItems = "OrderItems.csv".ReadStringMatrixFromCsv(true);

            var products = csvProd.Select(line =>
                new Product()
                {
                    ProductNr = line[0],
                    Name = line[1],
                    Price = Convert.ToDouble(line[2])
                }).ToList();


            var customers = csvOrderItems
                .GroupBy(line => line[2])
                .Select(customerGroup =>
                    new Customer
                    {
                        CustomerNr = customerGroup.Key,
                        Firstname = customerGroup.First()[4],
                        Lastname = customerGroup.First()[3]
                    })
                .Select(c =>
                {
                    // TODO: Initialize "UserName"
                    return c;
                })
                .ToList();

            var orders = csvOrderItems
                .GroupBy(line => line[0])
                .Select(orderGrp =>
                new Order
                {
                    OrderNr = orderGrp.Key,
                    Date = Convert.ToDateTime(orderGrp.First()[1]),
                    Customer = customers.SingleOrDefault(cust => cust.CustomerNr == orderGrp.First()[2])
                }).ToList();

            var items = csvOrderItems.Select(item =>
                new OrderItem
                {
                    Amount = Convert.ToInt32(item[6]),
                    Order = orders.SingleOrDefault(order => order.OrderNr == item[0]),
                    Product = products.SingleOrDefault(prod => prod.ProductNr == item[5])
                }).ToList();

            products.ForEach(p => p.OrderItems = items.FindAll(i => i.Product == p));
            return products;

        }
    }
}
