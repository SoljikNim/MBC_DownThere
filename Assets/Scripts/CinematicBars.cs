using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CinematicBars : MonoBehaviour
{
    public Material skyboxMat;
    public Camera cam;

    bool isOpen = false;
    public void SetCinematicFOV()
    {
        StartCoroutine(FOVRoutine(35));
    }

    public void ResetFOV()
    {
        StartCoroutine(FOVRoutine(60));
    }

    IEnumerator FOVRoutine(float targetFOV)
    {
        float start = cam.fieldOfView;
        float t = 0f;

        while (t < 1)
        {
            t += Time.deltaTime * 1.5f;
            cam.fieldOfView = Mathf.Lerp(start, targetFOV, t);
            yield return null;
        }
    }
  

    public void SetNightSky()
    {
        StartCoroutine(ChangeExposure(1f, 0.05f));
    }

    IEnumerator ChangeExposure(float start, float end)
    {
        float t = 0f;

        while (t < 1)
        {
            t += Time.deltaTime * 0.5f;
            float value = Mathf.Lerp(start, end, t);
            skyboxMat.SetFloat("_Exposure", value);
            yield return null;
        }
    }

    public void PlayCinematicTransition()
    {
        StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {
        // 1. 카메라 시네마틱 FOV
        StartCoroutine(FOVRoutine(35));

        // 2. 스카이박스 자연스럽게 어둡게
        StartCoroutine(ChangeExposure(1f, 0.05f));

        yield return new WaitForSeconds(3f);

        // 3. 지역 전환 완료 후 FOV 복귀
        StartCoroutine(FOVRoutine(60));
    }
}