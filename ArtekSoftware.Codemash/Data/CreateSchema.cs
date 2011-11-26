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
	public class CreateSchema : IDatabaseMigration
	{
		private readonly List<string> sqls = new List<string>
        {
            SessionEntity.CreateTableSql,
			SpeakerEntity.CreateTableSql,
			RefreshEntity.CreateTableSql,
			ScheduledSessionEntity.CreateTableSql,
			RemoteQueueEntity.CreateTableSql,
        };

		public string Name {
			get { return "create_schema"; }
		}

		public Action Action {
			get { return () => sqls.Select (x => new DbCommandSpec ().SetCommandText (x)).ToList ().ForEach (Execute); }
		}

		private void Execute (DbCommandSpec command)
		{
			UnitOfWork.Current.Session.ExecuteNonQuery (command);
		}
	}
}