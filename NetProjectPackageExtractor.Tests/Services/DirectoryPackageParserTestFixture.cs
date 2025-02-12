// -------------------------------------------------------------------------------------------------
// <copyright file="NuGetReaderTestFixture.cs" company="Starion Group S.A.">
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
    using System;
    using System.IO;
    using System.Collections.Generic;

    using NetProjectPackageExtractor;
    using NetProjectPackageExtractor.Services;

    using NUnit.Framework;

    /// <summary>
    /// Suite of tests for the <see cref="DirectoryPackageParser"/> class.
    /// </summary>
    [TestFixture]

    public class DirectoryPackageParserTestFixture
    {
        private DirectoryPackageParser directoryPackageParser;

        private List<Package> packages;

        private DirectoryInfo rootFolder;

        [SetUp]
        public void SetUp()
        {
            this.rootFolder = new DirectoryInfo(Path.Combine(TestContext.CurrentContext.TestDirectory, "Root"));
        }

        /// <summary>
        /// This tests if it is able to find the package management in the parent folders
        /// </summary>
        [Test]
        [Category("DirectoryPackageParser")]
        public void Verify_that_Parser_returns_packages()
        {
            var targetProjectFile = new FileInfo(Path.Combine(this.rootFolder.FullName, "SubFolder1", "Subfolder1.2", "SubsubFolder1.2.1", "SubSubSubFolder1.2.1.1", "SubSubSubFolder1.2.1.1.csproj"));
            var dictionnary = DirectoryPackageParser.SearchAndParse(targetProjectFile, this.rootFolder);
            Assert.That(dictionnary["NUnit3TestAdapter"], Is.EqualTo("4.3.0"));
        }

        /// <summary>
        /// This tests if it is able to find the package management in the parent folders
        /// </summary>
        [Test]
        [Category("DirectoryPackageParser")]
        public void Verify_that_Parser_returns_packages_from_subfolder()
        {
            var targetProjectFile = new FileInfo(Path.Combine(this.rootFolder.FullName, "SubFolder1", "Subfolder1.2", "SubsubFolder1.2.1", "SubSubFolder1.2.1.csproj"));
            var dictionnary = DirectoryPackageParser.SearchAndParse(targetProjectFile, this.rootFolder);
            Assert.That(dictionnary["NUnit3TestAdapter"], Is.EqualTo("4.3.0"));
        }

        /// <summary>
        /// This tests if it is able to find the package management in the same folder
        /// </summary>
        [Test]
        [Category("DirectoryPackageParser")]
        public void Verify_that_Parser_returns_packages_in_same_folder()
        {
            var targetProjectFile = new FileInfo(Path.Combine(this.rootFolder.FullName, "SubFolder1", "Subfolder1.2", "SubFolder1.2.csproj"));
            var dictionnary = DirectoryPackageParser.SearchAndParse(targetProjectFile, this.rootFolder);
            Assert.That(dictionnary["NUnit3TestAdapter"], Is.EqualTo("4.3.0"));
        }

        /// <summary>
        /// This tests if the project file has no directory.packages.props files in the parent folders
        /// </summary>
        [Test]
        [Category("DirectoryPackageParser")]
        public void Verify_that_Parser_returns_nothing()
        {
            var targetProjectFile = new FileInfo(Path.Combine(this.rootFolder.FullName, "root.csproj"));
            var dictionary = DirectoryPackageParser.SearchAndParse(targetProjectFile, this.rootFolder);
            Assert.That(dictionary.Count, Is.EqualTo(0));
        }
    
        /// <summary>
        /// This tests if the project file is in a parent folder of the target folder
        /// </summary>
        [Test]
        [Category("DirectoryPackageParser")]
        public void Verify_that_Parser_returns_exception()
        {
            var targetProjectFile = new FileInfo(Path.Combine(this.rootFolder.FullName, "root.csproj"));
            var targetFolderFile = new DirectoryInfo(Path.Combine(this.rootFolder.FullName, "SubFolder1"));

            Assert.Throws<ArgumentException>(() => DirectoryPackageParser.SearchAndParse(targetProjectFile, targetFolderFile));
        }
    }
}
