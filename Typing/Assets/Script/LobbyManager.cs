using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LobbyManager : MonoBehaviour
{
    #region Variable
    [Header("UI_RectTransform")]
    [SerializeField] private RectTransform roomJoinPanelRect;
    [SerializeField] private RectTransform roomMakingPanelRect;


    [Header("UI_Button")]
    [SerializeField] private Button mainRoomJoinButton;
    [SerializeField] private Button mainRoomMakingButton;

    [SerializeField] private Button roomJoinPlayerNameEnterButton;
    [SerializeField] private Button roomJoinRoomNameEnterButton;
    [SerializeField] private Button roomJoinTitleButton;

    [SerializeField] private Button roomMakingPlayerNameEnterButton;
    [SerializeField] private Button roomMakingRoomNameEnterButton;
    [SerializeField] private Button roomMakingTitleButton;

    [Header("UI_InputField")]
    [SerializeField] private InputField roomJoinPlayerNameInputField;
    [SerializeField] private InputField roomJoinRoomNameInputField;
    [SerializeField] private InputField roomMakingPlayerNameInputField;
    [SerializeField] private InputField roomMakingRoomNameInputField;

    #endregion

    private void Start()
    {
        AddButtonsOnClick();
    }

    private void Update()
    {

    }

    private void AddButtonsOnClick()
    {
        mainRoomJoinButton.onClick.AddListener(() => { roomJoinPanelRect.DOAnchorPosX(0, 1).SetEase(Ease.OutQuad); });
        mainRoomMakingButton.onClick.AddListener(() => { roomMakingPanelRect.DOAnchorPosX(0, 1).SetEase(Ease.OutQuad); });

        roomJoinTitleButton.onClick.AddListener(() => { roomJoinPanelRect.DOAnchorPosX(-2000, 1).SetEase(Ease.OutQuad); });
        roomMakingTitleButton.onClick.AddListener(() => { roomMakingPanelRect.DOAnchorPosX(2000, 1).SetEase(Ease.OutQuad); });

        roomJoinPlayerNameEnterButton.onClick.AddListener(() =>
        {
            if (!string.IsNullOrWhiteSpace(roomJoinPlayerNameInputField.text))
            {
                NetworkManager.SetPlayerName(roomJoinPlayerNameInputField.text);
                roomJoinPlayerNameInputField.gameObject.SetActive(false);
                roomJoinPlayerNameEnterButton.gameObject.SetActive(false);
            }
        });

        roomMakingPlayerNameEnterButton.onClick.AddListener(() =>
        {
            if (!string.IsNullOrWhiteSpace(roomMakingPlayerNameInputField.text))
            {
                NetworkManager.SetPlayerName(roomMakingPlayerNameInputField.text);
                roomMakingPlayerNameInputField.gameObject.SetActive(false);
                roomMakingPlayerNameEnterButton.gameObject.SetActive(false);
            }
        });


    }
}
