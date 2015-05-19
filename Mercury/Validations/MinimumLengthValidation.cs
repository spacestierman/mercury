using Mercury.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Validations
{
	public class MinimumLengthValidation : MercuryValidationRule
	{
		public int MinimumLength { get; set; }

		override public void LoadFromSettings(MercurySettings settings)
		{
			MinimumLength = settings.GetAsInteger("MinimumLength");
		}
	}
}
