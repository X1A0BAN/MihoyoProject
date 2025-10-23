using System.Collections;
using UnityEngine;

namespace EchoSphere.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class EchoWalker : MonoBehaviour
    {
        [Header("== Movement ==")]
        public float moveSpeed = 4.5f;
        public float sprintMultiplier = 1.35f;
        public float gravity = -16f;
        public float jumpHeight = 1.2f;

        [Header("== Mouse Look ==")]
        public Transform cameraPivot;        // 绑定到主角头顶空物体
        public float mouseXSensitivity = 120f;
        public float mouseYSensitivity = 100f;
        public float minPitch = -60f;
        public float maxPitch = 70f;

        [Header("== Drum (Melee) ==")]
        public float drumRange = 2.0f;       // 球体检测中心在前方 drumRange
        public float drumRadius = 1.3f;      // 球体半径
        public LayerMask enemyMask;          // 指定“敌人”Layer
        public float drumCooldown = 0.35f;
        public int drumDamage = 1;
        public AudioSource drumSfx;          // 鼓击音效（可空）
        public ParticleSystem drumHitFx;     // 鼓击命中特效（可空，实例化在敌人处）

        [Header("== Organ (Ranged) ==")]
        public MusicNoteProjectile notePrefab;  // 拖预制
        public Transform noteSpawn;             // 子弹出生点（相机前/角色手部）
        public float noteSpeed = 18f;
        public float organCooldown = 0.4f;
        public AudioSource organSfx;

        [Header("== Music Layers ==")]
        public Audio.MusicLayerManager musicManager; // 拖到场景中的管理器，可空
        public bool addLayerOnHit = true;            // 击杀时叠一层

        CharacterController _cc;
        Vector3 _velocity;
        float _pitch; // 上下看
        bool _canDrum = true;
        bool _canOrgan = true;

        void Awake()
        {
            _cc = GetComponent<CharacterController>();
            if (cameraPivot == null && Camera.main)
                cameraPivot = Camera.main.transform;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            HandleLook();
            HandleMove();

            if (Input.GetMouseButtonDown(0)) TryDrum();   // 鼓：左键
            if (Input.GetMouseButtonDown(1)) TryOrgan();  // 风琴：右键
            if (Input.GetKeyDown(KeyCode.Space)) TryJump();
        }

        void HandleMove()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            Vector3 input = new Vector3(h, 0, v);
            input = Vector3.ClampMagnitude(input, 1f);

            // 面朝摄像机方向移动（第三人称常用）
            Vector3 camForward = cameraPivot ? Vector3.Scale(cameraPivot.forward, new Vector3(1, 0, 1)).normalized : transform.forward;
            Vector3 camRight = cameraPivot ? cameraPivot.right : transform.right;
            camRight.y = 0; camRight.Normalize();

            Vector3 move = camForward * input.z + camRight * input.x;
            float speed = moveSpeed * (Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1f);

            _cc.Move(move * speed * Time.deltaTime);

            // 重力 & 落地
            if (_cc.isGrounded && _velocity.y < 0) _velocity.y = -2f;
            _velocity.y += gravity * Time.deltaTime;
            _cc.Move(_velocity * Time.deltaTime);

            // 朝向插值
            if (move.sqrMagnitude > 0.0001f)
            {
                Quaternion target = Quaternion.LookRotation(move, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, target, 12f * Time.deltaTime);
            }
        }

        void HandleLook()
        {
            if (!cameraPivot) return;
            float mx = Input.GetAxis("Mouse X") * mouseXSensitivity * Time.deltaTime;
            float my = Input.GetAxis("Mouse Y") * mouseYSensitivity * Time.deltaTime;

            transform.Rotate(Vector3.up, mx);

            _pitch -= my;
            _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);
            cameraPivot.localEulerAngles = new Vector3(_pitch, 0, 0);
        }

        void TryJump()
        {
            if (_cc.isGrounded)
            {
                _velocity.y = Mathf.Sqrt(-2f * gravity * jumpHeight);
            }
        }

        void TryDrum()
        {
            if (!_canDrum) return;
            StartCoroutine(DrumRoutine());
        }

        IEnumerator DrumRoutine()
        {
            _canDrum = false;
            if (drumSfx) drumSfx.Play();

            // 球体检测：中心=角色位置 + 前方 drumRange
            Vector3 center = transform.position + transform.forward * drumRange;
            Collider[] hits = Physics.OverlapSphere(center, drumRadius, enemyMask, QueryTriggerInteraction.Ignore);

            bool killedAny = false;
            foreach (var hit in hits)
            {
                var monster = hit.GetComponent<Enemies.NoiseMonster>();
                Debug.Log("aaa");
                if (monster != null)
                {
                    if (drumHitFx) Instantiate(drumHitFx, hit.transform.position, Quaternion.identity);
                    {
                        Debug.Log("aaa");
                        monster.TakeDamage(drumDamage);
                    }
                    if (monster.IsDead)
                    {
                        killedAny = true;
                        if (addLayerOnHit && musicManager) musicManager.AddNextLayer();
                    }
                }
                else
                {
                    // 没有示例脚本也照样销毁（兜底）
                    Destroy(hit.gameObject);
                    killedAny = true;
                    if (addLayerOnHit && musicManager) musicManager.AddNextLayer();
                }
            }

            // 可视化调试
            DebugDrawSphere(center, drumRadius, killedAny ? Color.yellow : Color.white, 0.25f);

            yield return new WaitForSeconds(drumCooldown);
            _canDrum = true;
        }

        void TryOrgan()
        {
            if (!_canOrgan || notePrefab == null || noteSpawn == null) return;
            StartCoroutine(OrganRoutine());
        }

        IEnumerator OrganRoutine()
        {
            _canOrgan = false;
            if (organSfx) organSfx.Play();

            var note = Instantiate(notePrefab, noteSpawn.position, noteSpawn.rotation);
            note.Fire(noteSpawn.forward, noteSpeed, enemyMask, OnProjectileKill);

            yield return new WaitForSeconds(organCooldown);
            _canOrgan = true;
        }

        void OnProjectileKill()
        {
            if (addLayerOnHit && musicManager) musicManager.AddNextLayer();
        }

        // Gizmo-like debug
        void DebugDrawSphere(Vector3 center, float radius, Color color, float duration)
        {
            #if UNITY_EDITOR
            int seg = 24;
            Vector3 last = center + Vector3.right * radius;
            for (int i = 1; i <= seg; i++)
            {
                float a = i * Mathf.PI * 2f / seg;
                Vector3 next = center + new Vector3(Mathf.Cos(a) * radius, 0, Mathf.Sin(a) * radius);
                Debug.DrawLine(last, next, color, duration);
                last = next;
            }
            #endif
        }

        // 圆形可视化（在 Scene 里看到范围）
        void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0.8f, 0.2f, 0.25f);
            Gizmos.DrawSphere(transform.position + transform.forward * drumRange, drumRadius);
        }
    }
}
