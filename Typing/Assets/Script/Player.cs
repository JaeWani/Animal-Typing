using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

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
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Hashtable ht = NetworkManager.GetCustomPropertiesCR();
            if (inputField.text == GameManager.instance.currentAttackSentence)
            {
                GameManager.NextAttackSentence();
                inputField.text = null;
            }
            else if (inputField.text == GameManager.instance.currentHealWord)
            {
                GameManager.NextHealWord();
                inputField.text = null;
            }
            else if (inputField.text == GameManager.instance.currentInterferenceWord)
            {
                GameManager.NextInterference();
                inputField.text = null;
            }
        }
    }
}
