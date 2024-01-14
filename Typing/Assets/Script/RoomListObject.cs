using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomListObject : MonoBehaviour
{
    [SerializeField] private Text roomInfoText;
    private Button button;

    private RoomInfo roomInfo;
    public RoomInfo RoomInfo
    {
        get { return roomInfo; }
        set
        {
            roomInfo = value;
            roomInfoText.text = roomInfo.Name + "  " + roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers;
            button.onClick.AddListener(() => OnEnterRoom(roomInfo.Name));
        }
    }
    private void Awake()
    {

    }

    private void OnEnterRoom(string roomName) 
    { 
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 2;
    }
}

