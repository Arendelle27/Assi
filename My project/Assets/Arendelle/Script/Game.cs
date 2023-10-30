using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState
{
    Start,
    InGame,
    End
}
public class Game : MonoSingleton<Game>
{
    public GameState status;

    public GameState Status
    {
        get { return status; }
        set
        {
            status = value;
            UIManager.Instance.UpdateUI();
        }
    }

    public CellMain CellMain;

    public EffectManager effectManager;

    public ExampleManager  exampleManager;

    public SpriteManager spriteManager;


    public float timer = 0;
    void Awake()
    {
        this.effectManager = new EffectManager();
        this.exampleManager = new ExampleManager();
        this.spriteManager = new SpriteManager();

        this.Status = GameState.Start;
    }
    // Update is called once per frame
    void Update()
    {
        if (this.Status == GameState.InGame)
        {
            this.timer+=Time.deltaTime;
            UIManager.Instance.resText.text = ((int)this.timer).ToString();

        }
    }

    public void Init()
    {
        this.timer = 0;
        InputManager.Instance.Init();
        CellAccManager.Instance.Init();
        UIManager.Instance.Init();
        if(this.exampleManager!=null)
        {
            this.exampleManager.Init();
        }
        this.CellMain.Init();
    }
}
