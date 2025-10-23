using System.Collections.Generic;
using UnityEngine;

namespace EchoSphere.Player
{
    public class WeaponInventory : MonoBehaviour
    {
        [System.Serializable]
        public class WeaponSlot
        {
            public string weaponName;
            public GameObject weaponObject;  // 鼓、风琴等武器模型
        }

        [Header("武器列表（按顺序切换）")]
        public List<WeaponSlot> weapons = new List<WeaponSlot>();

        [Header("当前武器索引")]
        [SerializeField] private int currentIndex = 0;

        private void Start()
        {
            if (weapons.Count == 0)
            {
                Debug.LogWarning("未设置任何武器");
                return;
            }

            // 初始化：只激活当前武器
            for (int i = 0; i < weapons.Count; i++)
                weapons[i].weaponObject.SetActive(i == currentIndex);
        }

        private void Update()
        {
            // 滚轮切换武器
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll > 0f)
                NextWeapon();
            else if (scroll < 0f)
                PreviousWeapon();

            // 数字键直接切换
            for (int i = 0; i < weapons.Count; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                    SwitchToWeapon(i);
            }
        }

        public void NextWeapon()
        {
            int next = (currentIndex + 1) % weapons.Count;
            SwitchToWeapon(next);
        }

        public void PreviousWeapon()
        {
            int prev = (currentIndex - 1 + weapons.Count) % weapons.Count;
            SwitchToWeapon(prev);
        }

        public void SwitchToWeapon(int index)
        {
            if (index < 0 || index >= weapons.Count) return;
            if (index == currentIndex) return;

            weapons[currentIndex].weaponObject.SetActive(false);
            currentIndex = index;
            weapons[currentIndex].weaponObject.SetActive(true);

            Debug.Log($"切换武器：{weapons[currentIndex].weaponName}");
        }

        public GameObject GetCurrentWeapon()
        {
            return weapons.Count > 0 ? weapons[currentIndex].weaponObject : null;
        }

        public string GetCurrentWeaponName()
        {
            return weapons.Count > 0 ? weapons[currentIndex].weaponName : "None";
        }
    }
}
