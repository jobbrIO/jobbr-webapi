using Jobbr.DevSupport.ReferencedVersionAsserter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jobbr.WebAPI.Tests
{
    [TestClass]
    public class PackagingTests
    {
        [TestMethod]
        public void Feature_NuSpec_IsCompliant()
        {
            var asserter = new Asserter(Asserter.ResolveProjectFile("Jobbr.Server.WebAPI", "Jobbr.Server.WebAPI.csproj"), Asserter.ResolveRootFile("Jobbr.Server.WebAPI.nuspec"));

            asserter.Add(new PackageExistsInBothRule("Jobbr.ComponentModel.Registration"));
            asserter.Add(new PackageExistsInBothRule("Jobbr.ComponentModel.Management"));
            asserter.Add(new PackageExistsInBothRule("Microsoft.Extensions.Logging.Abstractions"));
            asserter.Add(new PackageExistsInBothRule("SimpleInjector"));

            asserter.Add(new VersionIsIncludedInRange("Jobbr.ComponentModel.*"));
            asserter.Add(new VersionIsIncludedInRange("SimpleInjector"));

            asserter.Add(new NoMajorChangesInNuSpec("Jobbr.*"));

            var result = asserter.Validate();

            Assert.IsTrue(result.IsSuccessful, result.Message);
        }

        [TestMethod]
        public void Client_NuSpec_IsCompliant()
        {
            var asserter = new Asserter(Asserter.ResolveProjectFile("Jobbr.Client", "Jobbr.Client.csproj"), Asserter.ResolveRootFile("Jobbr.Client.nuspec"));

            asserter.Add(new NoExternalDependenciesRule());

            var result = asserter.Validate();

            Assert.IsTrue(result.IsSuccessful, result.Message);
        }
    }
}
