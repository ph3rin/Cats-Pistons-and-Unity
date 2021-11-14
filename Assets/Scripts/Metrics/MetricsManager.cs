using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using CatProcessingUnit.GameManagement;
using UnityEngine;

namespace CatProcessingUnit.Metrics
{
    [RequireComponent(typeof(RegisterService))]
    public class MetricsManager : MonoBehaviour, IService
    {
        private List<string> _metrics;
        private string _filename;

        public static MetricsManager I => ServiceLocator.GetService<MetricsManager>();

        public void Init()
        {
            _metrics = new List<string>();
            _filename = $"metrics_{DateTime.UtcNow:M-d-yy-hh-mm-ss}.txt";
            AddMetrics("GAME STARTS");
        }

#if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern void DownloadMetrics(string name, string data);
#else
        private static void DownloadMetrics(string name, string data) 
        {
        }
#endif

        public void AddMetrics(string data)
        {
            _metrics.Add($"[{DateTime.UtcNow:T}] {data}");
            if (_metrics.Count % 16 == 0)
            {
                SaveMetrics();
            }
        }

        public void SaveMetrics()
        {
            if (Application.platform != RuntimePlatform.WebGLPlayer)
            {
                using var file = File.Open(_filename, FileMode.Create);
                using var sw = new StreamWriter(file);
                foreach (var metric in _metrics)
                {
                    sw.WriteLine(metric);
                }

                sw.Flush();
            }
            else
            {
                Console.WriteLine("========= METRICS =========");
                Console.WriteLine(string.Join("\n", _metrics));
                
                DownloadMetrics(_filename, string.Join("\n", _metrics));
            }
        }

        public void OnApplicationQuit()
        {
            AddMetrics("GAME EXITS NORMALLY");
            SaveMetrics();
        }
    }
}