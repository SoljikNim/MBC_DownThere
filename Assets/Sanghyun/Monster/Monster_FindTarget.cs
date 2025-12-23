using System.Collections;
using UnityEngine;

public class Monster_FindTarget : MonoBehaviour
{
    public Monster_Movement movement;
    public Transform eyePos;
    public Transform target;
    public Player_Main player_main;
    public AudioSource findTarget;
    void Start()
    {
        movement = GetComponentInParent<Monster_Movement>();
        if (eyePos == null)
            eyePos = movement.eyePos;
    }

    public bool tryingTarget;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !tryingTarget && target == null)
        {
            print("1");
            tryingTarget = true;
            StartCoroutine(CheckTargetable(other));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tryingTarget = false;
        }
    }

    IEnumerator CheckTargetable(Collider other)
    {
        print("2");
        while (tryingTarget)
        {
            print("3");
            TryTarget(other);
            yield return null;
        }
    }

    public void SetTarget(GameObject _target)
    {
        findTarget.Play();
        player_main = _target.GetComponentInParent<Player_Main>();
        target = _target.transform;
        player_main.GetCaughted();
        player_main.currentEnemy = movement;

        movement.target = target;
        movement.player = player_main;
        movement.SetChase();

        StopAllCoroutines();
        StartCoroutine(movement.TryCatchTarget());
        StartCoroutine(movement.Miss_OutOfSight());
    }

    private void TryTarget(Collider other)
    {
        if (other.CompareTag("Player") && !ObjectInMiddle(other.transform) && !other.GetComponent<Player_Main>().isHide)
        {
            print("4");
            SetTarget(other.gameObject);
            tryingTarget = false;
        }
        else
        {
            /*print("other.CompareTag(\"Player\") : " + other.CompareTag("Player"));
            print("ObjectInMiddle(other.transform) : "+ ObjectInMiddle(other.transform));
            print("other.GetComponent<Player_Main>().isHide : "+ other.GetComponent<Player_Main>().isHide);*/
        }
    }

    // Inspector에서 검사할 레이어를 설정할 수 있도록 추가 (기본: 모든 레이어)
    public LayerMask obstructionMask = ~0;
    public bool ObjectInMiddle(Transform other)
    {
        if (eyePos != null && other != null)
        {
            RaycastHit hit;
            // Linecast가 어떤 콜라이더에 맞았는지 검사 (obstructionMask로 레이어 필터링)
            Debug.DrawLine(eyePos.position, other.position, Color.green, 0.1f);
            if (Physics.Linecast(eyePos.position, other.position, out hit, obstructionMask))
            {
                // 맞은 콜라이더가 타겟(또는 자식)이라면 장애물이 아닌 것으로 간주
                if (hit.collider != null && !(hit.collider.transform == other || hit.collider.transform.IsChildOf(other)))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
