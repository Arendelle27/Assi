using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;


public class Player : Unit
{
    private void Start()
    {
        this.Init();
    }

    private void Update()
    {
        if(Input.GetAxis("Horizontal")!=0|| Input.GetAxis("Vertical")!=0)
        {
            this.direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
            this.rb.velocity = this.direction * this.curSpeed;
        }
        else
        {
            this.rb.velocity = Vector2.zero;
            curPos= this.transform.position;
        }
    }

    Vector2 curPos;

    private void LateUpdate()
    {
        if(Input.GetAxis("Horizontal")== 0 && Input.GetAxis("Vertical") == 0)
        {
            if(this.curPos != (Vector2)this.transform.position)
            {
                this.transform.position = curPos;
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)//攻击检测
    {
        if (collision.CompareTag("Plant"))
        {
            Enemy en = collision.GetComponent<Enemy>();
            if (en != null)
            {
                if (en.curStatus == Unit_Status.dead)
                    return;

                SoundEffectManager.Instance.PlayerAttack();
                en.BeAttack(this.attack);
            }
        }
    }

    public override void BeAttack(float attack)
    {
        base.BeAttack(attack);
        if(this.CurHP<=0)
        {
            this.StopCor();
            Game.Instance.End();
        }
    }

    public override void Init()
    {
        this.GetDataAcc(PlantKind.Player);//获取数据
        base.Init();
        this.rb.velocity = Vector2.zero;
        this.transform.position = Vector2.zero;
    }
}
