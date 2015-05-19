using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Models
{
	public class MercuryField
	{
		public string Name { get; set; }
		public string DisplayName { get; set; }
		public bool IsPrimaryKey { get; set; }
		public MercuryFieldType Type { get; set; }
		public MercuryUiHint? UiHint { get; set; }

		public IEnumerable<MercuryValidationRule> ValidationRules { get; set; }
	}
}
