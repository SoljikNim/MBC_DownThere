using UnityEngine;

public class HologramSwitch : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<InterfaceManager>(out var _interface))
        {
            _interface.SetVisible(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<InterfaceManager>(out var _interface))
        {
            _interface.SetVisible(false);
        }
    }
}
