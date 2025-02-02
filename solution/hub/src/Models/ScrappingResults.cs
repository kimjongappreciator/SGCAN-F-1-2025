namespace hub.Models;

public class ScrappingResults
{
    public int id { get; set; }
    public string task_id { get; set; }
    public string url { get; set; }
    public string title { get; set; }
    public string content { get; set; }
    public string date { get; set; }
}