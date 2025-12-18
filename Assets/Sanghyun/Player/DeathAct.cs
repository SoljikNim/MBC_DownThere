using System.Collections;
using UnityEngine;
<<<<<<< Updated upstream
<<<<<<< Updated upstream
using UnityEngine.SceneManagement;
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes

public class DeathAct : MonoBehaviour
{
    public Renderer sightRender;
    public float blackOutTIme = 2f;
    public GameObject restartInterface;
    public float interfaceShowTimer = 2f;
<<<<<<< Updated upstream
<<<<<<< Updated upstream
    public GameObject[] camEffect;
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes

    void Start()
    {
        sightRender.material.SetFloat("_ApertureSize", 1);
        sightRender.material.SetFloat("_FeatheringEffect", 1);

        restartInterface.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDeath()
    {
        StartCoroutine(DeathCor());
    }

    IEnumerator DeathCor()
    {
        sightRender.material.SetFloat("_ApertureSize", 1);
        sightRender.material.SetFloat("_FeatheringEffect", 1);

        float blackOutTImer = blackOutTIme;
        while (blackOutTImer > 0) {
            blackOutTImer -= Time.deltaTime;

            sightRender.material.SetFloat("_ApertureSize", (blackOutTImer / blackOutTIme));
            sightRender.material.SetFloat("_FeatheringEffect", (blackOutTImer / blackOutTIme));
            yield return null;
        }
        sightRender.material.SetFloat("_ApertureSize", 0);
        sightRender.material.SetFloat("_FeatheringEffect", 0);

        yield return new WaitForSeconds(interfaceShowTimer);
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        camEffect[0].SetActive(false);
        camEffect[1].SetActive(false);
        restartInterface.SetActive(true);
    }

    public void MoveScene(string _sceneName)
    {
        SceneManager.LoadScene( _sceneName );
    }
=======

        restartInterface.SetActive(true);
    }
>>>>>>> Stashed changes
=======

        restartInterface.SetActive(true);
    }
>>>>>>> Stashed changes
}
