using System;
using System.Linq;
using Catnap;
using System.Collections.Generic;
using Catnap.Common.Logging;
using Catnap.Database;
using Catnap.Find;
using Catnap.Maps;
using Catnap.Adapters;
using Catnap.Maps.Impl;
using Catnap.Migration;
using System.Threading;

namespace ArtekSoftware.Codemash
{
	public class LocalQueueRepository : Repository<RemoteQueueEntity>
	{
		public int Count ()
		{
			var command = new DbCommandSpec ()
    			.SetCommandText ("select count(*) from " + RemoteQueueEntity.TableName);
			
			object possibleCount = UnitOfWork.Current.Session.ExecuteScalar (command);
			
			int count = 0;
			int.TryParse (possibleCount.ToString (), out count);
			
			return count;
		}
	}
}

