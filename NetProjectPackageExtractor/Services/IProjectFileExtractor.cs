// -------------------------------------------------------------------------------------------------
// <copyright file="IProjectFileExtractor.cs" company="Starion Group S.A.">
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

namespace NetProjectPackageExtractor.Services
{
    using System.Collections.Generic;

    using System.IO;

    /// <summary>
    /// The purpose of the <see cref="IProjectFileExtractor"/> is to recursively iterate through a directory
    /// tree and return all the .csproj files in this directory tree as an List of <see cref="FileInfo"/> objects
    /// </summary>
    public interface IProjectFileExtractor
    {
        /// <summary>
        /// Queries the directory structure 
        /// </summary>
        /// <param name="directoryInfo">
        /// The <see cref="DirectoryInfo"/> from which the .csproj files are to be queried.
        /// </param>
        /// <returns>
        /// An <see cref="IEnumerable{FileInfo}"/> of the found .csproj files.
        /// </returns>
        IEnumerable<FileInfo> QueryProjectFiles(DirectoryInfo directoryInfo);
    }
}