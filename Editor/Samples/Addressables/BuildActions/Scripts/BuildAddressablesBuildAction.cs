using System;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

namespace UnityMake
{
    public class BuildAddressablesBuildAction : UMakeBuildAction
    {
        public class InvalidProfileName : Exception
        {
            public InvalidProfileName(string message) : base(message)
            {
                
            }
        }
        
        public string profileName;
        public string buildPath;
        
        
        public override void Execute(UMake umake, UMakeTarget target)
        {
            var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
            
            if (umake.Parameters != null)
            {
                profileName = umake.Parameters.GetValue("umake.addressables.profile");
                buildPath = umake.Parameters.GetValue("umake.addressables.build_path");
            }

            if (!string.IsNullOrEmpty(profileName))
            {
                var profileId = addressableSettings.profileSettings.GetProfileId(profileName);
                if (string.IsNullOrEmpty(profileId))
                    throw new InvalidProfileName($"Couldn't find profile with name {profileName}");
                addressableSettings.activeProfileId = profileId;
            }
            if (!string.IsNullOrEmpty(buildPath))
            {
                var profileId = addressableSettings.activeProfileId;
                addressableSettings.profileSettings.SetValue(profileId, "RemoteBuildPath", buildPath);
            }
            
            AddressableAssetSettings.BuildPlayerContent();   
        }
    }

}