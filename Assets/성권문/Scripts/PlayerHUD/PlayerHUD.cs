using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class PlayerHUD : MonoBehaviourPun
{
    // ###############################################
    //             NAME : ARTSUNG                      
    //             MAIL : artsung410@gmail.com         
    // ###############################################

    [Header("PlayerStatusUI")]
    public Image playerHealthBar;
    public Image playerExperienceBar;
    public TextMeshProUGUI playerHealthBarTMpro;
    public TextMeshProUGUI playerExperienceBarTMpro;
    public TextMeshProUGUI playerInfoTMPro;

    [Header("InfoUI")]
    public GameObject EnemyStatusInfoUI;

    [Header("SkillUI")]
    public GameObject skillTable;

    [Header("ScoreUI")]
    public TextMeshProUGUI scoreTMPro;
    public TextMeshProUGUI timerTMPro;
    public static PlayerHUD Instance;

    private Health playerHp;
    private void Awake()
    {
        Instance = this;
        setHp();
    }

    private void setHp()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            playerHp = GameManager.Instance.CurrentPlayers[0].GetComponent<Health>();
        }
    }

    private void Start()
    {
        SetSkill();
        //Reset_Timer();
    }

    private void SetSkill()
    {
        int count = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().skillItems.Count;

        for (int i = 0; i < count; i++)
        {
            Item item = GameObject.FindGameObjectWithTag("GetCaller").gameObject.GetComponent<TrojanHorse>().skillItems[i];
            skillTable.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
            skillTable.transform.GetChild(i).GetChild(0).GetComponent<Skillicon>().item = item;
            skillTable.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = item.itemIcon;
        }
    }

    float sec;
    int min;

    void Update()
    {
        Timer();
        UpdateHealthUI();
    }

    void Timer()
    {
        sec += Time.deltaTime;
        timerTMPro.text = string.Format("{0:D2}:{1:D2}", min, (int)sec);

        if ((int)sec > 59)
        {
            sec = 0;
            min++;
        }
    }

    float hp;
    void UpdateHealthUI()
    {
        if (playerHp == null)
        {
            return;
        }

        if (photonView.IsMine)
        {
            playerHealthBar.fillAmount = playerHp.hpSlider3D.value / 250f;
            hp = (playerHp.hpSlider3D.value / 250f) * 100f;

            playerHealthBarTMpro.text = hp.ToString() + " / 100";
        }
    }
}
