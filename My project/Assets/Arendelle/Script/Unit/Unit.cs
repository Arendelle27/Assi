using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{
    #region 组件
    public Rigidbody2D rb;

    public SpriteRenderer sp;

    public Collider2D co;

    #endregion

    #region 属性
    [SerializeField, LabelText("ID"), Tooltip("该单位的id")]
    public int id;

    [SerializeField, LabelText("植物种类"), Tooltip("该单位的种类")]
    public PlantKind plantKind;//植物种类

    [SerializeField, LabelText("生命值上限"), Tooltip("该单位生命上限，基础生命值+等级提升生命值")]
    public float maxHP;//最大生命值

    [SerializeField, LabelText("攻击力"), Tooltip("该单位攻击的攻击力")]
    public float attack;//操作攻击的攻击力

    [SerializeField, LabelText("移动速度"), Tooltip("该单位正常情况下的移动速度")]
    public float normSpeed = 4.0f;

    [SerializeField, LabelText("开始回血用时"), Tooltip("脱战后开始回血的时间，只适用与玩家和随从")]
    public float idleTime = 2f;//脱战后开始回血的时间

    [SerializeField, LabelText("每秒回复生命值"), Tooltip("回血时，每秒中回复生命值数")]
    public float hpRecover = 1f;//每秒回复的生命值


    [SerializeField, LabelText("减速时间"), Tooltip("受到减速效果的持续时间")]
    public float slowtime = 3f;//受到减速效果的时间，不一定为3f
    #endregion

    public enum Unit_Status
    {
        idle,
        attack,
        dead,
        none
    }

    #region 当前状态
    public Unit_Status curStatus; //当前状态

    public float curHP=0f;
    public float CurHP
    {
        get
        {
            return curHP;
        }
        set
        {
            if (curHP > value)
            {
                if(this.gameObject.activeSelf)
                {
                    In = StartCoroutine(Injured());
                }
            }
            if (value > maxHP)
            {
                value = maxHP;
            }
            curHP = value;
        }
    }//当前的生命值

    public float curSpeed = 0f;//当前的移动速度

    public Vector2 direction=new Vector2(0,0);//移动方向
    #endregion

    #region 事件
    public Subject<int> deadSubject = new Subject<int>();
    public IObservable<int> OnDead => deadSubject;

    #endregion

    public virtual void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.sp = this.GetComponent<SpriteRenderer>();
        this.co = this.GetComponent<Collider2D>();

    }

    public virtual void Init()//初始化
    {
        this.CurHP = this.maxHP;
        this.curSpeed = this.normSpeed;
        this.direction=new Vector2(0,0);
    }

    public void GetDataAcc(PlantKind kind)//随从及玩家获取三种初始属性
    {
        this.sp.sprite = Game.Instance.spriteManager.characterSpriteDic[Tool.KIndToChaTy(kind)][characterStatus.Normal];
        this.maxHP =Game.Instance.dataManager.dataAcc[kind][DataType.Hp];
        this.normSpeed = Game.Instance.dataManager.dataAcc[kind][DataType.Speed];
        this.attack = Game.Instance.dataManager.dataAcc[kind][DataType.Attack];
    }

    public void GetDataEn(PlantKind kind)//敌人获取三种初始属性
    {
        this.sp.sprite = Game.Instance.spriteManager.characterSpriteDic[Tool.KIndToChaTy(kind)][characterStatus.Enemy];
        this.maxHP = Game.Instance.dataManager.dataEn[kind][DataType.Hp];
        this.normSpeed = Game.Instance.dataManager.dataEn[kind][DataType.Speed];
        this.attack = Game.Instance.dataManager.dataEn[kind][DataType.Attack];
    }

    public void StartSlow()//开始减速
    {
        if (sl != null)
        {
            StopCoroutine(sl);
            sl = null;
        }
        sl = StartCoroutine(Slow());
    }

    public Coroutine sl;//减速协程
    IEnumerator Slow()
    {
        this.curSpeed = this.normSpeed / 2;
        yield return new WaitForSeconds(slowtime);
        this.curSpeed  = this.normSpeed;

        StopCoroutine(sl);
        sl = null;
    }

    public virtual void StopCor()//停止协程
    {
        StopAllCoroutines();
        if (sl != null)
        {
            sl = null;
        }
        if (In != null)
        {
            In = null;
        }
        if (wR != null)
        {
            wR = null;
        }
    }


    public virtual void BeAttack(float attack)//被攻击
    {
        this.CurHP -= attack;

        if(wR!=null)
        {
            StopCoroutine(wR);
            wR = null;
        }
        if(this.gameObject.activeSelf)
        {
            wR = StartCoroutine(WaitRecover());
        }
    }

    public Coroutine wR;//等待回血协程

    public IEnumerator WaitRecover()//等待回血
    {
        yield return new WaitForSeconds(idleTime);
        while(true)
        {
            if (this.CurHP < this.maxHP)
            {
                this.CurHP += this.hpRecover/5;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }



    public Coroutine In;
    public IEnumerator Injured()
    {
        Sprite sp = this.sp.sprite;
        this.sp.sprite = Game.Instance.spriteManager.characterSpriteDic[Tool.KIndToChaTy(this.plantKind)][characterStatus.Injured];
        yield return new WaitForSeconds(0.3f);
        this.sp.sprite = sp;
        if(In != null)
        {
            StopCoroutine(In);
            In = null;
        }
    }
}
