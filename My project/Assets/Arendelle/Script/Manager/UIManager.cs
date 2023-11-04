using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UIManager: MonoSingleton<UIManager>
{
    public UIWorldElement uIyEles;


    public StartPanel startPlane;
    public GamePanel gamePlane;
    public ResPanel resPlane;

    public void UpdateUI(GameStatus status)
    {
        this.startPlane.gameObject.SetActive(status == GameStatus.Start);
        this.gamePlane.gameObject.SetActive(status == GameStatus.InGame);
        this.resPlane.gameObject.SetActive(status == GameStatus.End);
    }

    public void Init()
    {
        this.uIyEles.Init();
        this.gamePlane.Init();
    }
}
