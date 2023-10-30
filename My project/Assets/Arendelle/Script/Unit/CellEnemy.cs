using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public enum MoveType
{
    RaMove,
    AimMove,
    none
}

public class CellEnemy : Unit
{
    public MoveType moveType = MoveType.none;


    public float raMoveTimer = 2.0f;
    public float aimMoveTimer = 5.0f;

    public float curMoveTimer = 0f;

    bool startRaMove = false;
    bool startAimMove = false;

    public int id;

    public UnityAction<int> beAss;

    public float maxBlastSpeed = 10f;
    float blastSpeed = 0f;
    Vector2 blastdir = new Vector2(0,0);

    public float deadTime=3f;
    float curDeadTime = 0f;

    public bool canAss = false;
    void Start()
    {
        Observable
.EveryUpdate()
.Where(_ => !this.isinjured && this.spriteRenderer.sprite != Game.Instance.spriteManager.characterSpriteDic[Tool.CeKIndToChaTy(this.cellKind)][characterStatus.Enemy])
.Subscribe(_ =>
{
    this.spriteRenderer.sprite = Game.Instance.spriteManager.characterSpriteDic[Tool.CeKIndToChaTy(this.cellKind)][characterStatus.Enemy];
})
.AddTo(this);//当游戏对象销毁时，自动取消订阅


this.co.isTrigger = true;
        //this.spriteRenderer.color = Color.red;

        this.teamState = MoveState.Attack;
        startRaMove = true;

        //特殊植物:嘴唇花
        if (this.cellKind == CellKind.ZuiChunHua)
        {
            Observable
           .EveryUpdate()
           .Subscribe(_ =>
           {
               if (!this.enabled || this.teamState != MoveState.Attack)
                   return;
               if ((Game.Instance.CellMain.transform.position - this.transform.position).magnitude <= 3f)
               {
                   Game.Instance.CellMain.Slow();
               }
           })
           .AddTo(this);//当游戏对象销毁时，自动取消订阅
        }
    }

    public override void Update()
    {
        if (this.CurHP <= 0&&this.teamState==MoveState.Attack)
        {
            if (this.onDead != null)
                this.onDead(this.id);
            this.teamState = MoveState.Dead;
            this.spriteRenderer.color = Color.gray;
            curDeadTime = deadTime;
        }

        if(this.teamState==MoveState.Dead)
        {
            if (Input.GetButtonDown("Ass")&&this.canAss)
            {
                Game.Instance.CellMain.CurHP += 1;
                this.BeAssimilate();
                return;
            }
        }

        if(this.teamState==MoveState.Dead&&this.curDeadTime>0)
        {
            if(this.rb.velocity.magnitude!=0)
                this.rb.velocity=new Vector2(0,0);
            this.curDeadTime-= Time.deltaTime;
            if (this.curDeadTime<=0)
            {
                StartCoroutine(CellAccDeath());
            }
            return;
        }
        base.Update();

        if(!this.isinjured&&this.spriteRenderer.sprite!= Game.Instance.spriteManager.characterSpriteDic[Tool.CeKIndToChaTy(this.cellKind)][characterStatus.Enemy])
        {
            this.spriteRenderer.sprite = Game.Instance.spriteManager.characterSpriteDic[Tool.CeKIndToChaTy(this.cellKind)][characterStatus.Enemy];
        }

        if(this.blastSpeed>0)
        {
            blastSpeed -=3* blastSpeed*Time.deltaTime;
        }

        if(startRaMove)
        {
            this.moveType = MoveType.RaMove;
            startRaMove = false;
            StartCoroutine(RaDirection());

            Observable.Timer(System.TimeSpan.FromSeconds(raMoveTimer))
            .Subscribe(_ => {
                startAimMove = true;
            })
            .AddTo(this);
        }
        else if (startAimMove)
        {
            this.moveType = MoveType.AimMove;
            startAimMove = false;
            StopCoroutine(RaDirection());

            Observable.Timer(System.TimeSpan.FromSeconds(aimMoveTimer))
            .Subscribe(_ => {
                startRaMove = true;
            })
            .AddTo(this);
        }

        if (this.moveType == MoveType.AimMove)
        {
            this.rb.velocity =this.speed * (Vector2)(Game.Instance.CellMain.transform.position - this.transform.position).normalized+(blastSpeed * blastdir);
        }


    }

    IEnumerator CellAccDeath()
    {
        this.gameObject.SetActive(false);
        Game.Instance.exampleManager.DropOutExampleBll(this.transform.position);
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

    IEnumerator RaDirection()
    {
        while(this.moveType==MoveType.RaMove)
        {
            this.rb.velocity = this.speed * new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized+ this.blastSpeed * blastdir;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void BeAssimilate()
    {
        if (this.beAss != null)
            this.beAss(this.id);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CellMain cm = collision.GetComponent<CellMain>();
            if (cm != null)
                {
                if (!canAss)
                {
                    this.canAss = true;
                }

                if (this.teamState == MoveState.Dead)
                {
                    return;
                }
                if (this.teamState==MoveState.Attack)
                {
                    this.canAss = true;
                }
                {
                    cm.CurHP -= this.attack;
                    this.CurHP -= cm.attack;

                    this.blastdir = (this.transform.position - cm.transform.position).normalized;
                    this.blastSpeed = this.maxBlastSpeed;
                }
                }
        }
        //与角色碰撞对其造成伤害
        else if(collision.CompareTag("CellAcc"))
        {
            CellAcc ca = collision.GetComponent<CellAcc>();
            if (ca != null&&ca.enabled)
            {
            if (ca.CurHP < 0)
                    return;

                this.effect.Attack(this, ca);
                if (ca.cellKind==CellKind.ZuiChunHua)
                {
                    this.CurHP -= ca.Idleattack;
                }
                else if(ca.cellKind==CellKind.DuMogu)
                {
                    this.Decay();
                }
                else
                {
                    if (ca.isMove)
                    {
                        this.CurHP -= ca.attack;
                        ca.isMove=false;
                    }
                    else
                    {
                        this.CurHP -= ca.Idleattack;
                    }
                }

                this.blastdir = (this.transform.position - ca.transform.position).normalized;
                this.blastSpeed = this.maxBlastSpeed;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CellMain cm = collision.GetComponent<CellMain>();
            if (cm != null)
            {
                if (this.canAss)
                {
                    this.canAss = false;
                }
            }
        }
    }

}
