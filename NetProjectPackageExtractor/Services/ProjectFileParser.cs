// -------------------------------------------------------------------------------------------------
// <copyright file="ProjectFileParser.cs" company="Starion Group S.A.">
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
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// Parses the project file and extracts the referenced nuget packages
    /// </summary>
    public class ProjectFileParser : IProjectFileParser
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
        public IEnumerable<Package> Parse(IEnumerable<FileInfo> projectFiles, DirectoryInfo rootDirectory)
        {
            var result = new List<Package>();
            
            foreach (var projectFile in projectFiles)
            {
                var packages = ParseProjectFile(projectFile, rootDirectory);
                result.AddRange(packages);
            }

            return result;
        }

        /// <summary>
        /// Parses the provided Project file
        /// Eventually searches for a Directory.Build.props file and parses it (if it exists) for Central Package Management (CPM)
        /// See also https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management for more information
        /// </summary>
        /// <param name="projectFile">
        /// The subject project file (encapsulated by a <see cref="FileInfo"/> object).
        /// </param>
        /// <param name="rootDirectory">The root directory of the solution (which should contain the Directory.Build.props file if using CPM)</param>
        /// <returns>
        /// An <see cref="IEnumerable{Package}"/>
        /// </returns>
        private static IEnumerable<Package> ParseProjectFile(FileInfo projectFile, DirectoryInfo rootDirectory)
        {
            var document = new XmlDocument();

            var reader = projectFile.OpenRead();
            document.Load(reader);

            var projectTitle = Path.GetFileNameWithoutExtension(projectFile.Name);
            var projectVersion = string.Empty;

            var titleElement = document.SelectSingleNode("//Title");
            if (titleElement != null)
            {
                projectTitle = titleElement.InnerText;
            }
            else
            {
                var assemblyName = document.SelectSingleNode("//AssemblyName");
                if (assemblyName != null)
                {
                    projectTitle = assemblyName.InnerText;
                }
            }

            var versionElement = document.SelectSingleNode("//Version");
            if (versionElement != null)
            {
                projectVersion = versionElement.InnerText;
            }
            
            var packageReferenceElements = document.GetElementsByTagName("PackageReference");

            var dictionary = DirectoryPackageParser.SearchAndParse(projectFile, rootDirectory);

            foreach (var element in packageReferenceElements)
            {
                var xmlElement = (XmlNode)element;
                var name = xmlElement.Attributes["Include"]?.Value;

                var package = new Package
                {
                    ProjectTitle = projectTitle,
                    ProjectVersion = projectVersion,
                    Name = name,
                    Version = (xmlElement.Attributes["Version"]?.Value ??
                               xmlElement.Attributes["VersionOverride"]?.Value ??
                               dictionary[name] ?? 
                               String.Empty),
                };

                yield return package;
            }
        }
    }
}
