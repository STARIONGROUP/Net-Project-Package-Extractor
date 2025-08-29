// -------------------------------------------------------------------------------------------------
// <copyright file="Package.cs" company="Starion Group S.A.">
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

namespace NetProjectPackageExtractor
{
    /// <summary>
    /// The <see cref="Package"/> class represents a referenced a nuget package
    /// </summary>
    public class Package
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Package"/> class.
        /// </summary>
        /// <param name="name">
        /// The package
        /// </param>
        /// <param name="version">
        /// The project version.
        /// </param>
        /// <param name="licenseUrl">
        /// The license url of the pacakge
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        public Package(string name, string version, string licenseUrl, string description)
        {
            this.Name = name;
            this.Version = version;
            this.LicenseUrl = licenseUrl;
            this.Description = description;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Package"/> class.
        /// </summary>
        public Package()
        {
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the License type 
        /// </summary>
        public string License { get; internal set; }

        /// <summary>
        /// Gets or sets the license url.
        /// </summary>
        public string LicenseUrl { get; set; }

        /// <summary>
        /// Gets or sets the project url.
        /// </summary>
        public string ProjectUrl { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name of the name of the project that is referencing the package
        /// </summary>
        public string ProjectTitle { get; set; }

        /// <summary>
        /// Gets or sets the version of the project that is referencing the package
        /// </summary>
        public string ProjectVersion { get; set; }
    }
}
