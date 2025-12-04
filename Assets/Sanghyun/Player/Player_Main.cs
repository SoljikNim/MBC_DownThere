using System.Collections;
using UnityEngine;

public class Player_Main : MonoBehaviour
{
    public bool isHide = false;

    [Header("Caught Effect")]
    public GameObject caughtEffect;
    public float caughtEffectDuration = 0.75f;
    public AudioSource caughtSfx;
    void Start()
    {
        caughtEffect.SetActive(false);
        caughtEffectWait = new WaitForSeconds(caughtEffectDuration);
    }

    // Update is called once per frame
    void Update()
    {
        
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
