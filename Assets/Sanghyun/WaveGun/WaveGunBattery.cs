using UnityEngine;

public class WaveGunBattery : MonoBehaviour
{
    WaveGun waveGun;
    void Start()
    {
        waveGun = FindFirstObjectByType<WaveGun>();
    }

    public void GetAmmo()
    {
        waveGun.AddAmmo(1);
        Destroy(gameObject);
    }
}
