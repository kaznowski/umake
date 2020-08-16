using UnityEditor;

namespace UnityMake
{
	public sealed class AndroidKeyAction : UMakeBuildAction
	{
		public bool useCustomKeystore = false;
		public string keyStorePath = "user.keystore";
		public string keyStorePassword = "keyStorePassword";
		public string keyAliasName = "aliasName";
		public string keyAliasPassword = "keyAliasPassword";
	

		public override void Execute(UMake umake, UMakeTarget target)
		{
			if (umake.Parameters != null)
			{
				UpdateIfValid(umake.Parameters, "umake.android.keystore.path", ref keyStorePath);
				UpdateIfValid(umake.Parameters, "umake.android.keystore.keyPassword", ref keyStorePassword);
				UpdateIfValid(umake.Parameters, "umake.android.keystore.alias", ref keyAliasName);
				UpdateIfValid(umake.Parameters, "umake.android.keystore.aliasPassword", ref keyAliasPassword);
			}

			PlayerSettings.Android.useCustomKeystore = useCustomKeystore;
			PlayerSettings.keystorePass = keyStorePassword;
			PlayerSettings.keyaliasPass = keyAliasPassword;
			PlayerSettings.Android.keystoreName = keyStorePath;
			PlayerSettings.Android.keyaliasName = keyAliasName;
		}

		private void UpdateIfValid(IUMakeParameterProvider parameters, string key, ref string target)
		{
			var val = parameters.GetValue(key);
			if (!string.IsNullOrEmpty(val))
				target = val;
		}
	}
}
