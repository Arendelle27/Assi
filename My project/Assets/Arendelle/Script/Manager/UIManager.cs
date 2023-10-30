using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UIManager : MonoSingleton<UIManager>
{
    public UIWorldElement uIyEles;

    public Slider hp;
    public Image hpFill;

    public Button startButton;
    public Button exitButton;
    public Button resetBUtton;

    public Text resText;

    public GameObject startPlane;
    public GameObject gamePlane;
    public GameObject endPlane;
    void Start()
    {
        hp.maxValue = Game.Instance.CellMain.maxHP;

        startButton.OnClickAsObservable()
        .Subscribe(_ =>
        {
            Game.Instance.Status = GameState.InGame;
        })
        .AddTo(this);


        exitButton.OnClickAsObservable()
        .Subscribe(_ =>
        {
            Game.Instance.Status = GameState.Start;
        })
        .AddTo(this);

        resetBUtton.OnClickAsObservable()
        .Subscribe(_ =>
        {
            Game.Instance.Status = GameState.InGame;
        })
        .AddTo(this);
    }

// Update is called once per frame
void Update()
    {
        hp.value = Mathf.Lerp(hp.value, Game.Instance.CellMain.CurHP, 0.02f);
    }

    public void UpdateUI()
    {
            this.startPlane.SetActive(Game.Instance.Status==GameState.Start);
            this.gamePlane.SetActive(Game.Instance.Status==GameState.InGame);
            this.endPlane.SetActive(Game.Instance.Status == GameState.End);
    }

    public void Init()
    {
        this.hp.value= Game.Instance.CellMain.maxHP;
        this.uIyEles.ExperLIne.fillAmount = 0;
    }
}
