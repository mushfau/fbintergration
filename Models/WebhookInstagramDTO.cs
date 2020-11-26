// public class IValue
// {
//     public string id { get; set; }
//     public string text { get; set; }
// }

// public class WebhookInstagramDTO
// {
//     public string field { get; set; }
//     public IValue value { get; set; }
// }

public class IFValue
{
    public string id { get; set; }
    public string text { get; set; }
}
public class IChange
{

    public IFValue value { get; set; }

    public string field { get; set; }

}

public class IEntry
{
    public string id { get; set; }
    public int time { get; set; }
    public IChange[] changes { get; set; }
}

public class WebhookInstagramDTO
{
    public IEntry[] entry { get; set; }

}