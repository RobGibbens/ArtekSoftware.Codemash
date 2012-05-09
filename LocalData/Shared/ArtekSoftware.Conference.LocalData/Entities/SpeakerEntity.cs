using Vici.CoolStorage;

namespace ArtekSoftware.Conference.LocalData
{
  [MapTo("speaker")]
  public class SpeakerEntity : CSObject<SpeakerEntity, string>
  {
    public static string TableName = "speaker";

    public static string CreateTableSql = @"CREATE TABLE IF NOT EXISTS " + TableName +
                                          " ("
                                          //+ "Id VARCHAR PRIMARY KEY NOT NULL UNIQUE, "
                                          + "Slug VARCHAR PRIMARY KEY NOT NULL UNIQUE, "
                                          + "ConferenceSlug VARCHAR, "
                                          + "TwitterName VARCHAR, "
                                          + "BlogUrl VARCHAR, "
                                          + "CompanyName VARCHAR, "
                                          + "LinkedInUrl VARCHAR, "
                                          + "FacebookUrl VARCHAR, "
                                          + "Name VARCHAR, "
                                          + "FirstName VARCHAR, "
                                          + "LastName VARCHAR, "
                                          + "Description VARCHAR"
                                          + ")";

    //public string Id { get { return (string)GetField("Id"); } set { SetField("Id", value); } }
    public string Slug { get { return (string)GetField("Slug"); } set { SetField("Slug", value); } }
    public string ConferenceSlug { get { return (string)GetField("ConferenceSlug"); } set { SetField("ConferenceSlug", value); } }
    public string TwitterName { get { return (string)GetField("TwitterName"); } set { SetField("TwitterName", value); } }
    public string BlogUrl { get { return (string)GetField("BlogUrl"); } set { SetField("BlogUrl", value); } }
    public string CompanyName { get { return (string)GetField("CompanyName"); } set { SetField("CompanyName", value); } }
    public string LinkedInUrl { get { return (string)GetField("LinkedInUrl"); } set { SetField("LinkedInUrl", value); } }
    public string FacebookUrl { get { return (string)GetField("FacebookUrl"); } set { SetField("FacebookUrl", value); } }
    public string Name { get { return (string)GetField("Name"); } protected internal set { SetField("Name", value); } }
    public string FirstName { get { return (string)GetField("FirstName"); } set { SetField("FirstName", value); } }
    public string LastName { get { return (string)GetField("LastName"); } set { SetField("LastName", value); } }
    public string Description { get { return (string)GetField("Description"); } set { SetField("Description", value); } }
    //public IList<Guid> SessionsIds  { get { return (string)GetField("Slug"); } set { SetField("Slug", value); } }
  }
}