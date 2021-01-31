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

    /// <summary>
    ///  0=mercy,1=attack,2=escape
    /// </summary>
    public List<float> allOperateValues = new List<float>();
    public List<float> allOperateGrowValues = new List<float>();
    public List<string> allOperateNames = new List<string>();

    public override bool isPlayer()
    {
        return true;
    }

    public void Init()
    {
        curHP = configData.HP_Base;
        curMaxHP = configData.HP_Base;
        curATK = configData.atk_Base;

        var mercyRate = configData.actionMercyBaseRate;
        var attackRate = configData.actionAttackBaseRate;
        var escapeRate = configData.actionEscapeBaseRate;
        allOperateValues.Add(mercyRate);
        allOperateValues.Add(attackRate);
        allOperateValues.Add(escapeRate);

        var mercyGrowRate = configData.actionMercyGrowRate;
        var attackGrowRate = configData.actionAttackGrowRate;
        var escapeGrowRate = configData.actionEscapeGrowRate;
        allOperateGrowValues.Add(mercyGrowRate);
        allOperateGrowValues.Add(attackGrowRate);
        allOperateGrowValues.Add(escapeGrowRate);

        allOperateNames.Add("Mercy");
        allOperateNames.Add("Attack");
        allOperateNames.Add("Escape");

    }

    public float[] GetAllOperateValues()
    {
        return allOperateValues.ToArray();
    }

    public string[] GetAllOperateNames()
    {
        return allOperateNames.ToArray();
    }

    private List<float> cacheChangeValueList = new List<float>();
    /// <summary>
    ///  0=mercy,1=attack,2=escape
    /// </summary>
    public void ChangeValuesAfterOperate(int _index)
    {
        if (_index < 0 || _index >= allOperateValues.Count) return;
        var curGrow = allOperateGrowValues[_index];
        var curDown = -curGrow / (allOperateValues.Count - 1);

        var freeDown = 0f;
        for (int i = 0; i < allOperateValues.Count; i++)
        {
            if (i == _index)
            {
                //cacheChangeValueList[i] = curGrow;
            }
            else
            {
                var res = freeDown + allOperateValues[i] + curDown - configData.minestActionValue;
                freeDown = res;
            }
        }

        if(freeDown >= 0)
        {

        }
        //curGrow += freeDown;
        var sum = 0f;
        for (int i = 0; i < allOperateValues.Count; i++)
        {
            if (i == _index)
            {
                //allOperateValues[i] += curGrow;
            }
            else
            {
                allOperateValues[i] += curDown;
                allOperateValues[i] = Mathf.Max(configData.minestActionValue, allOperateValues[i]);
                sum += allOperateValues[i];
            }
        }
        allOperateValues[_index] = (1f - sum);
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
        //if(playerData == null)
        //{
            playerData = new PlayerData();
            playerData.configData = AllUnitConfigs.Instance.playerConfigData;
            playerData.Init();
        //}
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
