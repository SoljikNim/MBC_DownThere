using UnityEngine;

public class WaveBattery : MonoBehaviour
{
    public ItemManager itemManager;
    public AudioSource getSfx;
    public void GetAmmo()
    {
        itemManager = FindFirstObjectByType<ItemManager>();
        if (itemManager == null) return;
        itemManager.Wave_AddAmmo(1);
        getSfx.Play();
        Destroy(gameObject);
    }
}
