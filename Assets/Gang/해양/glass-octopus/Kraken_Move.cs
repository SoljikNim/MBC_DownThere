using System.Collections;
using UnityEngine;

public class Kraken_Move : MonoBehaviour
{
    public float liveTime = 5f;
    public float moveSpeed = 10f;
    public float fadeTime = 1f;
    public Renderer[] renderers;
    public Color matColor = Color.black;
    void Start()
    {
        StartCoroutine(FadeInOut());
    }

    IEnumerator FadeInOut()
    {
        float fadeInTime = fadeTime;
        while (fadeInTime > 0)
        {
            fadeInTime -= Time.deltaTime;
            SetColor(1 - (fadeInTime / fadeTime));
            yield return null;
        }
        yield return new WaitForSeconds(liveTime - fadeTime * 2);

        float fadeOutTime = fadeTime;
        while (fadeOutTime > 0)
        {
            fadeOutTime -= Time.deltaTime;
            SetColor(fadeOutTime / fadeTime);
            yield return null;
        }

        Destroy(gameObject);
    }

    void SetColor(float alpha)
    {
        matColor.a = alpha;
        foreach (var renderer in renderers) {
            renderer.material.SetColor("_BaseColor", matColor);
            renderer.material.SetColor("_Color", matColor);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
}
