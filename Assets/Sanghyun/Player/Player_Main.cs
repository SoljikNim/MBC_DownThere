using System.Collections;
using UnityEngine;

public class Player_Main : MonoBehaviour
{
    public bool isHide = false;

    [Header("Caught Effect")]
    public GameObject caughtEffect;
    public float caughtEffectDuration = 0.75f;
    public AudioSource caughtSfx;
    public Monster_Movement currentEnemy;

    public HideObject currentHideObject;



    void Start()
    {
        caughtEffect.SetActive(false);
        caughtEffectWait = new WaitForSeconds(caughtEffectDuration);
    }

    // Update is called once per frame
    void Update()
    {
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
