using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class LightCaught : MonoBehaviour
{
    public Player_Main player;

    private void OnEnable()
    {
        player = FindFirstObjectByType<Player_Main>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Monster1_Hitbox monster = other.GetComponent<Monster1_Hitbox>();
            if (monster != null)
            {
                monster.monsterFindTarget.SetTarget(player.gameObject);
            }
        }
    }
}
