using UnityEngine;

namespace EchoSphere.Enemies
{
    public class NoiseMonster : MonoBehaviour
    {
        public int maxHP = 1;
        public ParticleSystem purifyFx;  
        public AudioSource purifySfx;    

        int _hp;
        public bool IsDead => _hp <= 0;

        void Awake() => _hp = maxHP;

        public void TakeDamage(int amount)
        {
            if (IsDead) return;
            _hp -= Mathf.Max(1, amount);

            if (IsDead)
            {
                if (purifyFx) Instantiate(purifyFx, transform.position, Quaternion.identity);
                if (purifySfx) purifySfx.Play();
                Destroy(gameObject);
            }
        }
    }
}