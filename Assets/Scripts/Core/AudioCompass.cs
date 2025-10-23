using System.Linq;
using UnityEngine;
using EchoSphere.Enemies;

namespace EchoSphere.Core
{
    public class AudioCompass : MonoBehaviour
    {
        public AudioSource compassAudio;     // 一段持续的“杂音”Loop
        public float maxVolumeDistance = 20f;

        void Update()
        {
            var enemies = FindObjectsOfType<NoiseMonsterAI>();
            if (enemies.Length == 0 || compassAudio == null) return;

            // 找最近的敌人
            var playerPos = transform.position;
            var nearest = enemies.OrderBy(e => Vector3.Distance(e.transform.position, playerPos)).First();
            float d = Vector3.Distance(nearest.transform.position, playerPos);

            // 根据距离调音量与立体声Pan（简易“听觉雷达”）
            compassAudio.volume = Mathf.Clamp01(1f - d / maxVolumeDistance);
            Vector3 local = transform.InverseTransformPoint(nearest.transform.position).normalized;
            compassAudio.panStereo = Mathf.Clamp(local.x, -1f, 1f);
        }
    }
}