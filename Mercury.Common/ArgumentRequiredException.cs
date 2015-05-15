using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Common
{
    public class ArgumentRequiredException : Exception
    {
		public ArgumentRequiredException(string fieldName)
			: base(string.Format("{0} is required", fieldName))
		{

		}
    }
}
