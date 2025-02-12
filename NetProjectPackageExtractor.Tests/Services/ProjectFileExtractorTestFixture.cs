// -------------------------------------------------------------------------------------------------
// <copyright file="ProjectFileExtractorTestFixture.cs" company="Starion Group S.A.">
//
//   Copyright 2022-2024 Starion Group S.A.
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
    using System.IO;
    using System.Linq;

    using NetProjectPackageExtractor.Services;
    
    using NUnit.Framework;

    /// <summary>
    /// Suite of tests for the <see cref="ProjectFileExtractor"/> class.
    /// </summary>
    [TestFixture]
    public class ProjectFileExtractorTestFixture
    {
        private ProjectFileExtractor projectFileExtractor;

        private DirectoryInfo rootFolder;

        [SetUp]
        public void Setup()
        {
            this.projectFileExtractor = new ProjectFileExtractor();

            this.rootFolder = new DirectoryInfo(Path.Combine(TestContext.CurrentContext.TestDirectory, "Root"));
        }

        [Test]
        public void Verify_that_The_project_files_are_found()
        {
            var files = this.projectFileExtractor.QueryProjectFiles(rootFolder);

            Assert.That(files.Count(), Is.EqualTo(6));
        }
    }
}
