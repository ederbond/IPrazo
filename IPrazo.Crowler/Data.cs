namespace IPrazo.Crowler;

// Model to hold extracted data
public class Data
{
    public string Ip { get; set; }
    public string Port { get; set; }
    public string Protocol { get; set; }

    public override string ToString()
    {
        return $"{Ip}:{Port} ({Protocol})";
    }
}
