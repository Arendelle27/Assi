using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{

    public CellKind cellKind;//植物种类

    float curHP;//当前的生命值
    public float CurHP
    {
        get
        {
            return curHP;
        }
        set
        {
            if(curHP>value)
            {
                StartCoroutine(BeAttack());
                curIdleTime = 0f;
            }
            if(value>maxHP)
            {
                value = maxHP;
            }
            curHP = value;
        }
    }
    public float maxHP;//最大生命值
    public bool isinjured=false;//是否受伤


    public float Idleattack;//不主动操作敌人攻击反伤，没有就为0
    public float attack;//操作攻击的攻击力,没有就为0

    public float speed = 1.0f;
    public float normSpeed = 4.0f;

    public float idleTime=2f;//处于idle状态时间，累计n秒转换为rest状态开始生命值回复.
    public float curIdleTime = 0f;
    public float hpRecover = 1f;//每秒回复的生命值


    public Rigidbody2D rb;

    public SpriteRenderer spriteRenderer;

    public Collider2D co;

    public Effect effect;

        public enum State
    {
        Idle,//常态
        Decay,//中毒
        Slow,//减速
        Both,
        Dead,
    }
    public State abnormalstate;//异常状态机

    public enum MoveState
    {
        Idle,//不操作状态
        Attack,//操作主动攻击状态
        rest,//回复生命状态
        Dead,//死亡状态
    }
    public MoveState teamState;//状态机

    public float decaytime = 3f;//受到中毒效果的时间，不一定为3f
    float curDecaytime = 0f;

    public float slowtime = 3f;//受到减速效果的时间，不一定为3f
    public float curSlowtime = 0f;

    public UnityAction<int> onDead;

    public virtual void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        this.co = this.GetComponent<Collider2D>();

    }

    public virtual void Init()
    {
        this.CurHP = this.maxHP;
        this.speed = this.normSpeed;
        this.abnormalstate = State.Idle;
    }

    public  void Slow()
    {
        if(this.abnormalstate==State.Idle)
        {
            this.abnormalstate = State.Slow;
            speed = normSpeed / 2;
        }
        else if(this.abnormalstate==State.Decay)
        {
            this.abnormalstate = State.Both;
            speed = normSpeed / 2;
        }
        this.curSlowtime =slowtime;

    }

    public void Decay()
    {
        if (this.abnormalstate == State.Idle)
        {
            this.abnormalstate = State.Decay;
        }
        else if (this.abnormalstate == State.Slow)
        {
            this.abnormalstate = State.Both;
        }
        this.curDecaytime = decaytime;

    }

    public virtual void Update()
    {
        if (this.abnormalstate == State.Idle)
            return;

        if(abnormalstate == State.Slow && this.enabled)
        {
            this.curSlowtime -= Time.deltaTime;
            if(curSlowtime<=0)
            {
                abnormalstate = State.Idle;
                speed = normSpeed;
            }
        }
        else if(abnormalstate == State.Decay && this.enabled)
        {
            this.curDecaytime -= Time.deltaTime;
            this.CurHP -= 2 * Time.deltaTime;

            if (curDecaytime<=0)
            {
                abnormalstate = State.Idle;
            }
        }
        else if(abnormalstate == State.Both && this.enabled)
        {
            this.curSlowtime -= Time.deltaTime;
            this.curDecaytime -= Time.deltaTime;
            this.CurHP -= 2 * Time.deltaTime;
            if (curDecaytime <= 0)
            {
                abnormalstate = State.Slow;
            }
            if (curSlowtime <= 0)
            {
                abnormalstate = State.Decay;
                speed = normSpeed;
            }
        }


    }

    IEnumerator BeAttack()
    {
        this.isinjured = true;
        Sprite sp = this.spriteRenderer.sprite;
        this.spriteRenderer.sprite = Game.Instance.spriteManager.characterSpriteDic[Tool.CeKIndToChaTy(this.cellKind)][characterStatus.Injured];
        yield return new WaitForSeconds(0.5f);
        this.isinjured = false;
        this.spriteRenderer.sprite = sp;
    }
}
