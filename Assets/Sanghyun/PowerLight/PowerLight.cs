using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PowerLight : MonoBehaviour
{
    public InterfaceManager interfaceManager;
    public AudioSource buttonSfx;
    public GameObject lightObj;
    public float maxLightTime = 120f;
    public Light spotLight;
    public Transform caughtBox;
    Vector3 caughtBoxScale = Vector3.one;
    public float maxLightIntencity = 55.69f;

    private void Start()
    {
    }

    public ItemManager itemManager;
    private void OnEnable()
    {
        interfaceManager = FindFirstObjectByType<InterfaceManager>();
        itemManager = FindFirstObjectByType<ItemManager>();
        lightObj.SetActive(itemManager.light_isOn);
    }

    private void Update()
    {
        if (itemManager != null && itemManager.light_isOn) {
            if (itemManager.light_timer > 0)
            {
                float per = (itemManager.light_timer / itemManager.light_maxTime);
                itemManager.light_timer -= Time.deltaTime;
                caughtBoxScale.x = per;
                caughtBoxScale.y = per;
                caughtBoxScale.z = per;
                caughtBox.localScale = caughtBoxScale;
                spotLight.intensity = maxLightIntencity * per;

                interfaceManager.SetBattery(per);
            }
            else
            {
                itemManager.light_isOn = false;
                lightObj.SetActive(false);
            }
        }
    }

    public void ToggleLight()
    {
        buttonSfx.Play();
        itemManager.light_isOn = !itemManager.light_isOn;
        lightObj.SetActive(itemManager.light_isOn);
    }
}
