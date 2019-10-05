using LogReader.Database;
using LogReader.Domain;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace LogReader
{
  class Program
  {
    static void Main(string[] args)
    {
      //if (args.Length != 1)
      //{
      //  Console.WriteLine("Incorrect parameters. App requires path to logfile.txt");
      //  Environment.Exit(-1);
      //}
      //var path = args[0];
      //if (!File.Exists(path))
      //{
      //  Console.WriteLine("File was not found.");
      //  Environment.Exit(-1);
      //}
      //var info = new FileInfo(path);
      //if(info.Name != "logfile.txt")
      //{
      //  Console.WriteLine("File is not logfile.txt");
      //  Environment.Exit(-1);
      //}

      var path = @"C:\Users\skeni\source\repos\LogReader\LogReader.Tests\Samples\logfile.txt";

      var dbSave = new DbSaver<EventItem>(new LiteRepository<EventItem>());
      var toSave = new BufferBlock<EventItem>();
      var parser = new FileParser(toSave);
      var consumer = dbSave.Consume(toSave);
      var process = parser.ProcessFile(path);

      Task.WaitAll(process, toSave.Completion, consumer);

      Console.WriteLine("Done.");
    }
  }
}
