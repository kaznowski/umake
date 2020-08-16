using UnityEditor;
using UnityEngine;

namespace UnityMake
{
    public sealed class UpdateApplicationIdentifier : UMakeBuildAction
    {
        [SerializeField] private string applicationIdentifier = "com.doublecake.studio";


        public override void Execute(UMake umake, UMakeTarget target)
        {
            if (umake.Parameters != null)
            {
                UpdateIfValid(umake.Parameters, "umake.android.packagename", ref applicationIdentifier);
            }

            PlayerSettings.applicationIdentifier = applicationIdentifier;
        }

        private void UpdateIfValid(IUMakeParameterProvider parameters, string key, ref string target)
        {
            var val = parameters.GetValue(key);
            if (!string.IsNullOrEmpty(val))
                target = val;
        }
    }
}