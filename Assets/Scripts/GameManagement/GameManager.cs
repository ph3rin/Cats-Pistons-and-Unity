using System;
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

        [SerializeField] private SceneCollection _startSceneCollection;
        private static bool _isLoadingScenes;
        
        private void Awake()
        {
            I = this;
#if !UNITY_EDITOR
            ActivateOtherGameObjects();
            LoadSceneCollection(_startSceneCollection);
#endif
        }

        private void Bootstrap()
        {
            BootstrapServices();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void InitializeGameManager()
        {
            I = null;
            _isLoadingScenes = false;
#if UNITY_EDITOR
            _firstEnterPlaymode = true;
            _preprocessedSceneCount = 0;
#endif
        }

        public static void LoadSceneCollection(SceneCollection sceneCollection)
        {
            I.StartCoroutine(LoadScenes(sceneCollection.Select(s => s.BuildIndex).ToArray()));
        }

        private static void ActivateOtherGameObjects()
        {
            for (var index = 0; index < SceneManager.sceneCount; ++index)
            {
                var scene = SceneManager.GetSceneAt(index);
                var rootObjects = scene.GetRootGameObjects();
                foreach (var rootObject in rootObjects)
                {
                    if (rootObject == I.gameObject || rootObject.name != "__ROOT__") continue;
                    rootObject.SetActive(true);
                    for (var i = rootObject.transform.childCount - 1; i >= 0; --i)
                    {
                        rootObject.transform.GetChild(i).SetParent(null);
                    }
                }
            }
        }

        private static void BootstrapServices()
        {
            ServiceLocator.InitializeAllServices();
        }

        public static IEnumerator LoadScenes(int[] buildIndices)
        {
            if (_isLoadingScenes)
            {
                Debug.LogError("Scene loading already in progress");
                yield break;
            }

            _isLoadingScenes = true;
            yield return UnloadAllScenesInPlaymode();
            var scenesRemaining = buildIndices.Length;
            var finished = false;
            foreach (var buildIndex in buildIndices)
            {
                SceneManager.LoadScene(buildIndex, LoadSceneMode.Additive);
            }

            CallbackAfterSceneAwaken(scenesRemaining, () =>
            {
#if !UNITY_EDITOR
                    ActivateOtherGameObjects();
#endif
                BootstrapServices();
                finished = true;
            });

            yield return new WaitUntil(() => finished);
            _isLoadingScenes = true;
        }

        private static void CallbackAfterSceneAwaken(int count, Action callback)
        {
            void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
            {
                if (--count == 0)
                {
                    SceneManager.sceneLoaded -= HandleSceneLoaded;
                    callback?.Invoke();
                }
            }

            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private static Coroutine UnloadAllScenesInPlaymode()
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

            return I.StartCoroutine(Wait());
        }

        private static List<Scene> GetAllOtherLoadedScenes()
        {
            return
                Enumerable.Range(0, SceneManager.sceneCount)
                    .Select(SceneManager.GetSceneAt)
                    .Where(s => s != I.gameObject.scene)
                    .ToList();
        }

        private static bool _firstEnterPlaymode;
        private static int _preprocessedSceneCount;

        public static void NotifyScenePreprocessed()
        {
#if UNITY_EDITOR
            if (!_firstEnterPlaymode) return;
            _preprocessedSceneCount++;
            if (_preprocessedSceneCount == SceneManager.sceneCount)
            {
                I.Bootstrap();
                _firstEnterPlaymode = false;
            }
#endif
        }
    }
}