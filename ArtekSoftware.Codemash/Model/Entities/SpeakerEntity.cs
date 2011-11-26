using System;
using Catnap.Common.Logging;
using Catnap.Database;
using Catnap.Find;
using Catnap.Maps;
using Catnap.Adapters;
using Catnap.Maps.Impl;
using Catnap;
using Catnap.Migration;
using System.Collections.Generic;
using System.Linq;

namespace ArtekSoftware.Codemash
{
	public class SpeakerEntity : Entity
	{
		public static string TableName = "Speakers";
		public static string CreateTableSql = @"CREATE TABLE " + TableName + " (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, SpeakerURI VARCHAR, Name VARCHAR, Biography VARCHAR, TwitterHandle VARCHAR, BlogURL VARCHAR)";
		
		public string SpeakerURI { get; set; }

		public string Name { get; set; }

		public string Biography { get; set; }

		public string TwitterHandle { get; set; }

		public string BlogURL { get; set; }

	}
}