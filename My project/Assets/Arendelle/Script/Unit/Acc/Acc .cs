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

    #region ����
    [SerializeField, LabelText("�����������Ĺ���"), Tooltip("��ӱ����˵Ĺ�����")]
    public float Idleattack;//�������������˹������ˣ�û�о�Ϊ0

    #endregion


    public override void Init()
    {
        this.GetDataAcc(this.plantKind);

        this.co.isTrigger = false;
        this.gameObject.layer = LayerMask.NameToLayer("Acc");
        this.curStatus = Unit_Status.idle;//��ʼ��Ϊ����״̬
        base.Init();
    }

    public virtual void Update()
    {
        if (this.curStatus == Unit_Status.idle)//��̬�µ��ƶ���ʽ
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

    public virtual void OnTriggerEnter2D(Collider2D collision)//������
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
