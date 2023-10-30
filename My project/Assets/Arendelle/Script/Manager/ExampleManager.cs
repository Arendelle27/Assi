using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExampleManager
{
    int lever = 1;
    public int Lever
    {
        get
        {
            return lever;
        }
        set
        {
            if (value > lever)
            {
                UIManager.Instance.uIyEles.OpenPla();
            }
            lever = value;
        }
    }

    int exper = 0;
    public int Exper
    {         
        get
        {
            return exper;
        }
        set
        {
            exper = value;
            if (exper >= 100)
            {
                exper =0;
                Lever++;

                Debug.Log("����");
            }
        }
    }

    public List<GameObject> experList = new List<GameObject>();

    int curExBallnum;
    public ExampleManager()
    {
        Lever = 1;
        Exper = 0;
        curExBallnum=0;
        GenerateExperBall();
    }




    void OnAddExper(int exper)
    {
        this.Exper += exper;
    }

    public void DropOutExampleBll(Vector2 pos)
    {
        GameObject go= experList[curExBallnum];
        go.transform.position = pos;
        go.SetActive(true);
        curExBallnum++;
        if(curExBallnum>=experList.Count)
        {
            curExBallnum = 0;
        }
    }

    void GenerateExperBall()
    {
        for(int i = 0; i < 30; i++)
        {
            GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/ExampleBall"));
            go.SetActive(false);
            experList.Add(go);
            ExampleBall exampleBall = go.GetComponent<ExampleBall>();
            exampleBall.OnExperBallDropOut += OnAddExper;
        }
    }

    public void Init()
    {
        Lever = 1;
        Exper = 0;
        curExBallnum = 0;
        foreach (var item in experList)
        {
            item.SetActive(false);
        }
    }
}
