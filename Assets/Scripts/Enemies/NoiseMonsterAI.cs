using UnityEngine;
using UnityEngine.AI;
using EchoSphere.Audio;

namespace EchoSphere.Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NoiseMonsterAI : MonoBehaviour
    {
        public int maxHP = 3;
        public float sightRange = 10f;
        public float attackRange = 2.5f;
        public float pulseInterval = 1.2f;     // 音波环周期
        public Transform ringVisual;           // 用一个圆环做缩放展示
        public float ringMaxScale = 3.5f;
        public float ringMinScale = 0.2f;

        [HideInInspector] public float lastPerfectMoment; // 音波环“收束命中体心”时刻
        int _hp;
        NavMeshAgent _agent;
        Transform _player;

        void Awake()
        {
            _hp = maxHP;
            _agent = GetComponent<NavMeshAgent>();
            _player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        void Update()
        {
            if (_player)
            {
                float d = Vector3.Distance(transform.position, _player.position);
                if (d < sightRange) _agent.SetDestination(_player.position);
            }
            PulseRing();
        }

        float _pulseT;
        void PulseRing()
        {
            if (!ringVisual) return;

            _pulseT += Time.deltaTime;
            float t = (_pulseT % pulseInterval) / pulseInterval; // 0→1
            float scale = Mathf.Lerp(ringMaxScale, ringMinScale, t);
            ringVisual.localScale = new Vector3(scale, 1f, scale);

            // 当接近最小刻度，记为“完美判定时刻”
            if (t > 0.95f && Time.time - lastPerfectMoment > 0.5f)
                lastPerfectMoment = Time.time;
        }

        public void TakeDamage(int amount, bool stunOnPerfect)
        {
            _hp -= Mathf.Max(1, amount);
            if (_hp <= 0)
            {
                // 净化：叠加音乐层 + 销毁
                var mgr = FindObjectOfType<MusicLayerManager>();
                if (mgr) mgr.AddNextLayer();
                Destroy(gameObject);
            }
            else if (stunOnPerfect)
            {
                // 硬直（写法：短暂停止移动）
                StartCoroutine(StopAgent(0.3f));
            }
        }

        System.Collections.IEnumerator StopAgent(float time)
        {
            _agent.isStopped = true;
            yield return new WaitForSeconds(time);
            _agent.isStopped = false;
        }
    }
}
