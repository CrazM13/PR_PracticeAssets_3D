using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PRTileArea.RuleTile {
	[CustomEditor(typeof(RuleTile))]
	public class RuleTileEditor : Editor {

		SerializedProperty ruleTileID;
		SerializedProperty thumbnail;
		SerializedProperty tileMaterial;
		SerializedProperty tileMesh;
		SerializedProperty rules;

		private void OnEnable() {
			rules = serializedObject.FindProperty("rules");
			tileMaterial = serializedObject.FindProperty("tileMaterial");
			tileMesh = serializedObject.FindProperty("tileMesh");
			ruleTileID = serializedObject.FindProperty("tileID");
			thumbnail = serializedObject.FindProperty("tileThumbnail");
		}

		public override void OnInspectorGUI() {

			if (!EditorGUIUtility.wideMode) {
				EditorGUIUtility.wideMode = true;
				EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 212;
			}

			EditorGUILayout.PropertyField(ruleTileID);
			EditorGUILayout.PropertyField(thumbnail);
			EditorGUILayout.PropertyField(tileMaterial);
			EditorGUILayout.PropertyField(tileMesh, new GUIContent("Default Mesh"));
			EditorGUILayout.Space(10);

			for (int i = 0; i < rules.arraySize; i++) {
				SerializedProperty arrayValue = rules.GetArrayElementAtIndex(i);
				EditorGUILayout.PropertyField(arrayValue, GUIContent.none);
			}

			Rect rect = EditorGUILayout.GetControlRect();

			Rect addRect = new Rect(rect.x, rect.y, rect.width / 2, rect.height);
			Rect removeRect = new Rect(rect.x + (rect.width / 2), rect.y, rect.width / 2, rect.height);

			if (GUI.Button(addRect, new GUIContent("Add Rule"))) {
				rules.arraySize += 1;
			}

			if (GUI.Button(removeRect, new GUIContent("Remove Rule"))) {
				if (rules.arraySize > 1) {
					rules.arraySize -= 1;
				}
			}

			serializedObject.ApplyModifiedProperties();
		}

	}
}
