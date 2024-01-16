using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;

public enum Character
{
    AKA,
    NoAlLa,
    Frogy,
    Tusoteuthis
}
public class LobbyManager : MonoBehaviourPunCallbacks
{
    #region Variable
    [SerializeField] private InputField nickNameInputField;

    [SerializeField] private Button joinButton;
    [SerializeField] private Button nickNameEnterButton;

    [SerializeField] private Text connectionInfoText;

    [SerializeField] private RectTransform nickNamePanel;
    [SerializeField] private RectTransform roomJoinPanel;

    [SerializeField] private Button leftArrowButton;
    [SerializeField] private Button rightArrowButton;

    [SerializeField] private Image characterSelectImage;

    [SerializeField] private List<Sprite> animalSprites = new List<Sprite>();

    [SerializeField] private Character currentCharacter = Character.AKA;

    bool isJoinRoom = false;
    #endregion

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        joinButton.interactable = false;

        joinButton.onClick.AddListener(() =>
        {
            Connect();
            NetworkManager.instance.currentCharacter = currentCharacter;
            SoundManager.PlaySound("Button_Sound", 1, false);
        });
        nickNameEnterButton.onClick.AddListener(() =>
        {
            NickNameEnter(nickNameInputField.text);
            SoundManager.PlaySound("Button_Sound", 1, false);
        });

        leftArrowButton.onClick.AddListener(() =>
        {
            int a = (int)currentCharacter;
            if (a - 1 < 0) a = 3;
            else a--;
            currentCharacter = (Character)a;
            characterSelectImage.sprite = animalSprites[(int)currentCharacter];
        });

        rightArrowButton.onClick.AddListener(() => 
        {
            int a = (int)currentCharacter;
            if (a + 1 > 3) a = 0;
            else a++;
            currentCharacter = (Character)a;
            characterSelectImage.sprite = animalSprites[(int)currentCharacter];
        });

        if (PhotonNetwork.IsConnected)
        {
            connectionInfoText.text = "������ ����Ǿ����ϴ�.";
            joinButton.interactable = true;
        }
    }

    public override void OnConnectedToMaster()
    {
        connectionInfoText.text = "������ ����Ǿ����ϴ�.";
        joinButton.interactable = true;
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        connectionInfoText.text = "������ ������ϴ� !";
        joinButton.interactable = false;

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionInfoText.text = "�ٸ� ���� �������� �ʾ�, \n ���� ���� ���� �� �Դϴ�.";
        PhotonNetwork.CreateRoom("", new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "�濡 ���� �Ǿ����ϴ� !";
    }


    public void Connect()
    {
        joinButton.interactable = false;
        connectionInfoText.text = "���� �濡 �������Դϴ� !";

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    private void NickNameEnter(string nickName)
    {
        if (!string.IsNullOrWhiteSpace(nickName))
        {
            nickNamePanel.DOAnchorPosX(2000, 1).SetEase(Ease.OutQuad).OnComplete(() =>
             {
                 roomJoinPanel.DOAnchorPosX(0, 1).SetEase(Ease.OutQuad);
             });

            PhotonNetwork.NickName = nickName;
        }
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.CurrentRoom != null && !isJoinRoom && PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {

            isJoinRoom = true;
            PhotonNetwork.LoadLevel("In Game");
        }
    }

}
