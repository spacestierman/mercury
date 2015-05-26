using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Models
{
	public class MercuryEntityFieldJson
	{
		public string Name { get; set; }
		public string DisplayName { get; set; }
		public bool IsPrimaryKey { get; set; }
		public string Type { get; set; }
		public string UiHint { get; set; }

		public IEnumerable<MercuryValidationRuleJson> Validation { get; set; }
		
		public MercuryFieldReferenceJson Reference { get; set; }
		public bool HasReference
		{
			get { return Reference != null; }
		}
	}
}
