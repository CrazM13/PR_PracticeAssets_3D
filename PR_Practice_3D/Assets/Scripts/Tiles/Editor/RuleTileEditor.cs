using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RuleTile))]
public class RuleTileEditor : Editor {

	SerializedProperty rules;

	private void OnEnable() {
		rules = serializedObject.FindProperty("rules");
	}

	public override void OnInspectorGUI() {
		
		for (int i = 0; i < rules.arraySize; i++) {
			EditorGUILayout.PropertyField(rules.GetArrayElementAtIndex(i));
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
