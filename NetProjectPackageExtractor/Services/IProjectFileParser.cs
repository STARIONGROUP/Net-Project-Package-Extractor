// -------------------------------------------------------------------------------------------------
// <copyright file="IProjectFileParser.cs" company="Starion Group S.A.">
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

namespace NetProjectPackageExtractor.Services
{
    using System.Collections.Generic;

    using System.IO;

    /// <summary>
    /// Parses a project file and extracts the referenced nuget packages
    /// </summary>
    public interface IProjectFileParser
    {
        /// <summary>
        /// Parses the provided project files
        /// </summary>
        /// <param name="projectFiles">
        /// The project files to parse
        /// </param>
        /// <returns>
        /// An <see cref="IEnumerable{Package}"/>
        /// </returns>
        IEnumerable<Package> Parse(IEnumerable<FileInfo> projectFiles, DirectoryInfo rootDirectory);
    }
}
