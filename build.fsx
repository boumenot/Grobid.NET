#r @"packages\FAKE\tools\FakeLib.dll"
open System
open System.IO
open Fake
open Fake.ReleaseNotesHelper
open Fake.Testing

let authors = ["Christopher Boumenot"]
let project = "Grobid.Models"
let summary = "You are a summary."
let description = "You are a description."

let projectName = "Grobid.x64"
let projectDescription = "GROBID for .NET"
let projectSummary = projectDescription

let tags = ""

let testResultsDir = "./testresults"

let release = parseReleaseNotes (IO.File.ReadAllLines "RELEASE_NOTES.md")

MSBuildDefaults <- {
  MSBuildDefaults with
    ToolsVersion = Some "14.0"
    Verbosity = Some MSBuildVerbosity.Minimal }

let buildMode = getBuildParamOrDefault "buildMode" "Debug" 

let setParams defaults = {
  defaults with
    ToolsVersion = Some("14.0")
    Targets = ["Build"]
    Properties =
      [
        "Configuration", buildMode
      ]
  }

Target "Clean" (fun _ -> trace "You are a donkey...")

Target "BuildApp" (fun _ ->
                   build setParams "./grobid.sln"
                     |> DoNothing
)

Target "UnitTests" (fun _ ->
                    !! (sprintf "./test/Grobid.Test/bin/%s/**/Grobid*.Test.dll" buildMode)
                    ++ (sprintf "./test/Grobid.PdfToXml.Test/bin/%s/**/Grobid*.Test.dll" buildMode)
                    |> xUnit2 (fun x -> { x with XmlOutputPath = Some(testResultsDir @@ "xml");
                                                 ToolPath = "packages/xunit.runner.console/tools/xunit.console.exe"
                                          })
)

Target "Zip" (fun _ ->
              !! "content/lexicon/**/*" ++ "content/models/**/*"
              |> Zip "content" "grobid.zip"
)

// ==================================================
// NuGet

let referenceDependencies dependencies =
    let packagesDir = __SOURCE_DIRECTORY__  @@ "packages"
    [ for dependency in dependencies -> dependency, GetPackageVersion packagesDir dependency ]

// == grobid.X.Y.Z.nupkg
Target "NuGet" (fun _ ->
                    NuGet (fun x ->
                        { x with
                            Authors = authors
                            Project = projectName
                            Summary = projectSummary
                            Description = projectDescription
                            Version = release.NugetVersion
                            ReleaseNotes = String.Join(Environment.NewLine, release.Notes)
                            Tags = tags
                            OutputPath = "publish"
                            AccessKey = getBuildParamOrDefault "nugetkey" ""
                            Publish = hasBuildParam "nugetkey"
                            Dependencies = referenceDependencies [ "IKVM" ] @ [ "Grobid.Models", "1.0.0" ]
                            Files = [ (@"..\nuget\build\grobid.x64.props",                    Some @"build\grobid.x64.props", None)
                                      (@"..\lib\native\x64\libwapiti.dll",                    Some @"native\x64\",            None)
                                      (@"..\lib\native\x64\libwapiti_swig.dll",               Some @"native\x64",             None)
                                      (@"..\lib\java\Grobid.Java.dll",                        Some @"lib\net45",              None)
                                      (@"..\src\Grobid\bin" @@ buildMode @@ "Grobid.Net.dll", Some @"lib\net45",              None) ] })
                        ("nuget/" + projectName + ".nuspec")
)

// == grobid.models.X.Y.Z.nupkg

Target "NuGet.Models" (fun _ ->
                    NuGet (fun x ->
                        { x with
                            Authors = authors
                            Project = project
                            Summary = summary
                            Description = description
                            Version = release.NugetVersion
                            ReleaseNotes = String.Join(Environment.NewLine, release.Notes)
                            Tags = tags
                            OutputPath = "publish"
                            AccessKey = getBuildParamOrDefault "nugetkey" ""
                            Publish = hasBuildParam "nugetkey"
                            Files = [ (@"..\grobid.zip", Some "content", None) ] })
                        ("nuget/" + project + ".nuspec")
)

Target "Default" DoNothing

"Clean"
  ==> "BuildApp"

"UnitTests"
  ==> "Default"

RunTargetOrDefault "Default"
