using UnityEngine;
using EchoSphere.Core;
using EchoSphere.Enemies;

namespace EchoSphere.Player
{
    public class DrumWeapon : MonoBehaviour
    {
        public float range = 2.0f;
        public float radius = 1.2f;
        public LayerMask enemyMask;
        public int baseDamage = 1;
        public AudioSource hitSfx;
        public ParticleSystem perfectFx;

        [Header("风琴充能")]
        //public OrganWeapon organ;           // 拖引用
        public int perfectStreakForCharge = 3;

        int _perfectStreak;

        public void Attack()
        {
            Vector3 center = transform.position + transform.forward * range;
            Collider[] hits = Physics.OverlapSphere(center, radius, enemyMask, QueryTriggerInteraction.Ignore);

            bool any = false;
            foreach (var h in hits)
            {
                var ai = h.GetComponentInParent<NoiseMonsterAI>();
                if (!ai) ai = h.GetComponent<NoiseMonsterAI>();
                if (!ai) continue;

                bool isPerfect = Mathf.Abs(Time.time - ai.lastPerfectMoment) <= (RhythmConductor.I ? RhythmConductor.I.perfectWindow : 0.12f);

                int dmg = isPerfect ? baseDamage * 2 : baseDamage;
                ai.TakeDamage(dmg, isPerfect);
                any = true;

                if (isPerfect)
                {
                    _perfectStreak++;
                    if (perfectFx) Instantiate(perfectFx, ai.transform.position, Quaternion.identity);
                    /*if (_perfectStreak >= perfectStreakForCharge && organ)
                    {
                        organ.AddCharge(1);       // 给风琴充能1格
                        _perfectStreak = 0;
                    }*/
                }
                else _perfectStreak = 0;
            }

            if (any && hitSfx) hitSfx.Play();
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0.9f, 0.2f, 0.25f);
            Gizmos.DrawSphere(transform.position + transform.forward * range, radius);
        }
    }
}
