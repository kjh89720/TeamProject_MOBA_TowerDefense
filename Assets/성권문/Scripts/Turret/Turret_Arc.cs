using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################
public class Turret_Arc : Turret
{
    private Transform target;
    private EnemyMinion targetEnemy;

    [Header("사거리")]
    public float range = 15f;

    [Header("공격주기(초당 n번 발사)")]
    public float fireRate = 1f;

    [Header("회전체")]
    public Transform partToRotate;

    [Header("회전속도")]
    public float turnSpeed = 10f;

    [Header("====== 범위 레이저 속성======")]
    private Animator Arc_animator;

    private void Start()
    {
        Arc_animator = GetComponent<Animator>();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        StartCoroutine(Arc());
    }

    // 가장 가까운 적을 찾는다, 단 자주찾지는 않는다. 
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        // 가장 가까운 적과의 거리
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        // 적이 범위안에 들어왔고, 적과의 거리가 범위값보다 작을경우
        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<EnemyMinion>();
        }
        else
        {
            target = null;
        }
    }

    private void Update()
    {
        // 적이 범위밖으로 사라져 target이 null이 되면 리턴한다.
        if (target == null)
        {
            return;
        }
    }

    // ★ 광범위 레이저 발사
    IEnumerator Arc()
    {
        while (true)
        {
            if (target != null)
            {
                Vector3 dir = target.position - transform.position;
                Vector3 dir_y = new Vector3(dir.x, 0f, dir.z);
                partToRotate.transform.rotation = Quaternion.LookRotation(dir_y);

                if (Arc_animator.GetBool("onAttack") == false)
                {
                    Arc_animator.SetBool("onAttack", true);
                }
                else
                {
                    Arc_animator.SetBool("onAttack", false);
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    // 범위 그리기
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
