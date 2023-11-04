using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UniRx;



public class PlantManager : MonoSingleton<PlantManager>
{
    #region ����
    public float maxTime = 10f;//ÿ�����ʱ��

    public int maxAccNumber = 8;//����������

    public float addHP = 1f;//ѡ����������ֵ�����Ѫ��������

    public float addAttack = 1f;//ѡ����������������ҹ�����������

    public int addAccNum = 1;//ѡ����������������������������

    public float recovHp = 1f;//���յ��˻ظ�Ѫ����

    public int maxMonNum = 4;//��ʼÿ������������
    #endregion

    #region ״̬
    public Player player;//���

    List<GameObject> plants = new List<GameObject>();

    public Dictionary<int, Acc> accList = new Dictionary<int, Acc>();//����б�
    public Queue<int> accIds = new Queue<int>();//��¼���id������������������ֵʱ��ɾ����������
    public int curAccNum ;//��ǰ����������

    public Dictionary<int, Enemy> enemyList = new Dictionary<int, Enemy>();//�����б�

    public int wave = 0;//����


    public int curMonNum;//��ǰÿ����������

    public int plantId = 0;
    public int PlantId//ֲ��id
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
        GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Player");//�������Ԥ����
        this.player = Instantiate(playerPrefab).GetComponent<Player>();
        this.player.plantKind = PlantKind.Player;

        GeneratePlant();

        this.Init();
        this.player.gameObject.SetActive(false);
    }

    public void Init()//��ʼ��
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


    void GeneratePlant()//��Ϸһ��ʼ����49��ֲ��Ԥ���屸��
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

    public void StartGenEn()//��ʼˢ��
    {
        ge=StartCoroutine(GenerateEnemys());
    }

    Coroutine ge;//ˢ��Э��
    IEnumerator GenerateEnemys()//ˢ��
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

    void GenerateEnemy(Vector2 pos,PlantKind kind)//���ɵ�����
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

        en.Ass//����ת��
        .Subscribe(id =>
        {
            this.player.curHP += recovHp;//����һ�λָ�һ��Ѫ
            this.Assiminate(id);
        });

        en.OnDead//��������
        .Subscribe(id =>
        {
            this.OnDead(id);
        });


        plants[PlantId].transform.position = pos;
        ++plantId;
    }

    void Assiminate(int id)//ת��
    {
        if(this.accList.Count == curAccNum) //�������ޣ�ɾ����������
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

        acc.OnDead//��������
        .Subscribe(id =>
        {
            this.OnDead(id);
        });


        Destroy(this.enemyList[id]);
        this.enemyList.Remove(id);
    }

    void OnDead(int id)//����
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
