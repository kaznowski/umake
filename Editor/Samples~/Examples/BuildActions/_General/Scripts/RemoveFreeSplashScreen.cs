using UnityEditor;
using UnityEngine;
using UnityMake;

public class RemoveFreeSplashScreen : UMakeBuildAction
{
	public override void Execute(UMake umake, UMakeTarget target)
	{
		if (Application.HasProLicense())
		{
			PlayerSettings.SplashScreen.showUnityLogo = false;
			PlayerSettings.SplashScreen.show = false;
		}
	}
}