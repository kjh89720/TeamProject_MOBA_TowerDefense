using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

// 로비에서 로딩으로 넘어간 후, 발생할 일들을 정의한다.
// 1. 로딩 0% ~ 100% - 어떤 기준으로 할 지는 정해야한다.
// 2. 100%가 되면 Prototype_1 씬으로 모두 넘긴다.

public class LoadingMagager : MonoBehaviourPunCallbacks
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        loadingText.text = 0.ToString();
    }

    // isMain 테스트
    // Player 생성 프리펩
    public GameObject playerPrefab;

    // 로딩 상황을 보여줄 텍스트
    [SerializeField]
    private Text loadingText;

    // 로딩 게이지 변수
    private float loadingGage = 0f;

    // 델타 타임 변수
    private float deltaTime;

    // 기달리 시간
    private float waitTime = 0.5f;

    private void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
    }

    // 우선 임시로 시간에 맞는 로딩 게이지를 올려준다.
    private void Update()
    {
        // 내가 있는 곳이 로딩씬이면
        if (SceneManager.GetActiveScene().name == "Loading")
        {
            deltaTime += Time.deltaTime;
            loadingGage = (int)deltaTime;
            if (loadingGage == 2)
            {
                loadingGage = 1;
                ChangeMainScene();
            }
            loadingText.text = loadingGage.ToString() + "%";
        }
    }

    // 플레이어 위치
    // 아이디어
    // 1. 플레이어 홀이면 아래, 짝이면 위 고정
    // 2. 네트워크 상에서 나인 걸 증명할 수 있는 것이 필요함
    // 3. isMain을 사용한다고 하면 내 입력이 가능할때, 아래 입력이 불가능 한 플레이어는 위

    public void ChangeMainScene()
    {
        Debug.Log(PhotonNetwork.IsConnected);

        // 마스터 서버와 연결이 되어있고 로딩 게이지가 100%면
        if (PhotonNetwork.IsConnected && loadingGage == 1)
        {
            SceneManager.LoadScene("Prototype_1");
        }
    }
}
