using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class UIWorldElement : MonoBehaviour
{
    public GameObject buttonPanel;

    public Image ExperLIne;

    public Button attackUp;
    IDisposable atU;

    public Button hpUp;
    IDisposable hU;

    public Button accUp;
    IDisposable acU;
    void Start()
    {
        buttonPanel.SetActive(false);
    }

    void Update()
    {
        this.transform.position = PlantManager.Instance.player.transform.position;

        if (buttonPanel.activeSelf)
        {
            if (PlantManager.Instance.maxAccNumber >= 8)
            {
                this.accUp.gameObject.SetActive(false);
            }
            else
            {
                this.accUp.gameObject.SetActive(true);
            }
        }
        ExperLIne.fillAmount = Mathf.Lerp(ExperLIne.fillAmount, InterManager.Instance.Exper / 100f, 0.02f);

    }


    public void OpenPla()
    {
        if (buttonPanel.activeSelf)
        {
            PlantManager.Instance.HpUP();//下次升级之前未未选择，自动升级生命值
            this.CloseButPlaI();
        }

        buttonPanel.SetActive(true);
        this.cBP = StartCoroutine(CloseButPlaW());

        this.atU=attackUp.OnClickAsObservable()
.Subscribe(_ =>
{
PlantManager.Instance.AccUp();
    this.CloseButPlaI();
})
.AddTo(this);

        this.hU=hpUp.OnClickAsObservable()
.Subscribe(_ =>
{
    PlantManager.Instance.HpUP();
    this.CloseButPlaI();
})
.AddTo(this);


        if(PlantManager.Instance.curAccNum >= PlantManager.Instance.maxAccNumber)
        {
            this.accUp.gameObject.SetActive(false);
            return;
        }
       this.acU=accUp.OnClickAsObservable()
.Subscribe(_ =>
{
    PlantManager.Instance.AccUp();
    this.CloseButPlaI();
})
.AddTo(this);

    }


    Coroutine cBP;
    IEnumerator CloseButPlaW()
    {
        yield return new WaitForSeconds(3f);
        PlantManager.Instance.HpUP();//3秒之内未选择，自动升级生命值
        CloseButPlaI();
    }

    void CloseButPlaI()
    {
        buttonPanel.SetActive(false);
        if(atU != null)
            atU.Dispose();
        if (hU != null)
            hU.Dispose();
        if (acU != null)
            acU.Dispose();
        if (cBP != null)
        {
            StopCoroutine(cBP);
            cBP = null;
        }
    }

    public void Init()
    {
        ExperLIne.fillAmount = 0;
    }
}
