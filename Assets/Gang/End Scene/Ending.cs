using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public Renderer sightRender;
    public float blackOutTIme = 2f;
    public float waitTime1 = 2f;
    public GameObject endObject;
    public float waitTime2 = 2f;

    void Start()
    {
        endObject.SetActive(false);
        sightRender.material.SetFloat("_Alpha", 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartEnding()
    {
        StartCoroutine(EndingCor());
    }

    IEnumerator EndingCor()
    {
        sightRender.material.SetFloat("_Alpha", 0);

        float blackOutTImer = blackOutTIme;
        while (blackOutTImer > 0)
        {
            blackOutTImer -= Time.deltaTime;

            sightRender.material.SetFloat("_Alpha", 1 - (blackOutTImer / blackOutTIme));
            yield return null;
        }
        sightRender.material.SetFloat("_Alpha", 1);

        yield return new WaitForSeconds(waitTime1);

        endObject.SetActive(true);

        yield return new WaitForSeconds(waitTime2);

        SceneManager.LoadScene("OpeningScene");
    }
}
