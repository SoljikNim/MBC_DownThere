using UnityEngine;

public class WaveGunBattery : MonoBehaviour
{
    WaveGun waveGun;
    void Start()
    {
    }

    public void GetAmmo()
    {
        waveGun = FindFirstObjectByType<WaveGun>();
        if (waveGun == null) return;
        waveGun.AddAmmo(1);
        Destroy(gameObject);
    }
}
