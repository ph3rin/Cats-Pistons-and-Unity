using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
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
#endif
        [SerializeField] private int _buildIndex;

        public int BuildIndex
        {
            get => _buildIndex;
            set => _buildIndex = value;
        }
    }
}