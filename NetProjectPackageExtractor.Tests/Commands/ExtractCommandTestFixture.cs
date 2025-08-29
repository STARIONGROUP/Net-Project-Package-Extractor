// -------------------------------------------------------------------------------------------------
// <copyright file="ExtractCommandTestFixture.cs" company="Starion Group S.A.">
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

namespace NetProjectPackageExtractor.Tests.Commands
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using NetProjectPackageExtractor;
    using NetProjectPackageExtractor.Commands;
    using NetProjectPackageExtractor.Services;

    using NUnit.Framework;

    using Moq;
    using System.CommandLine.Invocation;

    /// <summary>
    /// Suite of tests for the <see cref="ExtractCommand"/> class.
    /// </summary>
    [TestFixture]
    public class ExtractCommandTestFixture
    {
        private List<Package> packages;

        private FileInfo resultFileInfo;

        private Mock<INuGetReader> nuGetReader;

        private Mock<IPackageToExcelWriter> packageToExcelWriter;

        private Mock<IProjectFileExtractor> projectFileExtractor;

        private Mock<IProjectFileParser> projectFileParser;

        private ExtractCommand.Handler handler;

        [SetUp]
        public void SetUp()
        {
            this.nuGetReader = new Mock<INuGetReader>();

            this.packageToExcelWriter = new Mock<IPackageToExcelWriter>();

            this.projectFileExtractor = new Mock<IProjectFileExtractor>();

            this.projectFileParser = new Mock<IProjectFileParser>();

            this.handler = new ExtractCommand.Handler(
                this.nuGetReader.Object,
                this.packageToExcelWriter.Object,
                this.projectFileExtractor.Object,
                this.projectFileParser.Object);
        }

        [Test]
        public async Task Verify_that_InvokeAsync_returns_0()
        {
            var invocationContext = new InvocationContext(null);

            this.handler.RootDirectory = new DirectoryInfo(TestContext.CurrentContext.WorkDirectory);
            this.handler.OutputReport =
                new FileInfo(Path.Combine(TestContext.CurrentContext.TestDirectory, "result.xlsx"));

            var result = await this.handler.InvokeAsync(invocationContext);

            this.nuGetReader.Verify(x => x.Update(It.IsAny<List<Package>>()),
                Times.Once());

            this.packageToExcelWriter.Verify(x => x.WriteSoftwareReuseFile(It.IsAny<IEnumerable<Package>>(),
                    It.IsAny<FileInfo>()),
                Times.Once());

            this.projectFileExtractor.Verify(x => x.QueryProjectFiles(It.IsAny<DirectoryInfo>()),
                Times.Once);

            this.projectFileParser.Verify(x => x.Parse(It.IsAny<IEnumerable<FileInfo>>(), It.IsAny<DirectoryInfo>()
                ), Times.Once);

            Assert.That(result, Is.EqualTo(0));
        }
    }
}