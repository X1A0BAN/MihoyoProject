using UnityEngine;

namespace EchoSphere.World
{
    public class ResonancePlatform : MonoBehaviour
    {
        public bool isStable = false;
        public Renderer visual;
        public Color unstableColor = new Color(1,0.6f,0.6f,1);
        public Color stableColor   = new Color(0.6f,1,0.6f,1);

        void Awake() => UpdateVisual();

        public void Stabilize()
        {
            if (isStable) return;
            isStable = true;
            // 启用碰撞器/刚体
            var col = GetComponent<Collider>(); if (col) col.enabled = true;
            UpdateVisual();
        }

        void UpdateVisual()
        {
            if (visual) visual.material.color = isStable ? stableColor : unstableColor;
            var col = GetComponent<Collider>();
            if (col) col.enabled = isStable;
        }
    }
}