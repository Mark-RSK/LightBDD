﻿using System;
using System.Threading.Tasks;
using LightBDD.Core.Execution;
using LightBDD.Core.Extensibility;
using LightBDD.Extensions.ContextualAsyncExecution;
using LightBDD.Implementation;

namespace LightBDD
{
    //TODO: test in separate project
    //TODO: rename project to be consistent
    public class StepCommentingExtension : IStepExecutionExtension
    {
        public async Task ExecuteAsync(IStep step, Func<Task> stepInvocation)
        {
            var stepProperty = ScenarioExecutionContext.Current.Get<CurrentStepProperty>();
            try
            {
                stepProperty.Step = step;
                await stepInvocation();
            }
            finally
            {
                stepProperty.Step = null;
            }
        }
    }
}