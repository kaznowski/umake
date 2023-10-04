using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityMake
{
	public sealed class UMakeTarget : ScriptableObject
	{
		public BuildTarget buildTarget = BuildTarget.StandaloneWindows;
#if UNITY_2021_1_OR_NEWER
		public StandaloneBuildSubtarget buildStandaloneSubtarget = StandaloneBuildSubtarget.Player;
#endif
		public BuildOptions buildOptions = BuildOptions.None;
		public string fileNameOverride = "Game_{TARGET}_{VERSION}.exe";

		public struct Path
		{
			public string path;
			public string directoryPath;
		}

		public List<UMakeBuildAction> preBuildActions;
		public List<UMakeBuildAction> postBuildActions;

		public void ExecutePreBuildActions(UMake umake)
		{
			foreach (var preBuildAction in preBuildActions)
			{
				Debug.LogFormat("Executing pre build action '{0}'", preBuildAction.name);

				if (preBuildAction != null)
					preBuildAction.Execute(umake, this);
			}
			umake.UpdateLastBuildInformation(this);
			AssetDatabase.SaveAssets();
		}

		public void ExecutePostBuildActions(UMake umake)
		{
			foreach (var postBuildAction in postBuildActions)
			{
				Debug.LogFormat("Executing post build action '{0}'", postBuildAction.name);

				if (postBuildAction != null)
					postBuildAction.Execute(umake, this);
			}
			
			AssetDatabase.SaveAssets();
		}

		public void Build(UMake umake, string buildPath)
		{
			if (string.IsNullOrEmpty(buildPath))
				return;

			ExecutePreBuildActions(umake);

			Path targetPath = new Path {path = buildPath};
			if (!UMakeCli.IsInCli)
			{
			
				targetPath = GetTargetPath(umake.version, UMake.GetBuildPath());

				if (Directory.Exists(targetPath.directoryPath))
				{
					Directory.Delete(targetPath.directoryPath, true);
					Directory.CreateDirectory(targetPath.directoryPath);
				}
			}
			else
			{
				UMake.BuildPathPref = buildPath;
			}

			string[] levels = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
#if UNITY_2021_1_OR_NEWER
			var buildPlayerOptions = new BuildPlayerOptions()
			{
				scenes = levels,
				locationPathName = targetPath.path,
				target = buildTarget,
				subtarget = (int) buildStandaloneSubtarget,
				options = buildOptions
			};
			BuildPipeline.BuildPlayer(buildPlayerOptions);
#else
			BuildPipeline.BuildPlayer(levels, targetPath.path, buildTarget, buildOptions);
#endif
		}

		public Path GetTargetPath(string version, string buildPath)
		{
			if (string.IsNullOrEmpty(buildPath))
				return new Path();

			buildPath = System.IO.Path.Combine(buildPath, name);
			if (!Directory.Exists(buildPath))
				Directory.CreateDirectory(buildPath);

			string targetName = name;
			if (!string.IsNullOrEmpty(fileNameOverride))
			{
				targetName = fileNameOverride;
				targetName = targetName.Replace("{VERSION}", "{0}");
				targetName = targetName.Replace("{TARGET}", "{1}");

				targetName = string.Format(targetName, version, name);
			}

			var targetPath = new Path();
			targetPath.directoryPath = buildPath;
			targetPath.path = System.IO.Path.Combine(buildPath, targetName);

			return targetPath;
		}
	}
}