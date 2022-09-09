using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CatProcessingUnit.GameManagement.Editor
{
    public class SceneReferenceCreationMenu
    {
        private const string MENU_NAME = "Assets/CPU/Create Scene Reference";

        [MenuItem(MENU_NAME, priority = -1000)]
        public static void CreateReference()
        {
            foreach (var obj in Selection.objects)
            {
                Debug.Assert(obj is SceneAsset);
                CreateReferenceFor(obj as SceneAsset);
            }
        }

        private static void CreateReferenceFor(SceneAsset sceneAsset)
        {
            var sceneRef = ScriptableObject.CreateInstance<SceneReference>();
            var serializedObj = new SerializedObject(sceneRef);
            serializedObj.FindProperty("_scene").objectReferenceValue = sceneAsset;
            serializedObj.ApplyModifiedProperties();
            var targetPath = AssetDatabase.GetAssetPath(sceneAsset).Replace(".unity", ".asset");
            Debug.Assert(!targetPath.Contains(".unity"));
            AssetDatabase.CreateAsset(sceneRef, targetPath);
        }

        [MenuItem(MENU_NAME, isValidateFunction: true)]
        private static bool Validate()
        {
            return Selection.objects.All(o => o is SceneAsset);
        }
    }
}