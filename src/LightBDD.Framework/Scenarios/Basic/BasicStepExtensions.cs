﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LightBDD.Framework.Extensibility;
using LightBDD.Framework.Scenarios.Basic.Implementation;

namespace LightBDD.Framework.Scenarios.Basic
{
    /// <summary>
    /// Extensions class allowing to use basic scenario syntax for defining LightBDD test steps.
    /// </summary>
    [DebuggerStepThrough]
    public static class BasicStepExtensions
    {
        /// <summary>
        /// Adds steps specified by <paramref name="steps"/> parameter.<br/>
        /// The steps would be executed in specified order.<br/>
        /// If given step throws, other are not executed.<br/>
        /// The step name is determined from corresponding action name.<br/>
        /// Example usage:
        /// <code>
        /// builder.AddSteps(
        ///     Given_the_user_is_about_to_login,
        ///     Given_the_user_entered_valid_login,
        ///     Given_the_user_entered_valid_password,
        ///     When_the_user_clicks_login_button,
        ///     Then_the_login_operation_should_be_successful,
        ///     Then_a_welcome_message_containing_user_name_should_be_returned);
        /// </code>
        /// Expected step signature:
        /// <code>
        /// void Given_the_user_is_about_to_login() { /* ... */ }
        /// </code>
        /// </summary>
        /// <param name="builder">Composite step builder.</param>
        /// <param name="steps">Steps to add.</param>
        /// <returns><paramref name="builder"/> instance.</returns>
        public static ICompositeStepBuilder<NoContext> AddSteps(this ICompositeStepBuilder<NoContext> builder, params Action[] steps)
        {
            builder.Integrate().AddSteps(steps.Select(BasicStepCompiler.ToSynchronousStep));
            return builder;
        }
        /// <summary>
        /// Adds steps specified by <paramref name="steps"/> parameter.<br/>
        /// The steps would be executed in specified order.<br/>
        /// If given step throws, other are not executed.<br/>
        /// The step name is determined from corresponding action name.<br/>
        /// Example usage:
        /// <code>
        /// builder.AddAsyncSteps(
        ///     Given_the_user_is_about_to_login,
        ///     Given_the_user_entered_valid_login,
        ///     Given_the_user_entered_valid_password,
        ///     When_the_user_clicks_login_button,
        ///     Then_the_login_operation_should_be_successful,
        ///     Then_a_welcome_message_containing_user_name_should_be_returned);
        /// </code>
        /// Expected step signature:
        /// <code>
        /// async Task Given_the_user_is_about_to_login() { /* ... */ }
        /// </code>
        /// </summary>
        /// <param name="builder">Composite step builder.</param>
        /// <param name="steps">Steps to add.</param>
        /// <returns><paramref name="builder"/> instance.</returns>
        public static ICompositeStepBuilder<NoContext> AddAsyncSteps(this ICompositeStepBuilder<NoContext> builder, params Func<Task>[] steps)
        {
            builder.Integrate().AddSteps(steps.Select(BasicStepCompiler.ToAsynchronousStep));
            return builder;
        }
    }
}