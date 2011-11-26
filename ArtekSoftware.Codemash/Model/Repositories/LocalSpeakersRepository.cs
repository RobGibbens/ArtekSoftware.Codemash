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
using Catnap.Find.Conditions;

namespace ArtekSoftware.Codemash
{
	public class LocalSpeakersRepository : Repository<SpeakerEntity>
	{
		public int Count ()
		{
			var command = new DbCommandSpec ()
    			.SetCommandText ("select count(*) from Speakers");
			
			object possibleCount = UnitOfWork.Current.Session.ExecuteScalar (command);
			
			int count = 0;
			int.TryParse (possibleCount.ToString (), out count);
			
			return count;
		}
		
		public SpeakerEntity FindByName (string name)
		{
			var criteria = new Criteria ();
			criteria.Add (Condition.Equal<SpeakerEntity> (x => x.Name, name));
			var speaker = this.Find (criteria).SingleOrDefault();
			
			return speaker;
		}
		
		public void Cache (IList<Speaker> dtos)
		{
			int count = this.Count ();
			
			if (count > 0) {
				var allCurrentEntities = this.Find ();
			
				foreach (var entity in allCurrentEntities) {
					this.Delete (entity.Id);
				}	
			}
			
			foreach (var dto in dtos) {
				var entity = new SpeakerEntity ()
				{
					Biography = dto.Biography,
					BlogURL = dto.BlogURL,
					Name = dto.Name,
				//Sessions = dto.Sessions,
					SpeakerURI = dto.SpeakerURI,
					TwitterHandle = dto.TwitterHandle,
				};
				
				this.Save (entity);
			}
			
		}
		
	}
}

