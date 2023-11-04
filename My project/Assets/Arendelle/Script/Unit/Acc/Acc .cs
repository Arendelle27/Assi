using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Acc : Unit
{

    #region 属性
    [SerializeField, LabelText("非主动攻击的攻击"), Tooltip("随从被打反伤的攻击立")]
    public float Idleattack;//不主动操作敌人攻击反伤，没有就为0

    #endregion


    public override void Init()
    {
        this.GetDataAcc(this.plantKind);

        this.co.isTrigger = false;
        this.gameObject.layer = LayerMask.NameToLayer("Acc");
        this.curStatus = Unit_Status.idle;//初始化为闲置状态
        base.Init();
    }

    public virtual void Update()
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
            }
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)//进入检测
    {
        if (collision.CompareTag("Plant"))
        {
            Enemy en = collision.GetComponent<Enemy>();
            if (en != null)
            {
                if(en.curStatus==Unit_Status.dead)
                {
                    return;
                }

                SoundEffectManager.Instance.AccAttack();
                if(this.curStatus==Unit_Status.attack)
                {
                    en.BeAttack(this.attack);
                }
                else if(this.curStatus==Unit_Status.idle)
                {
                    en.BeAttack(this.Idleattack);
                }
            }
        }
    }

   
}
