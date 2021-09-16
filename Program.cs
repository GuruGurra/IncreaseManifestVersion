using System;
using System.IO;

namespace IncreaseManifestVersion
{
	class Program
	{
		/// <summary>
		/// Increases the revision level one step in the referenced Manifest.json file. 
		/// To be used in a deployment script for Stream Deck projects and ensure
		/// that the deployment to Stream Deck will be accepted without the need to 
		/// uninstall the previous version beforehand.
		/// </summary>
		/// <param name="args">path to the Manifest.json file</param>
		static int Main(string[] args)
		{
			if (args.Length == 1)
			{
				try
				{
					if (File.Exists(args[0]))
					{
						string fileContent = System.IO.File.ReadAllText(args[0]);
						int versionPosition = fileContent.IndexOf("\"Version\":");
						if (versionPosition > 0)
						{
							int versionStart = fileContent.IndexOf("\"", versionPosition + 10) + 1;
							int versionEnd = fileContent.IndexOf("\"", versionStart + 1) - 1;

							string stringVersion = fileContent.Substring(versionStart, versionEnd - versionStart + 1);
							if (!stringVersion.Contains('.'))
							{
								// Version-TryParse require the following format: major.minor[.build[.revision]]
								// If version in json file has only a major version, add ".0"
								stringVersion += ".0";
							}
							if (Version.TryParse(stringVersion, out Version version))
							{
								// If Build and/or Revision is not present in the original file, set them to 0
								int useBuild = (version.Build >= 0) ? version.Build : 0;
								int useRevision = (version.Revision >= 0) ? version.Revision : 0;

								// Create new version number
								version = new Version(version.Major, version.Minor, useBuild, useRevision + 1);

								// Replace version string in file content
								string stringToBeReplaced = fileContent.Substring(versionPosition, versionEnd - versionPosition + 2);
								string newString = $"\"Version\": \"{version}\"";
								fileContent = fileContent.Replace(stringToBeReplaced, newString);

								// Replace file
								System.IO.File.WriteAllText(args[0], fileContent);
								Console.WriteLine($"The version in file '{args[0]}' increased to {version}.");
							}
							else
							{
								Console.WriteLine($"Could not find the version info in file '{args[0]}'.");
								return (int)ExitCode.VersionNotFound;
							}
						}
						else
						{
							Console.WriteLine($"Could not find the version info in file '{args[0]}'.");
							return (int)ExitCode.VersionNotFound;
						}
					}
					else
					{
						Console.WriteLine($"The file '{args[0]}' cannot be found.");
						return (int)ExitCode.FileNotFound;
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Exception: {ex.ToString()}");
					return (int)ExitCode.UnknownError;
				}
			}
			else
			{
				Console.WriteLine("Usage:");
				Console.WriteLine("IncreaseManifestVersion _pathToManifestFile_");
				return (int)ExitCode.ArgumentMissing;
			}
			return (int)ExitCode.Success;
		}

		enum ExitCode : int
		{
			Success = 0,
			VersionNotFound = 1,
			FileNotFound = 2,
			ArgumentMissing = 3,
			UnknownError = 10
		}
	}
}
