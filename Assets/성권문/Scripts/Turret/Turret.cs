using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

// ###############################################
//             NAME : ARTSUNG                      
//             MAIL : artsung410@gmail.com         
// ###############################################

public class Turret : MonoBehaviourPun
{
    public float currentHealth;
    public float maxHealth;
    public Image healthbarImage;
    public GameObject ui;
    public GameObject destroyParticle;
    public float destorySpeed;

    [Header("Ÿ�� TAG")]
    public string enemyTag;

    protected void OnEnable()
    {
        currentHealth = maxHealth;
        GameManager.Instance.CurrentTurrets.Add(gameObject);

        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1 && photonView.IsMine)
            {
                gameObject.tag = "Blue";
                enemyTag = "Red";
            }

            else
            {
                gameObject.tag = "Red";
                enemyTag = "Blue";
            }
        }

        else
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 2 && photonView.IsMine)
            {
                gameObject.tag = "Red";
                enemyTag = "Blue";
            }

            else
            {
                gameObject.tag = "Blue";
                enemyTag = "Red";
            }
        }
    }

    public void Damage(float damage)
    {
        Debug.Log("Damage ����");
        photonView.RPC("TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    public void TakeDamage(float damage)
    {
        Debug.Log("Damage RPC����");

        currentHealth -= damage;
        healthbarImage.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Destroy();
            return;
        }
    }

    public void Destroy()
    {
        StartCoroutine(Destructing());
        StartCoroutine(Destruction(PhotonNetwork.Instantiate(destroyParticle.name, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), transform.rotation)));
    }

    IEnumerator Destructing()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.01f);
            transform.Translate(Vector3.down * Time.deltaTime * destorySpeed);
        }
    }

    IEnumerator Destruction(GameObject particle)
    {
        yield return new WaitForSeconds(1.5f);
        PhotonNetwork.Destroy(particle);
        PhotonNetwork.Destroy(gameObject);
    }
}
