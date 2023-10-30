using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{

    public CellKind cellKind;//ֲ������

    float curHP;//��ǰ������ֵ
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
    public float maxHP;//�������ֵ
    public bool isinjured=false;//�Ƿ�����


    public float Idleattack;//�������������˹������ˣ�û�о�Ϊ0
    public float attack;//���������Ĺ�����,û�о�Ϊ0

    public float speed = 1.0f;
    public float normSpeed = 4.0f;

    public float idleTime=2f;//����idle״̬ʱ�䣬�ۼ�n��ת��Ϊrest״̬��ʼ����ֵ�ظ�.
    public float curIdleTime = 0f;
    public float hpRecover = 1f;//ÿ��ظ�������ֵ


    public Rigidbody2D rb;

    public SpriteRenderer spriteRenderer;

    public Collider2D co;

    public Effect effect;

        public enum State
    {
        Idle,//��̬
        Decay,//�ж�
        Slow,//����
        Both,
        Dead,
    }
    public State abnormalstate;//�쳣״̬��

    public enum MoveState
    {
        Idle,//������״̬
        Attack,//������������״̬
        rest,//�ظ�����״̬
        Dead,//����״̬
    }
    public MoveState teamState;//״̬��

    public float decaytime = 3f;//�ܵ��ж�Ч����ʱ�䣬��һ��Ϊ3f
    float curDecaytime = 0f;

    public float slowtime = 3f;//�ܵ�����Ч����ʱ�䣬��һ��Ϊ3f
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
