using CatProcessingUnit.GameManagement;
using UnityEngine;

namespace CatProcessingUnit.Audio
{
    [RequireComponent(typeof(RegisterService))]
    public class AudioManager : MonoBehaviour, IService
    {
        [SerializeField] private OneShotPlayer _oneShotPlayerPrefab;

        public OneShotPlayer CreatePlayer()
        {
            return Instantiate(_oneShotPlayerPrefab);
        }
        
        public void Init()
        {
        }
    }
}