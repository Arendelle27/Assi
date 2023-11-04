using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UniRx;



public class PlantManager : MonoSingleton<PlantManager>
{
    #region 属性
    public float maxTime = 10f;//每波间隔时间

    public int maxAccNumber = 8;//最大随从数量

    public float addHP = 1f;//选择升级生命值，玩家血量增加数

    public float addAttack = 1f;//选择升级攻击力，玩家攻击力增加数

    public int addAccNum = 1;//选择升级随从数量，随从数量增加数

    public float recovHp = 1f;//吸收敌人回复血量数

    public int maxMonNum = 4;//初始每波最大敌人数量
    #endregion

    #region 状态
    public Player player;//玩家

    List<GameObject> plants = new List<GameObject>();

    public Dictionary<int, Acc> accList = new Dictionary<int, Acc>();//随从列表
    public Queue<int> accIds = new Queue<int>();//记录随从id，当随从数量超过最大值时，删除最早的随从
    public int curAccNum ;//当前最大随从数量

    public Dictionary<int, Enemy> enemyList = new Dictionary<int, Enemy>();//敌人列表

    public int wave = 0;//波数


    public int curMonNum;//当前每波敌人数量

    public int plantId = 0;
    public int PlantId//植物id
    {
        get
        {
            return plantId;
        }
        set
        {
            plantId = value;
            if (plantId >= 49)
                plantId = 0;
        }
    }

    #endregion

    public void Awake()
    {
        GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Player");//加载玩家预制体
        this.player = Instantiate(playerPrefab).GetComponent<Player>();
        this.player.plantKind = PlantKind.Player;

        GeneratePlant();

        this.Init();
        this.player.gameObject.SetActive(false);
    }

    public void Init()//初始化
    {
        this.player.Init();
        this.curAccNum =4;
        this.wave = 0;
        this.PlantId = 0;
        this.accIds.Clear();

        if (ge != null)
        {
            StopCoroutine(ge);
            ge = null;
        }

        foreach (GameObject p in plants)
        {
            Unit acc = p.GetComponent<Unit>();
            if(acc != null)
            {
                acc.StopCor();
                Destroy(acc);
            }
            p.SetActive(false);
        }

        this.accList.Clear();
        this.enemyList.Clear();
    }


    void GeneratePlant()//游戏一开始生成49个植物预制体备用
    {
        GameObject plantPrefab = Resources.Load<GameObject>("Prefabs/Plant");
        for (int i = 0; i < 49; i++)
        {
            GameObject plant = Instantiate(plantPrefab,this.transform);
            plant.transform.position = new Vector2(0, 0);
            plant.SetActive(false);
            plants.Add(plant);
        }
    }

    public void StartGenEn()//开始刷怪
    {
        ge=StartCoroutine(GenerateEnemys());
    }

    Coroutine ge;//刷怪协程
    IEnumerator GenerateEnemys()//刷怪
    {
        while (true)
        {
            wave++;
            if (wave % 3 == 0 && wave != 0)
            {
                maxMonNum += 2;
            }
            curMonNum = maxMonNum;
            if(this.enemyList.Count>25)
            {
                curMonNum /= 2;
            }
            for (int i = 0; i < curMonNum; i++)
            {
                float x = UnityEngine.Random.Range(-12, 12);
                if (x < 10 && x > -10)
                {
                    if (x > 0)
                        x = UnityEngine.Random.Range(10, 12);
                    else
                        x = UnityEngine.Random.Range(-12, -10);
                }
                float y = UnityEngine.Random.Range(-8, 8);
                if (y < 6 && y > -6)
                {
                    if (y > 0)
                        y = UnityEngine.Random.Range(6, 8);
                    else
                        y = UnityEngine.Random.Range(-8, -6);
                }

                PlantKind kind;
                switch (UnityEngine.Random.Range(0, 4))
                {
                    case 0:
                        kind = PlantKind.TaHuang;
                        break;
                    case 1:
                        kind = PlantKind.DuMogu;
                        break;
                    case 2:
                        kind = PlantKind.ZuiChunHua;
                        break;
                    default:
                        kind = PlantKind.HuoLongGuo;
                        break;
                }
                GenerateEnemy(new Vector2(x, y), kind);
            }
            yield return new WaitForSeconds(maxTime);
        }
    }

