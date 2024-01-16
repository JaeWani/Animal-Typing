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
        if (Input.GetKeyDown(KeyCode.Return))
        {
            inputField.ActivateInputField();
            if (inputField.text == GameManager.instance.currentAttackSentence)
            {
                Debug.Log("°ø°Ý");
                GameManager.NextAttackSentence(GameManager.instance.randomAttackValue, GameManager.instance.photonView.GetInstanceID(), currentState);
                inputField.text = null;
            }
            else if (inputField.text == GameManager.instance.currentHealWord)
            {
                Debug.Log("Èú");
                GameManager.NextHealWord(GameManager.instance.randomHealValue, GameManager.instance.photonView.GetInstanceID(), currentState);
                inputField.text = null;
            }
            else if (inputField.text == GameManager.instance.currentInterferenceWord)
            {
                Debug.Log("¹æÇØ");
                GameManager.NextInterference(GameManager.instance.randomInterference, GameManager.instance.photonView.GetInstanceID());
                inputField.text = null;
            }
        }
    }
}
