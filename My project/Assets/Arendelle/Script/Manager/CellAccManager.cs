using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CellAccManager : MonoSingleton<CellAccManager>
{
    public GameObject cellAccPrefab;

    public Dictionary<int,CellAcc> cellAccList=new Dictionary<int, CellAcc>();
    public Queue<int> cellAccIds=new Queue<int>();
    public int maxAccNumber = 5;

    public Dictionary<int,CellEnemy> cellEnemyList=new Dictionary<int, CellEnemy>();

    public float curTime=0;
    public float maxTime = 10f;

    public int maxMonsterNumber = 4;
    public int curmonsternumber = 0;

    public int wave = 0;

    public int enemyId=0;

    public float addPlHP=1f;

    public float addPlAttack=1f;

    public int addAccNumber=1;
    public void Awake()
    {
        
    }

    void Start()
    {
        enemyId = 0;
        wave= 0;
    }

    void Update()
    {
        if (Game.Instance.Status != GameState.InGame)
            return;
        curTime -= Time.deltaTime;
        if (curTime <= 0f)
        {
            wave++;
            if (wave % 3 == 0&&wave!=0)
            {
                maxMonsterNumber += 2;
            }
            curTime = maxTime;
            curmonsternumber = maxMonsterNumber;

            for (int i = 0; i < curmonsternumber; i++)
            {
                float x = UnityEngine.Random.Range(-12, 12);
                if(x<10&&x>-10)
                {
                    if(x>0)
                        x=UnityEngine.Random.Range(10, 12);
                    else
                        x=UnityEngine.Random.Range(-12, -10);
                }
                float y = UnityEngine.Random.Range(-8, 8);
                if (y < 6 && y > -6)
                {
                    if (y > 0)
                        y = UnityEngine.Random.Range(6, 8);
                    else
                        y = UnityEngine.Random.Range(-8, -6);
                }

                
                GenerateCellAcc(new Vector2(x, y),Game.Instance.effectManager.RaKind());
            }
        }
    }


    public void GenerateCellAcc(Vector2 position,CellKind kind)
    {
        GameObject cellAcc = Instantiate(cellAccPrefab, position, Quaternion.identity);
        CellAcc ca = cellAcc.GetComponent<CellAcc>();
        if(ca!=null)
        {
            ca.cellKind= kind;
            ca.effect= Game.Instance.effectManager.effectDic[kind];
            ca.effect.type = CellAccType.Acc;
            
            ca.maxHP = Game.Instance.effectManager.dataAcc[kind][DataType.Hp];
            ca.attack = Game.Instance.effectManager.dataAcc[kind][DataType.Attack];
            ca.normSpeed = Game.Instance.effectManager.dataAcc[kind][DataType.Speed];

            ca.gameObject.layer= LayerMask.NameToLayer("Enemy");
            ca.onDead+=RemoveCell;
            ca.Init();

            
            ca.enabled = false;
        }
        CellEnemy ce = cellAcc.GetComponent<CellEnemy>();
        if (ce != null)
        {
            ce.spriteRenderer.sprite = Game.Instance.spriteManager.characterSpriteDic[Tool.CeKIndToChaTy(kind)][characterStatus.Enemy];
            ce.id = enemyId;
            ce.cellKind = kind;
            ce.effect= Game.Instance.effectManager.effectDic[kind];
            ce.effect.type = CellAccType.Enemy;

            ce.maxHP = Game.Instance.effectManager.dataEn[kind][DataType.Hp];
            ce.attack = Game.Instance.effectManager.dataEn[kind][DataType.Attack];
            ce.normSpeed = Game.Instance.effectManager.dataEn[kind][DataType.Speed];

            ce.Init();

            ce.beAss += Assiminate;
            cellEnemyList.Add(ce.id,ce);
            enemyId++;


            ce.enabled = true;
        }
    }

    void Assiminate(int id)//×ª»»
    {

        if (!cellEnemyList.ContainsKey(id))
            return;
        cellAccList.Add(id, cellEnemyList[id].GetComponent<CellAcc>());

        cellAccIds.Enqueue(id);
        if(cellAccIds.Count>maxAccNumber)
        {
            int id1 = cellAccIds.Dequeue();
            if (cellAccList.ContainsKey(id1)&&cellAccList[id1].isMove && cellAccList[id1]!=null)
            {
                cellAccIds.Enqueue(id1);
                id1 = cellAccIds.Dequeue();
            }
            if(cellAccList.ContainsKey(id1))
            Destroy(cellAccList[id1].gameObject);
            cellAccList.Remove(id1);
        }

        cellAccList[id].enabled = true;
        cellEnemyList[id].enabled = false;

        cellAccList[id].spriteRenderer.sprite = Game.Instance.spriteManager.characterSpriteDic[Tool.CeKIndToChaTy(cellAccList[id].cellKind)][characterStatus.Normal];
        cellAccList[id].spriteRenderer.color = Color.white;
        cellAccList[id].co.isTrigger = false;
        cellAccList[id].gameObject.layer = LayerMask.NameToLayer("Acc");

        cellEnemyList.Remove(id);
    }

    void RemoveCell(int id)
    {
        if(cellAccList.ContainsKey(id))
        {
            cellAccList.Remove(id);
        }
        else if(cellEnemyList.ContainsKey(id))
        {
            cellEnemyList.Remove(id);
        }
    }

    public void AttackUP()
    {
        Game.Instance.CellMain.attack += addPlAttack;

    }

    public void HpUP()
    {
        Game.Instance.CellMain.maxHP += addPlHP;
        Game.Instance.CellMain.CurHP += addPlHP;
        UIManager.Instance.hp.maxValue = Game.Instance.CellMain.maxHP;

    }

    public void AccUp()
    {
        this.maxAccNumber+=addAccNumber;
    }

    public void Init()
    {
        this.curTime = 0;
        this.curmonsternumber=0;
        this.maxAccNumber = 4;
        this.wave = 0;
        this.enemyId = 0;

        foreach(var cellAcc in this.cellAccList)
        {
            Destroy(cellAcc.Value.gameObject);
        }
        foreach (var cellEnemy in this.cellEnemyList)
        {
                Destroy(cellEnemy.Value.gameObject);
        }
        this.cellAccList.Clear();
        this.cellAccIds.Clear();
        this.cellEnemyList.Clear();
    }
}
