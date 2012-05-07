using System;
using Vici.CoolStorage;

namespace ArtekSoftware.Conference.LocalData
{
	[MapTo("scheduledSession")]
	public class ScheduledSessionEntity :  CSObject<ScheduledSessionEntity, string>
	{
		public static string TableName = "scheduledSession";
		public static string CreateTableSql = @"CREATE TABLE IF NOT EXISTS " + TableName +
				" ("
				+ "Id VARCHAR PRIMARY KEY NOT NULL, "
				+ "Slug VARCHAR, " 
				+ "ConferenceSlug VARCHAR, "
				+ "Abstract VARCHAR, "
				+ "Difficulty VARCHAR, "
				+ "TwitterHashTag VARCHAR, "
				+ "Title VARCHAR, "
				+ "Start VARCHAR, "
				+ "End VARCHAR, "
				+ "Room VARCHAR"
				+ ")";

		public string Id { get { return (string)GetField ("Id"); } set { SetField (
					"Id",
					value
				); } }

		public string Slug  { get { return (string)GetField ("Slug"); } set { SetField (
					"Slug",
					value
				); } }

		public string ConferenceSlug { get { return (string)GetField ("ConferenceSlug"); } set { SetField (
					"ConferenceSlug",
					value
				); } }

		public string Abstract { get { return (string)GetField ("Abstract"); } set { SetField (
					"Abstract",
					value
				); } }

		public string Difficulty { get { return (string)GetField ("Difficulty"); } set { SetField (
					"Difficulty",
					value
				); } }

		public string TwitterHashTag { get { return (string)GetField ("TwitterHashTag"); } set { SetField (
					"TwitterHashTag",
					value
				); } }

		public string Title { get { return (string)GetField ("Title"); } set { SetField (
					"Title",
					value
				); } }

		public DateTime Start { get { return (DateTime)GetField ("Start"); } set { SetField (
					"Start",
					value
				); } }

		public DateTime End { get { return (DateTime)GetField ("End"); } set { SetField (
					"End",
					value
				); } }

		public string Room { get { return (string)GetField ("Room"); } set { SetField (
					"Room",
					value
				); } }
	}
}

