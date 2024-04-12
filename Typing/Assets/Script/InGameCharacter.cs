using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;

public class InGameCharacter : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    PhotonView pv;
    public bool isMaster = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        pv = GetComponent<PhotonView>();

        pv.RPC("FlipSet", RpcTarget.All);
    }

    private void FilpSet()
    {
        if (pv.IsMine)
        {
            if (PhotonNetwork.IsMasterClient) transform.DOLocalMoveX(-6, 1).SetEase(Ease.OutQuad);
            else transform.DOLocalMoveX(6, 1).SetEase(Ease.OutQuad);

            if (PhotonNetwork.IsMasterClient) spriteRenderer.flipX = false;
            else spriteRenderer.flipX = true;
        }
    }
}
