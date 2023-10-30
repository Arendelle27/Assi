using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class ExampleBall : MonoBehaviour
{
    public int exper = 50;

    public UnityAction<int> OnExperBallDropOut;
    void OnEnable()
    {
        Observable.Timer(System.TimeSpan.FromSeconds(3f))
    .Subscribe(_ => { 
        if(this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(false);
        }
    })
    .AddTo(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(Game.Instance.CellMain!=null)
        {
            if((Game.Instance.CellMain.transform.position-this.transform.position).magnitude<=1f)
            {
                if(this.OnExperBallDropOut!=null)
                {
                    this.OnExperBallDropOut(exper);
                }
                this.gameObject.SetActive(false);
            }
        }
    }



}
