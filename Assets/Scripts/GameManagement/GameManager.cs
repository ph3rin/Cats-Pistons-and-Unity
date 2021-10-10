using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CatProcessingUnit.GameManagement
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager I { get; private set; }

        [SerializeField] private List<int> _startScenes;

        private void Awake()
        {
            I = this;
#if !UNITY_EDITOR
            ActivateOtherGameObjects();
            LoadStartScenes();
#else
            BootstrapScenes();
#endif
        }

        private void LoadStartScenes()
        {
            StartCoroutine(LoadScenes(_startScenes.ToArray()));
        }

        private void ActivateOtherGameObjects()
        {
            for (var index = 0; index < SceneManager.sceneCount; ++index)
            {
                var scene = SceneManager.GetSceneAt(index);
                var rootObjects = scene.GetRootGameObjects();
                foreach (var rootObject in rootObjects)
                {
                    if (rootObject == gameObject) return;
                    rootObject.SetActive(true);
                    for (var i = rootObject.transform.childCount - 1; i >= 0; --i)
                    {
                        rootObject.transform.GetChild(i).SetParent(null);
                    }
                }
            }
        }

        private void BootstrapScenes()
        {
            print($"Start game frame: {Time.frameCount}");
            ServiceLocator.InitializeAllServices();
        }

        public IEnumerator LoadScenes(int[] buildIndices)
        {
            yield return UnloadAllScenesInPlaymode();
            var scenesRemaining = buildIndices.Length;
            var finished = false;
            foreach (var buildIndex in buildIndices)
            {
                SceneManager.LoadScene(buildIndex, LoadSceneMode.Additive);
            }

            void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
            {
                if (--scenesRemaining == 0)
                {
                    SceneManager.sceneLoaded -= HandleSceneLoaded;
#if !UNITY_EDITOR
                    ActivateOtherGameObjects();
#endif
                    BootstrapScenes();
                    finished = true;
                }
            }

            SceneManager.sceneLoaded += HandleSceneLoaded;
            yield return new WaitUntil(() => finished);
        }

        private Coroutine UnloadAllScenesInPlaymode()
        {
            var otherScenes = GetAllOtherLoadedScenes();
            var operations = otherScenes.Select(SceneManager.UnloadSceneAsync);

            IEnumerator Wait()
            {
                foreach (var op in operations)
                {
                    yield return op;
                }
            }

            return StartCoroutine(Wait());
        }

        private List<Scene> GetAllOtherLoadedScenes()
        {
            return
                Enumerable.Range(0, SceneManager.sceneCount)
                    .Select(SceneManager.GetSceneAt)
                    .Where(s => s != gameObject.scene)
                    .ToList();
        }
    }
}