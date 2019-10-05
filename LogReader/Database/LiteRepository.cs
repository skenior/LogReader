using LiteDB;

namespace LogReader.Database
{
  public class LiteRepository<T> : IRepository<T>
  {
    private readonly string _fileName;

    public LiteRepository(string fileName = "data.db")
    {
      _fileName = fileName;
    }

    public void Save(T item)
    {
      using (var db = new LiteDatabase(_fileName))
      {
        var collection = db.GetCollection<T>();
        collection.Insert(item);
      }
    }
  }
}
