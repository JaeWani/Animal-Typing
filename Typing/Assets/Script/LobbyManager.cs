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
        connectionInfoText.text = "서버와 연결되었습니다.";
        joinButton.interactable = true;
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        connectionInfoText.text = "연결이 끊겼습니다 !";
        joinButton.interactable = false;

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionInfoText.text = "다른 방이 존재하지 않아, 새로 방을 생성 중 입니다.";
        PhotonNetwork.CreateRoom("", new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "방에 참가 되었습니다 !";
    }


    public void Connect()
    {
        joinButton.interactable = false;
        connectionInfoText.text = "랜덤 방에 참가중입니다 !";

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
