using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;


public enum HeroAttackType
{
    Melee,
    Ranged,
}

public class PlayerBehaviour : MonoBehaviourPun
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public static Vector3 CurrentPlayerPos;

    [Header("공격 방식")]
    public HeroAttackType heroAttackType;

    [Header("회전 속도")]
    [SerializeField] float RotateSpeed = 0.5f;
    public float RotateVelocity;

    [Header("레이어설정(확장성 고려하여 추가함)")]
    public LayerMask Layer;
    RaycastHit Hit;

    // 레이 찍을 카메라
    public Camera Cam;

    [Header("타겟")]
    public GameObject targetedEnemy;

    [Header("공격중인가")]
    public bool perfomMeleeAttack = false;


    public string EnemyTag;
    public bool IsAttack;

    #region Other Components
    NavMeshAgent _agent;
    Stats _statScript;


    #endregion

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _statScript = GetComponent<Stats>();
        _agent.enabled = false;
        _agent.enabled = true;
    }

    private void OnEnable()
    {

    }




    private void Start()
    {
        //PhotonView photonView = PhotonView.Get(this);
        //photonView.RPC("RPCStorageCaller", RpcTarget.MasterClient, playerStorage._id, playerStorage.session_id, playerStorage.userName, playerStorage.playerNumber, playerStorage.zera, playerStorage.ace, playerStorage.bet_id);

        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1 && photonView.IsMine)
            {
                gameObject.tag = "Blue";
                EnemyTag = "Red";
            }

            else
            {
                gameObject.tag = "Red";
                //gameObject.layer = 8;
                EnemyTag = "Blue";
            }
        }

        else
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 2 && photonView.IsMine)
            {
                gameObject.tag = "Red";
                EnemyTag = "Blue";
            }

            else
            {
                gameObject.tag = "Blue";
                //gameObject.layer = 8;

                EnemyTag = "Red";
            }
        }
    }



    private void Update()
    {
        // 플레이어 위치정보 카메라로 보냄
        if (photonView.IsMine)
        {
            CurrentPlayerPos = transform.position;
            _agent.speed = _statScript.MoveSpeed;
            MoveTo();

            // s키 누르면 멈춤
            if (Input.GetKeyDown(KeyCode.S))
            {
                _agent.SetDestination(CurrentPlayerPos);
                _agent.stoppingDistance = 0f;
            }
        }
    }

    public Ray ray;
    public void MoveTo()
    {

        HeroAliveCheck();

        // a + 좌클릭 이동
        AutoTargetInput();
            ray = Cam.ScreenPointToRay(Input.mousePosition);

        // 우클릭시 이동
        if (Input.GetMouseButton(1))
        {
            // 우클릭하면 좌클릭이동 스위치를 취소해줌
            inputA = false;

            // 마우스위치에서 쏜 raycast가 물체에 맞는다면, 그곳이 navmesh도착지점
            if (Physics.Raycast(ray, out Hit, Mathf.Infinity))
            {
                MoveOntheGround(Hit);
                GetTargetedObject();
            }
        }

        MoveEnemyPosition();
    }


    /// <summary>
    /// 적이 살아있는지 확인하는 메서드
    /// </summary>
    public void HeroAliveCheck()
    {
        //if (targetedEnemy != null)
        //{
        //    if (targetedEnemy.GetComponent<HeroCombat>() != null)
        //    {
        //        if (targetedEnemy.GetComponent<HeroCombat>().isHeroAlive)
        //        {
        //            targetedEnemy = null;
        //        }
        //    }
        //}
    }

    public void MoveOntheGround(RaycastHit hit)
    {
        // MOVEMENT
        // 클릭한 지점이 목적지
        _agent.SetDestination(hit.point);
        targetedEnemy = null;
        _agent.stoppingDistance = 0;

        // ROTATION
        Quaternion lookAtTheTarget = Quaternion.LookRotation(hit.point - transform.position);
        // 캐릭터가 타겟을 향해 회전하는 속도 보간
        // RotateSpeed값이 작을수록 빠르게 회전함
        float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, lookAtTheTarget.eulerAngles.y, ref RotateVelocity, RotateSpeed * (Time.deltaTime * 5));

        transform.eulerAngles = new Vector3(0, rotationY, 0);
    }

    public void MoveEnemyPosition()
    {
        // 타겟이 있을때
        if (targetedEnemy != null)
        {
            float dist = Vector3.Distance(transform.position, targetedEnemy.transform.position) - 0.5f;

            // 타겟이 공격범위 밖일때
            if (dist > _statScript.attackRange)
            {
                // 그 위치로 이동한다
                _agent.SetDestination(targetedEnemy.transform.position);
                _agent.stoppingDistance = _statScript.attackRange;

                Quaternion rotationToLookAt = Quaternion.LookRotation(targetedEnemy.transform.position - transform.position);

                float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                    rotationToLookAt.eulerAngles.y,
                    ref RotateVelocity,
                    RotateSpeed * (Time.deltaTime * 5));

                transform.eulerAngles = new Vector3(0, rotationY, 0);
            }
            else
            {
                _agent.SetDestination(targetedEnemy.transform.position);
                _agent.stoppingDistance = _statScript.attackRange;

                // 타겟을 바라본다
                Quaternion rotationToLookAt = Quaternion.LookRotation(targetedEnemy.transform.position - transform.position);

                float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                    rotationToLookAt.eulerAngles.y,
                    ref RotateVelocity,
                    RotateSpeed * (Time.deltaTime * 5));

                transform.eulerAngles = new Vector3(0, rotationY, 0);

                // 내가 근접캐라면
                if (heroAttackType == HeroAttackType.Melee)
                {
                    // 공격 수행 스위치를 true로 바꿈
                    perfomMeleeAttack = true;
                    //photonView.RPC("IsAttack", RpcTarget.All);
                }
            }

        }
    }



    private void GetTargetedObject()
    {
        if (Hit.collider.CompareTag(EnemyTag))
        {
            targetedEnemy = Hit.collider.gameObject;
        }
        else
        {
            targetedEnemy = null;
        }
    }


    bool inputA = false;

    private void AutoTargetInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            inputA = true;
            Debug.Log($"inputA : {inputA}");
        }

        if (Input.GetMouseButtonDown(0) && inputA)
        {
            inputA = false;
            // 누른 위치로 이동한다
            if (Physics.Raycast(Cam.ScreenPointToRay(Input.mousePosition), out Hit, Mathf.Infinity))
            {
                MoveOntheGround(Hit);
            }
            AutoTargetMove();

        }


    }

    float detectiveRange = 8f;
    private void AutoTargetMove()
    {
        
        // 태그 달고있는 게임오브젝트로 찾기
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(EnemyTag);

        // 터렛과 가장 가까운 대상 임시로 선언
        //GameObject _shortTarget = null;

        //콜라이더가 하나라도 검출되면 실행
        if (enemies.Length > 0)
        {

            // 검출된 콜라이더만큼 반복해주기
            foreach (GameObject _colTarget in enemies)
            {

                // 감지범위내에 적이 들어온다면 
                // 그 적을 타겟에 넣어줌
                float distance = Vector3.Distance(transform.position, _colTarget.transform.position);
                if (distance <= detectiveRange)
                {
                    // 미니언과 플레이어가 같이 있을땐 플레이어를 넣어준다
                    if (_colTarget.GetComponent<PlayerBehaviour>() != null)
                    {
                        GameObject enemyPlayer = _colTarget.GetComponent<PlayerBehaviour>().gameObject;
                        Debug.Log($"enemyPlayer : {enemyPlayer}");
                        targetedEnemy = enemyPlayer;
                    }
                    else
                    {
                        Debug.Log($"target : {_colTarget}");
                        targetedEnemy = _colTarget;
                    }

                }
            }
        }

    }




}