using System.Collections;
using UnityEngine;

public class Neon_Glitch : MonoBehaviour
{
    public GameObject basePanel;
    public GameObject[] glitchPanels;

    public Vector2 glitchRandomTime = new Vector2(0.1f, 1f);
    public float glitchDutation = 0.1f;

    private void Start()
    {
        StartCoroutine(RandomGlitch());
    }

    IEnumerator RandomGlitch()
    {
        int currentGlitchPanel = Random.Range(0, glitchPanels.Length);
        while (true)
        {
            float randomTime = Random.Range(glitchRandomTime.x, glitchRandomTime.y);
            yield return new WaitForSeconds(randomTime);
            // Glitch On
            basePanel.SetActive(false);
            glitchPanels[currentGlitchPanel].SetActive(true);
            yield return new WaitForSeconds(glitchDutation);
            // Glitch Off
            glitchPanels[currentGlitchPanel].SetActive(false);
            basePanel.SetActive(true);
            // 다음 글리치 패널 선택
            currentGlitchPanel = Random.Range(0, glitchPanels.Length);
        }
    }
}
