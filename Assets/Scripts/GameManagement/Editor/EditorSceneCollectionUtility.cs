using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace CatProcessingUnit.GameManagement.Editor
{
    public static class EditorSceneCollectionUtility
    {
        public static void LoadInEditor(this SceneCollection sceneCollection)
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            var scenesToClose =
                Enumerable.Range(0, SceneManager.sceneCount).Select(SceneManager.GetSceneAt).ToList();
            foreach (var scene in scenesToClose)
            {
                EditorSceneManager.CloseScene(scene, true);
            }

            EditorSceneManager.OpenScene(EditorBuildSettings.scenes.First(s => s.enabled).path);
            var sceneRefs = sceneCollection.ToList();
            for (var sceneId = 0; sceneId < sceneRefs.Count; sceneId++)
            {
                var sceneRef = sceneRefs[sceneId];
                var runtimeScene = sceneRef.LoadInEditor();
                if (sceneId == sceneRefs.Count - 1)
                {
                    SceneManager.SetActiveScene(runtimeScene);
                }
            }
        }
    }
}