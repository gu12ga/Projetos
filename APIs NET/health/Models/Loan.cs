namespace health;

public class Loan
{
    public int idLoan { get; set; }
    public string type { get; set; }
    public int rate { get; set; }

    public virtual ICollection<LoanMatches> LoanMatches { get; set; }
}
