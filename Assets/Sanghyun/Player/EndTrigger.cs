using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour
{
    public Renderer sightRender;
    public float blackOutTIme = 2f;
    public float interfaceShowTimer = 2f;
    public GameObject[] camEffect;
    public string endingScene;

    void Start()
    {
        sightRender.material.SetFloat("_ApertureSize", 1);
        sightRender.material.SetFloat("_FeatheringEffect", 1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(EndingCor());
    }

    IEnumerator EndingCor()
    {
        sightRender.material.SetFloat("_ApertureSize", 1);
        sightRender.material.SetFloat("_FeatheringEffect", 1);

        float blackOutTImer = blackOutTIme;
        while (blackOutTImer > 0)
        {
            blackOutTImer -= Time.deltaTime;

            sightRender.material.SetFloat("_ApertureSize", (blackOutTImer / blackOutTIme));
            sightRender.material.SetFloat("_FeatheringEffect", (blackOutTImer / blackOutTIme));
            yield return null;
        }
        sightRender.material.SetFloat("_ApertureSize", 0);
        sightRender.material.SetFloat("_FeatheringEffect", 0);

        yield return new WaitForSeconds(interfaceShowTimer);
        SceneManager.LoadScene(endingScene);
    }
}
