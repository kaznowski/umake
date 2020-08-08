using UnityEditor;
using UnityEngine;

namespace UnityMake
{
	public struct Property : System.IDisposable
	{
		public static Property Do( Rect totalPosition, GUIContent label, SerializedProperty property )
		{
			EditorGUI.BeginProperty( totalPosition, label, property );
			return new Property();
		}

		public void Dispose()
		{
			EditorGUI.EndProperty();
		}
	}
}