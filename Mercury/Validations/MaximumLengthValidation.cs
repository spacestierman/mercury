using Mercury.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Validations
{
	public class MaximumLengthValidation : MercuryValidationRule
	{
		public int MaximumLength { get; set; }

		override public void LoadFromSettings(MercurySettings settings)
		{
			MaximumLength = settings.GetAsInteger("MaximumLength");
		}
	}
}
