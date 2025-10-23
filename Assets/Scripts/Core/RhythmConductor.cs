using UnityEngine;

namespace EchoSphere.Core
{
    public class RhythmConductor : MonoBehaviour
    {
        [Tooltip("BPM（每分钟节拍数）")]
        public float bpm = 80f;

        [Tooltip("拍点容差（秒），鼓击在 |t - 拍点| <= window 视为完美")]
        public float perfectWindow = 0.12f;

        float _secPerBeat;
        float _songStart;

        public static RhythmConductor I { get; private set; }

        void Awake()
        {
            I = this;
            _secPerBeat = 60f / Mathf.Max(1f, bpm);
            _songStart = Time.time;
        }

        public bool IsOnBeat(float time) => Mathf.Abs(TimeToBeatOffset(time)) <= perfectWindow;

        public float TimeToBeatOffset(float time)
        {
            float t = time - _songStart;
            float beatIndex = Mathf.Round(t / _secPerBeat);
            float beatTime = beatIndex * _secPerBeat;
            return t - beatTime; // 距离最近拍点的偏差
        }

        public float SecPerBeat => _secPerBeat;
    }
}