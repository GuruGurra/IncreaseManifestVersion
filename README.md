# IncreaseManifestVersion
Stream Deck will only accept an existing plugin update if the version number in the manifest.json file is higher than the already installed version of the plugin.

This command-line program can be used in a Build event in Visual Studio or a deployment script for Stream Deck projects and ensure that the deployment to Stream Deck will be accepted without the need to uninstall the previous version beforehand. 

Upon execution, the program will locate the version info in the referenced Manifest.json file and increase the Revision level one step. Example:

Existing line in Manifest.json =>
"Version": "2.0.4.58",

The same line after execution of this program. =>
"Version": "2.0.4.59",

Usage:

IncreaseManifestVersion.exe _pathToManifestFile_

Remarks:

The version number in the original Manifest file has the format "major.minor.build.revision" and can be anything from a single-digit ("2") to a full version ("2.0.4.1"). 
If minor, build, or Revision is absent in the original file, they will be set to 0 before increasing the Revision level. 
(i.e., "2" will be padded to "2.0.0.0" and then the Revision will be increased one step)
