using System;
using System.Collections.Generic;

using Microsoft.Build.Utilities;

using nanoFramework.MSBuildTasks.Utils;

namespace nanoFramework.MSBuildTasks.Pipelines
{
    internal sealed class PipelineRunner<TContext> : IPipelineRunner<TContext>
        where TContext : class
    {
        private readonly IEnumerable<IPipelineStep<TContext>> _pipelineSteps;
        private readonly TaskLoggingHelper _taskLoggingHelper;

        public PipelineRunner(
            IEnumerable<IPipelineStep<TContext>> pipelineSteps,
            TaskLoggingHelper taskLoggingHelper)
        {
            _pipelineSteps = ParamChecker.Check(pipelineSteps, nameof(pipelineSteps));
            _taskLoggingHelper = ParamChecker.Check(taskLoggingHelper, nameof(taskLoggingHelper));
        }

        public bool Run(TContext context)
        {
            foreach (var step in _pipelineSteps)
            {
                if (_taskLoggingHelper.HasLoggedErrors)
                {
                    return false;
                }

                step.Handle(context);
            }

            return !_taskLoggingHelper.HasLoggedErrors;
        }
    }
}
