using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Core
{
	public interface IBuildStep
	{
		bool ConflictsWith(IBuildStep buildStep);
		IBuildStep MergeWith(IEnumerable<IBuildStep> otherSteps);
		void Execute(FileContentsProvider outputProvider);
	}
}
