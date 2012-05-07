using System;
using System.IO;
using Ploeh.AutoFixture;
using Vici.CoolStorage;

namespace ArtekSoftware.Conference.LocalData.Tests.Int
{
  public class TestBase
  {
    protected IFixture Fixture;

    public virtual void Setup()
    {
      Fixture = new Fixture().Customize(new MultipleCustomization());

      CreateDatabase();
    }

    private void CreateDatabase()
    {
      var personalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
      string dbName = Path.Combine(personalFolder, "mydb.db3");
      CSConfig.SetDB(new CSDataProviderSQLite("Data Source=" + dbName));

      CSDatabase.ExecuteNonQuery(SessionEntity.CreateTableSql);
      CSDatabase.ExecuteNonQuery(SpeakerEntity.CreateTableSql);
    }
  }
}