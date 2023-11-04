using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class InterBall : MonoBehaviour
{
    #region 属性
    [SerializeField, LabelText("经验值"), Tooltip("每个经验球加的经验值")]
    public int exper = 34;
    #endregion

    #region 事件
    public UnityAction<int> OnExperBallDropOut;
    #endregion


    void OnEnable()
    {
        Observable.Timer(System.TimeSpan.FromSeconds(3f))
    .Subscribe(_ =>
    {
        if (this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(false);
        }
    })
    .AddTo(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlantManager.Instance.player != null)
        {
            if ((PlantManager.Instance.player.transform.position - this.transform.position).magnitude <= 1f)
            {
                if (this.OnExperBallDropOut != null)
                {
                    this.OnExperBallDropOut(exper);
                }
                this.gameObject.SetActive(false);
            }
        }
    }



}
