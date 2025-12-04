using UnityEngine;

public class HideObject : MonoBehaviour
{
    public GameObject target;
    public Transform hidePos;
    public Transform outPos;
    public Animator anim;

    private void Start()
    {
        
    }

    public void HideTarget()
    {
        if (target != null && hidePos != null)
        {
            anim.SetTrigger("In");
            target.transform.position = hidePos.position;
            target.transform.rotation = hidePos.rotation;
        }
    }

    public void OutTarget()
    {
        if (target != null && outPos != null)
        {
            anim.SetTrigger("Out");
            target.transform.position = outPos.position;
            target.transform.rotation = outPos.rotation;
        }
    }
}
