#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CatProcessingUnit.GameManagement
{
    public class DeactivateSceneRootObjects : IProcessSceneWithReport
    {
        public int callbackOrder => 0;

        public void OnProcessScene(Scene scene, BuildReport report)
        {
            var isBuild = report != null;
            if (isBuild)
            {
                var roots = scene.GetRootGameObjects();
                var newRoot = new GameObject("ROOT");
                foreach (var go in roots)
                {
                    if (go.GetComponent<GameManager>() != null) continue;
                    go.transform.SetParent(newRoot.transform);
                }
                newRoot.SetActive(false);
            }
        }
    }
}

#endif