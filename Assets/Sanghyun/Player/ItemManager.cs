using Newtonsoft.Json.Bson;
using Unity.Burst.CompilerServices;
using Unity.VRTemplate;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [Header("WaveGun")]
    public int wave_ammo = 1;
    public float wave_cooldownTimer = 0;
    public bool wave_canFire = true;

    [Header("Scanner")]
    public float scanner_cooldownTimer = 0f;
    public bool scanner_canScan = true;

    [Header("PowerLight")]
    public bool light_isOn = true;
    public float light_maxTime = 120f;
    public float light_timer = 120f;

    public void Wave_AddAmmo(int _count)
    {
        wave_ammo += _count;
        WaveGun wavegun = FindFirstObjectByType<WaveGun>();
        if (wavegun == null) return;
        wavegun.SetAmmoText();
    }

    public void Flash_Charge()
    {
        light_timer = light_maxTime;
    }
}
