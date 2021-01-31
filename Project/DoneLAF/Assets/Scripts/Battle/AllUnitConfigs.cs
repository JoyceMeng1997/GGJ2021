using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerConfigData
{
    public int HP_Base = 10;//基础生命
    public int HP_Recover = 1;//每回合的回复
    public int atk_Base = 1;
    //public float actionMercy;
    //public float actionAttack;
    //public float actionEscape;

    public float minestActionValue = 0.05f;

    public float actionMercyBaseRate = 0.4f;
    public float actionAttackBaseRate = 0.25f;
    public float actionEscapeBaseRate = 0.35f;

    public float actionMercyGrowRate = 0.05f;
    public float actionAttackGrowRate = 0.1f;
    public float actionEscapeGrowRate = 0.08f;

}

[System.Serializable]
public class ReactionConfigData
{
    [Tooltip("0=仁慈,1=攻击,2=逃跑")]
    public int actionId;
    [Tooltip("操作条改变大小")]
    [Range(0,1f)]
    public float actionEffect;
    [Tooltip("反应时的提示文本")]
    public string actionText;
}

[System.Serializable]
public class UnitConfigData
{
    public uint id;
    public string name;
    public string textOnShow;
    public uint[] skillIds;
    public uint[] items;
    public int atk;
    public int hp;

    [Tooltip("反应，面对仁慈/攻击/逃跑的结果")]
    public ReactionConfigData[] reaction;//

    [Tooltip("玩家遭遇对方时操作条上提示的文本")]
    public string[] operateTexts;

    [Tooltip("玩家击败对方时的文本")]
    public string[] winTexts;

    [Tooltip("玩家输给对方时的文本")]
    public string[] lossTexts;


    public int maxRound;
}

[System.Serializable]
public class AllUnitConfigs : MonoBehaviour
{
    public PlayerConfigData playerConfigData;
    public UnitConfigData[] unitConfigDatas;

    public uint[] stageEnemyIds;

    private Dictionary<uint, UnitConfigData> unitConfigDatasDic = new Dictionary<uint, UnitConfigData>();

    private static AllUnitConfigs _instance;
    public static AllUnitConfigs Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        _instance = this;
        for (int i = 0; i < unitConfigDatas.Length; i++)
        {
            var item = unitConfigDatas[i];
            if (unitConfigDatasDic.ContainsKey(item.id))
            {
                Debug.LogErrorFormat("unit configs contains id {0},skipped it", item.id);
            }
            else
            {
                unitConfigDatasDic.Add(item.id, item);
            }
        }
    }

    private void Start()
    {
        
    }

    public UnitConfigData GetEnemyConfigDataById(uint _id)
    {
        UnitConfigData ret = null;
        if(!unitConfigDatasDic.TryGetValue(_id,out ret))
        {
            Debug.LogErrorFormat("id:{0} is not found in all unit configs",_id);
        }
        return ret;
    }

    public UnitConfigData GetEnemyConfigDataByStageId(uint _id)
    {
        if(_id >= 0 &&  _id < stageEnemyIds.Length)
        {
            var enemyId = stageEnemyIds[_id];
            return GetEnemyConfigDataById(enemyId);
        }
        else
        {
            return null;
        }
    }

}
