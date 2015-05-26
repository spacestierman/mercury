using Mercury.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Models
{
	public class MercuryFieldReference
	{
		public MercuryEntity Entity { get; private set; }
		public MercuryField Display { get; private set; }

		public MercuryFieldReference(MercuryEntity entity, MercuryField display)
		{
			if (entity == null)
			{
				throw new ArgumentRequiredException("entity");
			}
			Entity = entity;

			if (display == null)
			{
				throw new ArgumentRequiredException("display");
			}
			Display = display;
		}
	}
}
