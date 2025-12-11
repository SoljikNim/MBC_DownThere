using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class WaveGun : MonoBehaviour
{
    public UniversalRendererData universalRendererData;
    FullScreenPassRendererFeature waveFeature;
    public float waveTime = 1.5f;
    public GameObject wavePrefab;

    public int ammo = 1;
    public float cooldownTime = 2f;

    public TextMeshProUGUI ammoCount;
    public Image ammoTimer;

    public AudioSource fireSfx;

    public bool canFire = true;
    void Start()
    {
        ammoCount.text = ammo.ToString("F0");
        if (universalRendererData.TryGetRendererFeature<FullScreenPassRendererFeature>(out var waveRender))
        {
            waveFeature = waveRender;
        }
        waveFeature.passMaterial.SetFloat("_WaveValue", -0.1f);
        AddAmmo(0);
        ammoTimer.fillAmount = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire()
    {
        if (ammo > 0 && canFire)
        {
            AddAmmo(-1);
            StartCoroutine(Cooldown());
            StartCoroutine(GunFire());
        }
    }

    public void AddAmmo(int _int)
    {
        ammo += _int;
        ammoCount.text = ammo.ToString("F0");
    }

    IEnumerator Cooldown()
    {
        canFire = false;
        float duration = 0f;
        ammoTimer.fillAmount = 0f;
        while (duration < cooldownTime)
        {
            duration = Mathf.Clamp(duration + Time.deltaTime, 0f, cooldownTime);
            ammoTimer.fillAmount = duration / cooldownTime;
            yield return null;
        }
        canFire = true;
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
