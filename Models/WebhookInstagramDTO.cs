public class IValue
{
    public string id { get; set; }
    public string text { get; set; }
}

public class WebhookInstagramDTO
{
    public string field { get; set; }
    public IValue value { get; set; }
}
 