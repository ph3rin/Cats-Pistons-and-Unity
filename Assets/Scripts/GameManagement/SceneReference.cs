using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

using UnityEngine.SceneManagement;


namespace CatProcessingUnit.GameManagement
{
    [CreateAssetMenu(menuName = "CPU/Scene Reference")]
    public class SceneReference : ScriptableObject
    {
#if UNITY_EDITOR
        [SerializeField] private SceneAsset _scene;

        public bool Preprocess()
        {
            var newBuildIndex = SceneUtility.GetBuildIndexByScenePath(
                AssetDatabase.GetAssetPath(_scene));
            if (newBuildIndex != _buildIndex)
            {
                EditorUtility.SetDirty(this);
                _buildIndex = newBuildIndex;
            }
            return _buildIndex >= 0;
        }

        public Scene LoadInEditor(bool additive = true)
        {
            var mode = additive ? OpenSceneMode.Additive : OpenSceneMode.Single;
            return EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(_scene), mode);
        }
#endif
        [SerializeField] private int _buildIndex;

        public int BuildIndex
        {
            get => _buildIndex;
            set => _buildIndex = value;
        }
    }
}