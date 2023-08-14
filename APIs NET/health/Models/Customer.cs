namespace health;

public class Customer
{
    public string name { get; set; }

    public string cpf { get; set; }

    public int age { get; set; }

    public string location { get; set; }

    public int income { get; set; }

    public virtual ICollection<LoanMatches> LoanMatches { get; set; }

}
