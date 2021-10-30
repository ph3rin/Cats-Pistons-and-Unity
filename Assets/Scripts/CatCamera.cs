using System.Collections;
using CatProcessingUnit.GameManagement;
using CatProcessingUnit.Machineries;
using DG.Tweening;
using UnityEngine;

namespace CatProcessingUnit
{
    [RequireComponent(typeof(RegisterService), typeof(Camera))]
    public class CatCamera : MonoBehaviour, IService
    {
        private CatRenderer _catRenderer;
        private Camera _camera;

        public void Init()
        {
            _camera = GetComponent<Camera>();
            _catRenderer = ServiceLocator.GetService<CatRenderer>();
        }

        public Tween FocusCat()
        {
            var catPos = _catRenderer.transform.position;
            return DOTween
                .Sequence(gameObject)
                .Append(_camera.DOOrthoSize(2.0f, 1.0f))
                .Join(transform.DOMoveX(catPos.x, 1.0f))
                .Join(transform.DOMoveY(catPos.y, 1.0f));
        }
    }
}