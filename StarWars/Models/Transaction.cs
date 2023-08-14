namespace StarWars;

public class Transaction
{
    public string client_id { get; set; }
    public string purchase_id { get; set; }
    public int total_to_pay { get; set; }
    public string card_number { get; set; }
    public string date { get; set; }
    public virtual Cliente cliente { get; set; }
    public virtual CreditCard creditcard { get; set; }

}
