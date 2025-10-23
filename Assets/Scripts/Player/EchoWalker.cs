using System;
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
        public Transform cameraPivot;    // 绑定到主角头顶空物体
        public float mouseXSensitivity = 90f;
        public float mouseYSensitivity = 80f;
        public float minPitch = -60f;
        public float maxPitch = 70f;

        [Header("== Drum (Melee) ==")]
        public float drumRange = 2.0f;
        public float drumRadius = 1.0f;
        public LayerMask enemyMask;
        public float drumCooldown = 0.35f;
        public int drumDamage = 1;
        public AudioSource drumSfx;
        public ParticleSystem drumHitFx;

        [Header("== Organ (Ranged) ==")]
        public MusicNoteProjectile notePrefab;
        public Transform noteSpawnPoint;
        public AudioSource organSfx;
        public float organCooldown = 0.6f;

        private CharacterController controller;
        private Vector3 velocity;
        private float pitch;
        private bool isGrounded = true;
        private bool canDrum = true;
        private bool canOrgan = true;

        private void Start()
        {
            controller = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            HandleCamera();
            HandleMovement();
            HandleAttack();
        }

        private void FixedUpdate()
        {
            isGrounded = controller.isGrounded;
        }

        private void HandleCamera()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseXSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseYSensitivity * Time.deltaTime;

            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            cameraPivot.localRotation = Quaternion.Euler(pitch, 0, 0);
            transform.Rotate(Vector3.up * mouseX);
        }

        private void HandleMovement()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 camForward = cameraPivot.forward;
            Vector3 camRight = cameraPivot.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDir = (camForward * vertical + camRight * horizontal).normalized;

            float speed = moveSpeed * (Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1f);
            controller.Move(moveDir * speed * Time.deltaTime);

            // ✅ 调试：检测地面状态
            //isGrounded = controller.isGrounded;
            if (isGrounded && velocity.y < 0)
                velocity.y = -2f;

            // ✅ 调试：输出状态
            if (isGrounded)
                Debug.Log("Grounded");
            else
                Debug.Log("Airborne");

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                Debug.Log($"Jump! velocity.y = {velocity.y}");
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }


        private void HandleAttack()
        {
            // 鼓（近战攻击）
            if (Input.GetMouseButtonDown(0) && canDrum)
                StartCoroutine(PerformDrumAttack());

            // 风琴（远程攻击）
            if (Input.GetMouseButtonDown(1) && canOrgan)
                StartCoroutine(PerformOrganAttack());
        }

        private IEnumerator PerformDrumAttack()
        {
            canDrum = false;

            if (drumSfx) drumSfx.Play();

            Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward * drumRange, drumRadius, enemyMask);
            foreach (var hit in hits)
            {
                if (drumHitFx)
                    Instantiate(drumHitFx, hit.transform.position, Quaternion.identity);

                // 模拟伤害
                Debug.Log($"命中敌人：{hit.name}");
            }

            yield return new WaitForSeconds(drumCooldown);
            canDrum = true;
        }

        private IEnumerator PerformOrganAttack()
        {
            canOrgan = false;

            if (organSfx) organSfx.Play();

            if (notePrefab && noteSpawnPoint)
                Instantiate(notePrefab, noteSpawnPoint.position, noteSpawnPoint.rotation);

            yield return new WaitForSeconds(organCooldown);
            canOrgan = true;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + transform.forward * drumRange, drumRadius);
        }
    }
}
