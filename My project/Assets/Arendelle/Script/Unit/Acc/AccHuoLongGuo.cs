using System;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using UniRx;

public class AccHuoLongGuo:Acc
{
    public Queue<Vector2> mousePositionQueue = new Queue<Vector2>();

    float dotInter = 0;
    public override void Update()
    {
        if (this.curStatus == Unit_Status.idle)//常态下的移动方式
        {
            if ((this.transform.position - PlantManager.Instance.player.transform.position).magnitude >= 2f)
            {
                this.rb.velocity = curSpeed * (PlantManager.Instance.player.transform.position - this.transform.position).normalized;
            }
            else if ((this.transform.position - PlantManager.Instance.player.transform.position).magnitude <= 1.5f)
            {
                this.rb.velocity = -1 * curSpeed * (PlantManager.Instance.player.transform.position - this.transform.position).normalized;
            }
            else
            {
                this.rb.velocity = PlantManager.Instance.player.rb.velocity;
                if (this.isback)
                {
                    this.isback = false;
                }
            }
        }

        if (!this.isback && this.sp.IsTorch(Camera.main.ScreenToWorldPoint(Input.mousePosition)) && Input.GetButtonDown("Acc"))
        {
            this.curStatus = Unit_Status.attack;
            aDP = StartCoroutine(AddDotPostion());
        }
        else if (this.curStatus == Unit_Status.attack && Input.GetButtonUp("Acc"))
        {
            if (this.aDP != null)
            {
                StopCoroutine(this.aDP);
                aDP = null;
            }
        }

        if (this.curStatus == Unit_Status.attack)
        {
            PositiveMove();
        }
    }


    Coroutine aDP;//主动移动
    IEnumerator AddDotPostion()
    {
        while (true)
        {
            this.mousePositionQueue.Enqueue(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            yield return new WaitForSeconds(0.02f);
        }
    }

    bool isMove = false;//是否处于移动状态
    bool isback = false;//是否处于返回状态返回
    void PositiveMove()
    {
        if (this.mousePositionQueue.Count > 0)
        {
            if (!isMove)
            {
                isMove = true;
                Vector2 pos = mousePositionQueue.Dequeue();
                Tweener tw = transform.DOMove(pos, this.normSpeed).SetSpeedBased();
                tw.OnComplete(() =>
                {
                    isMove = false;
                });
            }
        }
        else
        {
            if (this.aDP == null)
            {
                this.curStatus = Unit_Status.idle;
                this.isback = true;
            }
        }
    }

    public override void OnTriggerEnter2D(Collider2D collision)//进入检测
    {
        if (collision.CompareTag("Plant"))
        {
            Enemy en = collision.GetComponent<Enemy>();
            if (en != null)
            {
                if (en.curStatus == Unit_Status.dead)
                    return;

                SoundEffectManager.Instance.AccAttack();
                if (this.curStatus == Unit_Status.attack)
                {
                    this.mousePositionQueue.Clear();
                    en.BeAttack(this.attack);
                    if (this.aDP != null)
                    {
                        StopCoroutine(this.aDP);
                        aDP = null;
                    }

                }
                else if (this.curStatus == Unit_Status.idle)
                {
                    en.BeAttack(this.Idleattack);
                }
            }
        }
    }

    public override void StopCor()
    {
        base.StopCor();

        if (this.aDP != null)
        {
            StopCoroutine(this.aDP);
            aDP = null;
        }
    }

    public override void BeAttack(float attack)
    {
        if (this.curStatus != Unit_Status.attack)
        {
            base.BeAttack(attack);
        }

        if (this.CurHP <= 0)
        {
            this.StopCor();
            this.deadSubject.OnNext(this.id);
        }
    }
}

