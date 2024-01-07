using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;

[CustomPropertyDrawer(typeof(Pool))]
public class PoolPropertyDrawer : PropertyDrawer
{
    #region Getters
    private float singleLineProperty { get { return EditorGUIUtility.singleLineHeight; } }
    private float singleLineWithHeaderProperty { get { return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing * 5; } }
    private float tagHeight { get { return singleLineWithHeaderProperty; } }
    private float prefabHeight { get { return singleLineProperty; } }
    private float sizeHeight { get { return singleLineProperty; } }
    private float isUIElementHeight { get { return singleLineProperty; } }
    private float renderModeHeight { get { return singleLineWithHeaderProperty; } }
    private float cameraHeight { get { return singleLineProperty; } }
    private float uiScaleModeHeight { get { return singleLineWithHeaderProperty; } }
    private float referenceResolutionHeight { get { return singleLineProperty; } }
    #endregion

    private SerializedProperty tag;
    private SerializedProperty prefab;
    private SerializedProperty size;
    #region UI Property
    private SerializedProperty isUIElement;
    #region Canvas
    private SerializedProperty renderMode;
    private SerializedProperty camera;
    #endregion
    #region Canvas Scaler
    private SerializedProperty uiScaleMode;
    private SerializedProperty referenceResolution;
    #endregion
    #endregion

    // Get the element height
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        isUIElement = property.FindPropertyRelative("isUIElement");
        renderMode = property.FindPropertyRelative("renderMode");
        uiScaleMode = property.FindPropertyRelative("uiScaleMode");

        float totalHeight = EditorGUIUtility.singleLineHeight;
        if (property.isExpanded)
        {
            totalHeight += GetTotalHeight();
            totalHeight += EditorGUIUtility.standardVerticalSpacing * 2f;
        }

