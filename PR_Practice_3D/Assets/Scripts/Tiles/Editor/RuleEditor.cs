using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(Rule))]
public class RuleEditor : PropertyDrawer {

	private readonly Color noneColour = Color.grey;
	private readonly Color notMatchColour = Color.red;
	private readonly Color matchColour = Color.green;

	private const float LAYER_SIZE = 99;
	private const float TOP_MARGIN = 48;
	private const float LEFT_MARGIN = 50;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		return (LAYER_SIZE * 3) + 16 + TOP_MARGIN;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

		EditorGUI.BeginProperty(position, label, property);

		// Draw label
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

		// Calculate rects
		var layer0Rect = new Rect(position.x + LEFT_MARGIN, position.y + TOP_MARGIN, LAYER_SIZE, LAYER_SIZE);
		var layer1Rect = new Rect(position.x + LEFT_MARGIN, position.y + LAYER_SIZE + 8 + TOP_MARGIN, LAYER_SIZE, LAYER_SIZE);
		var layer2Rect = new Rect(position.x + LEFT_MARGIN, position.y + (LAYER_SIZE * 2) + 16 + TOP_MARGIN, LAYER_SIZE, LAYER_SIZE);

		SerializedProperty rules = property.FindPropertyRelative("ruleArea");


		EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, 16), property.FindPropertyRelative("mesh"));
		EditorGUI.PropertyField(new Rect(position.x, position.y + 16, position.width, 16), property.FindPropertyRelative("meshRotation"));

		//EditorGUI.PrefixLabel(layer0Rect, new GUIContent("Top"));
		DrawLayerGrid(layer0Rect, rules);
		//EditorGUI.PrefixLabel(layer1Rect, new GUIContent("Middle"));
		DrawMidLayerGrid(layer1Rect, rules);
		//EditorGUI.PrefixLabel(layer2Rect, new GUIContent("Bottom"));
		DrawLayerGrid(layer2Rect, rules, 18);

		EditorGUI.EndProperty();
	}

	private void DrawLayerGrid(Rect position, SerializedProperty property, int offset = 0) {

		float width = position.width / 3f;
		float height = position.height / 3f;

		for (int x = 0; x < 3; x++) {
			for (int y = 0; y < 3; y++) {
				int index = offset + (x * 3) + y;
				
				Rect rect = new Rect(position.x + (width * x) + x, position.y + (width * y) + y, width - 2, height - 2);
				if (index < property.arraySize) DrawGridCell(rect, property.GetArrayElementAtIndex(index));
			}
		}

	}

	private void DrawMidLayerGrid(Rect position, SerializedProperty property) {

		float width = position.width / 3f;
		float height = position.height / 3f;

		for (int x = 0; x < 3; x++) {
			for (int y = 0; y < 3; y++) {
				if (x == 1 && y == 1) continue;

				int index;
				if (y == 1 && x > 1) {
					index = 9 + ((x - 1) * 3) + y;
				} else {
					index = 9 + (x * 3) + y;
				}
				
				Rect rect = new Rect(position.x + (width * x) + x, position.y + (width * y) + y, width - 2, height - 2);
				if (index < property.arraySize) DrawGridCell(rect, property.GetArrayElementAtIndex(index));
			}
		}

	}

	private void DrawGridCell(Rect rect, SerializedProperty property) {

		int value = property.enumValueIndex;

		switch (value) {
			case (int) RuleAreaTypes.NONE:
				EditorGUI.DrawRect(rect, noneColour);
				break;
			case (int) RuleAreaTypes.NO_MATCH:
				EditorGUI.DrawRect(rect, notMatchColour);
				break;
			case (int) RuleAreaTypes.MATCH:
				EditorGUI.DrawRect(rect, matchColour);
				break;
		}

		Event e = Event.current;
		
		if (e.type == EventType.MouseDown) {
			if (rect.Contains(e.mousePosition)) {
				e.Use();
				property.enumValueIndex = (value + 1) % 3;
			}
		}
	}

}
