using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlantKind
{
    TaHuang,
    DuMogu,
    ZuiChunHua,
    HuoLongGuo,
    Player
}

public enum AccOrEnemy
{
    Acc,
    Enemy,
}

public enum DataType
{
    Attack,
    Hp,
    Speed,
}
public class DataManager
{
    public Dictionary<PlantKind, Acc> accDic ;
    public Dictionary<PlantKind, Enemy> enDic;

    public Dictionary<PlantKind, Dictionary<DataType, float>> dataAcc;
    public Dictionary<PlantKind, Dictionary<DataType, float>> dataEn;
    public DataManager()
    {
         accDic= new Dictionary<PlantKind, Acc>() {
            {PlantKind.TaHuang,new AccTaHuang()},
            {PlantKind.DuMogu,new AccDuMogu()},
            {PlantKind.ZuiChunHua,new AccZuiChunHua()},
            {PlantKind.HuoLongGuo,new AccHuoLongGuo()},
        };

        enDic = new Dictionary<PlantKind, Enemy>()
        {
            {PlantKind.TaHuang,new EnTaHuang()},
            {PlantKind.DuMogu,new EnDuMogu()},
            {PlantKind.ZuiChunHua,new EnZuiChunHua()},
            {PlantKind.HuoLongGuo,new EnHuoLongGuo()},
        };

        dataAcc = new Dictionary<PlantKind, Dictionary<DataType, float>>()
        {
            {PlantKind.TaHuang,new Dictionary<DataType, float>()
            {
                {DataType.Attack,2},
                {DataType.Hp,4},
                {DataType.Speed,10},
            }},
            {PlantKind.DuMogu,new Dictionary<DataType, float>()
            {
                {DataType.Attack,0},
                {DataType.Hp,4},
                {DataType.Speed,5},
            }},
            {PlantKind.ZuiChunHua,new Dictionary<DataType, float>()
            {
                {DataType.Attack,0},
                {DataType.Hp,6},
                {DataType.Speed,5},
            }},
            {PlantKind.HuoLongGuo,new Dictionary<DataType, float>()
            {
                {DataType.Attack,3},
                {DataType.Hp,6},
                {DataType.Speed,5},
            }},
            {PlantKind.Player,new Dictionary<DataType, float>()
            {
                {DataType.Attack,3},
                {DataType.Hp,8},
                {DataType.Speed,6},
            }},
        };

        dataEn = new Dictionary<PlantKind, Dictionary<DataType, float>>()
        {
            {PlantKind.TaHuang,new Dictionary<DataType, float>()
            {
                {DataType.Attack,1},
                {DataType.Hp,4},
                {DataType.Speed,2},
            }},
            {PlantKind.DuMogu,new Dictionary<DataType, float>()
            {
                {DataType.Attack,1},
                {DataType.Hp,4},
                {DataType.Speed,1},
            }},
            {PlantKind.ZuiChunHua,new Dictionary<DataType, float>()
            {
                {DataType.Attack,1},
                {DataType.Hp,6},
                {DataType.Speed,1},
            }},
            {PlantKind.HuoLongGuo,new Dictionary<DataType, float>()
            {
                {DataType.Attack,2},
                {DataType.Hp,6},
                {DataType.Speed,2},
            }},
        };
    }

}
