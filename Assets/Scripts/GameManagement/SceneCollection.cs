using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



namespace CatProcessingUnit.GameManagement
{
    [CreateAssetMenu(menuName = "CPU/Scene Collection")]
    public class SceneCollection : ScriptableObject, IReadOnlyList<SceneReference>
    {
        [SerializeField] private SceneCollection _basedOn;
        [SerializeField] private List<SceneReference> _sceneReferences;

        public IEnumerator<SceneReference> GetEnumerator()
        {
            return _basedOn != null
                ? _basedOn.Concat(_sceneReferences).GetEnumerator()
                : _sceneReferences.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) this).GetEnumerator();
        }

        public int Count => _sceneReferences.Count;

        public SceneReference this[int index] => _sceneReferences[index];
    }
}