using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameStatus
{
    Start,
    InGame,
    End
}
public class Game : MonoSingleton<Game>
{
    #region 管理器
    public DataManager dataManager;
    public SpriteManager spriteManager;

    public InterManager interManager;
    #endregion 

    public float timer = 0;//计时器

    GameStatus status;
    public GameStatus Status
    {
        get { return status; }
        set
        {
            status = value;
            UIManager.Instance.UpdateUI(value);
        }
    }

    private void Awake()
    {
        this.dataManager = new DataManager();
        this.spriteManager = new SpriteManager();

        this.interManager = new InterManager();

    }
    private void Start()
    {
        this.Init();
        this.Status = GameStatus.Start;

    }

    public void StartGame()
    {
        PlantManager.Instance.player.gameObject.SetActive(true);
        PlantManager.Instance.StartGenEn();
        UIManager.Instance.gamePlane.hp.maxValue = PlantManager.Instance.player.maxHP;
        this.Status = GameStatus.InGame;
    }

    private void Update()
    {
        if (this.Status == GameStatus.InGame)
        {
            this.timer += Time.deltaTime;
        }
    }

    public void End()
    {
        this.Status = GameStatus.End;
        this.Init();
    }

    public void Init()
    {
        this.timer = 0;
        PlantManager.Instance.Init();
        PlantManager.Instance.player.gameObject.SetActive(false);

        InterManager.Instance.Init();
        UIManager.Instance.Init();
    }
}
