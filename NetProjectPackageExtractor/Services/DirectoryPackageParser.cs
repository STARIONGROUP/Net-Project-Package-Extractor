// -------------------------------------------------------------------------------------------------
// <copyright file="DirectoryPackageParser.cs" company="Starion Group S.A.">
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
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;

    public class DirectoryPackageParser : IDirectoryPackageParser
    {
        /// <summary>
        /// Search for the closest Directory.Packages.props file in the directory structure from the csproj file.
        /// </summary>
        /// <param name="projectFile">The location of the csproj file</param>
        /// <param name="sourceDirectory">The source directory provided by the user in the command line</param>
        /// <returns>A dictionary where the key is the package name, and the value the version number</returns>
        public static Dictionary<string, string> SearchAndParse(FileInfo projectFile, DirectoryInfo sourceDirectory)
        {
            var subDirectory = projectFile.Directory;
            if (sourceDirectory.FullName.Contains(projectFile.DirectoryName) && sourceDirectory.FullName != projectFile.DirectoryName)
            {
                throw new ArgumentException("The source directory must be a parent of the project file");
            }

            FileInfo directoryPackagesFile = null;
            while (subDirectory.FullName.Contains(sourceDirectory.FullName))
            {
                var directoryPackagesFiles = subDirectory.EnumerateFiles("Directory.Packages.props", SearchOption.TopDirectoryOnly);
                if (directoryPackagesFiles.Any())
                {
                    directoryPackagesFile = directoryPackagesFiles.First();
                    break;
                }
                subDirectory = subDirectory.Parent;
            }
            return ParseDirectoryPackage(directoryPackagesFile);
        }

        /// <summary>
        /// Parses the directory file for the version
        /// </summary>
        /// <param name="directoryFile">The xml file containing the Package version.</param>
        /// <returns>A dictionary where the key is the package name, and the value the version number</returns>
        private static Dictionary<string, string> ParseDirectoryPackage(FileInfo directoryFile)
        {
            if (directoryFile == null)
            {
                return new Dictionary<string, string>();
            }
            var document = new XmlDocument();

            var reader = directoryFile.OpenRead();
            document.Load(reader);

            var packageReferenceElements = document.GetElementsByTagName("PackageVersion");

            var dictionnary = new Dictionary<string, string>();
            foreach (XmlNode element in packageReferenceElements)
            {
                var key = element.Attributes["Include"].Value;
                var value = element.Attributes["Version"].Value;
                dictionnary.Add(key, value);
            }

            return dictionnary;
        }
    }
}
