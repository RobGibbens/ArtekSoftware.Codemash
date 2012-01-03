using System;
using Catnap;

namespace ArtekSoftware.Codemash
{
	public class DatabaseMigrationBootstrapper
	{
		public void Migrate ()
		{
			using (UnitOfWork.Start()) {
				DatabaseMigrator.Execute ();
			}		
		}
	}
}

