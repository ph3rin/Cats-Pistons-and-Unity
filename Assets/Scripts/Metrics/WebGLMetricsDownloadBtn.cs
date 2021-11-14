using System;
using UnityEngine;

namespace CatProcessingUnit.Metrics
{
    public class WebGLMetricsDownloadBtn : MonoBehaviour
    {
        private void Awake()
        {
            if (Application.platform != RuntimePlatform.WebGLPlayer)
            {
                Destroy(gameObject);
            }
        }
        
    }
}