using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class CellAcc : Unit
{
    public bool isMove = false;

    public int id;

    private void OnEnable()
    {
        
    }


    void Start()
    {
        Observable
        .EveryUpdate()
.Where(_ => !this.isinjured && this.spriteRenderer.sprite != Game.Instance.spriteManager.characterSpriteDic[Tool.CeKIndToChaTy(this.cellKind)][characterStatus.Normal])
.Subscribe(_ =>
{
    this.spriteRenderer.sprite = Game.Instance.spriteManager.characterSpriteDic[Tool.CeKIndToChaTy(this.cellKind)][characterStatus.Normal];
})
.AddTo(this);//当游戏对象销毁时，自动取消订阅

        //特殊植物:嘴唇花
        if (this.cellKind == CellKind.ZuiChunHua)
        {
            Observable
           .EveryUpdate()
           .Subscribe(_ =>
           {
               if (!this.enabled)
                   return;
               if (CellAccManager.Instance.cellEnemyList.Count != 0)
               {
                   foreach (var cellAcc in CellAccManager.Instance.cellEnemyList)
                   {
                       if (cellAcc.Value.gameObject.activeSelf&&(cellAcc.Value.transform.position - this.transform.position).magnitude <= 3f)
                       {
                           cellAcc.Value.Slow();
                       }
                   }
               }
           })
           .AddTo(this);//当游戏对象销毁时，自动取消订阅
        }
    }

    public bool isBack=false;
    public override void Update()
    {
        if (this.CurHP <= 0)
        {
            if(this.onDead!=null)
                this.onDead(this.id);
            StartCoroutine(CellAccDeath());
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

        if (!isMove)
        {
            if ((this.transform.position - Game.Instance.CellMain.transform.position).magnitude >= 2f)
            {
                isBack=true;
                this.rb.velocity = speed * (Game.Instance.CellMain.transform.position - this.transform.position).normalized;
            }
            else if ((this.transform.position - Game.Instance.CellMain.transform.position).magnitude <= 1.5f)
            {
                this.rb.velocity = -1 * speed * (Game.Instance.CellMain.transform.position - this.transform.position).normalized;
            }
            else
            {
                if (isBack) isBack = false;
                this.rb.velocity = Game.Instance.CellMain.rb.velocity;
            }
        }
    }


    public IEnumerator BragMove()
    {
        while (this.isMove)
        {
            if (InputManager.Instance.mousePositionQueue.Count > 0 )
            {
                Vector2 mousePosition = InputManager.Instance.mousePositionQueue.Dequeue();
                if((mousePosition-(Vector2)this.transform.position).magnitude>0.2f)
                    this.rb.velocity = speed * (mousePosition - (Vector2)this.transform.position).normalized;
                else
                    this.rb.velocity = new Vector2(0, 0);
                yield return new WaitForSeconds(0.03f);
            }
        }
    }


    IEnumerator CellAccDeath()
    {
        this.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        Destroy(this.gameObject);
    }
}
