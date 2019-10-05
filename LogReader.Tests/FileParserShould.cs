using LogReader.Domain;
using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace LogReader.Tests
{
  [TestFixture]
  public class FileParserShould
  {
    [TestCase(@"Samples\logfile.txt")]
    public async Task fill_buffer_with_data(string path)
    {
      var buffer = new BufferBlock<EventItem>();
      var parser = new FileParser(buffer);

      await parser.ProcessFile(path);

      Assert.That(buffer.Count, Is.Not.EqualTo(0));
    }

    [TestCase(@"Samples\logfile.txt")]
    public async Task result_is_half_the_size_of_file(string path)
    {
      var buffer = new BufferBlock<EventItem>();
      var parser = new FileParser(buffer);

      await parser.ProcessFile(path);

      var lines = File.ReadLines(path).Count();
      Assert.That(buffer.Count, Is.EqualTo(lines / 2));
    }
  }
}
