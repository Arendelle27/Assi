using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UIWorldElement : MonoBehaviour
{
    public GameObject buttonPanel;

    public Image ExperLIne;

    public Button attackUp;
    public Button hpUp;
    public Button accUp;
    void Start()
    {
        buttonPanel.SetActive(false);
             attackUp.OnClickAsObservable()
    .Subscribe(_ =>
    {
        CellAccManager.Instance.AccUp();
        buttonPanel.SetActive(false);
        StopCoroutine(CloseButPla());
    })
    .AddTo(this);

             hpUp.OnClickAsObservable()
    .Subscribe(_ =>
    {
        CellAccManager.Instance.HpUP();
        buttonPanel.SetActive(false);
        StopCoroutine(CloseButPla());
    })
    .AddTo(this);

             accUp.OnClickAsObservable()
    .Subscribe(_ =>
    {
        CellAccManager.Instance.AccUp();
        buttonPanel.SetActive(false);
        StopCoroutine(CloseButPla());
    })
    .AddTo(this);

    }

    // Update is called once per frame
    void Update()
    {
        if(buttonPanel.activeSelf)
        {
            if (CellAccManager.Instance.maxAccNumber >= 8)
            {
                this.accUp.gameObject.SetActive(false);
            }
            else
            {
                this.accUp.gameObject.SetActive(true);
            }
        }

        else
        {
            accUp.interactable = true;
        }
        ExperLIne.fillAmount =Mathf.Lerp(ExperLIne.fillAmount,Game.Instance.exampleManager.Exper / 100f,0.02f);

    }


     public void OpenPla()
    {
        buttonPanel.SetActive(true);
        StartCoroutine( CloseButPla());

    }

    IEnumerator CloseButPla()
    {
        if (this.buttonPanel.activeSelf)
        {
            yield return new WaitForSeconds(3f);
            buttonPanel.SetActive(false);
        }
    }
}
