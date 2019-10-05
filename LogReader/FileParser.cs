using LogReader.Domain;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace LogReader
{
  public class FileParser
  {
    // worst case scenario this buffer will hold half of file content
    private readonly ConcurrentDictionary<string, long> _partialBuffer;
    private readonly BufferBlock<EventItem> _buffer;

    public FileParser(BufferBlock<EventItem> buffer)
    {
      _partialBuffer = new ConcurrentDictionary<string, long>();
      _buffer = buffer;
    }

    public async Task ProcessFile(string path)
    {
      while (IsFileLocked(path))
      {
        await Task.Delay(500);
      }
      var tasks = File.ReadLines(path)
        .Select(line => JsonConvert.DeserializeObject<FileEvent>(line))
        .Select(ProcessEvent);

      await Task.WhenAll(tasks);
      _buffer.Complete();
    }

    private async Task ProcessEvent(FileEvent item)
    {
      if (_partialBuffer.ContainsKey(item.Id))
      {
        _partialBuffer.Remove(item.Id, out var part);
        await _buffer.SendAsync(new EventItem(item, part));
      }
      else
      {
        _partialBuffer.TryAdd(item.Id, item.Timestamp);
      }
    }

    private bool IsFileLocked(string path)
    {
      FileStream stream = null;

      try
      {
        stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None);
      } catch (IOException)
      {
        return true;
      }
      finally
      {
        if (stream != null)
          stream.Close();
      }

      return false;
    }
  }
}
