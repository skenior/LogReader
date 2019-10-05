using LiteDB;

namespace LogReader.Database
{
  public class LiteRepository<T> : IRepository<T>
  {
    public void Save(T item)
    {
      using (var db = new LiteDatabase("data.db"))
      {
        var collection = db.GetCollection<T>();
        collection.Insert(item);
      }
    }
  }
}
