using UnityEditor;
using UnityEngine;

namespace CatProcessingUnit.GameManagement.Editor
{
    [CustomEditor(typeof(SceneCollection))]
    public class SceneCollectionInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.UpdateIfRequiredOrScript();
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Load"))
                {
                    (serializedObject.targetObject as SceneCollection).LoadInEditor();
                }
            }
            GUILayout.EndHorizontal();
        }
    }
}