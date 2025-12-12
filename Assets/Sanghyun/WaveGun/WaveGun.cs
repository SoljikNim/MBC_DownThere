using System.Collections;
using TMPro;
using Unity.VRTemplate;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class WaveGun : MonoBehaviour
{
    public ItemManager itemManager;
    public UniversalRendererData universalRendererData;
    FullScreenPassRendererFeature waveFeature;
    public float waveTime = 1.5f;
    public GameObject wavePrefab;

    public float cooldownTime = 2f;

    public TextMeshProUGUI ammoCount;
    public Image ammoTimer;

    public AudioSource fireSfx;

    void Start()
    {
        if (universalRendererData.TryGetRendererFeature<FullScreenPassRendererFeature>(out var waveRender))
        {
            waveFeature = waveRender;
        }
        waveFeature.passMaterial.SetFloat("_WaveValue", -0.1f);
    }

    private void OnEnable()
    {
        itemManager = FindFirstObjectByType<ItemManager>();

        ammoCount.text = itemManager.wave_ammo.ToString("F0");
        SetAmmoText();
    }

    // Update is called once per frame
    void Update()
    {
        if (itemManager.wave_cooldownTimer < cooldownTime)
        {
            itemManager.wave_cooldownTimer = Mathf.Clamp(itemManager.wave_cooldownTimer + Time.deltaTime, 0f, cooldownTime);
            ammoTimer.fillAmount = itemManager.wave_cooldownTimer / cooldownTime;
            if (itemManager.wave_cooldownTimer == cooldownTime)
                ammoTimer.fillAmount = 1f;
        }
    }

    public void Fire()
    {
        if (itemManager.wave_ammo > 0 && itemManager.wave_canFire)
        {
            itemManager.wave_ammo--;
            SetAmmoText();
            StartCoroutine(Cooldown());
            StartCoroutine(GunFire());
        }
    }

    public void SetAmmoText()
    {
        ammoCount.text = itemManager.wave_ammo.ToString("F0");
    }

    IEnumerator Cooldown()
    {
        itemManager.wave_canFire = false;
        itemManager.wave_cooldownTimer = 0f;
        ammoTimer.fillAmount = 0f;
        while (itemManager.wave_cooldownTimer < cooldownTime)
        {
            yield return null;
        }
        itemManager.wave_canFire = true;
    }

    IEnumerator GunFire()
    {
        float duration = 0f;
        float waveValue = 0f;

        if (fireSfx != null)
        {
            fireSfx.Play();
        }
        Instantiate(wavePrefab, transform.position, Quaternion.identity);

        while (duration < waveTime)
        {
            duration = Mathf.Clamp(duration + Time.deltaTime, 0f, waveTime);
            waveValue = Mathf.Lerp(0f, 1f, duration / waveTime);
            waveFeature.passMaterial.SetFloat("_WaveValue", waveValue);
            yield return null;
        }
        waveFeature.passMaterial.SetFloat("_WaveValue", -0.1f);
    }
}
