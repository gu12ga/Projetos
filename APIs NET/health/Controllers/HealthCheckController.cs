namespace health;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("controller")]
public class HealthCheckController : Controller
{
    private readonly Context _context;

    public HealthCheckController()
    {
        _context = new Context;
    }

    [HttpPost(Name = "get")]
    public IActionResult Get([FromBody] Customer customer)
    {
        var existingCustomer = _context.Customer.FirstOrDefault(c => c.Cpf == customer.Cpf);

        if (existingCustomer == null)
        {
            AddCustomerAndLoanMatches(customer);

            _context.SaveChanges();
        }

        var loansQuery = from lm in _context.LoanMatches
                         join l in _context.Loan on lm.IdLoan equals l.IdLoan
                         where lm.Cpf == customer.Cpf
                         select new { l.Type, l.Rate };

        var result = new
        {
            CustomerName = customer.Name,
            Loans = loansQuery
        };

        return Ok(result);
    }

    private void AddCustomerAndLoanMatches(Customer customer)
    {
        _context.Customer.Add(customer);
        _context.SaveChanges();

        AddLoanMatchesBasedOnCustomerProfile(customer);
    }

    private void AddLoanMatchesBasedOnCustomerProfile(Customer customer)
    {
        if (IsEligibleForPersonalLoan(customer))
        {
            AddLoanMatch(1, customer);
        }

        if (IsEligibleForGuaranteedLoan(customer))
        {
            AddLoanMatch(2, customer);
        }

        if (IsEligibleForConsolidatedLoan(customer))
        {
            AddLoanMatch(3, customer);
        }
    }

    private bool IsEligibleForPersonalLoan(Customer customer)
    {
        return customer.Income <= 3000;
    }

    private bool IsEligibleForGuaranteedLoan(Customer customer)
    {
        return customer.Income >= 3000 && customer.Income < 5000 && customer.Location.Equals("SP");
    }

    private bool IsEligibleForConsolidatedLoan(Customer customer)
    {
        return customer.Income >= 5000 || (customer.Age < 30 && customer.Location.Equals("SP"));
    }

    private void AddLoanMatch(int loanId, Customer customer)
    {
        _context.LoanMatches.Add(new LoanMatches
        {
            IdLoan = loanId,
            Cpf = customer.Cpf
        });
    }
}