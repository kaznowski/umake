using UnityEditor;
using UnityEngine;

namespace UnityMake
{
	[CustomEditor( typeof( UMakeBuildAction ), true )]
	public sealed class UMakeBuildActionEditor : Editor
	{
		public void DrawBaseInspectorGUI()
		{
			base.OnInspectorGUI();
		}

		public override void OnInspectorGUI()
		{
			var buildAction = target as UMakeBuildAction;
			buildAction.OnInspectorGUI( this );
		}
	}
}