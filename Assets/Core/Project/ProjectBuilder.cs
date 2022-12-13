using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Core.Project{
	public static class ProjectBuilder{
		[MenuItem("Project Builder/Build project Default", false, 0)]
		public static void BuildProject(){
			var target = EditorUserBuildSettings.activeBuildTarget;
			var buildSetting = new BuildSetting("", BuildSetting.DefineSymbolConfig.Release);

			// Handle command line arguments.
			if(UnityEditorInternal.InternalEditorUtility.inBatchMode){
				var commandLineArgumentToValue = ParseCommandLineArgument();

				if(commandLineArgumentToValue.ContainsKey("-outputPath"))
					buildSetting.OutputPath = commandLineArgumentToValue["-outputPath"];
				if(commandLineArgumentToValue.ContainsKey("-defineSymbolConfig"))
					buildSetting.SymbolConfig = (BuildSetting.DefineSymbolConfig)Enum.Parse(
						typeof(BuildSetting.DefineSymbolConfig), commandLineArgumentToValue["-defineSymbolConfig"]);
			}

			BuildProject(target, buildSetting);
		}

		[MenuItem("Project Builder/Build project on Android", false, 0)]
		public static void BuildProjectAndroid(){
			BuildProject(BuildTarget.Android, new BuildSetting("", BuildSetting.DefineSymbolConfig.Release));
		}

		private static void BuildProject(BuildTarget buildTarget, BuildSetting buildSetting){
			var defineSymbolBeforeBuild =
					PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildPipeline.GetBuildTargetGroup(buildTarget));
			var buildPlayerOption = new BuildPlayerOptions{
				scenes = EditorBuildSettings.scenes.Where((s) => s.enabled)
						.Select((s) => s.path).ToArray(),
				locationPathName = GetBuildPath(buildTarget, buildSetting.OutputPath),
				target = buildTarget,
				options = BuildOptions.None,
			};
			PlayerSettings.SetScriptingDefineSymbolsForGroup(
				BuildPipeline.GetBuildTargetGroup(buildTarget),
				BuildSetting.symbolConfigToDefineSymbol[buildSetting.SymbolConfig]
			);

			var buildReport = BuildPipeline.BuildPlayer(buildPlayerOption);
			if(buildReport.summary.result == BuildResult.Succeeded){
				Debug.Log("[ProjectBuilder] Build Success: Time:" + buildReport.summary.totalTime + " Size:" +
						  buildReport.summary.totalSize + " bytes");
				if(UnityEditorInternal.InternalEditorUtility.inBatchMode)
					EditorApplication.Exit(0);
			}
			else{
				if(UnityEditorInternal.InternalEditorUtility.inBatchMode)
					EditorApplication.Exit(1);
				throw new Exception("[ProjectBuilder] Build Failed: Time:" + buildReport.summary.totalTime +
									" Total Errors:" + buildReport.summary.totalErrors);
			}

			PlayerSettings.SetScriptingDefineSymbolsForGroup(
				BuildPipeline.GetBuildTargetGroup(buildTarget), defineSymbolBeforeBuild);
			AssetDatabase.SaveAssets();
			Debug.Log(buildSetting);
			Debug.Log("Build project at: " + buildPlayerOption.locationPathName);
		}

		private static string GetBuildPath(BuildTarget buildTarget, string outputPath = ""){
			var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
			var fileName = PlayerSettings.productName + GetFileExtension(buildTarget);
			var timeStamp = DateTime.Now.ToString("yyyyMMdd-HH-mm");

			outputPath = (outputPath == "") ? desktopPath : outputPath;
			var buildPath = Path.Combine(outputPath, PlayerSettings.productName, $"{buildTarget}_{timeStamp}",
				fileName);
			buildPath = buildPath.Replace(@"\", @"\\");

			return buildPath;
		}

		private static string GetFileExtension(BuildTarget target){
			// ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
			switch(target){
				case BuildTarget.StandaloneWindows64:
					return ".exe";
				case BuildTarget.StandaloneOSX:
					return ".app";
				case BuildTarget.StandaloneLinux64:
					return ".x86_64";
				case BuildTarget.WebGL:
					return ".webgl";
				case BuildTarget.Android:
					return EditorUserBuildSettings.buildAppBundle ? ".aab" : ".apk";
				case BuildTarget.iOS:
					return ".iosversion";
				default:
					throw new Exception("No corresponding extension!");
			}
		}

		private static Dictionary<string, string> ParseCommandLineArgument(){
			var commandLineArgToValue = new Dictionary<string, string>();
			string[] customCommandLineArg ={ "-outputPath", "-defineSymbolConfig" };
			var commandLineArg = Environment.GetCommandLineArgs();

			for(var i = 0; i < commandLineArg.Length; i++){
				foreach(var arg in customCommandLineArg)
					if(commandLineArg[i] == arg)
						commandLineArgToValue.Add(arg,
							commandLineArg[(i + 1) % commandLineArg.Length]);
			}

			return commandLineArgToValue;
		}
	}

	public class BuildSetting{
		public enum DefineSymbolConfig{
			Debug,
			Release
		}

		public static readonly Dictionary<DefineSymbolConfig, string> symbolConfigToDefineSymbol =
				new(){
					[DefineSymbolConfig.Debug] = "Debug;",
					[DefineSymbolConfig.Release] = "Release;"
				};

		public string OutputPath;
		public DefineSymbolConfig SymbolConfig;

		public BuildSetting(string outputPath, DefineSymbolConfig symbolConfig){
			OutputPath = outputPath;
			SymbolConfig = symbolConfig;
		}

		public override string ToString(){
			return $"{nameof(BuildSetting)}: symbolConfig={SymbolConfig}, outputPath={OutputPath}";
		}
	}
}