// -------------------------------------------------------------------------------------------------
// <copyright file="ProjectFileParserTestFixture.cs" company="Starion Group S.A.">
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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using NetProjectPackageExtractor.Services;

    using NUnit.Framework;

    /// <summary>
    /// Suite of tests for the <see cref="ProjectFileParser"/> class.
    /// </summary>
    [TestFixture]
    public class ProjectFileParserTestFixture
    {
        private ProjectFileParser projectFileParser;

        private List<FileInfo> projectFiles;

        private DirectoryInfo rootFolder;

        [SetUp]
        public void Setup()
        {
            var projectFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "Root", "root.csproj"); 
            
            this.rootFolder = new DirectoryInfo(Path.GetDirectoryName(projectFile));

            this.projectFiles = [new FileInfo(projectFile)];

            this.projectFileParser = new ProjectFileParser();
        }

        [Test]
        public void Verify_that_Parser_returns_packages()
        {
            var package = this.projectFileParser.Parse(projectFiles, rootFolder).Single();

            Assert.That(package.ProjectTitle, Is.EqualTo("Root project"));
            Assert.That(package.ProjectVersion, Is.EqualTo("0.0.1"));
            Assert.That(package.Name, Is.EqualTo("Microsoft.NET.Test.Sdk"));
            Assert.That(package.Version, Is.EqualTo("16.11.0"));
        }
    }
}
