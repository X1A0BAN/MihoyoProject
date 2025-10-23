using UnityEngine;

namespace EchoSphere.Core
{
    public class NoiseScanner : MonoBehaviour
    {
        public KeyCode scanKey = KeyCode.Q;
        public float scanRadius = 12f;
        public LayerMask enemyMask;
        public Material highlightMat; // 高亮材质
        public float highlightTime = 2f;

        void Update()
        {
            if (Input.GetKeyDown(scanKey)) DoScan();
        }

        void DoScan()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, scanRadius, enemyMask);
            foreach (var h in hits)
            {
                var r = h.GetComponentInChildren<Renderer>();
                if (!r) continue;
                StartCoroutine(Flash(r));
            }
        }

        System.Collections.IEnumerator Flash(Renderer r)
        {
            var old = r.sharedMaterial;
            if (highlightMat) r.sharedMaterial = highlightMat;
            yield return new WaitForSeconds(highlightTime);
            if (r) r.sharedMaterial = old;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.2f, 1f, 0.7f, 0.15f);
            Gizmos.DrawSphere(transform.position, scanRadius);
        }
    }
}