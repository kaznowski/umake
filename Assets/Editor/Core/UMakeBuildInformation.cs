using System;
using UnityEngine;


public class UMakeBuildInformation : ScriptableObject
{
    [Header("Platform Build Infos")]
    public RuntimePlatform Platform;
    public RuntimePlatform EditorPlatform;
    public string TargetGroup;
    public string BuildSymbols;
    
    [Header("Bundle Infos")]
    public string ApplicationIdentifier;
    public string Version;
    public string VersionCode;
    public string BuildDate;
    public int BuildTimeZone = -3;
    
    [Header("Commit Infos")]
    public string CommitUID = "undefined";
    public string CommitBranch = "undefined";

    public string CommitUIDShort => CommitUID.Substring(0, 7);
}