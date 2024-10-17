using System;

public class Address
{
    private string _street;      
    private string _city;        
    private string _state;       
    private string _country;      

    
    public Address(string street, string city, string state, string country)
    {
        _street = street;
        _city = city;
        _state = state;
        _country = country;
    }

    
    public bool IsInUSA()
    {
        return _country.Equals("USA", StringComparison.OrdinalIgnoreCase);
    }

    
    public string GetFullAddress()
    {
        return $"{_street}\n{_city}, {_state}\n{_country}";
    }
}

public class Product
{
    private string _name;          
    private string _productId;     
    private double _price;         
    private int _quantity;         

    
    public Product(string name, string productId, double price, int quantity)
    {
        _name = name;
        _productId = productId;
        _price = price;
        _quantity = quantity;
    }

    
    public double GetTotalCost()
    {
        return _price * _quantity;
    }

    
    public string Name => _name;
    public string ProductId => _productId;
}

public class Customer
{
    private string _name;      
    private Address _address;   

   
    public Customer(string name, Address address)
    {
        _name = name;
        _address = address;
    }

   
    public bool IsInUSA()
    {
        return _address.IsInUSA();
    }

    
    public string Name => _name;
    public Address Address => _address;
}

public class Order
{
    private List<Product> _products;  
    private Customer _customer;        
    private const double ShippingCostUSA = 5.00;  
    private const double ShippingCostInternational = 35.00; 

    
    public Order(Customer customer)
    {
        _customer = customer;
        _products = new List<Product>();
    }

    
    public void AddProduct(Product product)
    {
        _products.Add(product);
    }

    
    public double CalculateTotalPrice()
    {
        double total = 0;

        foreach (Product product in _products)
        {
            total += product.GetTotalCost();  
        }

       
        if (_customer.IsInUSA())
        {
            total += ShippingCostUSA;
        }
        else
        {
            total += ShippingCostInternational;
        }

        return total;
    }

    
    public string GetPackingLabel()
    {
        string label = "Packing Label:\n";
        foreach (Product product in _products)
        {
            label += $"{product.Name} (ID: {product.ProductId})\n";
        }
        return label;
    }

    public string GetShippingLabel()
    {
        string label = "Shipping Label:\n";
        label += $"{_customer.Name}\n";
        label += $"{_customer.Address.GetFullAddress()}";
        return label;
    }
}

class Program
{
    static void Main()
    {
        
        Address address1 = new Address("123 Wood St", "Salt Lake City", "UT", "USA");
        Address address2 = new Address("456 Maple St", "Calgary", "AB", "Canada");

        Customer customer1 = new Customer("John Doe", address1);
        Customer customer2 = new Customer("Jane Smith", address2);

        Order order1 = new Order(customer1);
        Order order2 = new Order(customer2);

        Product product1 = new Product("Widget", "W001", 10.00, 2);
        Product product2 = new Product("Gadget", "G001", 15.00, 1);
        Product product3 = new Product("Thingamajig", "T001", 7.50, 3);
        Product product4 = new Product("Doodad", "D001", 20.00, 1);

        order1.AddProduct(product1);
        order1.AddProduct(product2);
        order2.AddProduct(product3);
        order2.AddProduct(product4);

        Console.WriteLine(order1.GetPackingLabel());
        Console.WriteLine(order1.GetShippingLabel());
        Console.WriteLine($"Total Price: ${order1.CalculateTotalPrice():F2}\n");

        Console.WriteLine(order2.GetPackingLabel());
        Console.WriteLine(order2.GetShippingLabel());
        Console.WriteLine($"Total Price: ${order2.CalculateTotalPrice():F2}");
    }
}

