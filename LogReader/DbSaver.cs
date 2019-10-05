using LogReader.Database;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace LogReader
{
  public class DbSaver<T>
  {
    private readonly IRepository<T> _repository;

    public DbSaver(IRepository<T> repository)
    {
      _repository = repository;
    }

    public async Task Consume(BufferBlock<T> queue)
    {
      while (await queue.OutputAvailableAsync())
      {
        _repository.Save(await queue.ReceiveAsync());
      }

    }
  }
}
