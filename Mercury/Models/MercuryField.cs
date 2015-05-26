using Mercury.Common;
using Mercury.Validations;
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

		public MercuryFieldReference Reference { get; set; }

		public bool IsLastField { get; internal set; }

		public bool IsRequired
		{
			get
			{
				return IsPrimaryKey || ValidationRules.Any(x => x is RequiredValidation);
			}
		}

		public string TypeForCSharp
		{
			get
			{
				switch (Type)
				{
					case MercuryFieldType.INTEGER:
						return "int";
					case MercuryFieldType.TEXT:
					case MercuryFieldType.VARCHAR:
						return "string";
					default:
						throw new FormattedException("Unable to determine field type for \"{0}\".", Type);
				}
			}
		}

		public string TypeForSql
		{
			get
			{
				switch (Type)
				{
					case MercuryFieldType.INTEGER:
						return "int";
					case MercuryFieldType.TEXT:
					case MercuryFieldType.VARCHAR:
						return "nvarchar(255)"; /// TODO: Check for maximum length
					default:
						throw new FormattedException("Unable to determine field type for \"{0}\".", Type);
				}
			}
		}

		public bool HasReference
		{
			get { return Reference != null; }
		}

		public bool IsNotLastField
		{
			get { return !IsLastField; }
		}
	}
}
