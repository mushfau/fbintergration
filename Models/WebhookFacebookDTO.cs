public class Post
{
    public string status_type { get; set; }
    public bool is_published { get; set; }
    public string updated_time { get; set; }
    public string permalink_url { get; set; }
    public string promotion_status { get; set; }
    public string id { get; set; }
}


public class From
{
    public string id { get; set; }
    public string name { get; set; }
}

public class FValue
{
    public From from { get; set; }
    // public Post post { get; set; }
    public string message { get; set; }
    public string post_id { get; set; }
    public string comment_id { get; set; }
    public int created_time { get; set; }
    public string item { get; set; }
    public string parent_id { get; set; }
    public string verb { get; set; }
}
public class Change
{

    public FValue value { get; set; }

    public string field { get; set; }

}

public class Entry
{
    public string id { get; set; }
    public int time { get; set; }
    public Change[] changes { get; set; }
}

public class WebhookFacebookDTO
{
    public Entry[] entry { get; set; }

}