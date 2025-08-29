// -------------------------------------------------------------------------------------------------
// <copyright file="ExtractCommand.cs" company="Starion Group S.A.">
// 
//   Copyright 2022-2025 Starion Group S.A.
// 
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace NetProjectPackageExtractor.Commands
{
    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    
    using NetProjectPackageExtractor.Resources;
    using NetProjectPackageExtractor.Services;

    using Spectre.Console;

    /// <summary>
    /// The <see cref="RootCommand"/> that generates extracts the package information and generates
    /// a Software Reuse File spreadsheet
    /// </summary>
    public class ExtractCommand : RootCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtractCommand"/>
        /// </summary>
        public ExtractCommand() : base("Package Extractor")
        {
            var noLogoOption = new Option<bool>(
                name: "--no-logo",
                description: "Suppress the logo",
                getDefaultValue: () => false);
            this.AddOption(noLogoOption);

            var rootDirectoryInfoOption = new Option<DirectoryInfo>(
                name: "--root-directory",
                description: "The directory that contains the project files, this directory is process recursively",
                getDefaultValue: () =>
                {
                    var strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    return new FileInfo(strExeFilePath).Directory;
                });
            rootDirectoryInfoOption.AddAlias("-rd");
            rootDirectoryInfoOption.IsRequired = true;
            this.AddOption(rootDirectoryInfoOption);

            var reportFileOption = new Option<FileInfo>(
                name: "--output-report",
                description: "The path to the SRF report file",
                getDefaultValue: () => new FileInfo("SRF-report.xlsx"));
            reportFileOption.AddAlias("-o");
            reportFileOption.IsRequired = true;
            this.AddOption(reportFileOption);
        }

        /// <summary>
        /// The Command Handler of the <see cref="ExtractCommand"/>
        /// </summary>
        public new class Handler : ICommandHandler
        {
            /// <summary>
            /// The (injected) <see cref="INuGetReader"/> that is used to update <see cref="Package"/>s with
            /// information from the local nuget cache
            /// </summary>
            private readonly INuGetReader nuGetReader;

            /// <summary>
            /// The (injected) <see cref="IPackageToExcelWriter"/> used to write specific SRF data to an Excel file
            /// </summary>
            private readonly IPackageToExcelWriter packageToExcelWriter;

            /// <summary>
            /// The (injected) <see cref="IProjectFileExtractor"/> used to recursively iterate through a directory
            /// tree and return all the .csproj files in this directory tree as a List of <see cref="FileInfo"/> objects
            /// </summary>
            private readonly IProjectFileExtractor projectFileExtractor;
            
            /// <summary>
            /// The (injected) <see cref="IProjectFileParser"/> used to parses a project file and extract the
            /// referenced nuget packages
            /// </summary>
            private readonly IProjectFileParser projectFileParser;

            /// <summary>
            /// Initializes a nwe instance of the <see cref="Handler"/> class.
            /// </summary>
            /// <param name="nuGetReader">
            /// The (injected) <see cref="INuGetReader"/> that is used to update <see cref="Package"/>s with
            /// information from the local nuget cache
            /// </param>
            /// <param name="packageToExcelWriter">
            /// The (injected) <see cref="IPackageToExcelWriter"/> used to write specific SRF data to an Excel file
            /// </param>
            /// <param name="projectFileExtractor">
            /// The (injected) <see cref="IProjectFileExtractor"/> used to recursively iterate through a directory
            /// tree and return all the .csproj files in this directory tree as an List of <see cref="FileInfo"/> objects
            /// </param>
            /// <param name="projectFileParser">
            /// The (injected) <see cref="IProjectFileParser"/> used to parses a project file and extract the
            /// referenced nuget packages
            /// </param>
            public Handler(INuGetReader nuGetReader, IPackageToExcelWriter packageToExcelWriter, IProjectFileExtractor projectFileExtractor, IProjectFileParser projectFileParser)
            {
                this.nuGetReader = nuGetReader
                                   ?? throw new ArgumentNullException(nameof(nuGetReader));
                this.packageToExcelWriter = packageToExcelWriter
                                            ?? throw new ArgumentNullException(nameof(packageToExcelWriter));
                this.projectFileExtractor = projectFileExtractor
                                            ?? throw new ArgumentNullException(nameof(projectFileExtractor));
                this.projectFileParser = projectFileParser
                                         ?? throw new ArgumentNullException(nameof(projectFileParser));
            }

            /// <summary>
            /// Gets or sets the value indicating whether the logo should be shown or not
            /// </summary>
            public bool NoLogo { get; set; }

            /// <summary>
            /// Gets or sets the <see cref="DirectoryInfo"/> (and subfolders) in which the .csproj files are located
            /// </summary>
            public DirectoryInfo RootDirectory { get; set; }

            /// <summary>
            /// Gets or sets the <see cref="FileInfo"/> where the SRF report is to be generated
            /// </summary>
            public FileInfo OutputReport { get; set; }

            /// <summary>
            /// Invokes the <see cref="ICommandHandler"/>
            /// </summary>
            /// <param name="context">
            /// The <see cref="InvocationContext"/> 
            /// </param>
            /// <returns>
            /// 0 when successful, another if not
            /// </returns>
            public int Invoke(InvocationContext context)
            {
                throw new NotSupportedException();
            }

            /// <summary>
            /// Asynchronously invokes the <see cref="ICommandHandler"/>
            /// </summary>
            /// <param name="context">
            /// The <see cref="InvocationContext"/> 
            /// </param>
            /// <returns>
            /// 0 when successful, another if not
            /// </returns>
            public async Task<int> InvokeAsync(InvocationContext context)
            {
                if (!this.NoLogo)
                {
                    AnsiConsole.Markup($"[blue]{ResourceLoader.QueryLogo()}[/]");
                }

                if (!this.RootDirectory.Exists)
                {
                    AnsiConsole.MarkupLine($"[red]The specified csproj source directory does not exist[/]");
                    AnsiConsole.MarkupLine($"[purple]{this.RootDirectory.FullName}[/]");
                    return -1;
                }

                try
                {
                    await AnsiConsole.Status()
                        .AutoRefresh(true)
                        .SpinnerStyle(Style.Parse("green bold"))
                        .Start("Preparing Warp Engines...", ctx =>
                        {
                            Thread.Sleep(1500);

                            ctx.Status("Reading .csproj files at Warp 3...");
                            Thread.Sleep(1500);

                            var csprojFiles = this.projectFileExtractor.QueryProjectFiles(this.RootDirectory);

                            var packages = projectFileParser.Parse(csprojFiles, this.RootDirectory).ToList();

                            AnsiConsole.MarkupLine($"[grey]LOG:[/] A total of [bold]{packages.Count}[/] packages were read");
                            ctx.Status("Updating Package information at Warp 7...");
                            Thread.Sleep(1500);

                            this.nuGetReader.Update(packages);

                            ctx.Status($"Generating report at Warp 11, Captain..., SLOW DOWN!");
                            Thread.Sleep(1500);

                            this.packageToExcelWriter.WriteSoftwareReuseFile(packages, OutputReport);
                            
                            AnsiConsole.MarkupLine($"[grey]LOG:[/] SRF report generated at [bold]{this.OutputReport.FullName}[/]");
                            Thread.Sleep(1500);
                            ctx.Status("[green]Dropping to impulse speed[/]");
                            Thread.Sleep(1500);

                            return Task.FromResult(0);
                            
                        });
                }
                catch (Exception ex)
                {
                    AnsiConsole.WriteLine();
                    AnsiConsole.MarkupLine("[red]An exception occurred[/]");
                    AnsiConsole.MarkupLine("[green]Dropping to impulse speed[/]");
                    AnsiConsole.MarkupLine("[red]please report an issue at[/]");
                    AnsiConsole.MarkupLine("[link] https://github.com/STARIONGROUP/Net-Project-Package-Extractor/issues [/]");
                    AnsiConsole.WriteLine();
                    AnsiConsole.WriteException(ex);
                    
                    return -1;
                }

                return 0;
            }
        }
    }
}
