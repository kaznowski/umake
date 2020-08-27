using System;
using UnityEditor;
using UnityEngine;
using UnityMake;

public class UpdateOrIncrementBuildVersionCode : UMakeBuildAction
{
    /// <summary>
    /// If target VersionCode is Equals 
    /// </summary>
    [Tooltip(
        "If targetVersionCode equals or less than 0 (ZERO), this action will ignore the number and will work incrementing the current build version. Remember: CLI Number always has priority")]
    [SerializeField]
    private int targetVersionCode = -1;

    public override void Execute(UMake umake, UMakeTarget target)
    {
        var scriptableTarget = targetVersionCode.ToString();

        if (targetVersionCode < 0)
        {
            if (target.buildTarget == BuildTarget.Android)
            {
                PlayerSettings.Android.bundleVersionCode++;
                scriptableTarget = PlayerSettings.Android.bundleVersionCode.ToString();
            }
            else if (target.buildTarget == BuildTarget.iOS)
            {
                if (int.TryParse(PlayerSettings.iOS.buildNumber, out int buildNumber))
                {
                    buildNumber++;
                    scriptableTarget = buildNumber.ToString();
                    PlayerSettings.iOS.buildNumber = buildNumber.ToString();
                }
            }
        }

        string externalVersion = scriptableTarget;
        umake.Parameters.UpdateIfValid("build.number", ref externalVersion);

        if (!string.Equals(externalVersion, scriptableTarget, StringComparison.CurrentCultureIgnoreCase))
        {
            if (target.buildTarget == BuildTarget.Android)
            {
                if (int.TryParse(externalVersion, out int b))
                {
                    PlayerSettings.Android.bundleVersionCode = b;
                }
            }
            else if (target.buildTarget == BuildTarget.iOS)
            {
                PlayerSettings.iOS.buildNumber = externalVersion;
            }
        }

        Debug.Log($"Update Version number to: {externalVersion}");
    }
}

public static class UMakeParameterProviderExtensions
{
    public static void UpdateIfValid(this IUMakeParameterProvider parameters, string key, ref string target)
    {
        if (parameters == null)
            return;

        var val = parameters.GetValue(key);
        if (!string.IsNullOrEmpty(val))
            target = val;
    }
}