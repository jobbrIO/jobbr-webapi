using System.Reflection;
using Jobbr.DevSupport.ReferencedVersionAsserter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jobbr.WebApi.Tests
{
    [TestClass]
    public class PackagingTests
    {
        private readonly Asserter featureAsserter = new Asserter(Asserter.ResolvePackagesConfig("Jobbr.Server.WebApi"), Asserter.ResolveRootFile("Jobbr.Server.WebApi.nuspec"));
        private readonly Asserter client = new Asserter(Asserter.ResolvePackagesConfig("Jobbr.Client"), Asserter.ResolveRootFile("Jobbr.Client.nuspec"));

        private readonly bool isPre = Assembly.GetExecutingAssembly().GetInformalVersion().Contains("-");

        [TestMethod]
        public void Feature_KnownReferences_AreDeclared()
        {
            this.featureAsserter.Add(new PackageExistsInBothRule("Jobbr.ComponentModel.Registration"));

            var result = this.featureAsserter.Validate();

            Assert.IsTrue(result.IsSuccessful, result.Message);
        }

        [TestMethod]
        public void Feature_PreComponentModelsPre_ExactSameVersions()
        {
            if (!this.isPre)
            {
                // This rule is only valid for Pre-Release versions because we only need exact match on PreRelease Versions
                return;
            }

            this.featureAsserter.Add(new ExactVersionMatchRule("Jobbr.ComponentModel.*"));

            var result = this.featureAsserter.Validate();

            Assert.IsTrue(result.IsSuccessful, result.Message);
        }

        [TestMethod]
        public void Feature_ComponentModels_InRange()
        {
            this.featureAsserter.Add(new VersionIsIncludedInRange("Jobbr.ComponentModel.*"));

            var result = this.featureAsserter.Validate();

            Assert.IsTrue(result.IsSuccessful, result.Message);
        }

        [TestMethod]
        public void Feature_AllDependencies_NoMajorVersionChangeAllowed()
        {
            this.featureAsserter.Add(new NoMajorChangesInNuSpec("Jobbr.*"));
            this.featureAsserter.Add(new NoMajorChangesInNuSpec("Microsoft.*"));

            var result = this.featureAsserter.Validate();

            Assert.IsTrue(result.IsSuccessful, result.Message);
        }

        [TestMethod]
        public void Client_KnownReferences_AreNone()
        {
            this.client.Add(new NoExternalDependenciesRule());

            var result = this.featureAsserter.Validate();

            Assert.IsTrue(result.IsSuccessful, result.Message);
        }
    }
}
