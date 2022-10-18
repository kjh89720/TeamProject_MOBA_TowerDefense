using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySatatus : Enemybase
{
    // ###############################################
    //             NAME : KimJaeMin                      
    //             MAIL : woals1566@gmail.com         
    // ###############################################

    public Transform _target;
  
    private Transform _PrevTarget;
    Rigidbody _rigidbody;
    enum ESTATE
    {
        move,
        attack
    }
    ESTATE _estate;
   public bool canAttack { get; private set; }
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    
    float distance;

    void Awake()
    {
         
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _PrevTarget = _target;
        _navMeshAgent.enabled = false;
        _navMeshAgent.enabled = true;


    }

    private void Start()
    {
        _navMeshAgent.speed = 5f;
        StartCoroutine(StateChange());
    }

    private IEnumerator move() // 움직임
    {
        while (true)
        {
            if (_target == null)
            {
                _target = _PrevTarget;
            }
            _animator.SetBool("Attack",false);
            _navMeshAgent.SetDestination(_target.position);
            transform.LookAt(_target.position);
            distance = Vector3.Distance(_target.position, transform.position);
            if (distance < 2f)
            {
                _estate = ESTATE.attack;
                Debug.Log($"{_estate}");

                break;
            }
            yield return null;

        }
    }
    private IEnumerator Attack() // 공격
    {
        while (true)
        {
            if (_navMeshAgent.enabled == false)
            {
                break;
            }
            // 구분
            if (_target == null)
            {
                _target = _PrevTarget;
            }
           float AttackDistance = Vector3.Distance(transform.position,_target.position);
            _animator.SetBool("Attack",true);
            // 애니메이션 추가 + 공격데미지 입히기
            //yield return new WaitForSeconds(1f); //공격쿨타임
            canAttack = true;

                if (AttackDistance > 2f)
                {
                    _estate = ESTATE.move;
                   
                }                 
        yield return null;
        }
    }

    private IEnumerator StateChange()
    {     
        while (true)
        {

            switch (_estate)
            {
                case ESTATE.attack:
                    StartCoroutine(Attack());
                    StopCoroutine(move());
                    break;
                default:
                    StartCoroutine(move());
                    StopCoroutine(Attack());
                    break;

            }
        yield return null;
        }

    }

  

   
}

