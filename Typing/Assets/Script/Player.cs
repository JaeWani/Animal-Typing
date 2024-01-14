using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Player : MonoBehaviour
{
    private InputField inputField;

    public PlayerState currentState = PlayerState.Master;

    void Start()
    {
        inputField = GameManager.instance.inputField;

        if (PhotonNetwork.IsMasterClient) currentState = PlayerState.Master;
        else currentState = PlayerState.User;
    }

    private void Update()
    {
        Check();
    }

    private void Check()
    {
        if (Input.GetKeyDown(KeyCode.A)) 
        {
            Debug.Log("Ű�� ����");
            if (inputField.text == GameManager.instance.currentAttackSentence)
            {
                Debug.Log("����� ��");
                GameManager.SetRandomAttackSentence(GameManager.instance.randomValue, GameManager.instance.photonView.GetInstanceID());
            }
        }
    }
}
