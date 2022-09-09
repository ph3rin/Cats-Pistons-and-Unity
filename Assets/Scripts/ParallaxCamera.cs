using System;
using UnityEngine;

namespace CatProcessingUnit
{
    [RequireComponent(typeof(Camera))]
    public class ParallaxCamera : MonoBehaviour
    {
        [SerializeField] private Camera _mainCam;
        [SerializeField] private float _parallaxScale;
        
        private Vector3 _mainCamRootPos;
        private Vector3 _myRootPos;
        
        private void OnEnable()
        {
            _mainCamRootPos = _mainCam.transform.CameraPos();
            _myRootPos = transform.CameraPos();
        }

        private void Update()
        {
            var currentMainCamPos = _mainCam.transform.CameraPos();
            var offset = currentMainCamPos - _mainCamRootPos;
            offset.z = 0;
            transform.position = _myRootPos + offset * _parallaxScale;
        }
    }
}