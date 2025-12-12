using System.Collections;
using TMPro;
using Unity.VRTemplate;
using UnityEngine;
using UnityEngine.UI;

public class Scanner : MonoBehaviour
{
    public AudioSource scanSfx;
    public AudioSource readySfx;
    public GameObject scanPrefab;
    public float cooldown = 10f;

    public TextMeshProUGUI statusText;
    public TextMeshProUGUI cooldownText;
    public Image cooldownBar;

    public ItemManager itemManager;
    private void OnEnable()
    {
        itemManager = FindFirstObjectByType<ItemManager>();

        if (itemManager.scanner_canScan)
        {
            statusText.text = "Available";
            cooldownText.text = "-";
            cooldownBar.fillAmount = 1;
        }
        else
        {
            statusText.text = "Deavailable";
        }

    }

    public void StartScan()
    {
        if (itemManager.scanner_canScan)
            StartCoroutine(ScanCor());
    }

    private void Update()
    {
        if (itemManager.scanner_cooldownTimer > 0)
        {
            itemManager.scanner_cooldownTimer -= Time.deltaTime;
            cooldownText.text = $"{itemManager.scanner_cooldownTimer:0.00}";
            cooldownBar.fillAmount = 1 - (itemManager.scanner_cooldownTimer / cooldown);
            if (itemManager.scanner_cooldownTimer <= 0)
            {
                cooldownBar.fillAmount = 1f;

                statusText.text = "Available";
                cooldownText.text = "-";
                cooldownBar.fillAmount = 1;
                readySfx.Play();
                itemManager.scanner_canScan = true;
            }
        }
    }

    IEnumerator ScanCor()
    {
        itemManager.scanner_canScan = false;
        statusText.text = "Deavailable";
        scanSfx.Play();
        Instantiate(scanPrefab, transform.position, transform.rotation);

        itemManager.scanner_cooldownTimer = cooldown;
        yield return null;
    }
}
