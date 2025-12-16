using System.Collections;
using UnityEngine;

public class AutoRotation : MonoBehaviour
{
    public float rotationSpd = 10f;
    public Vector2 aphlaRange = new Vector2(0.2f, 0.75f);
    public Color lightColor;
    Renderer[] renderers;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        StartCoroutine(AlphaCor());
    }

    // Update is called once per frame
    void Update()
    {
        // 매 프레임 Y축으로 회전 (로컬 Y축 기준)
        transform.Rotate(Vector3.up, rotationSpd * Time.deltaTime, Space.Self);
    }

    public float a = 0;
    public float h = 0;
    public float b = 0;
    public float k = 0;
    float x = 0;
    IEnumerator AlphaCor()
    {
        while (true)
        {
            x += Time.deltaTime;
            float currentAlpha = a * Mathf.Sin((x - h) / b) + k;
            lightColor.a = currentAlpha;
            //print("currentAlpha : " + currentAlpha);
            foreach (Renderer r in renderers) {
                r.material.SetColor("_BaseColor", lightColor);
            }
            yield return null;
        }
    }
}
