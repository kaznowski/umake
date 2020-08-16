using System;
using UnityMake;
using UnityEditor;
using UnityEngine;


namespace UnityMake
{
    public interface IUMakeParameterProvider
    {
        string UMakeTarget { get; }
        string UMakeBuildPath { get; }

        string GetValue(string key, string defaultValue = "");
    }

    public sealed class UMake : ScriptableObject
    {
        public const string buildPathPrefKey = "UMake_BuildPath_";

        public UMakeBuildInformation uMakeBuildInformation;

        public string version = "0.0";

        [UMakeTargetActions] public UMakeTarget[] targets;

        private static Option<UMake> instance = Functional.None;

        public IUMakeParameterProvider Parameters { get; set; }

        public static string BuildPathPref
        {
            get { return EditorPrefs.GetString(buildPathPrefKey + PlayerSettings.productName, ""); }
            set { EditorPrefs.SetString(buildPathPrefKey + PlayerSettings.productName, value); }
        }

        public static Option<UMake> Get()
        {
            instance = instance.OrElse(() => AssetDatabaseHelper.FindAssetOfType<UMake>());
            return instance;
        }

        public static Option<UMakeTarget> GetTarget(string targetName)
        {
            return
                from umake in Get()
                from target in umake.targets.First(t => t.name == targetName)
                select target;
        }

        public static string GetBuildPath()
        {
            string path = BuildPathPref;

            if (!string.IsNullOrEmpty(path))
                return path;

            return EditorUtility.OpenFolderPanel("Build Path", path, "Builds");
        }

        public void UpdateLastBuildInformation(UMakeTarget target)
        {
            if (uMakeBuildInformation == null)
            {
                Debug.LogWarning("Skip Last Build Information in UMake because information file not defined");
                return;
            }

            BuildTargetGroup targetGroup = BuildPipeline.GetBuildTargetGroup(target.buildTarget);
            uMakeBuildInformation.Platform = GetRunTimePlatform(target.buildTarget);
            uMakeBuildInformation.EditorPlatform = Application.platform;
            uMakeBuildInformation.TargetGroup = Enum.GetName(typeof(BuildTargetGroup), targetGroup);
            uMakeBuildInformation.ApplicationIdentifier = Application.identifier;
            uMakeBuildInformation.Version = Application.version;
            switch (target.buildTarget)
            {
                case BuildTarget.iOS:
                    uMakeBuildInformation.VersionCode = PlayerSettings.iOS.buildNumber;
                    break;
                case BuildTarget.Android:
                    uMakeBuildInformation.VersionCode = PlayerSettings.Android.bundleVersionCode.ToString();
                    break;
                default:
                    uMakeBuildInformation.VersionCode = "0";
                    break;
            }

            uMakeBuildInformation.BuildSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
            uMakeBuildInformation.CommitUID = "undefined";
            uMakeBuildInformation.CommitBranch = "undefined";
            if (Parameters != null)
            {
                uMakeBuildInformation.VersionCode =
                    Parameters.GetValue("build.number", uMakeBuildInformation.VersionCode);
                uMakeBuildInformation.CommitUID = Parameters.GetValue("umake.commit.uid", "undefined");
                uMakeBuildInformation.CommitBranch = Parameters.GetValue("umake.commit.branch", "undefined");
            }
        }

        private RuntimePlatform GetRunTimePlatform(BuildTarget targetBuildTarget)
        {
            switch (targetBuildTarget)
            {
                case BuildTarget.StandaloneOSX: return RuntimePlatform.OSXPlayer;
                case BuildTarget.StandaloneWindows: return RuntimePlatform.WindowsPlayer;
                case BuildTarget.iOS: return RuntimePlatform.IPhonePlayer;
                case BuildTarget.Android: return RuntimePlatform.Android;
                case BuildTarget.StandaloneWindows64: return RuntimePlatform.WindowsPlayer;
                case BuildTarget.WebGL: return RuntimePlatform.WebGLPlayer;
                case BuildTarget.WSAPlayer: return RuntimePlatform.WSAPlayerX64;
                case BuildTarget.StandaloneLinux64: return RuntimePlatform.LinuxPlayer;
                case BuildTarget.PS4: return RuntimePlatform.PS4;
                case BuildTarget.XboxOne: return RuntimePlatform.XboxOne;
                case BuildTarget.tvOS: return RuntimePlatform.tvOS;
                case BuildTarget.Switch: return RuntimePlatform.Switch;
                case BuildTarget.Lumin: return RuntimePlatform.Lumin;
                case BuildTarget.Stadia: return RuntimePlatform.Stadia;
            }

            return Application.platform;
        }
    }
}