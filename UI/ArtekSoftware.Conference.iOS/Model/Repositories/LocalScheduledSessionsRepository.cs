using Catnap;
using Catnap.Database;
using Catnap.Find;
using Catnap.Find.Conditions;
using System.Linq;

namespace ArtekSoftware.Codemash
{
	public class LocalScheduledSessionsRepository : Repository<ScheduledSessionEntity>
	{
		public int Count ()
		{
			var command = new DbCommandSpec ()
    			.SetCommandText ("select count(*) from " + ScheduledSessionEntity.TableName);
			
			object possibleCount = UnitOfWork.Current.Session.ExecuteScalar (command);
			
			int count = 0;
			int.TryParse (possibleCount.ToString (), out count);
			
			return count;
		}
	
		public ScheduledSessionEntity GetScheduledSession (string uri)
		{
			ScheduledSessionEntity session = null;
			var criteria = new Criteria ();
			criteria.Add (Condition.Equal<ScheduledSessionEntity> (x => x.URI, uri));
			var sessions = this.Find (criteria);
			
			if (sessions != null && sessions.Any()) {
				session = sessions.First ();
			}
				
			return session;
		}
	}
	
	
}

