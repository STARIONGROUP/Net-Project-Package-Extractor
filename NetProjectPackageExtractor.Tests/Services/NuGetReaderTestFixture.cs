// -------------------------------------------------------------------------------------------------
// <copyright file="NuGetReaderTestFixture.cs" company="Starion Group S.A.">
//
//   Copyright 2022-2025 Starion Group S.A.
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace NetProjectPackageExtractor.Tests.Services
{
    using System.Collections.Generic;

    using NetProjectPackageExtractor;
    using NetProjectPackageExtractor.Services;

    using NUnit.Framework;

    /// <summary>
    /// Suite of tests for the <see cref="NuGetReader"/> class.
    /// </summary>
    [TestFixture]
    public class NuGetReaderTestFixture
    {
        private NuGetReader? nuGetReader;

        [SetUp]
        public void SetUp()
        {
            this.nuGetReader = new NuGetReader();
        }

        [Test]
        [Category("NugetCache")]
        public void Verify_that_nuspec_details_can_be_read_and_packages_are_update()
        {
            var package = new Package
            {
                Name = "Microsoft.NET.Test.Sdk",
                Version = "17.4.0"
            };

            var packages = new List<Package>() { package };

            this.nuGetReader.Update(packages);

            Assert.That(package.LicenseUrl, Is.EqualTo("https://aka.ms/deprecateLicenseUrl"));
        }

        [Test]
        [Category("NugetCache")]
        public void Verify_that_nuspec_details_can_be_read_and_packages_are_update_2()
        {
            var package = new Package
            {
                Name = "coverlet.collector",
                Version = "3.1.2"
            };

            var packages = new List<Package>() { package };

            this.nuGetReader.Update(packages);

            Assert.That(package.LicenseUrl, Is.EqualTo("https://licenses.nuget.org/MIT"));
        }
    }
}
