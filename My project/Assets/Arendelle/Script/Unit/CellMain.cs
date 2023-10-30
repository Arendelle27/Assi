using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class CellMain : Unit
{

    void Start()
    {
        this.Init();
    }

    public override void Update()
    {
        if(Game.Instance.Status!=GameState.InGame)
            return;
        if (this.CurHP <= 0)
        {
            Game.Instance.Status = GameState.End;
            Game.Instance.Init();
        }

        base.Update();

        if (curIdleTime < idleTime)
        {
            curIdleTime += Time.deltaTime;
        }
        else
        {
            CurHP += hpRecover * Time.deltaTime;
        }

        if (!this.isinjured && this.spriteRenderer.sprite != Game.Instance.spriteManager.characterSpriteDic[Tool.CeKIndToChaTy(this.cellKind)][characterStatus.Normal])
        {
            this.spriteRenderer.sprite = Game.Instance.spriteManager.characterSpriteDic[Tool.CeKIndToChaTy(this.cellKind)][characterStatus.Normal];
        }

        Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        this.rb.velocity = dir * speed;
    }

    public override void Init()
    {
        this.transform.position = new Vector2(0, 0);
        this.maxHP = Game.Instance.effectManager.dataAcc[CellKind.Player][DataType.Hp];
        this.normSpeed = Game.Instance.effectManager.dataAcc[CellKind.Player][DataType.Speed];
        this.attack= Game.Instance.effectManager.dataAcc[CellKind.Player][DataType.Attack];

        this.CurHP = this.maxHP;
        this.speed = this.normSpeed;
        this.curIdleTime = 0f;
        this.rb.velocity = Vector2.zero;
    }
}
