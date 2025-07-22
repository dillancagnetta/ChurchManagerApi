using System.ComponentModel.DataAnnotations;
using Codeboss.Types;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Domain.Common;

public class Currency : Enumeration<Currency, string>
{
    public Currency() { Value = "ZAR"; }
        
    public Currency(string value) => Value = value;

    public static Currency ZAR = new("ZAR");
    public static Currency USD = new("USD");
    public static Currency ESPEES = new("ESPEES");
    // Implicit conversion from string
    public static implicit operator Currency(string value) => new(value);
}

public record Money
{
    [MaxLength(3)] public string? Currency { get; set; }
    [Precision(18, 2)] public decimal Amount { get; set; }

    // ORM required
    private Money(){}

    public Money(Currency currency, decimal amount)
    {
        Currency = currency.Value;
        Amount = amount <= 0 ? 0 : amount;
    }
    
    public Money(Money money)
    {
        if (money is null) throw new ArgumentNullException(nameof(money));

        Currency = money.Currency;
        Amount = money.Amount <= 0 ? 0 : money.Amount;
    }
        
    public override string ToString() => $"{Currency}|{Amount}";

    public static Money Parse(string value)
    {
        var parts = value.Split('|');
        return new Money(parts[0], decimal.Parse(parts[1]));
    }
}