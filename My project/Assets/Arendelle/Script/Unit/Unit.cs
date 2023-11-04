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
    #region ���
    public Rigidbody2D rb;

    public SpriteRenderer sp;

    public Collider2D co;

    #endregion

    #region ����
    [SerializeField, LabelText("ID"), Tooltip("�õ�λ��id")]
    public int id;

    [SerializeField, LabelText("ֲ������"), Tooltip("�õ�λ������")]
    public PlantKind plantKind;//ֲ������

    [SerializeField, LabelText("����ֵ����"), Tooltip("�õ�λ�������ޣ���������ֵ+�ȼ���������ֵ")]
    public float maxHP;//�������ֵ

    [SerializeField, LabelText("������"), Tooltip("�õ�λ�����Ĺ�����")]
    public float attack;//���������Ĺ�����

    [SerializeField, LabelText("�ƶ��ٶ�"), Tooltip("�õ�λ��������µ��ƶ��ٶ�")]
    public float normSpeed = 4.0f;

    [SerializeField, LabelText("��ʼ��Ѫ��ʱ"), Tooltip("��ս��ʼ��Ѫ��ʱ�䣬ֻ��������Һ����")]
    public float idleTime = 2f;//��ս��ʼ��Ѫ��ʱ��

    [SerializeField, LabelText("ÿ��ظ�����ֵ"), Tooltip("��Ѫʱ��ÿ���лظ�����ֵ��")]
    public float hpRecover = 1f;//ÿ��ظ�������ֵ


    [SerializeField, LabelText("����ʱ��"), Tooltip("�ܵ�����Ч���ĳ���ʱ��")]
    public float slowtime = 3f;//�ܵ�����Ч����ʱ�䣬��һ��Ϊ3f
    #endregion

    public enum Unit_Status
    {
        idle,
        attack,
        dead,
        none
    }

    #region ��ǰ״̬
    public Unit_Status curStatus; //��ǰ״̬

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
    }//��ǰ������ֵ

    public float curSpeed = 0f;//��ǰ���ƶ��ٶ�

    public Vector2 direction=new Vector2(0,0);//�ƶ�����
    #endregion

    #region �¼�
    public Subject<int> deadSubject = new Subject<int>();
    public IObservable<int> OnDead => deadSubject;

    #endregion

    public virtual void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.sp = this.GetComponent<SpriteRenderer>();
        this.co = this.GetComponent<Collider2D>();

    }

    public virtual void Init()//��ʼ��
    {
        this.CurHP = this.maxHP;
        this.curSpeed = this.normSpeed;
        this.direction=new Vector2(0,0);
    }

    public void GetDataAcc(PlantKind kind)//��Ӽ���һ�ȡ���ֳ�ʼ����
    {
        this.sp.sprite = Game.Instance.spriteManager.characterSpriteDic[Tool.KIndToChaTy(kind)][characterStatus.Normal];
        this.maxHP =Game.Instance.dataManager.dataAcc[kind][DataType.Hp];
        this.normSpeed = Game.Instance.dataManager.dataAcc[kind][DataType.Speed];
        this.attack = Game.Instance.dataManager.dataAcc[kind][DataType.Attack];
    }

    public void GetDataEn(PlantKind kind)//���˻�ȡ���ֳ�ʼ����
    {
        this.sp.sprite = Game.Instance.spriteManager.characterSpriteDic[Tool.KIndToChaTy(kind)][characterStatus.Enemy];
        this.maxHP = Game.Instance.dataManager.dataEn[kind][DataType.Hp];
        this.normSpeed = Game.Instance.dataManager.dataEn[kind][DataType.Speed];
        this.attack = Game.Instance.dataManager.dataEn[kind][DataType.Attack];
    }

    public void StartSlow()//��ʼ����
    {
        if (sl != null)
        {
            StopCoroutine(sl);
            sl = null;
        }
        sl = StartCoroutine(Slow());
    }

    public Coroutine sl;//����Э��
    IEnumerator Slow()
    {
        this.curSpeed = this.normSpeed / 2;
        yield return new WaitForSeconds(slowtime);
        this.curSpeed  = this.normSpeed;

        StopCoroutine(sl);
        sl = null;
    }

    public virtual void StopCor()//ֹͣЭ��
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


    public virtual void BeAttack(float attack)//������
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

    public Coroutine wR;//�ȴ���ѪЭ��

    public IEnumerator WaitRecover()//�ȴ���Ѫ
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
