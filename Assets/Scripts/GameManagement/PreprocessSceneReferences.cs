#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace CatProcessingUnit.GameManagement
{
    public class PreprocessSceneReferences : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void PreprocessPlaymode()
        {
            PreprocessAllSceneReferences();
        }
        
        public void OnPreprocessBuild(BuildReport report)
        {
            PreprocessAllSceneReferences();
        }

        private static void PreprocessAllSceneReferences()
        {
            foreach (var guid in AssetDatabase.FindAssets($"t:{nameof(SceneReference)}"))
            {
                var sceneRef = AssetDatabase.LoadAssetAtPath<SceneReference>(
                    AssetDatabase.GUIDToAssetPath(guid));
                if (sceneRef == null) continue;
                if (!sceneRef.Preprocess())
                {
                    throw new BuildFailedException($"{sceneRef.name} is not included" +
                                                   $"in the build settings");
                }
            }
        }
    }
}

#endif