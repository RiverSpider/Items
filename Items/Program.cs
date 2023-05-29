using System;
using System.Collections.Generic;
using System.Linq;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class Supplier
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class ProductSupply
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int SupplierId { get; set; }
    public DateTime SupplyDate { get; set; }
    public int Quantity { get; set; }
}

public class DataContext
{
    public List<Product> Products { get; set; }
    public List<Supplier> Suppliers { get; set; }
    public List<ProductSupply> ProductSupplies { get; set; }

    public DataContext()
    {
        Products = new List<Product>();
        Suppliers = new List<Supplier>();
        ProductSupplies = new List<ProductSupply>();
    }
}

public class Program
{
    static void Main()
    {
        var context = new DataContext();

        var product1 = new Product { Id = 1, Name = "Product 1" };
        var product2 = new Product { Id = 2, Name = "Product 2" };
        var supplier1 = new Supplier { Id = 1, Name = "Supplier 1" };
        var supplier2 = new Supplier { Id = 2, Name = "Supplier 2" };
        var supply1 = new ProductSupply { Id = 1, ProductId = 1, SupplierId = 1, SupplyDate = new DateTime(2021, 10, 1), Quantity = 10 };
        var supply2 = new ProductSupply { Id = 2, ProductId = 2, SupplierId = 1, SupplyDate = new DateTime(2021, 10, 1), Quantity = 5 };
        var supply3 = new ProductSupply { Id = 3, ProductId = 2, SupplierId = 2, SupplyDate = new DateTime(2021, 10, 2), Quantity = 8 };

        context.Products.Add(product1);
        context.Products.Add(product2);
        context.Suppliers.Add(supplier1);
        context.Suppliers.Add(supplier2);
        context.ProductSupplies.Add(supply1);
        context.ProductSupplies.Add(supply2);
        context.ProductSupplies.Add(supply3);

        // 1) Сделать по дате поставки товаров: кто поставил, в каком количестве
        var date = new DateTime(2021, 10, 1);

        var query1 = from ps in context.ProductSupplies
                     join p in context.Products on ps.ProductId equals p.Id
                     join s in context.Suppliers on ps.SupplierId equals s.Id
                     where ps.SupplyDate == date
                     select new { SupplierName = s.Name, ProductName = p.Name, Quantity = ps.Quantity };

        Console.WriteLine("Query 1:");
        foreach (var item in query1)
        {
            Console.WriteLine($"{item.SupplierName} - {item.ProductName} - {item.Quantity}");
        }

        // 2) Сгруппировать по товару: кто поставщики данного товара
        var query2 = from ps in context.ProductSupplies
                     join p in context.Products on ps.ProductId equals p.Id
                     join s in context.Suppliers on ps.SupplierId equals s.Id
                     group s.Name by p.Name into g
                     select new { ProductName = g.Key, Suppliers = g.ToList() };

        Console.WriteLine("Query 2:");
        foreach (var item in query2)
        {
            Console.WriteLine($"{item.ProductName} - {string.Join(", ", item.Suppliers)}");
        }

        // 3) Сгруппировать по поставщику: поставщик и какие товары он поставлял
        var query3 = from ps in context.ProductSupplies
                     join p in context.Products on ps.ProductId equals p.Id
                     join s in context.Suppliers on ps.SupplierId equals s.Id
                     group p.Name by s.Name into g
                     select new { SupplierName = g.Key, Products = g.ToList() };

        Console.WriteLine("Query 3:");
        foreach (var item in query3)
        {
            Console.WriteLine($"{item.SupplierName} - {string.Join(", ", item.Products)}");
        }
    }
}