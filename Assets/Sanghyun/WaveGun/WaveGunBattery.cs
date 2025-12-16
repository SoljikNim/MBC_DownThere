using UnityEngine;

public class WaveGunBattery : MonoBehaviour
{
    public ItemManager itemManager;

    public void GetAmmo()
    {
        itemManager = FindFirstObjectByType<ItemManager>();
        if (itemManager == null) return;
        itemManager.Wave_AddAmmo(1);
        Destroy(gameObject);
    }
}
