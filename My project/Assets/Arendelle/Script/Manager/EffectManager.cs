using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellKind
{
    TaHuang,
    DuMogu,
    ZuiChunHua,
    HuoLongGuo,
    Player
}

public enum CellAccType
{
    Acc,
    Enemy,
    None
}

public enum DataType
{
    Attack,
    Hp,
    Speed,
}
public class EffectManager
{
    public Dictionary<CellKind, Effect> effectDic ;

    public Dictionary<CellKind, Dictionary<DataType, float>> dataAcc;
    public Dictionary<CellKind, Dictionary<DataType, float>> dataEn;
    public EffectManager()
    {
        effectDic = new Dictionary<CellKind, Effect>() {
            {CellKind.TaHuang,new TaHuang()},
            {CellKind.DuMogu,new DuMogu()},
            {CellKind.ZuiChunHua,new ZuiChunHua()},
            {CellKind.HuoLongGuo,new HuoLongGuo()},
        };

        dataAcc = new Dictionary<CellKind, Dictionary<DataType, float>>()
        {
            {CellKind.TaHuang,new Dictionary<DataType, float>()
            {
                {DataType.Attack,2},
                {DataType.Hp,4},
                {DataType.Speed,10},
            }},
            {CellKind.DuMogu,new Dictionary<DataType, float>()
            {
                {DataType.Attack,0},
                {DataType.Hp,4},
                {DataType.Speed,5},
            }},
            {CellKind.ZuiChunHua,new Dictionary<DataType, float>()
            {
                {DataType.Attack,0},
                {DataType.Hp,6},
                {DataType.Speed,5},
            }},
            {CellKind.HuoLongGuo,new Dictionary<DataType, float>()
            {
                {DataType.Attack,3},
                {DataType.Hp,6},
                {DataType.Speed,5},
            }},
            {CellKind.Player,new Dictionary<DataType, float>()
            {
                {DataType.Attack,3},
                {DataType.Hp,8},
                {DataType.Speed,6},
            }},
        };

        dataEn = new Dictionary<CellKind, Dictionary<DataType, float>>()
        {
            {CellKind.TaHuang,new Dictionary<DataType, float>()
            {
                {DataType.Attack,1},
                {DataType.Hp,4},
                {DataType.Speed,2},
            }},
            {CellKind.DuMogu,new Dictionary<DataType, float>()
            {
                {DataType.Attack,1},
                {DataType.Hp,4},
                {DataType.Speed,1},
            }},
            {CellKind.ZuiChunHua,new Dictionary<DataType, float>()
            {
                {DataType.Attack,1},
                {DataType.Hp,6},
                {DataType.Speed,1},
            }},
            {CellKind.HuoLongGuo,new Dictionary<DataType, float>()
            {
                {DataType.Attack,2},
                {DataType.Hp,6},
                {DataType.Speed,2},
            }},
        };
    }

    public CellKind RaKind()
    {
        switch (Random.Range(0, 4))
        {
            case 0:
                return CellKind.TaHuang;
            case 1:
                return CellKind.DuMogu;
            case 2:
                return CellKind.ZuiChunHua;
            case 3:
                return CellKind.HuoLongGuo;
            default:
                return CellKind.TaHuang;
        }
    }
}
