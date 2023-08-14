using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;


namespace StarWars;

[ApiController]
[Route("starstore")]
public class StarStore : Controller
{
     [HttpGet("products", Name = "products")]
    public IActionResult Get()
    {

       using (var context = new Context())
        {
           var produtos =  from produto in context.Produto
                           select new{title = produto.Title, price = produto.Price, zipCode = produto.ZipCode, seller = produto.Seller, thumbnailHd = produto.ThumbnailHd, date = produto.Date};
           
           return Ok(produtos.ToList()); 
                
        }  
        
    }

    [HttpPost("product", Name = "product")]
    public async Task<IActionResult> Post([FromBody] Produto produto)
    {
            if (produto == null)
            {
                return BadRequest("Invalid input");
            }
        
         var newProduct = new Produto
            {
                Title = produto.Title,
                Price = produto.Price,
                ZipCode = produto.ZipCode,
                Seller = produto.Seller,
                ThumbnailHd = produto.ThumbnailHd,
                Date = produto.Date
            };

        using (var context = new Context())
        {
           context.Produto.Add(newProduct);
           await context.SaveChangesAsync();    
        }
        
        return Ok();
    }

    [HttpPost("buy", Name = "buy")]
public async Task<IActionResult> Buy([FromBody] JsonElement input)
{  

    string client_id2 = input.GetProperty("client_id").GetString();
    string card_number2 = input.GetProperty("credit_card").GetProperty("card_number").GetString();
    
    
    using (var context = new Context())
    {
        var query = from cliente in context.Cliente
                    where cliente.client_id == client_id2
                    select cliente.client_id;

        if (!query.Any())
        {
            var cliente = new Cliente
            {
                client_id = client_id2,
                client_name = input.GetProperty("client_name").GetString()
            };

            context.Cliente.Add(cliente);
        }

        var query2 = from creditCart in context.CreditCart
                     where creditCart.client_id == client_id2 && creditCart.card_number == card_number2
                     select creditCart.client_id;

        if (!query2.Any())
        {
            var credit = new CreditCard
            {
                client_id = client_id2,
                card_number = card_number2,
                card_holder_name = input.GetProperty("credit_card").GetProperty("card_holder_name").GetString(),
                value =  input.GetProperty("credit_card").GetProperty("value").GetInt32(),
                cvv = input.GetProperty("credit_card").GetProperty("cvv").GetInt32(),
                exp_date = input.GetProperty("credit_card").GetProperty("exp_date").GetString()
            };

            context.CreditCart.Add(credit);
        }

        var purchase_id2 = Guid.NewGuid().ToString("D");

        var transaction = new Transaction
        {
            purchase_id = purchase_id2,
            client_id = client_id2,
            total_to_pay = input.GetProperty("total_to_pay").GetInt32(),
            card_number = card_number2,
            date = DateTime.Now.ToString("dd/MM/yyyy")
        };

        context.Transaction.Add(transaction);
        await context.SaveChangesAsync();

        return Ok();
    }
}


   [HttpGet("history", Name = "history")]
    public IActionResult GetHistory()
    {
using (var context = new Context())
        {
           var query = from transaction in context.Transaction join card in context.CreditCart on transaction.card_number equals card.card_number
                       select new {transaction.client_id, transaction.purchase_id, card.value, transaction.date, card.card_number};

            List<object> modifiedResults = new List<object>();

            foreach (var result in query)
            {
               var modifiedCardNumber = "**** **** **** " + result.card_number[12] + result.card_number[13] + result.card_number[14] + result.card_number[15];

               var modifiedResult = new
               {
                  result.client_id,
                  result.purchase_id,
                  result.value,
                  result.date,
                  card_number = modifiedCardNumber
               };

               modifiedResults.Add(modifiedResult);
            }

            return Ok(modifiedResults);    
        }
        
    }

      [HttpGet("history/{clientId}", Name = "historyId")]
    public IActionResult GetHistoryCliente(string clientId)
    {
       using (var context = new Context())
        {
           var query = from transaction in context.Transaction join card in context.CreditCart on transaction.card_number equals card.card_number
                       where transaction.client_id == clientId
                       select new {transaction.client_id, transaction.purchase_id, card.value, transaction.date, card.card_number};

            List<object> modifiedResults = new List<object>();

            foreach (var result in query)
            {
               var modifiedCardNumber = "**** **** **** " + result.card_number[12] + result.card_number[13] + result.card_number[14] + result.card_number[15];

               var modifiedResult = new
               {
                  result.client_id,
                  result.purchase_id,
                  result.value,
                  result.date,
                  card_number = modifiedCardNumber
               };

               modifiedResults.Add(modifiedResult);
            }

            return Ok(modifiedResults);    
        }
        
    }


}
