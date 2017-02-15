using LightBDD.Core.Configuration;
using LightBDD.Framework.Reporting.Configuration;
using LightBDD.Framework.Reporting.Formatters;
using LightBDD.MsTest2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LightBDD.Example.AcceptanceTests.MsTest
{
    [TestClass]
    public class LightBddIntegration
    {
        [AssemblyInitialize]
        public static void Setup(TestContext testContext) { LightBddScope.Initialize(OnConfigure); }
        [AssemblyCleanup]
        public static void Cleanup() { LightBddScope.Cleanup(); }

        private static void OnConfigure(LightBddConfiguration configuration)
        {
            configuration
                .ReportWritersConfiguration()
                .Clear()
                .AddFileWriter<PlainTextReportFormatter>("~\\Reports\\{TestDateTimeUtc:yyyy-MM-dd-HH_mm_ss}_FeaturesReport.txt");
        }
    }
}