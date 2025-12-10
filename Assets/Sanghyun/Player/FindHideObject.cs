using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class FindHideObject : MonoBehaviour
{
    public GameObject player;
    public HideObject hideObject;
    public Player_Main player_Main;
    public InputActionReference interactButton;

    public HapticImpulsePlayer haptic;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HideObject"))
        {
            haptic.SendHapticImpulse(0.3f, 0.25f);
            hideObject = other.GetComponent<HideObject>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HideObject"))
        {
            if (!player_Main.isHide)
                hideObject = null;
        }
    }
    void OnEnable()
    {
        interactButton.action.performed += HideInteraction;
    }

    void HideInteraction(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (hideObject != null)
            {
                if (!player_Main.isHide)
                {
                    player_Main.currentHideObject = hideObject;
                    player_Main.SetHide(true);
                    hideObject.target = player;

                    hideObject.anim.SetTrigger("In");
                    player.transform.position = hideObject.hidePos.position;
                    player.transform.rotation = hideObject.hidePos.rotation;
                }
                else
                {
                    player_Main.currentHideObject = null;
                    player_Main.SetHide(false);

                    hideObject.anim.SetTrigger("Out");
                    player.transform.position = hideObject.outPos.position;
                    player.transform.rotation = hideObject.outPos.rotation;

                    hideObject.target = null;

                    hideObject = null;
                }
            }
        }
    }
}
