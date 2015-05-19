using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Models
{
	public class MercuryEntity
	{
		public string Name { get; set; }
		public string DisplayName { get; set; }
		public IEnumerable<MercuryField> Fields { get; set; }
	}
}
