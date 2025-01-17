using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// Place-bet
public struct placeBet
{
    public string[] players_session_id;
    public string bet_id;
}

public class MatchDetails
{
}

// Winner
public struct winner
{
    public string betting_id;
    public string winner_player_id;
    public MatchDetails match_details; // 빈 값
}

// Disconnect
public struct disconnect
{
    public string betting_id;
}

public class APIStorage : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // 데이터 저장은 2인대전만 한다.
    // [0] : Player1(Host), [1] : Player2(Client)

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public int[] playerNumber = new int[2];

    public string apiKey = "70pNqHWqzZ0DXwsIP0e0bA";

    // 유저 배팅 준비 상태
    public bool[] ready = new bool[2];

    // 유저의 재화를 저장할 변수
    public string[] zera = new string[2];
    public string[] ace = new string[2];
    // API 호출 상태 코드 200이 아니면 에러를 반환한다.
    public string[] statusCode = new string[2];

    // 유저 고유 ID
    public string[] _id = new string[2];

    // 유저 고유 이름
    public string[] userName = new string[2];

    // 매번 바뀌는 세션 ID
    public string[] session_id = new string[2];

    // Post 메서드 호출 시, "success"
    public string[] message = new string[2];

    // 승리자 배당금
    public string amount_won;

    // 배팅에 필요한 ID
    public string[] bet_id = new string[2];

    // 배팅을 했다는 배팅 정보가 묶여 있는 ID
    public string betting_id;

    // 우승자 id
    public string winner_id = "633b86420e028f7ecb10fd09";

    // (임시) MetaMask _id(테스트 용도)
    public string MetaMaskSessionID = "eiry4c7tix9T06Q2yObbrghBrQTETTTeorDSAv2R";

}
