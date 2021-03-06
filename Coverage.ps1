
Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$base_dir = resolve-path .\
$packageFolder = "$base_dir\BuildTools\"
$testResultsDir = "$base_dir\TestResults\"

nuget install "NuGet.CommandLine" -Version "3.4.3" -OutputDirectory "$packageFolder" -NonInteractive -Verbosity detailed
$nuget = (Resolve-Path "$packageFolder\Nuget.CommandLine.*\tools\nuget.exe").ToString()

& $nuget install "OpenCover" -Version "4.6.519" -OutputDirectory "$packageFolder" -NonInteractive -Verbosity detailed
$opencover = (Resolve-Path "$packageFolder\OpenCover.*\tools\OpenCover.Console.exe").ToString()

& $nuget install "ReportGenerator" -Version "2.5.2" -OutputDirectory "$packageFolder" -NonInteractive -Verbosity detailed
$reportGenerator = (Resolve-Path "$packageFolder\ReportGenerator.*\tools\ReportGenerator.exe").ToString()

if(-not (Test-Path $testResultsDir)) {
    mkdir $testResultsDir
}

$coverageResultsFile = "$base_dir\CoverageResult.xml"
if (Test-Path $coverageResultsFile) {
    Remove-Item $coverageResultsFile;
}

function RunCoverage {
param(
    [Parameter(Mandatory=$true)][string]$projectFolder
)   
    Set-Location $projectFolder
    & $opencover -oldstyle -log:Verbose -register:user -mergeoutput -hideskipped:All "-searchdirs:$projectFolder\bin\Debug\net452;$projectFolder\bin\Debug\net462" -target:dotnet.exe "-targetargs:test -c Debug" -filter:"+[csMACnz.SeaOrDew*]* -[csMACnz.SeaOrDew.*Tests]AutoGeneratedProgram" -output:"$coverageResultsFile"
    Set-Location $base_dir
}

RunCoverage $base_dir\test\csMACnz.SeaOrDew.Tests
RunCoverage $base_dir\test\csMACnz.SeaOrDew.AspNetCoreTests

# dotnet test -c Release .\test\csMACnz.SeaOrDew.Tests\csMACnz.SeaOrDew.Tests.csproj
    
# dotnet test -c Release .\test\csMACnz.SeaOrDew.AspNetCoreTests\csMACnz.SeaOrDew.AspNetCoreTests.csproj


& $reportGenerator -reports:$coverageResultsFile -targetdir:$testResultsDir  -reporttypes:Html