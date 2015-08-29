#r @"packages\FAKE\tools\FakeLib.dll"
open Fake
open Fake.Testing

let authors = ["Christopher Boumenot"]

let projectName = "Grobid.NET"
let projectDescription = "GROBID for .NET"
let projectSummary = projectDescription

let testResultsDir = "./testresults"

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

Target "Default" DoNothing

"Clean"
  ==> "BuildApp"

"UnitTests"
  ==> "Default"

RunTargetOrDefault "Default"
