using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace LogReader.Database
{
  public interface IRepository<T>
  {
    void Save(T item);
  }
}