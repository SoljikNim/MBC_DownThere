using UnityEngine;

public class Monster1_Hitbox : MonoBehaviour
{
    public Monster_Movement monsterMovement;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetStun()
    {
        monsterMovement.Stun();
    }
}
