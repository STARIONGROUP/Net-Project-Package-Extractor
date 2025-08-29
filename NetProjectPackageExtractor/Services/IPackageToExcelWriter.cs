// -------------------------------------------------------------------------------------------------
// <copyright file="IPackageToExcelWriter.cs" company="Starion Group S.A.">
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
    /// Parses the project file and extracts the referenced nuget packages
    /// </summary>
    public interface IPackageToExcelWriter
    {
        /// <summary>
        /// Writes specific SRF data to an Excel file
        /// </summary>
        /// <param name="packages">
        /// The project files to parse
        /// </param>
        /// <param name="result">
        /// The location where the file should be written to
        /// </param>
        void WriteSoftwareReuseFile(IEnumerable<Package> packages, FileInfo result);
    }
}
