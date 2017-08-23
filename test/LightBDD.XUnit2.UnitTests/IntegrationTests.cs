﻿using System;
using System.Linq;
using System.Threading.Tasks;
using LightBDD.Core.Results;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.Framework.Scenarios.Extended;
using Xunit;
using Xunit.Abstractions;

namespace LightBDD.XUnit2.UnitTests
{
    [FeatureDescription("desc")]
    [ScenarioCategory("Category B")]
    public class IntegrationTests : FeatureFixture
    {
        public IntegrationTests(ITestOutputHelper output)
            : base(output)
        {
        }

        [Scenario]
        [Label(nameof(It_should_capture_scenario_name))]
        public void It_should_capture_scenario_name()
        {
            Runner.RunScenario(Some_step);
            var result = GetScenarioResult(nameof(It_should_capture_scenario_name));
            Assert.Equal("It should capture scenario name", result.Info.Name.ToString());
        }

        [Scenario]
        [Label(nameof(It_should_capture_scenario_name_after_await))]
        public async Task It_should_capture_scenario_name_after_await()
        {
            await Task.Yield();
            Runner.RunScenario(Some_step);
            var result = GetScenarioResult(nameof(It_should_capture_scenario_name_after_await));
            Assert.Equal("It should capture scenario name after await", result.Info.Name.ToString());
        }

        [Scenario]
        [ScenarioCategory("Category A")]
        [Label(nameof(It_should_capture_nunit_specific_attributes))]
        public void It_should_capture_nunit_specific_attributes()
        {
            Runner.RunScenario(Some_step);

            var result = FeatureRunnerProvider.GetRunnerFor(GetType()).GetFeatureResult();
            Assert.Equal("desc", result.Info.Description);

            var scenario = GetScenarioResult(nameof(It_should_capture_nunit_specific_attributes));
            Assert.Equal(
                new[] { "Category A", "Category B" },
                scenario.Info.Categories.ToArray());
        }

        [Scenario]
        [Label(nameof(It_should_capture_nunit_ignore_assertion))]
        public void It_should_capture_nunit_ignore_assertion()
        {
            try
            {
                Runner.RunScenario(Ignored_step);
            }
            catch
            {
            }
            var result = GetScenarioResult(nameof(It_should_capture_nunit_ignore_assertion));
            Assert.Equal(ExecutionStatus.Ignored, result.Status);
            Assert.Equal("Step 1: manually ignored", result.StatusDetails);
        }

        [Fact]
        public void Runner_should_throw_meaningful_exception_if_scenario_is_not_run_from_Scenario_attribute()
        {
            Exception ex = Assert.Throws<InvalidOperationException>(() => Runner.RunScenario(Some_step));
            Assert.Equal(
                "Unable to locate Scenario name. Please ensure that scenario is executed from method with [Scenario] attribute.",
                ex.Message);
        }

        [Scenario]
        public async Task Runner_should_support_async_void_scenarios()
        {
            var finished = false;
            Action step1 = async () =>
            {
                await Task.Delay(200);
                finished = true;
            };
            Action step2 = () => Assert.True(finished);

            await Runner.RunScenarioActionsAsync(step1, step2);
        }

        [Scenario]
        public void Runner_should_not_support_async_void_scenarios_if_executed_in_sync_mode()
        {
            Action step = async () => await Task.Delay(200);
            var ex = Assert.Throws<InvalidOperationException>(() => Runner.RunScenario(step));
            Assert.Equal(
                    "Only steps being completed upon return can be run synchronously (all steps have to return completed task). Consider using Async scenario methods for async Task or async void steps.",
                    ex.Message);
        }

        [Scenario]
        [InlineData("abc")]
        [InlineData("def")]
        public void Runner_should_support_parameterized_scenarios_with_value(string value)
        {
            Runner.RunScenario(_ => Step_with_parameter(value));
            Assert.Contains($"SCENARIO: Runner should support parameterized scenarios with value \"{value}\"", ConfiguredLightBddScope.CapturedNotifications);
        }

        [Scenario]
        [IgnoreScenario("scenario reason")]
        [Label(nameof(Runner_should_ignore_scenario_with_IgnoreScenarioAttribute))]
        public void Runner_should_ignore_scenario_with_IgnoreScenarioAttribute()
        {
            var ex = Assert.ThrowsAny<Exception>(() => Runner.RunScenario(_ => Some_step()));
            Assert.Equal("scenario reason", ex.Message);
            var result = GetScenarioResult(nameof(Runner_should_ignore_scenario_with_IgnoreScenarioAttribute));

            Assert.Equal(ExecutionStatus.Ignored, result.Status);
            Assert.Equal("Scenario: scenario reason", result.StatusDetails);
            Assert.Equal(ExecutionStatus.NotRun, result.GetSteps().Single().Status);
        }

        [Scenario]
        [Label(nameof(Runner_should_ignore_step_with_IgnoreScenarioAttribute))]
        public void Runner_should_ignore_step_with_IgnoreScenarioAttribute()
        {
            var ex = Assert.ThrowsAny<Exception>(() => Runner.RunScenario(_ => Declaratively_ignored_step()));
            Assert.Equal("step reason", ex.Message);
            var result = GetScenarioResult(nameof(Runner_should_ignore_step_with_IgnoreScenarioAttribute));

            Assert.Equal(ExecutionStatus.Ignored, result.Status);
            Assert.Equal(ExecutionStatus.Ignored, result.GetSteps().Single().Status);
            Assert.Equal("Step 1: step reason", result.StatusDetails);
        }

        [IgnoreScenario("step reason")]
        private void Declaratively_ignored_step()
        {
        }

        private void Step_with_parameter(string value)
        {
            Assert.NotNull(value);
            Assert.Equal(3, value.Length);
        }

        private void Ignored_step()
        {
            StepExecution.Current.IgnoreScenario("manually ignored");
        }

        private void Some_step()
        {
        }

        private IScenarioResult GetScenarioResult(string scenarioId)
        {
            return FeatureRunnerProvider.GetRunnerFor(GetType())
                .GetFeatureResult()
                .GetScenarios()
                .Single(s => s.Info.Labels.Contains(scenarioId));
        }
    }
}
