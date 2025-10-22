using System.Collections;
using UnityEngine;

namespace EchoSphere.Audio
{
    public class MusicLayerManager : MonoBehaviour
    {
        [Tooltip("将每一条乐器层的 AudioSource 拖进来，初始 volume 为 0")]
        public AudioSource[] layers;
        public float fadeTime = 1.2f;

        int _current = 0;

        void Awake()
        {
            if (layers == null) return;
            foreach (var a in layers)
            {
                if (!a) continue;
                a.volume = Mathf.Clamp01(a.volume);
                if (a.volume > 0f) continue;
                a.playOnAwake = false; // 由我们控制
            }
        }

        public void AddNextLayer()
        {
            if (layers == null || layers.Length == 0) return;
            if (_current >= layers.Length) return;

            var a = layers[_current++];
            if (!a) return;

            if (!a.isPlaying) a.Play();
            StopAllCoroutines();
            StartCoroutine(FadeIn(a, fadeTime));
        }

        IEnumerator FadeIn(AudioSource a, float time)
        {
            float t = 0f;
            float start = a.volume;
            while (t < time)
            {
                t += Time.deltaTime;
                a.volume = Mathf.Lerp(start, 1f, t / time);
                yield return null;
            }
            a.volume = 1f;
        }
    }
}