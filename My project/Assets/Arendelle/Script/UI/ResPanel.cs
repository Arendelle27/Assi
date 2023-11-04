using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ResPanel : MonoBehaviour
{
        public Button exitButton;
        public Button resetButton;

        public Text resText;

    private void OnEnable()
    {
        this.resText.text = ((int)Game.Instance.timer).ToString();
    }

    private void Start()
    {
        exitButton.OnClickAsObservable()
.Subscribe(_ =>
{
    if (this.gameObject.activeSelf)
    {
        Game.Instance.Status = GameStatus.Start;
    }
})
.AddTo(this);

        resetButton.OnClickAsObservable()
.Subscribe(_ =>
{
    if(this.gameObject.activeSelf)
    {
        Game.Instance.StartGame();
    }
})
.AddTo(this);
    }
}
