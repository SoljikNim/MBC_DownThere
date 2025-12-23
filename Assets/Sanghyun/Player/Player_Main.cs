using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using XR.Interaction.Toolkit.Samples;

public class Player_Main : MonoBehaviour
{
    public bool isHide = false;

    [Header("Caught Effect")]
    public GameObject caughtEffect;
    public float caughtEffectDuration = 0.75f;
    public AudioSource caughtSfx;
    public Monster_Movement currentEnemy;
    public InterfaceManager interfaceManager;
    public DynamicMoveProvider controller;

    public InputActionReference runButton;
    public InputActionReference moveButton;
    public float runThrehold = 0.5f;
    public float walkSpd = 7;
    public float runSpd = 10f;
    public bool isRunning = false;

    public HideObject currentHideObject;

    public AudioSource heartBeatSfx;

    public float maxStamina = 7f;
    public float stamina = 7f;
    public float staminaHealMult = 0.75f;

    public AudioSource runSfx;

    void Start()
    {
        caughtEffect.SetActive(false);
        caughtEffectWait = new WaitForSeconds(caughtEffectDuration);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 runInput = runButton.action.ReadValue<Vector2>();
        Vector2 moveInput = moveButton.action.ReadValue<Vector2>();
        if (runInput.y >= runThrehold && moveInput.magnitude > 0)
        {
            if (!isRunning)
            {
                SetRunning(true);
            }
        }
        else
        {
            if (isRunning)
            {
                SetRunning(false);
            }
        }


        if (isRunning && stamina > 0)
        {
            stamina = Mathf.Clamp(stamina - Time.deltaTime, 0, maxStamina);
            interfaceManager.SetStamina(stamina / maxStamina);
            if (stamina == 0)
            {
                SetRunning(false);
            }
        }
        else if (stamina < maxStamina)
        {
            stamina = Mathf.Clamp(stamina + Time.deltaTime * staminaHealMult, 0, maxStamina);
            interfaceManager.SetStamina(stamina / maxStamina);
        }

        if (currentEnemy != null && !heartBeatSfx.isPlaying)
        {
            interfaceManager.SetEnemy(currentEnemy.transform);
            heartBeatSfx.Play();
        }
        else if (currentEnemy == null && heartBeatSfx.isPlaying)
        {
            interfaceManager.SetEnemy(null);
            heartBeatSfx.Stop();
        }
    }

    public void SetRunning(bool _bool)
    {
        if (_bool && (stamina <= 0))
            _bool = false;

        isRunning = _bool;
        if (_bool)
            runSfx.Play();
        else
            runSfx.Stop();

        controller.moveSpeed = _bool ? runSpd : walkSpd;
    }

    public void SetHide(bool _bool)
    {
        isHide = _bool;
        if (currentEnemy != null)
        {
            if (_bool)
            {
                currentEnemy.PlayerHiding();
            }
            else
                currentEnemy.watchPlayerHiding = false;
        }
    }

    public void GetCaughted()
    {
        StartCoroutine(SetCaughtEffect());
    }

    WaitForSeconds caughtEffectWait;
    IEnumerator SetCaughtEffect()
    {
        if (caughtSfx != null)
        {
            caughtSfx.Play();
        }   

        caughtEffect.SetActive(true);
        yield return caughtEffectWait;
        caughtEffect.SetActive(false);
    }
}
