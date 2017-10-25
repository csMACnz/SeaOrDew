Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$base_dir = resolve-path .\
$packageFolder = "$base_dir\BuildTools\"
$coverageResultsFile = "$base_dir\CoverageResult.xml"

nuget install "NuGet.CommandLine" -Version "3.4.3" -OutputDirectory "$packageFolder" -NonInteractive -Verbosity detailed
$nuget = (Resolve-Path "$packageFolder\Nuget.CommandLine.*\tools\nuget.exe").ToString()

& $nuget install "coveralls.net" -Version "0.8.0-unstable0013" -OutputDirectory "$packageFolder" -NonInteractive -Verbosity detailed
$coveralls = (Resolve-Path "$packageFolder\coveralls.net*\tools\csmacnz.coveralls.exe").ToString()

& $coveralls --opencover -i "$coverageResultsFile" --useRelativePaths --treatUploadErrorsAsWarnings