    void GenerateEnemy(Vector2 pos,PlantKind kind)//生成单个怪
    {
        if(this.enemyList.ContainsKey(plantId)||this.accList.ContainsKey(plantId))
        {
            ++plantId;
        }
        Enemy en;
        switch (kind)
        {
            case PlantKind.DuMogu:
                en = plants[PlantId].AddComponent<EnDuMogu>();
                break;
            case PlantKind.HuoLongGuo:
                en = plants[PlantId].AddComponent<EnHuoLongGuo>();
                break;
            case PlantKind.TaHuang:
                en = plants[PlantId].AddComponent<EnTaHuang>();
                break;
            case PlantKind.ZuiChunHua:
                en = plants[PlantId].AddComponent<EnZuiChunHua>();
                break;
            default:
                en = plants[PlantId].AddComponent<EnDuMogu>();
                break;
        }
        this.enemyList.Add(PlantId, en);
        en.id = PlantId;
        en.plantKind = kind;

        plants[PlantId].SetActive(true);
        en.Init();

        en.Ass//订阅转换
        .Subscribe(id =>
        {
            this.player.curHP += recovHp;//吸收一次恢复一滴血
            this.Assiminate(id);
        });

        en.OnDead//订阅死亡
        .Subscribe(id =>
        {
            this.OnDead(id);
        });


        plants[PlantId].transform.position = pos;
        ++plantId;
    }

    void Assiminate(int id)//转换
    {
        if(this.accList.Count == curAccNum) //到达上限，删除最早的随从
        {
            int i=this.accIds.Dequeue();
            while(!accList.ContainsKey(i))
            {
                i = this.accIds.Dequeue();
            }

            if (this.accList[i].curStatus==Unit.Unit_Status.attack)
            {
                this.accIds.Enqueue(i);
                i=this.accIds.Dequeue();
                while (!accList.ContainsKey(i))
                {
                    i = this.accIds.Dequeue();
                }
            }
            accList[i].StopCor();
            Destroy(this.accList[i]);
            this.accList.Remove(i);
            this.plants[i].SetActive(false);
        }
        this.accIds.Enqueue(id);

        Acc acc;
        switch (enemyList[id].plantKind)
        {
            case PlantKind.DuMogu:
                acc = plants[id].AddComponent<AccDuMogu>();
                break;
            case PlantKind.HuoLongGuo:
                acc = plants[id].AddComponent<AccHuoLongGuo>();
                break;
            case PlantKind.TaHuang:
                acc = plants[id].AddComponent<AccTaHuang>();
                break;
            case PlantKind.ZuiChunHua:
                acc = plants[id].AddComponent<AccZuiChunHua>();
                break;
            default:
                acc = plants[id].AddComponent<AccDuMogu>();
                break;
        }
        this.accList.Add(id, acc);
        acc.id = id;
        acc.plantKind = enemyList[id].plantKind;

        acc.Init();

        acc.OnDead//订阅死亡
        .Subscribe(id =>
        {
            this.OnDead(id);
        });


        Destroy(this.enemyList[id]);
        this.enemyList.Remove(id);
    }

    void OnDead(int id)//死亡
    {
        if (this.enemyList.ContainsKey(id))
        {
            Destroy(this.enemyList[id]);
            this.enemyList.Remove(id);

            InterManager.Instance.DropOutExampleBll(plants[id].transform.position);
        }
        else if (this.accList.ContainsKey(id))
        {
            Destroy(this.accList[id]);
            this.accList.Remove(id);
        }
        plants[id].SetActive(false);
    }

    public void AttackUP()
    {
        this.player.attack += addAttack;

    }

    public void HpUP()
    {
        this.player.maxHP += addHP;
        this.player.CurHP += addHP;
        UIManager.Instance.gamePlane.hp.maxValue = this.player.maxHP;

    }

    public void AccUp()
    {
        this.maxAccNumber += addAccNum;
    }

}