        return totalHeight;
    }

    private float GetTotalHeight()
    {
        float poolHeight = tagHeight + prefabHeight + sizeHeight;
        float uiHeight = isUIElementHeight;
        float verticalSpacing = EditorGUIUtility.standardVerticalSpacing * 4;

        if (isUIElement.boolValue)
        {
            float canvasHeight = renderModeHeight;
            switch ((RenderMode)renderMode.enumValueIndex)
            {
                case RenderMode.WorldSpace:
                    break;
                case RenderMode.ScreenSpaceCamera:
                    canvasHeight += cameraHeight;
                    break;
                case RenderMode.ScreenSpaceOverlay:
                    break;
            }

            float canvasScalerHeight = uiScaleModeHeight;
            switch ((CanvasScaler.ScaleMode)uiScaleMode.enumValueIndex)
            {
                case CanvasScaler.ScaleMode.ConstantPixelSize:
                    break;
                case CanvasScaler.ScaleMode.ScaleWithScreenSize:
                    canvasScalerHeight += referenceResolutionHeight;
                    break;
                case CanvasScaler.ScaleMode.ConstantPhysicalSize:
                    break;
                default:
                    break;
            }
            uiHeight += canvasHeight + canvasScalerHeight;
        }

        float totalHeight = poolHeight + uiHeight + verticalSpacing;
        return totalHeight;
    }

    // Draw the drawer
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Find the properties
        tag = property.FindPropertyRelative("tag");
        prefab = property.FindPropertyRelative("prefab");
        size = property.FindPropertyRelative("size");
        isUIElement = property.FindPropertyRelative("isUIElement");
        renderMode = property.FindPropertyRelative("renderMode");
        camera = property.FindPropertyRelative("camera");
        uiScaleMode = property.FindPropertyRelative("uiScaleMode");
        referenceResolution = property.FindPropertyRelative("referenceResolution");

        // Make the foldout box
        Rect foldOutBox = new Rect(position.min.x, position.min.y, position.size.x, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(foldOutBox, property.isExpanded, label);

        // Draw the properties
        if (property.isExpanded)
        {
            Rect newPosition = position;

            DrawTagProperty(ref newPosition);
            DrawPrefabProperty(ref newPosition);
            DrawSizeProperty(ref newPosition);
            DrawIsUIElementProperty(ref newPosition);
            if (isUIElement.boolValue)
            {
                DrawRenderModeProperty(ref newPosition);
                switch ((RenderMode)renderMode.enumValueIndex)
                {
                    case RenderMode.ScreenSpaceCamera:
                        DrawRenderModeScreenSpaceCameraProperties(ref newPosition);
                        break;
                    default:
                        break;
                }

                DrawUIScaleModeProperty(ref newPosition);
                switch ((CanvasScaler.ScaleMode)uiScaleMode.enumValueIndex)
                {
                    case CanvasScaler.ScaleMode.ScaleWithScreenSize:
                        DrawUIScaleModeScaleWithScreenSizeProperties(ref newPosition);
                        break;
                    default:
                        break;
                }
            }
        }
        EditorGUI.EndProperty();
    }

    private void DrawTagProperty(ref Rect position)
    {
        float posX = position.min.x;
        float posY = position.min.y + EditorGUIUtility.singleLineHeight;
        float width = position.size.x;
        float height = tagHeight;
        //float height = EditorGUIUtility.singleLineHeight;

        // Set the position for the next property
        position = new Rect(posX, posY + height + EditorGUIUtility.standardVerticalSpacing, width, height);

        // Draw area for tag property
        Rect drawArea = new Rect(posX, posY, width, height);
        EditorGUI.PropertyField(drawArea, tag, new GUIContent("Tag"));
    }

    private void DrawPrefabProperty(ref Rect position)
    {
        float posX = position.min.x;
        float posY = position.min.y;
        float width = position.size.x;
        float height = prefabHeight;

        // Set the position for the next property
        position = new Rect(posX, posY + height + EditorGUIUtility.standardVerticalSpacing, width, height);

        // Draw area for the prefab property
        Rect drawArea = new Rect(posX, posY, width, height);
        EditorGUI.PropertyField(drawArea, prefab, new GUIContent("Prefab"));
    }

    private void DrawSizeProperty(ref Rect position)
    {
        float posX = position.min.x;
        float posY = position.min.y;
        float width = position.size.x;
        float height = sizeHeight;

        // Set the position for the next property
        position = new Rect(posX, posY + height + EditorGUIUtility.standardVerticalSpacing, width, height);

        // Draw area for the prefab property
        Rect drawArea = new Rect(posX, posY, width, height);
        EditorGUI.PropertyField(drawArea, size, new GUIContent("Size"));
    }

    private void DrawIsUIElementProperty(ref Rect position)
    {
        float posX = position.min.x;
        float posY = position.min.y;
        float width = position.size.x;
        float height = isUIElementHeight;
        //float height = EditorGUIUtility.singleLineHeight;

        // Set the position for the next property
        position = new Rect(posX, posY + height + EditorGUIUtility.standardVerticalSpacing, width, height);

        // Draw area for the isUIElement property
        Rect drawArea = new Rect(posX, posY, width, height);
        EditorGUI.PropertyField(drawArea, isUIElement, new GUIContent("Is UI Element"));
    }

    private void DrawRenderModeProperty(ref Rect position)
    {
        float posX = position.min.x;
        float posY = position.min.y;
        float width = position.size.x;
        float height = renderModeHeight;

        // Set the position for the next property
        position = new Rect(posX, posY + height + EditorGUIUtility.standardVerticalSpacing, width, height);

        // Draw area for the renderMode property
        Rect drawArea = new Rect(posX, posY, width, height);
        EditorGUI.PropertyField(drawArea, renderMode, new GUIContent("Render Mode"));
    }

    private void DrawRenderModeScreenSpaceCameraProperties(ref Rect position)
    {
        DrawCameraProperty(ref position);
    }

    private void DrawCameraProperty(ref Rect position)
    {
        float posX = position.min.x;
        float posY = position.min.y;
        float width = position.size.x;
        float height = cameraHeight;

        // Set the position for the next property
        position = new Rect(posX, posY + height + EditorGUIUtility.standardVerticalSpacing, width, height);

        // Draw area for the camera property
        Rect drawArea = new Rect(posX, posY, width, height);
        EditorGUI.PropertyField(drawArea, camera, new GUIContent("Render Camera"));
    }

    private void DrawUIScaleModeProperty(ref Rect position)
    {
        float posX = position.min.x;
        float posY = position.min.y;
        float width = position.size.x;
        float height = uiScaleModeHeight;

        // Set the position for the next property
        position = new Rect(posX, posY + height + EditorGUIUtility.standardVerticalSpacing, width, height);

        // Draw area for the camera property
        Rect drawArea = new Rect(posX, posY, width, height);
        EditorGUI.PropertyField(drawArea, uiScaleMode, new GUIContent("UI Scale Mode"));
    }

    private void DrawUIScaleModeScaleWithScreenSizeProperties(ref Rect position)
    {
        DrawReferenceResolutionProperty(ref position);
    }

    private void DrawReferenceResolutionProperty(ref Rect position)
    {
        float posX = position.min.x;
        float posY = position.min.y;
        float width = position.size.x;
        float height = referenceResolutionHeight;

        // Set the position for the next property
        position = new Rect(posX, posY + height + EditorGUIUtility.standardVerticalSpacing, width, height);

        // Draw area for the camera property
        Rect drawArea = new Rect(posX, posY, width, height);
        EditorGUI.PropertyField(drawArea, referenceResolution, new GUIContent("Reference Resolution"));
    }
}

