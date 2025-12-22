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
        if (getSfx != null)
            FindFirstObjectByType<PlayerAudioPlayer>().GetComponent<AudioSource>().PlayOneShot(getSfx.clip);
        Destroy(gameObject);
    }
}
