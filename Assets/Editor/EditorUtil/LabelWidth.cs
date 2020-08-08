using UnityEngine;
using UnityEditor;

namespace UnityMake
{
	public struct LabelWidth : System.IDisposable
	{
		private readonly float savedLabelWidth;

		public static LabelWidth Do( float labelWidth )
		{
			var savedLabelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = labelWidth;

			return new LabelWidth( savedLabelWidth );
		}

		private LabelWidth( float savedLabelWidth )
		{
			this.savedLabelWidth = savedLabelWidth;
		}

		public void Dispose()
		{
			EditorGUIUtility.labelWidth = savedLabelWidth;
		}
	}
}