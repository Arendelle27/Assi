using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    public Slider hp;

    void Update()
    {
        hp.value = Mathf.Lerp(hp.value, PlantManager.Instance.player.CurHP, 0.02f);
    }

    public void Init()
    {
        hp.maxValue = PlantManager.Instance.player.maxHP;
    }
}
