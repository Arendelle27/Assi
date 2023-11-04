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
    #region ����
    [SerializeField, LabelText("����ƶ�ʱ��"), Tooltip("�õ�λ�ƶ������У�����ƶ�״̬��ʱ��")]
    public float raMoveTimer = 2.0f;

    [SerializeField, LabelText("Ŀ���ƶ�ʱ��"), Tooltip("�õ�λ�ƶ������У���Ŀ���ƶ�״̬��ʱ��")]
    public float aimMoveTimer = 4.0f;


    [SerializeField, LabelText("�������ٶ�"), Tooltip("������˲����ٶ�")]
    public float maxBlastSpeed = 15f;

    [SerializeField, LabelText("��������ʱ��"), Tooltip("����ֵ��պ�ʼ��ʱ")]
    public float deadTime = 2f;
    #endregion

    [SerializeField, LabelText("�ж�ʱ��"), Tooltip("�ܵ��ж�Ч���ĳ���ʱ��")]
    public float decaytime = 3f;//�ܵ��ж�Ч����ʱ�䣬��һ��Ϊ3f
    #region ״̬
    public float blastSpeed = 0f;//������ʱ�ٶ�

    public Vector2 blastdir= new Vector2(0,0);//������ʱ����

    public bool canAss = false;//�Ƿ�Ӵ�����ң�����������ʱ�����ж��Ƿ���Ա�����
    #endregion


    #region �¼�
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

    Coroutine move;//�ƶ�Э��
    IEnumerator Move()//�ƶ���ʽ
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

    public void StartDecay()//��ʼ�ж�
    {
        if (de != null)
        {
            StopCoroutine(de);
            de = null;
        }
        de = StartCoroutine(Decay());
    }

    public Coroutine de;//����Э��
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

    private void Update()//�ƶ�
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

    public virtual void OnTriggerEnter2D(Collider2D collision)//������
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

    public virtual void OnTriggerExit2D(Collider2D collision)//�뿪���
    {
        if (collision.CompareTag("Player"))
        {
            if (this.canAss)
            {
                this.canAss = false;
            }
        }
    }

    public override void BeAttack(float attack)//������������ж�
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
    void StartDeadTime()//������ʱ
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
