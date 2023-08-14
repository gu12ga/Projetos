namespace StarWars;

public class CreditCard
{
    public string client_id { get; set; }
    public string card_number { get; set; }
    public string card_holder_name { get; set; }
    public int value { get; set; }
    public int cvv { get; set; }
    public string exp_date { get; set; }

    public virtual Cliente cliente { get; set; }
    
     public virtual ICollection<Transaction> transactions { get; set; }

}
