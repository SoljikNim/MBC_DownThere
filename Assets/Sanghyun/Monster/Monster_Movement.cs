using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Monster_Movement : MonoBehaviour
{
    public Monster_FindTarget findTarget;
    public Monster1_JumpScare jumpScare;
    public Animator anim;
    public NavMeshAgent agent;
    public bool freezeMove = false;
    public float wanderRange = 10.0f;
    public float wanderSpeed = 3.0f;
    public float chaseSpeed = 6.0f;
    public float detectionRange = 5.0f;
    public float catchRange = 1.0f;
    public float missRange = 10.0f;
    public float missTime = 5.0f;

    public Transform eyePos;
    public Transform target;
    public Player_Main player;

    public AudioSource stunSfx;
    void Start()
    {
        findTarget = GetComponentInChildren<Monster_FindTarget>();
        jumpScare = GetComponent<Monster1_JumpScare>();
        SetWander();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }

    public void Movement()
    {
        if (freezeMove) return;
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
        else
        {
            if (agent.hasPath)
            {
                //print("가는중");
            }
            else
            {
                Vector3 randomDestPos = Random.insideUnitSphere * wanderRange;
                agent.SetDestination(transform.position + randomDestPos);
                //print("목표지점 설정");
            }
        }
    }

    public void SetWander()
    {
        target = null;
        findTarget.target = null;
        player = null;
        findTarget.player_main = null;
        anim.SetTrigger("Walk");
        agent.speed = wanderSpeed;
    }

    public void SetChase()
    {
        anim.SetTrigger("Run");
        agent.speed = chaseSpeed;
    }

    public void Stun()
    {
        StartCoroutine(StunCor());
    }

    public float stunTime = 5f;
    IEnumerator StunCor()
    {
        freezeMove = true;
        agent.ResetPath();
        anim.SetTrigger("Stun");
        stunSfx.Play();
        yield return new WaitForSeconds(stunTime);
        freezeMove = false;
        if (target != null)
            SetChase();
        else
            SetWander();
    }

    public IEnumerator TryCatchTarget()
    {
        while (target != null)
        {
            float distance = Vector3.Distance(eyePos.position, target.position);
            if (distance <= catchRange)
            {
                if (player.isHide && !watchPlayerHiding)
                {
                    SetWander();
                    yield break;
                }
                freezeMove = true;
                print("타겟 잡음");
                agent.ResetPath();
                ActiveJumpScare();
                yield break;
            }
            yield return null;
        }
    }

    public bool watchPlayerHiding = false;
    public void PlayerHiding()
    {
        if (!CheckTarget_OutOfSight())
        {
            watchPlayerHiding = true;
        }
    }

    public void ActiveJumpScare()
    {
        if (!player.isHide)
        {
            jumpScare.StartJumpScare1();
        }
        else
        {
            if (watchPlayerHiding)
            {
                if (player.currentHideObject != null)
                {
                    transform.position = player.currentHideObject.MonsterPos.position;
                    transform.rotation = player.currentHideObject.MonsterPos.rotation;
                    jumpScare.StartJumpScare2();
                }
                else
                    jumpScare.StartJumpScare1();
            }
        }
    }

    public IEnumerator Miss_OutOfSight()
    {
        while (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance > missRange)
            {
                yield return new WaitForSeconds(missTime);
                distance = Vector3.Distance(transform.position, target.position);
                if (distance > missRange)
                {
                    SetWander();
                    yield break;
                }
            }
            
            if (CheckTarget_OutOfSight())
            {
                print("시야 가림");
                yield return new WaitForSeconds(missTime);
                if (CheckTarget_OutOfSight())
                {
                    print("타켓 취소");
                    SetWander();
                    yield break;
                } 
            }

            yield return null;
        }
    }

    // Inspector에서 검사할 레이어를 설정할 수 있도록 추가 (기본: 모든 레이어)
    public LayerMask obstructionMask = ~0;
    public bool CheckTarget_OutOfSight()
    {
        if (eyePos != null && target != null)
        {
            RaycastHit hit;
            // Linecast가 어떤 콜라이더에 맞았는지 검사 (obstructionMask로 레이어 필터링)
            Debug.DrawLine(eyePos.position, target.position, Color.red, 0.1f);
            if (Physics.Linecast(eyePos.position, target.position, out hit, obstructionMask))
            {
                // 맞은 콜라이더가 타겟(또는 자식)이라면 장애물이 아닌 것으로 간주
                if (hit.collider != null && !(hit.collider.transform == target || hit.collider.transform.IsChildOf(target)))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
