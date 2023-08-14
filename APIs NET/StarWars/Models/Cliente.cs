namespace StarWars;

public class Cliente
{
    public string client_id { get; set; }
    public string client_name{ get; set; }

    public virtual ICollection<CreditCard> creditcards { get; set; }

    public virtual ICollection<Transaction> transactions { get; set; }

}
