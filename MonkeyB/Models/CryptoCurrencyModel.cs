
public class CryptoCurrencyModel
{
    public Bitcoin bitcoin { get; set; }
    public Dogecoin dogecoin { get; set; }
    public Dogecoin litecoin { get; set; }
    public Ethereum ethereum { get; set; }
}

public class Bitcoin
{
    public float eur { get; set; }
}

public class Dogecoin
{
    public float eur { get; set; }
}

public class Litecoin
{
    public float eur { get; set; }
}

public class Ethereum
{
    public float ethereum { get; set; }
}