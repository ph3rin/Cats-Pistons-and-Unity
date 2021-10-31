using CatProcessingUnit.GameManagement;
using UnityEngine;

namespace CatProcessingUnit.LevelManagement
{
    [CreateAssetMenu(menuName = "CPU/Level Descriptor")]
    public class LevelDescriptor : ScriptableObject
    {
        [SerializeField] private string _levelName;
        [SerializeField] private SceneCollection _sceneCollection;
        
        public string LevelName => _levelName;
        public SceneCollection SceneCollection => _sceneCollection;
    }
}