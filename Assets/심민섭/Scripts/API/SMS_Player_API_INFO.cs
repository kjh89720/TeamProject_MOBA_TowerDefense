﻿using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class SMS_Player_API_INFO : MonoBehaviourPun
{
    private GameObject apiStorageObj;
    private APIStorage aPIStorage;
    public GameObject APIStoragePre;
    public GameObject PostAPICallerPre;
    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Instantiate(APIStoragePre, Vector3.zero, Quaternion.identity);
            Instantiate(PostAPICallerPre, Vector3.zero, Quaternion.identity);
            apiStorageObj = GameObject.FindGameObjectWithTag("APIStorage").gameObject;
            aPIStorage = apiStorageObj.GetComponent<APIStorage>();
            GameObject getCallerObj = GameObject.FindGameObjectWithTag("GetCaller").gameObject;
            PlayerStorage playerStorage = getCallerObj.GetComponent<PlayerStorage>();
            // 데이터를 APIStorage에 넣는다.
            aPIStorage._id[0] = playerStorage._id;
            aPIStorage.session_id[0] = playerStorage.session_id;
            aPIStorage.userName[0] = playerStorage.userName;
            aPIStorage.zera[0] = playerStorage.zera;
            aPIStorage.ace[0] = playerStorage.ace;
            aPIStorage.bet_id[0] = playerStorage.bet_id;
        }


        if (!PhotonNetwork.IsMasterClient)
        {
            GameObject getCallerObj = GameObject.FindGameObjectWithTag("GetCaller").gameObject;
            PlayerStorage playerStorage = getCallerObj.GetComponent<PlayerStorage>();
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("RPCStorageCaller", RpcTarget.MasterClient, playerStorage._id, playerStorage.session_id, playerStorage.userName, playerStorage.playerNumber, playerStorage.zera, playerStorage.ace, playerStorage.bet_id);
        }
    }

    /*[SerializeField]
    private Text text;*/

    [PunRPC]
    private void RPCStorageCaller(string id, string session_id, string userName, int playerNumber, string zera, string ace, string bet_id)
    {
        //text.text = $"id : {id}\nsession_id : {session_id}\nuserName : {userName}\nplayerNumber : {playerNumber}\nzera : {zera}\nace : {ace}\nbet_id : {bet_id}";
        apiStorageObj = GameObject.FindGameObjectWithTag("APIStorage").gameObject;
        aPIStorage = apiStorageObj.GetComponent<APIStorage>();
        aPIStorage._id[1] = id;
        aPIStorage.session_id[1] = session_id;
        aPIStorage.userName[1] = userName;
        aPIStorage.playerNumber[1] = playerNumber;
        aPIStorage.zera[1] = zera;
        aPIStorage.ace[1] = ace;
        aPIStorage.bet_id[1] = bet_id;
    }
    // 호스트 쪽에서 RPC호출한다.
    // RPC함수에는 리모트 데이터를 호스트쪽에 띄워주는 것으로 확인한다.
}
