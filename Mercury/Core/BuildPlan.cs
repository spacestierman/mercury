using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Core
{
	public class BuildPlan
	{
		private List<IBuildStep> _steps;

		public BuildPlan()
		{
			_steps = new List<IBuildStep>();
		}

		public void Add(IBuildStep step)
		{
			_steps.Add(step);
		}

		public void Merge(BuildPlan otherPlan)
		{
			List<IBuildStep> removeConflictedSteps = new List<IBuildStep>();
			foreach (IBuildStep step in otherPlan._steps)
			{
				IEnumerable<IBuildStep> conflicts = FindConflicts(step);
				if (!conflicts.Any())
				{
					Add(step);
				}
				else
				{
					IBuildStep mergedStep = step.MergeWith(conflicts);
					Add(mergedStep);
					removeConflictedSteps.AddRange(conflicts);
				}
			}

			// Since the conflicts are "resolved" by adding a new merged BuildStep, we need to remove the old conflicted steps so that we're not doubled-up.
			foreach (IBuildStep conflict in removeConflictedSteps)
			{
				_steps.Remove(conflict);
			}
		}

		public void Execute(FileContentsProvider outputProvider)
		{
			foreach (IBuildStep step in _steps)
			{
				step.Execute(outputProvider);
			}
		}

		private IEnumerable<IBuildStep> FindConflicts(IBuildStep step)
		{
			List<IBuildStep> conflicts = new List<IBuildStep>();
			foreach (IBuildStep existingStep in _steps)
			{
				if (existingStep.ConflictsWith(step))
				{
					conflicts.Add(existingStep);
				}
			}
			return conflicts;
		}
	}
}
