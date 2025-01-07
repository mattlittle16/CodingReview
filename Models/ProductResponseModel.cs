namespace CodingTest.Models;

public record struct ProductResponseModel(
    Guid Id, 
    string Name, 
    decimal Price,
    int QuantityRemaining
);