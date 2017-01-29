﻿using LightBDD.Core.Extensibility;
using LightBDD.Scenarios.Basic.Implementation;

namespace LightBDD.Scenarios.Basic
{
    public static class BasicScenarioExtensions
    {
        public static IBasicScenarioRunner Basic(this IBddRunner runner)
        {
            return new BasicScenarioRunner(runner.Integrate());
        }
    }
}
