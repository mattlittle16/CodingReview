using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using CodingTest.Configuration;
using CodingTest.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CodingTest.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly EnvironmentSettings _settings;
    private readonly IValidator<AddProductRequestModel> _addProductValidator;

    public ProductController(IValidator<AddProductRequestModel> addProductValidator)
    {
        _settings = new EnvironmentSettings();
        _addProductValidator = addProductValidator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        using (var client = new HttpClient())
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_settings.ProductApiUrl}/products");
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", _settings.ProductApiKey);

                var response = await client.SendAsync(request);
                var product = JsonSerializer.Deserialize<IList<ProductResponseModel>>(await response.Content.ReadAsStringAsync());
                return Ok(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
                return BadRequest(ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
        }
    }

    [HttpPost]
    [Route("updateproduct/id/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateProduct(Guid Productid)
    {
        using (var client = new HttpClient())
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_settings.ProductApiUrl}/upateproduct/{Productid}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", _settings.ProductApiKey);

                Console.WriteLine($"Updating product {Productid}");

                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
                return BadRequest(ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
        }
    }

    [HttpPost]
    [Route("addproduct}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddProuct(AddProductRequestModel requestModel)
    {
        var validatorResult = await _addProductValidator.ValidateAsync(requestModel);
        if (!validatorResult.IsValid)
        {
            return BadRequest();
        }

        using (var client = new HttpClient())
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_settings.ProductApiUrl}/addproduct");
                request.Content = new StringContent(JsonSerializer.Serialize(requestModel), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", _settings.ProductApiKey);

                Console.WriteLine($"Adding product {requestModel}");

                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
                return BadRequest(ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
        }
    }
}
