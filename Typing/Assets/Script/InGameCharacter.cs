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

    public PlayerState playerState;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        pv = GetComponent<PhotonView>();

        float x = playerState == PlayerState.Master ? -5 : 5;
        transform.DOMove(new Vector3(x, 2, 0), 0.5f);

        spriteRenderer.flipX = playerState == PlayerState.Master ? false : true;
    }
}
