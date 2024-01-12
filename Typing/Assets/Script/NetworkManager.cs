using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region Variable
    public static NetworkManager instance;
    #endregion


    #region Unity_Function
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        Screen.SetResolution(1920, 1080, false);
        PhotonNetwork.ConnectUsingSettings();
    }

    private void Update()
    {

    }
    #endregion

    #region Photon_Function
    public static void CreateRoom(string roomName) => PhotonNetwork.CreateRoom(roomName);
    public static void JoinRoom(string roomName) => PhotonNetwork.JoinRoom(roomName);

    public static void SetPlayerName(string playerName)
    {
        PhotonNetwork.NickName = playerName;
        Debug.Log(PhotonNetwork.NickName);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("아이고난");
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
    }
    #endregion
}
