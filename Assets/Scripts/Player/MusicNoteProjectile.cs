using System;
using UnityEngine;

namespace EchoSphere.Player
{
    public class MusicNoteProjectile : MonoBehaviour
    {
        public float lifeTime = 4f;
        public int damage = 1;
        public ParticleSystem hitFx;   // 命中特效
        public AudioSource hitSfx;     // 命中音效

        Vector3 _velocity;
        LayerMask _enemyMask;
        Action _onKill;

        public void Fire(Vector3 dir, float speed, LayerMask enemyMask, Action onKill)
        {
            _velocity = dir.normalized * speed;
            _enemyMask = enemyMask;
            _onKill = onKill;
            Destroy(gameObject, lifeTime);
        }

        void Update()
        {
            float dt = Time.deltaTime;
            Vector3 next = transform.position + _velocity * dt;
            // Raycast 前进路径，避免穿透
            if (Physics.Raycast(transform.position, _velocity.normalized, out var hit, _velocity.magnitude * dt, ~0, QueryTriggerInteraction.Ignore))
            {
                transform.position = hit.point;
                // 命中敌人
                if (((1 << hit.collider.gameObject.layer) & _enemyMask) != 0)
                {
                    var monster = hit.collider.GetComponent<Enemies.NoiseMonster>();
                    if (monster != null) monster.TakeDamage(damage);
                    else Destroy(hit.collider.gameObject);

                    if (hitFx) Instantiate(hitFx, hit.point, Quaternion.identity);
                    if (hitSfx) hitSfx.Play();
                    _onKill?.Invoke();
                }

                Destroy(gameObject);
                return;
            }

            transform.position = next;
            if (_velocity.sqrMagnitude > 0.01f) transform.forward = _velocity.normalized;
        }
    }
}