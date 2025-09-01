﻿namespace Catalog.Domain.Entity;

public class Product
{
    public string Id { get; set; } = null;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}