using Mercury.Core;
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

		public MercuryEntity()
		{
			Fields = new List<MercuryField>();
		}

		public string NameSingular
		{
			get { return Pluralizer.MakeSingular(Name); }
		}

		public string NamePlural
		{
			get { return Pluralizer.MakePlural(Name); }
		}

		public MercuryField PrimaryKey
		{
			get
			{
				return Fields.First(x => x.IsPrimaryKey);
			}
		}

		public bool HasPrimaryKey
		{
			get
			{
				return Fields.Any(x => x.IsPrimaryKey);
			}
		}

		public bool HasMultiplePrimaryKeys
		{
			get
			{
				return Fields.Count(x => x.IsPrimaryKey) > 1;
			}
		}

		public bool HasAnyFieldsThatNeedReferences
		{
			get
			{
				return Fields.Any(x => x.HasReference);
			}
		}
	}
}
