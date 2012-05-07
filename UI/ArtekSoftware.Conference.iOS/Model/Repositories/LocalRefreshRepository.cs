//using System.Linq;
//using Catnap;
//using Catnap.Find;
//using Catnap.Find.Conditions;
//
//namespace ArtekSoftware.Codemash
//{
//	public class LocalRefreshRepository: Repository<RefreshEntity>
//	{
//		public RefreshEntity GetSession ()
//		{
//			RefreshEntity refresh;
//			var criteria = new Criteria ();
//			criteria.Add (Condition.Equal<RefreshEntity> (x => x.EntityName, "Session"));
//			var refreshs = this.Find (criteria);
//			if (refreshs != null && refreshs.Count() > 0) {
//				refresh = refreshs.FirstOrDefault ();
//			}
//			else
//			{
//				refresh = new RefreshEntity () { EntityName = "Session" };
//			}
//			
//			return refresh;
//		}
//	}
//}
//
