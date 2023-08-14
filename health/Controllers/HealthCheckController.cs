namespace health;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("controller")]
public class HealthCheckController: Controller
{
[HttpPost(Name = "get")]
public IActionResult Get([FromBody] Customer customer)
{
    using (var context = new Context())
    {
        var query = from c in context.Customer
                    where c.cpf == customer.cpf
                    select c;

        if (!query.Any())
        {
            context.Customer.Add(customer);

            var loanMatche = new LoanMatches
            {
                idLoan = 1,
                cpf = customer.cpf
            };

            context.LoanMatches.Add(loanMatche);

            bool x = false;

            if (customer.income >= 5000 && customer.age <= 30)
            {
                x = true;
            }
            else if (customer.income < 5000 && customer.income > 3000 && customer.location.Equals("SP"))
            {
                x = true;
            }
            else if (customer.income <= 3000 && customer.location.Equals("SP") && customer.age <= 30)
            {
                x = true;
            }

            if (x)
            {
                var loanMatche2 = new LoanMatches
                {
                    idLoan = 2,
                    cpf = customer.cpf
                };

                context.LoanMatches.Add(loanMatche2);
            }

            if (customer.income > 5000)
            {
                var loanMatche3 = new LoanMatches
                {
                    idLoan = 3,
                    cpf = customer.cpf
                };

                context.LoanMatches.Add(loanMatche3);
            }

            context.SaveChanges(); // Salva as alterações no banco de dados
        }

        var query2 = from lm in context.LoanMatches
                     join l in context.Loan on lm.idLoan equals l.idLoan
                     where lm.cpf == customer.cpf
                     select new { l.type, l.rate };

        var result = new
        {
            customer = customer.name,
            loans = query2
        };

        return Ok(result);
    }
}

}
