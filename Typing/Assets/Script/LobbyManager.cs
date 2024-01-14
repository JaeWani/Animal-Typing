using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    #region Variable
    [SerializeField] private InputField nickNameInputField;

    [SerializeField] private Button joinButton;
    [SerializeField] private Button nickNameEnterButton;

    [SerializeField] private Text connectionInfoText;

    [SerializeField] private RectTransform nickNamePanel;
    [SerializeField] private RectTransform roomJoinPanel;

    bool isJoinRoom = false;
    #endregion

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        joinButton.interactable = false;

        joinButton.onClick.AddListener(() => Connect());
        nickNameEnterButton.onClick.AddListener(() => NickNameEnter(nickNameInputField.text));
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
        connectionInfoText.text = "�ٸ� ���� �������� �ʾ�, ���� ���� ���� �� �Դϴ�.";
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
            nickNamePanel.DOAnchorPosX(2000,1).SetEase(Ease.OutQuad).OnComplete(() => 
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
