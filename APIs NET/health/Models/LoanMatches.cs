namespace health;

public class LoanMatches
{
    public int idLoan { get; set; }

    public string cpf { get; set; }

    public virtual Customer Customer { get; set; }
    public virtual Loan Loan { get; set; }

}
