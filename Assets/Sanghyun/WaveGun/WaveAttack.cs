using System.Collections;
using UnityEngine;

public class WaveAttack : MonoBehaviour
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

        while (scanTimer > 0)
        {
            float _currentScale = Mathf.Lerp(0, MaxRadius, 1 - (scanTimer / scanDuration));
            currentScale.x = _currentScale;
            currentScale.y = _currentScale;
            currentScale.z = _currentScale;
            transform.localScale = currentScale;

            scanTimer -= Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Monster1_Hitbox>().SetStun();
        }
    }
}
