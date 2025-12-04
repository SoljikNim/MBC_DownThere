using System.Collections;
using UnityEngine;

public class OutLine_Manager : MonoBehaviour
{
    public Transform playerPos;
    public float StartRange = 5f;
    public float EndRange = 0.5f;
    public Renderer[] renderers;
    public bool reverse = false;
    private void Start()
    {
        GetChildRenderers();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(OutLine_Effect());
    }

    void GetChildRenderers()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

    IEnumerator OutLine_Effect()
    {
        while (true)
        {
            float playerDistance = Vector3.Distance(transform.position, playerPos.position);
            if ((playerDistance <= StartRange) && playerDistance >= EndRange)
            {
                float currentAlpha = (playerDistance - EndRange) / (StartRange - EndRange);
                if (reverse)
                {
                    currentAlpha = 1f - currentAlpha;
                }

                foreach (Renderer rend in renderers)
                {
                    foreach (Material mat in rend.materials)
                    {
                        mat.SetFloat("_Alpha", currentAlpha);
                    }
                }
            }
            else
            {
                foreach (Renderer rend in renderers)
                {
                    foreach (Material mat in rend.materials)
                    {
                        mat.SetFloat("_Alpha", 0f);
                    }
                }
            }
            yield return null;
        }
    }
}
