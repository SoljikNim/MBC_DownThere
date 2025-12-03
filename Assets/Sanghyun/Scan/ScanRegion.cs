using System.Collections;
using UnityEngine;

public class ScanRegion : MonoBehaviour
{
    public float MaxRadius = 100;
    public float scanDuration = 10f;
    public Vector3 currentScale = Vector3.zero;

    void Start()
    {
        transform.localScale = currentScale;
        StartCoroutine(Scan());
    }

    IEnumerator Scan()
    {
        float scanTimer = scanDuration;
        Material mat = GetComponent<Renderer>().material;
        float startAlpha = 1f;

        while (scanTimer > 0)
        {
            float _currentScale = Mathf.Lerp(0, MaxRadius, 1 - (scanTimer / scanDuration));
            currentScale.x = _currentScale;
            currentScale.y = _currentScale;
            currentScale.z = _currentScale;
            transform.localScale = currentScale;

            mat.SetFloat("_Alpha", Mathf.Lerp(startAlpha, 0, 1 - (scanTimer / scanDuration)));

            scanTimer -= Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
