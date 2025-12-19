using UnityEngine;

public class FlashBattery : MonoBehaviour
{
    public ItemManager itemManager;
    public AudioSource getSfx;

    public void GetAmmo()
    {
        itemManager = FindFirstObjectByType<ItemManager>();
        if (itemManager == null) return;
        itemManager.Flash_Charge();
        getSfx.PlayOneShot(getSfx.clip);
        Destroy(gameObject);
    }
}
