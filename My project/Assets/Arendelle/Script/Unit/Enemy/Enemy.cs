using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;


public class Enemy : Unit
{
    #region 属性
    [SerializeField, LabelText("随机移动时间"), Tooltip("该单位移动周期中，随机移动状态用时间")]
    public float raMoveTimer = 2.0f;

    [SerializeField, LabelText("目标移动时间"), Tooltip("该单位移动周期中，朝目标移动状态用时间")]
    public float aimMoveTimer = 4.0f;


    [SerializeField, LabelText("被弹开速度"), Tooltip("被弹开瞬间的速度")]
    public float maxBlastSpeed = 15f;

    [SerializeField, LabelText("死亡持续时间"), Tooltip("生命值清空后开始计时")]
    public float deadTime = 2f;
    #endregion

    [SerializeField, LabelText("中毒时间"), Tooltip("受到中毒效果的持续时间")]
    public float decaytime = 3f;//受到中毒效果的时间，不一定为3f
    #region 状态
    public float blastSpeed = 0f;//被弹开时速度

    public Vector2 blastdir= new Vector2(0,0);//被弹开时方向

    public bool canAss = false;//是否接触到玩家，用于在死亡时间里判断是否可以被吸收
    #endregion


    #region 事件
    private Subject<int> assSubject = new Subject<int>();
    public IObservable<int> Ass => assSubject;
    #endregion

    public override void Init()
    {
        this.GetDataEn(this.plantKind);

        this.co.isTrigger = true;
        this.gameObject.layer = LayerMask.NameToLayer("Enemy");
        this.curStatus = Unit_Status.attack;

        base.Init();

        this.StartMove();
    }

    public override void StopCor()
    {
        base.StopCor();

        if(move!=null)
        {
            move = null;
        }
        if(de!=null)
        {
            de=null;
        }
    }

    void StartMove()
    {
        move = StartCoroutine(Move());
    }

    Coroutine move;//移动协程
    IEnumerator Move()//移动方式
    {
        while(this.curStatus==Unit_Status.attack)
        {
            for (int i = 0; i < (int)(this.aimMoveTimer / 0.5f); i++)
            {
                this.direction = (Vector2)(PlantManager.Instance.player.transform.position - this.transform.position).normalized;
                yield return new WaitForSeconds(0.5f);
            }
            for (int i = 0; i < (int)(this.raMoveTimer/0.5f); i++) 
            {
                this.direction = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    public void StartDecay()//开始中毒
    {
        if (de != null)
        {
            StopCoroutine(de);
            de = null;
        }
        de = StartCoroutine(Decay());
    }

    public Coroutine de;//减速协程
    IEnumerator Decay()
    {
        for (int i = 0; i < decaytime / 1f; i++)
        {
            this.CurHP -= 2;
            yield return new WaitForSeconds(1f);
        }
        StopCoroutine(de);
        de = null;

    }

    private void Update()//移动
    {
        if (this.curStatus==Unit_Status.attack) 
        { 
            this.rb.velocity=direction*curSpeed+ blastSpeed * blastdir; 

            if(this.blastSpeed>0)
            {
                blastSpeed -= 5 * blastSpeed * Time.deltaTime;
            }
        }
        else if(this.curStatus==Unit_Status.dead)
        {
            this.rb.velocity = Vector2.zero;
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)//进入检测
    {
        if(collision.CompareTag("Player")||collision.CompareTag("Plant"))
        {
            Player pl = collision.GetComponent<Player>();
            if (pl != null)
            {
                canAss= true;
                if(this.curStatus==Unit_Status.dead)
                    return;
                pl.BeAttack(this.attack);
                this.blastSpeed = this.maxBlastSpeed;
                this.blastdir = (this.transform.position - pl.transform.position).normalized;
            }

            Acc acc = collision.GetComponent<Acc>();
            if (acc != null)
            {
                if (this.curStatus == Unit_Status.dead)
                    return;
                acc.BeAttack(this.attack);
                this.blastSpeed = this.maxBlastSpeed;
                this.blastdir = (this.transform.position - acc.transform.position).normalized;
            }
        }
    }

    public virtual void OnTriggerExit2D(Collider2D collision)//离开检测
    {
        if (collision.CompareTag("Player"))
        {
            if (this.canAss)
            {
                this.canAss = false;
            }
        }
    }

    public override void BeAttack(float attack)//被攻击后进行判定
    {
        this.CurHP -= attack;
        if (this.CurHP<=0)
            {
                this.StopCor();

                this.curStatus=Unit_Status.dead;
                this.StartDeadTime();
            }
    }

    IDisposable dT;
    void StartDeadTime()//死亡计时
    {
        if (this.dT != null)
        {
            return;
        }
        this.sp.sprite = Game.Instance.spriteManager. characterSpriteDic[Tool.KIndToChaTy(this.plantKind)][characterStatus.Enemy];
        this.sp.color = Color.gray;
        this.dT = Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                if(this.deadTime<=0)
                {
                    this.sp.color = Color.white;
                    deadSubject.OnNext(this.id);
                    deadSubject.OnCompleted();
                    this.dT.Dispose();
                }
                if (this.canAss)
                {
                    if (Input.GetButtonDown("Ass"))
                    {
                        this.sp.color = Color.white;
                        assSubject.OnNext(this.id);
                        assSubject.OnCompleted();
                        this.dT.Dispose();
                        return;
                    }
                }
                if(this.deadTime>=0) 
                {
                    this.deadTime -= Time.deltaTime;
                }
            })
            .AddTo(this);
    }


}
