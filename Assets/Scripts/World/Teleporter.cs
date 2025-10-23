using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    [Header("��������")]
    [Tooltip("����Ŀ�곡��������")]
    public string targetSceneName; // ��������������Inspector������

    [Tooltip("��Ҵ��͵��³�����ĳ������ǩ")]
    public string spawnPointTag = "SpawnPoint";

    [Header("��������")]
    [Tooltip("��Ҫ���ض��������ܴ���")]
    public bool requireKeyPress = true;

    [Tooltip("���Ͱ���")]
    public KeyCode activationKey = KeyCode.E;

    [Header("�Ӿ�Ч��")]
    public ParticleSystem teleportEffect;

    private bool playerInRange = false;

    // ����ҽ��봥������Χ
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (!requireKeyPress)
            {
                // ����Ҫ������ֱ�Ӵ���
                Teleport();
            }
            else
            {
                // ��ʾ��ʾ��Ϣ����Ҫ��ʵ��UI��ʾ��
                ShowHint("�� E ������");
            }
        }
    }

    // ������뿪��������Χ
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            HideHint();
        }
    }

    // ��ⰴ������
    private void Update()
    {
        if (playerInRange && requireKeyPress && Input.GetKeyDown(activationKey))
        {
            Teleport();
        }
    }

    // ִ�д���
    private void Teleport()
    {
        // ���Ŵ�����Ч
        if (teleportEffect != null)
        {
            Instantiate(teleportEffect, transform.position, transform.rotation);
        }

        // ����Ŀ�곡��
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogError("�����Ŵ���: δ����Ŀ�곡������!");
        }
    }

    private void ShowHint(string message)
    {
        // ������ʵ����ʾUI��ʾ���߼�
        Debug.Log(message);
    }

    private void HideHint()
    {
        // ������ʵ������UI��ʾ���߼�
        Debug.Log("������ʾ");
    }
}