using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TitleASDASKLDPJASPFAS : MonoBehaviour
{
    public float waitTime = 1f;
    public float fadeTime = 1f;
    public SpriteRenderer image;
    public Color startColor;
    public Color endColor;

    void Start()
    {
        StartCoroutine(TitleCor());
    }

    IEnumerator TitleCor()
    {
        yield return new WaitForSeconds(waitTime);
        float timer = fadeTime;

        Color imageColor = image.color;

        while (timer > 0f) {
            timer -= Time.deltaTime;
            float currentValue = (timer / fadeTime);
            Color color = Color.Lerp(endColor, startColor, currentValue);

            imageColor = color;
            imageColor.a = currentValue;
            image.color = imageColor;
            yield return null;
        }

        imageColor.a = 0;
        image.color = imageColor;
    }
}
