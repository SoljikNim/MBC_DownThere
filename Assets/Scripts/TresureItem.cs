using UnityEngine;

public class TresureItem : MonoBehaviour
{
    public ItemManager itemManager;
    public AudioSource getSfx;
    public void GetAmmo()
    {
        itemManager = FindFirstObjectByType<ItemManager>();
        if (itemManager == null) return;
        itemManager.AddTresure();
        getSfx.Play();
        Destroy(gameObject);
    }
}
