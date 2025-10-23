using System.Collections;
using UnityEngine;

namespace EchoSphere.World
{
    public class PillarLift : MonoBehaviour
    {
        public Transform pillar;     // 要上升的物体
        public float liftHeight = 4f;
        public float liftTime = 1.2f;
        bool _used;

        // 被风琴音符命中时调用（在 MusicNoteProjectile 里用 tag 检测并调用）
        public void Activate()
        {
            if (_used || pillar == null) return;
            _used = true;
            StartCoroutine(Lift());
        }

        IEnumerator Lift()
        {
            Vector3 start = pillar.position;
            Vector3 end = start + Vector3.up * liftHeight;
            float t = 0;
            while (t < liftTime)
            {
                t += Time.deltaTime;
                pillar.position = Vector3.Lerp(start, end, t / liftTime);
                yield return null;
            }
            pillar.position = end;
        }
    }
}