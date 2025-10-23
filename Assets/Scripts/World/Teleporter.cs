using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    [Header("传送设置")]
    [Tooltip("输入目标场景的名称")]
    public string targetSceneName; // 公共变量，可在Inspector中设置

    [Tooltip("玩家传送到新场景后的出生点标签")]
    public string spawnPointTag = "SpawnPoint";

    [Header("传送条件")]
    [Tooltip("需要按特定按键才能传送")]
    public bool requireKeyPress = true;

    [Tooltip("传送按键")]
    public KeyCode activationKey = KeyCode.E;

    [Header("视觉效果")]
    public ParticleSystem teleportEffect;

    private bool playerInRange = false;

    // 当玩家进入触发器范围
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (!requireKeyPress)
            {
                // 不需要按键，直接传送
                Teleport();
            }
            else
            {
                // 显示提示信息（需要你实现UI提示）
                ShowHint("按 E 键传送");
            }
        }
    }

    // 当玩家离开触发器范围
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            HideHint();
        }
    }

    // 检测按键输入
    private void Update()
    {
        if (playerInRange && requireKeyPress && Input.GetKeyDown(activationKey))
        {
            Teleport();
        }
    }

    // 执行传送
    private void Teleport()
    {
        // 播放传送特效
        if (teleportEffect != null)
        {
            Instantiate(teleportEffect, transform.position, transform.rotation);
        }

        // 加载目标场景
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogError("传送门错误: 未设置目标场景名称!");
        }
    }

    private void ShowHint(string message)
    {
        // 在这里实现显示UI提示的逻辑
        Debug.Log(message);
    }

    private void HideHint()
    {
        // 在这里实现隐藏UI提示的逻辑
        Debug.Log("隐藏提示");
    }
}