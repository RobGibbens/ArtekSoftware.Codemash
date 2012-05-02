using System;
using System.Collections.Generic;
using System.Linq;
using Catnap;
using Catnap.Database;
using Catnap.Migration;

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