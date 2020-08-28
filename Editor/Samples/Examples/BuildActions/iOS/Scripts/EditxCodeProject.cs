using System;
using UnityEditor;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using System.IO;
using UnityEngine;
using UnityMake;

public class EditxCodeProject : UMakeBuildAction // Will execute after XCode project is built
{
    [SerializeField] private xCodeProjectEntry[] _projectEntries;

    public override void Execute(UMake umake, UMakeTarget target)
    {
        if (target.buildTarget == BuildTarget.iOS) // Check if the build is for iOS 
        {
#if UNITY_IOS

            string plistPath = target.GetTargetPath(umake.version, UMake.GetBuildPath()).path + "/Info.plist";

            if (UMakeCli.IsInCli)
                plistPath = UMake.GetBuildPath() + "/Info.plist";

            PlistDocument plist = new PlistDocument(); // Read Info.plist file into memory
            plist.ReadFromString(File.ReadAllText(plistPath));

            PlistElementDict rootDict = plist.root;

            foreach (var entry in _projectEntries)
            {
                UpdateEntry(rootDict, entry);
            }

            File.WriteAllText(plistPath, plist.WriteToString()); // Override Info.plist
#endif
        }
    }
#if UNITY_IOS

    private void UpdateEntry(PlistElementDict rootDict, xCodeProjectEntry entry)
    {
        switch (entry.Type)
        {
            case xCodeProjectEntry.ValueType.String:
                rootDict.SetString(entry.Key, entry.Value);
                break;
            case xCodeProjectEntry.ValueType.Bool:
                rootDict.SetBoolean(entry.Key, entry.GetBool());
                break;
            case xCodeProjectEntry.ValueType.Float_Real:
                rootDict.SetReal(entry.Key, entry.GetReal());
                break;
            case xCodeProjectEntry.ValueType.DateTime:
                rootDict.SetDate(entry.Key, entry.GetDate());
                break;
            case xCodeProjectEntry.ValueType.Int:
                rootDict.SetInteger(entry.Key, entry.GetInt());
                break;
        }
    }
#endif
    [System.Serializable]
    public class xCodeProjectEntry
    {
        public string Key;
        public string Value;
        public ValueType Type;

        public enum ValueType
        {
            String,
            Bool,
            Float_Real,
            DateTime,
            Int,
        }

        public bool GetBool()
        {
            bool.TryParse(Value, out bool result);
            return result;
        }

        public float GetReal()
        {
            float.TryParse(Value, out float result);
            return result;
        }

        public DateTime GetDate()
        {
            DateTime.TryParse(Value, out DateTime result);
            return result;
        }

        public int GetInt()
        {
            int.TryParse(Value, out int result);
            return result;
        }
    }
}