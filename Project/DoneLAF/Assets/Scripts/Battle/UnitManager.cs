using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ItemPack
{
    public int id;
    public int num;
}

public abstract class UnitData
{
    public int id;
    public int curHP;
    public int curMaxHP;
    public int curATK;

    public List<ItemPack> ownItems = new List<ItemPack>();

    public abstract bool isPlayer();
}

public class PlayerData : UnitData
{
    public PlayerConfigData configData;
    public override bool isPlayer()
    {
        return true;
    }

    public void Init()
    {
        curHP = configData.HP_Base;
        curMaxHP = configData.HP_Base;
        curATK = configData.atk_Base;
    }

}

public class EnemyData : UnitData
{
    public UnitConfigData configData;
    public override bool isPlayer()
    {
        return false;
    }

    public void Init()
    {
        curHP = configData.hp;
        curMaxHP = configData.hp;
        curATK = configData.atk;
    }

}

public class UnitManager
{
    public PlayerData playerData;
    public EnemyData enemyData;

    public void InitPlayer()
    {
        if(playerData == null)
        {
            playerData = new PlayerData();
            playerData.configData = AllUnitConfigs.Instance.playerConfigData;
            playerData.Init();


        }
    }

    public void CreateEnemy(uint _id)
    {
        var config = AllUnitConfigs.Instance.GetEnemyConfigDataById(_id);
        if(config == null)
        {
            Debug.LogErrorFormat("create enemy entity failed!");
            return;
        }
        enemyData = new EnemyData();
        enemyData.configData = config;
        enemyData.Init();
    }

    public void CreateEnemy(UnitConfigData _configData)
    {
        var config = _configData;
        if (config == null)
        {
            Debug.LogErrorFormat("create enemy entity failed!");
            return;
        }
        enemyData = new EnemyData();
        enemyData.configData = config;
        enemyData.Init();
    }

}
