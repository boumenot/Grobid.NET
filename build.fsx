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

let projectName = "Grobid.NET"
let projectDescription = "GROBID for .NET"
let projectSummary = projectDescription

let tags = ""

let testResultsDir = "./testresults"

let release = parseReleaseNotes (IO.File.ReadAllLines "RELEASE_NOTES.md")

MSBuildDefaults <- {
  MSBuildDefaults with
    ToolsVersion = Some "12.0"
    Verbosity = Some MSBuildVerbosity.Minimal }

let buildMode = getBuildParamOrDefault "buildMode" "Debug" 

let setParams defaults = {
  defaults with
    ToolsVersion = Some("12.0")
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

// == grobid.X.Y.Z.nupkg

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
