using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Common
{
	public class ShouldNeverHappenException : Exception
	{
		public ShouldNeverHappenException(string message) : base(message)
		{

		}
	}
}
