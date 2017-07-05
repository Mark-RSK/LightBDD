using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LightBDD.Core.Configuration;
using LightBDD.Framework.Extensibility;

namespace LightBDD.Framework.Scenarios.Extended.Implementation
{
    [DebuggerStepThrough]
    internal class ExtendedCompositeStepBuilder<TContext>
    {
        private readonly IIntegrableCompositeStepBuilder _builder;
        private readonly ExtendedStepCompiler<TContext> _stepCompiler;

        private ExtendedCompositeStepBuilder(IIntegrableCompositeStepBuilder builder, LightBddConfiguration configuration)
        {
            _builder = builder;
            _stepCompiler = new ExtendedStepCompiler<TContext>(configuration);
        }

        public static ExtendedCompositeStepBuilder<TContext> Create(IIntegrableCompositeStepBuilder builder, LightBddConfiguration configuration)
        {
            return new ExtendedCompositeStepBuilder<TContext>(builder, configuration);
        }

        public void AddSteps(Expression<Func<TContext, Task>>[] steps)
        {
            _builder.AddSteps(steps.Select(_stepCompiler.ToStep));
        }

        public void AddSteps(Expression<Action<TContext>>[] steps)
        {
            _builder.AddSteps(steps.Select(_stepCompiler.ToStep));
        }

        public void AddRepeatedStep<T>(IEnumerable<T> arguments, Expression<Action<T>> step)
        {
            _builder.AddSteps(_stepCompiler.ToRepeatedStep(arguments, step));
        }
    }
}