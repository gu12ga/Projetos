using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;


namespace StarWars;

[ApiController]
[Route("starstore")]
public class StarStoreController : Controller
{
    private readonly Context _context;

    public StarStoreController()
    {
        _context = new Context;
    }

    [HttpGet("products", Name = "products")]
    public IActionResult GetProducts()
    {
        var products = from produto in _context.Produto
                       select new
                       {
                           title = produto.Title,
                           price = produto.Price,
                           zipCode = produto.ZipCode,
                           seller = produto.Seller,
                           thumbnailHd = produto.ThumbnailHd,
                           date = produto.Date
                       };

        return Ok(products.ToList());
    }

    [HttpPost("product", Name = "product")]
    public async Task<IActionResult> CreateProduct([FromBody] Produto produto)
    {
        if (produto == null)
        {
            return BadRequest("Invalid input");
        }

        _context.Produto.Add(produto);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("buy", Name = "buy")]
    public async Task<IActionResult> Buy([FromBody] JsonElement input)
    {
        string clientId = input.GetProperty("client_id").GetString();
        string cardNumber = input.GetProperty("credit_card").GetProperty("card_number").GetString();

        await EnsureClientExistsAsync(clientId, input);

        await EnsureCreditCardExistsAsync(clientId, cardNumber, input);

        await AddTransactionAsync(clientId, cardNumber, input);

        await _context.SaveChangesAsync();

        return Ok();
    }

    private async Task EnsureClientExistsAsync(string clientId, JsonElement input)
    {
        var query = from cliente in _context.Cliente
                    where cliente.client_id == clientId
                    select cliente.client_id;

        if (!query.Any())
        {
            var cliente = new Cliente
            {
                client_id = clientId,
                client_name = input.GetProperty("client_name").GetString()
            };

            _context.Cliente.Add(cliente);
        }
    }

    private async Task EnsureCreditCardExistsAsync(string clientId, string cardNumber, JsonElement input)
    {
        var query = from creditCard in _context.CreditCart
                    where creditCard.client_id == clientId && creditCard.card_number == cardNumber
                    select creditCard.client_id;

        if (!query.Any())
        {
            var credit = new CreditCard
            {
                client_id = clientId,
                card_number = cardNumber,
                card_holder_name = input.GetProperty("credit_card").GetProperty("card_holder_name").GetString(),
                value = input.GetProperty("credit_card").GetProperty("value").GetInt32(),
                cvv = input.GetProperty("credit_card").GetProperty("cvv").GetInt32(),
                exp_date = input.GetProperty("credit_card").GetProperty("exp_date").GetString()
            };

            _context.CreditCart.Add(credit);
        }
    }

    private async Task AddTransactionAsync(string clientId, string cardNumber, JsonElement input)
    {
        var purchaseId = Guid.NewGuid().ToString("D");

        var transaction = new Transaction
        {
            purchase_id = purchaseId,
            client_id = clientId,
            total_to_pay = input.GetProperty("total_to_pay").GetInt32(),
            card_number = cardNumber,
            date = DateTime.Now.ToString("dd/MM/yyyy")
        };

        _context.Transaction.Add(transaction);
    }

   [HttpGet("history", Name = "history")]
    public IActionResult GetPurchaseHistory()
    {
        var query = from transaction in _context.Transaction
                    join card in _context.CreditCart on transaction.CardNumber equals card.CardNumber
                    select new
                    {
                        transaction.ClientId,
                        transaction.PurchaseId,
                        card.Value,
                        transaction.Date,
                        CardNumberLastDigits = "**** **** **** " + card.CardNumber.Substring(12)
                    };

        var modifiedResults = query.ToList();

        return Ok(modifiedResults);
    }

    [HttpGet("history/{clientId}", Name = "historyId")]
    public IActionResult GetClientPurchaseHistory(string clientId)
    {
        var query = from transaction in _context.Transaction
                    join card in _context.CreditCart on transaction.CardNumber equals card.CardNumber
                    where transaction.ClientId == clientId
                    select new
                    {
                        transaction.ClientId,
                        transaction.PurchaseId,
                        card.Value,
                        transaction.Date,
                        CardNumberLastDigits = "**** **** **** " + card.CardNumber.Substring(12)
                    };

        var modifiedResults = query.ToList();

        return Ok(modifiedResults);
    }
}
