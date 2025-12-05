using UnityEngine;

public class Monster1 : MonoBehaviour
{
    public GameObject jumpScare1;
    public GameObject jumpScare2;
    public GameObject currentHideObject;

    void Start()
    {
        if (jumpScare1 != null)
        {
            jumpScare1.SetActive(false);
        }
        if (jumpScare2 != null)
        {
            jumpScare2.SetActive(false);
        }
    }

    public void StartJumpScare2()
    {
        jumpScare2.SetActive(true);
        if (currentHideObject != null)
        {
            Animator objectAnim = currentHideObject.GetComponent<Animator>();
            if (objectAnim != null)
            {
                objectAnim.SetTrigger("Opened");
            }
        }
    }
}
