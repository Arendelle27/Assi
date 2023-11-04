using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class StartPanel : MonoBehaviour
{
    public Button startButton;

    private void Start()
    {
        startButton.OnClickAsObservable()
.Subscribe(_ =>
{
    if (this.gameObject.activeSelf)
    {
        Game.Instance.StartGame();
    }
})
.AddTo(this);
    }
}
