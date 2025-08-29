// -------------------------------------------------------------------------------------------------
// <copyright file="PackageToExcelWriterTestFixture.cs" company="Starion Group S.A.">
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
    using System.IO;

    using NetProjectPackageExtractor.Services;

    using NUnit.Framework;
    
    /// <summary>
    /// Suite of tests for the <see cref="PackageToExcelWriter"/> class
    /// </summary>
    [TestFixture]
    public class PackageToExcelWriterTestFixture
    {
        private PackageToExcelWriter packageToExcelWriter;

        private List<Package> packages;

        private FileInfo resultFileInfo;

        [SetUp]
        public void SetUp()
        {
            var package = new Package
            {
                Name = "nuget-name",
                Version = "x,y,z",
                Description = "a nuget description",
                LicenseUrl = "https://nuget.org",
                ProjectTitle = "project title",
                ProjectUrl = "https://projet-url.com",
                ProjectVersion = "1.2.3"
            };

            this.packages = new List<Package> { package };
            
            this.resultFileInfo = new FileInfo(Path.Combine(TestContext.CurrentContext.TestDirectory, "results.xlsx"));
        }

        [Test]
        public void Verify_that_report_is_written_without_exception()
        {
            this.packageToExcelWriter = new PackageToExcelWriter();

            Assert.That(() =>
                {
                    this.packageToExcelWriter.WriteSoftwareReuseFile(this.packages, this.resultFileInfo);
                }, 
                Throws.Nothing);
        }

        [Test]
        public void Verify_that_when_packages_result_are_null_exceptions_are_thrown()
        {
            this.packageToExcelWriter = new PackageToExcelWriter();

            Assert.That(() =>
            {
                this.packageToExcelWriter.WriteSoftwareReuseFile(null, this.resultFileInfo);
            }, Throws.ArgumentNullException);

            Assert.That(() =>
            {
                this.packageToExcelWriter.WriteSoftwareReuseFile(this.packages, null);
            }, Throws.ArgumentNullException);
        }
    }
}
