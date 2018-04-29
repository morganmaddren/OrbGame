using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public enum MinionBuffs
{
    Regrow,
    Entangled,
    HuntersMark,
}

public enum HeroBuffs
{
    Meditate,
    Leer
}

public class BuffFactory
{
    static Dictionary<MinionBuffs, string> minionBuffPaths = new Dictionary<MinionBuffs, string>()
    {
        { MinionBuffs.Regrow , "Buffs/MinionBuffs/RegrowBuff" },
        { MinionBuffs.Entangled , "Buffs/MinionBuffs/EntangledBuff" },
        { MinionBuffs.HuntersMark , "Buffs/MinionBuffs/HunterMarkBuff" },
    };

    static Dictionary<HeroBuffs, string> heroBuffPaths = new Dictionary<HeroBuffs, string>()
    {
        { HeroBuffs.Meditate , "Buffs/HeroBuffs/MeditateBuff" },
        { HeroBuffs.Leer , "Buffs/HeroBuffs/LeerBuff" },
    };

    public static BuffBase CreateBuff<TBuffKey>(TBuffKey buff)
    {
        int key = Convert.ToInt32(buff);
        string path = null;
        if (typeof(TBuffKey) == typeof(MinionBuffs))
            path = minionBuffPaths[(MinionBuffs)key];
        else if (typeof(TBuffKey) == typeof(HeroBuffs))
            path = heroBuffPaths[(HeroBuffs)key];

        var prefab = Game.Resources.LoadPrefab<BuffBase>(path);

        return prefab;
    }
}