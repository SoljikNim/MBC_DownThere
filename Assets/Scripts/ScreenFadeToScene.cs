using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScreenFadeToScene : MonoBehaviour
{
    public Image fadeImage; // ∞À¿∫ Image (Canvas ¿ß)
    public float fadeDuration = 2f;
    public string nextSceneName = "GameScene";

    public void StartFadeAndLoad()
    {
        StartCoroutine(FadeOutAndLoad());
    }

    private IEnumerator FadeOutAndLoad()
    {
        float elapsed = 0f;
        Color c = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsed / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}