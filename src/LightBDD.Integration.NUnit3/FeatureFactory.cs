﻿using System;
using LightBDD.Core.Extensibility;
using LightBDD.Integration.NUnit3;

namespace LightBDD
{
    /// <summary>
    /// Class allowing to instantiate <see cref="IFeatureBddRunner"/> that is being configured to work with NUnit 3 framework.
    /// </summary>
    //TODO: rename
    public static class FeatureFactory
    {
        /// <summary>
        /// Returns <see cref="IFeatureBddRunner"/> for given <paramref name="featureType"/>.
        /// </summary>
        /// <param name="featureType">Feature type.</param>
        /// <returns><see cref="IFeatureBddRunner"/> object.</returns>
        public static IFeatureBddRunner GetRunnerFor(Type featureType)
        {
            return NUnit3FeatureCoordinator.GetInstance().RunnerFactory.GetRunnerFor(featureType);
        }
    }
}