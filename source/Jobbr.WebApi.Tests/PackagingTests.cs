using Jobbr.DevSupport.ReferencedVersionAsserter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jobbr.WebApi.Tests
{
    [TestClass]
    public class PackagingTests
    {
        [TestMethod]
        [Ignore]
        public void Feature_NuSpec_IsCompliant()
        {
            var asserter = new Asserter(Asserter.ResolvePackagesConfig("Jobbr.Server.WebApi"), Asserter.ResolveRootFile("Jobbr.Server.WebApi.nuspec"));

            asserter.Add(new PackageExistsInBothRule("Jobbr.ComponentModel.Registration"));
            asserter.Add(new PackageExistsInBothRule("Jobbr.ComponentModel.Management"));
            asserter.Add(new VersionIsIncludedInRange("Jobbr.ComponentModel.*"));
            asserter.Add(new NoMajorChangesInNuSpec("Jobbr.*"));
            asserter.Add(new NoMajorChangesInNuSpec("Microsoft.*"));

            var result = asserter.Validate();

            Assert.IsTrue(result.IsSuccessful, result.Message);
        }

        [TestMethod]
        [Ignore]
        public void Client_NuSpec_IsCompliant()
        {
            var asserter = new Asserter(Asserter.ResolvePackagesConfig("Jobbr.Client"), Asserter.ResolveRootFile("Jobbr.Client.nuspec"));

            asserter.Add(new NoExternalDependenciesRule());

            var result = asserter.Validate();

            Assert.IsTrue(result.IsSuccessful, result.Message);
        }
    }
}
