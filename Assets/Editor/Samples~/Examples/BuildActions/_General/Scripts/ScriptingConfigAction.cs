using UnityEditor;
using UnityEngine;

namespace UnityMake
{
    public class ScriptingConfigAction : UMakeBuildAction
    {
        [SerializeField]
        private BuildTargetGroup targetGroup;
        [SerializeField]
        private ScriptingImplementation scriptingImplementation;


        public override void Execute(UMake umake, UMakeTarget target)
        {
            PlayerSettings.SetScriptingBackend(targetGroup, scriptingImplementation);
        }

        private void UpdateIfValid(IUMakeParameterProvider parameters, string key, ref string target)
        {
            var val = parameters.GetValue(key);
            if (!string.IsNullOrEmpty(val))
                target = val;
        }
    }
}