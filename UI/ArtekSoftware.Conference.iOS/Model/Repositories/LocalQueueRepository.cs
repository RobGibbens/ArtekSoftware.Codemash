//using Catnap;
//using Catnap.Database;
//
//namespace ArtekSoftware.Codemash
//{
//	public class LocalQueueRepository : Repository<RemoteQueueEntity>
//	{
//		public int Count ()
//		{
//			var command = new DbCommandSpec ()
//    			.SetCommandText ("select count(*) from " + RemoteQueueEntity.TableName);
//			
//			object possibleCount = UnitOfWork.Current.Session.ExecuteScalar (command);
//			
//			int count = 0;
//			int.TryParse (possibleCount.ToString (), out count);
//			
//			return count;
//		}
//	}
//}
//
