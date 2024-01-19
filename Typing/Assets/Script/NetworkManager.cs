using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;


public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region Variable
    public static NetworkManager instance;

    [SerializeField] private string playerNickName;
    public string PlayerNickName
    {
        get { return playerNickName; }
        set
        {
            playerNickName = value;
            PhotonNetwork.NickName = value;
        }
    }

    public Character currentCharacter;

    public Character player_1_Character;
    public Character player_2_Character;

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
    }

    private void Update()
    {

    }
    #endregion

    #region Photon_Function
    public static void CreateRoom(string roomName)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.IsVisible = true;

        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }
    public static void JoinRoom(string roomName) => PhotonNetwork.JoinRoom(roomName);

    public static void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public static void PrintCurrentRoom() => Debug.Log(PhotonNetwork.CurrentRoom);

    public override void OnConnectedToMaster()
    {
        Debug.Log("서버 연결 됨");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Room Joined!");
        check();
    }


    public override void OnCreatedRoom()
    {
        Debug.Log("Room Created!");
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log("이새끼 나감");
    }
    public override void OnLeftRoom()
    {
        Debug.Log("방을 나갔습니다.");
    }

    public static void check() => instance.CheckPlayerCharacter();
    public void CheckPlayerCharacter()
    {

        NetworkManager.instance.player_1_Character = (Character)PhotonNetwork.MasterClient.CustomProperties["icon"];
        foreach (var item in PhotonNetwork.PlayerList)
        {
            if (!PhotonNetwork.IsMasterClient)
                NetworkManager.instance.player_2_Character = (Character)item.CustomProperties["icon"];
        }
    }
    #endregion
}
