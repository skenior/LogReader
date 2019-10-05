using System;

namespace LogReader.Domain
{
  public class EventItem
  {
    public string Id { get; }
    public long Duration { get; }
    public string Type { get; }
    public string Host { get; }
    public bool Alert { get; }
    public EventItem(FileEvent instance, long otherTimestamp)
    {
      Id = instance.Id;
      Type = instance.Type;
      Host = instance.Host;
      Duration = Math.Abs(instance.Timestamp - otherTimestamp);
      Alert = Duration > 4;
    }
  }
}
