using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

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
        Debug.Log("���� ���� ��");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Room Joined!");
        check();
    }
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        GameManager.instance.currentAttackSentence = (string)PhotonNetwork.CurrentRoom.CustomProperties["MainSentence"];
        GameManager.instance.currentHealWord = (string)PhotonNetwork.CurrentRoom.CustomProperties["HealSentence"];
        GameManager.instance.currentInterferenceWord = (string)PhotonNetwork.CurrentRoom.CustomProperties["InterferenceSentence"];
        Debug.Log("아이고난");
    }
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
    {
        Debug.Log("Player Properties Value : ");
        bool isNull = true;
        foreach (var item in changedProps.Keys) if ((string)item == "HP") isNull = false;
        if (targetPlayer.IsMasterClient && !isNull) GameManager.instance.Player_1_Score = (int)changedProps["HP"];
        if (!targetPlayer.IsMasterClient && !isNull) GameManager.instance.Player_2_Score = (int)changedProps["HP"];
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room Created!");
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log("�̻��� ����");
    }
    public override void OnLeftRoom()
    {
        Debug.Log("���� �������ϴ�.");
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

    private void _SetCustomPropertiesCR(string name, string value)
    {
        Hashtable ht = new Hashtable();
        ht[name] = value;

        PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
    }
    public static void SetCustomPropertiesCR(string name, string value) => instance._SetCustomPropertiesCR(name, value);

    private void _SetCustomPropertiesLP(string name, int value)
    {
        Hashtable ht = new Hashtable();
        ht[name] = value;

        PhotonNetwork.LocalPlayer.SetCustomProperties(ht);
    }
    public static void SetCustomPropertiesLP(string name, int value) => instance._SetCustomPropertiesLP(name, value);

    private Hashtable _GetCustomPropertiesCR()
    {
        Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;

        return ht;
    }
    public static Hashtable GetCustomPropertiesCR() => instance._GetCustomPropertiesCR();

    private Hashtable _GetCustomPropertiesLP()
    {
        Hashtable ht = PhotonNetwork.LocalPlayer.CustomProperties;

        return ht;
    }
    public static Hashtable GetCustomPropertiesLP() => instance._GetCustomPropertiesCR();
    #endregion
}
