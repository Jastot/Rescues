#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Rescues
{
    [CustomEditor(typeof(CameraTrigger))]
    public class CameraTriggerEditor : Editor
    {
        #region Fields

        CameraTrigger _cameraTrigger;
        SerializedProperty _targetPoint;
        SerializedProperty _focusTime;

        #endregion


        #region UnityMethods

        void OnEnable()
        {
            _cameraTrigger = (CameraTrigger)target;
            _targetPoint = serializedObject.FindProperty("TargetPoint");
            _focusTime = serializedObject.FindProperty("FocusTime");
        }

        private void OnSceneGUI()
        {
            EditorGUI.BeginChangeCheck();
            Vector3 newTargetPosition = Handles.PositionHandle(_cameraTrigger.TargetPoint, Quaternion.identity);
            Handles.DrawSolidDisc(_cameraTrigger.TargetPoint, Vector3.forward, 0.5f);
            Handles.Label(newTargetPosition + Vector3.down * 0.5f, $"{_cameraTrigger.name} camera trigger target point");
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_cameraTrigger, "Change TargetPoint");
                _cameraTrigger.TargetPoint = newTargetPosition;
                serializedObject.Update();
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(_targetPoint);
            EditorGUILayout.PropertyField(_focusTime);

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}
#endif