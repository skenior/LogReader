namespace LogReader.Domain
{
  public class FileEvent
  {
    public string Id { get; set; }
    public string State { get; set; }
    public string Type { get; set; }
    public string Host { get; set; }
    public long Timestamp { get; set; }
  }
}
