namespace CodingTest.Models;

public record struct AddProductRequestModel(string Name, decimal Price, int QuantityRemaining);