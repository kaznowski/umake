using UnityEditor;
using UnityEngine;

namespace UnityMake
{
    public class AndroidScriptionConfigAction : ScriptingConfigAction
    {
        [SerializeField] private bool ARMv7 = true;
        [SerializeField] private bool ARM64 = false;
        [SerializeField] private bool BuildAppBundle = false;

        public override void Execute(UMake umake, UMakeTarget target)
        {
            base.Execute(umake,target);
            var both = ARM64 && ARMv7;
            if (both)
            {
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
            }
            else if (ARMv7)
            {
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
            }
            else if(ARM64)
            {
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            }
            
            EditorUserBuildSettings.buildAppBundle = BuildAppBundle;
        }
    }
}