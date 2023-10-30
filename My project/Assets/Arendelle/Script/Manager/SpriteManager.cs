using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum characterType
{
    Player,
    PlayerPri,
    TH,
    HLG,
    DMG,
    ZCH
}

public enum characterStatus
{
    Normal,
    Enemy,
    Injured
}

public class SpriteManager 
{


    public Dictionary<characterType, Dictionary<characterStatus, Sprite>> characterSpriteDic;

    public SpriteManager()
    {
        Dictionary<characterStatus, Sprite> playerSpriteDic = new Dictionary<characterStatus, Sprite>();
        playerSpriteDic.Add(characterStatus.Normal, Resources.Load<Sprite>("Art/Player/PlayerNor"));
        playerSpriteDic.Add(characterStatus.Injured, Resources.Load<Sprite>("Art/Player/PlayerInj"));

        Dictionary<characterStatus, Sprite> playerPriSpriteDic = new Dictionary<characterStatus, Sprite>();
        playerPriSpriteDic.Add(characterStatus.Normal, Resources.Load<Sprite>("Art/Player/PlayerPriNor"));
        playerPriSpriteDic.Add(characterStatus.Injured, Resources.Load<Sprite>("Art/Player/PlayerPriInj"));

        Dictionary<characterStatus, Sprite> thSpriteDic = new Dictionary<characterStatus, Sprite>();
        thSpriteDic.Add(characterStatus.Normal, Resources.Load<Sprite>("Art/TH/THAcc"));
        thSpriteDic.Add(characterStatus.Enemy, Resources.Load<Sprite>("Art/TH/THEn"));
        thSpriteDic.Add(characterStatus.Injured, Resources.Load<Sprite>("Art/TH/THInj"));

        Dictionary<characterStatus, Sprite> hlgSpriteDic = new Dictionary<characterStatus, Sprite>();
        hlgSpriteDic.Add(characterStatus.Normal, Resources.Load<Sprite>("Art/HLG/HLGAcc"));
        hlgSpriteDic.Add(characterStatus.Enemy, Resources.Load<Sprite>("Art/HLG/HLGEn"));
        hlgSpriteDic.Add(characterStatus.Injured, Resources.Load<Sprite>("Art/HLG/HLGInj"));

        Dictionary<characterStatus, Sprite> dmgSpriteDic = new Dictionary<characterStatus, Sprite>();
        dmgSpriteDic.Add(characterStatus.Normal, Resources.Load<Sprite>("Art/DMG/DMGAcc"));
        dmgSpriteDic.Add(characterStatus.Enemy, Resources.Load<Sprite>("Art/DMG/DMGEn"));
        dmgSpriteDic.Add(characterStatus.Injured, Resources.Load<Sprite>("Art/DMG/DMGInj"));

        Dictionary<characterStatus, Sprite> zchSpriteDic = new Dictionary<characterStatus, Sprite>();
        zchSpriteDic.Add(characterStatus.Normal, Resources.Load<Sprite>("Art/ZCH/ZCHAcc"));
        zchSpriteDic.Add(characterStatus.Enemy, Resources.Load<Sprite>("Art/ZCH/ZCHEn"));
        zchSpriteDic.Add(characterStatus.Injured, Resources.Load<Sprite>("Art/ZCH/ZCHInj"));

        characterSpriteDic = new Dictionary<characterType, Dictionary<characterStatus, Sprite>>()
        {
            {characterType.Player,playerSpriteDic },
            {characterType.PlayerPri,playerPriSpriteDic },
            {characterType.TH,thSpriteDic },
            {characterType.HLG,hlgSpriteDic },
            {characterType.DMG,dmgSpriteDic },
            {characterType.ZCH,zchSpriteDic },
        };

    }

}
