using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;

public class InGameCharacter : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public bool isMaster = false;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (isMaster) transform.DOLocalMoveX(-6,1).SetEase(Ease.OutQuad);
        else transform.DOLocalMoveX(6, 1).SetEase(Ease.OutQuad);

    }

}
