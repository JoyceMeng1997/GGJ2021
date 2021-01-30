using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleManager : MonoBehaviour
{
    #region UI ctrl
    public Text txt_playerHP;
    public Image img_playerHpBar;

    public Text txt_enemyName;
    public Text txt_enemyInfo;
    public Text txt_enemyHP;
    public Image img_enemyHpBar;
    public Image imp_enemyPic;

    public OperateBar operateBar;
    #endregion

    #region static
    public static BattleManager Instance
    {
        get { return _instance; }
    }
    private static BattleManager _instance;
    #endregion

    #region battle info
    /// <summary>
    /// 在谁的回合,0=自己，1=敌人,-1=缺省
    /// </summary>
    int curInRoundPid = -1;

    int curBattleRound = 0;

    uint curStageId = 0;
    public bool canPlayerOperate = false;

    public UnitManager unitManager;

    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;
        unitManager = new UnitManager();
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        unitManager.InitPlayer();

        UpdatePlayerInfo(unitManager.playerData, false);
        CreateNewBattle();
    }

    public void CreateNewBattle()
    {
        Debug.LogFormat("CreateNewBattle");
        var enemyConfigData = AllUnitConfigs.Instance.GetEnemyConfigDataByStageId(curStageId);
        if(enemyConfigData == null)
        {
            Debug.LogErrorFormat("found no enemy config data");
            return;
        }

        unitManager.CreateEnemy(enemyConfigData);
        UpdateEnemyInfo(unitManager.enemyData, false);

        //
        RoundStart(0);
    }

    public void EndBattle(bool win)
    {
        if (win)
        {
            curStageId++;
            if(curStageId >= AllUnitConfigs.Instance.stageEnemyIds.Length)
            {
                GameOver(true);
            }

            CreateNewBattle();

        }
        else
        {
            GameOver();
        }
    }

    public void GameOver(bool win = false)
    {
        if (win)
        {
            Debug.LogErrorFormat("Game Over!!!!!!!!!!WIN!!!!!!!!!!!!!!!");

        }
        else
        {
            Debug.LogErrorFormat("Game Over!!!!!!!!!!!LOSS!!!!!!!!!!!!!!");

        }
    }


    #region battle round
    public void RoundStart(int _roundPid)
    {
        Debug.LogFormat("RoundStart :{0}",_roundPid);

        curInRoundPid = _roundPid;

        bool isPlayer = curInRoundPid == 0;
        canPlayerOperate = isPlayer;

        RoundStartJudge();

        if (isPlayer)
        {
            //StartCoroutine(RoundRunning());
            StartCoroutine("RoundRunning");
        }
        else
        {
            EnemyAction();
        }

    }

    public void RoundStartJudge()
    {
        
    }

    IEnumerator RoundRunning()
    {
        yield return new WaitForSeconds(0.5f);

        //operateBar.SetupOperateView()
        Debug.LogFormat("RoundRunning :{0}", curInRoundPid);
        //test todo
        int damage = unitManager.playerData.curATK;
        if(!DamageToTarget(damage, 0))
        {
            RoundEnd();
        }


    }

    public void RoundEnd()
    {
        Debug.LogFormat("End :{0}", curInRoundPid);
        curInRoundPid = curInRoundPid == 0 ? 1 : 0;
        canPlayerOperate = false;
        //StartCoroutine(IEWaitRoundEnd());
        StartCoroutine("IEWaitRoundEnd");
    }

    IEnumerator IEWaitRoundEnd()
    {
        yield return new WaitForSeconds(1);
        RoundStart(curInRoundPid);
    }

    public void EnemyAction()
    {
        int damage = unitManager.enemyData.curATK;
        if(!DamageToTarget(damage, 1))
        {
            RoundEnd();
        }

    }


    #endregion

    #region Calc
    /// <summary>
    /// target 0=攻击敌人，1=攻击玩家
    /// </summary>
    /// <param name="_damage"></param>
    /// <param name="_target ">0=攻击敌人，1=攻击玩家</param>
    public bool DamageToTarget(int _damage,int _target)
    {
        if(_target == 0)
        {
            unitManager.enemyData.curHP -= _damage;
            UpdateEnemyInfo(unitManager.enemyData, true);
            if (unitManager.enemyData.curHP < 0)
            {
                EndBattle(true);
                return true;
            }
        }
        else
        {
            unitManager.playerData.curHP -= _damage;
            UpdatePlayerInfo(unitManager.playerData, true);

            if (unitManager.playerData.curHP < 0)
            {
                EndBattle(false);
                return true;
            }

        }
        return false;
    }

    #endregion


    #region UI update
    public void UpdatePlayerInfo(PlayerData _data,bool _isAnim = true)
    {
        if(_data == null)
        {
            Debug.LogErrorFormat("PlayerData is null!!");
            return;
        }
        txt_playerHP.text = string.Format("{0}/{1}", _data.curHP, _data.curMaxHP);

        img_playerHpBar.DOKill(true);
        if (_isAnim)
        {
            float duration = 0.2f;
            img_playerHpBar.DOFillAmount(_data.curHP * 1f / _data.curMaxHP,duration);
        }
        else
        {
            img_playerHpBar.fillAmount = _data.curHP * 1f / _data.curMaxHP;
        }
    }

    public void UpdateEnemyInfo(EnemyData _data,bool _isAnim = true)
    {
        if (_data == null)
        {
            Debug.LogErrorFormat("Enemy data is null!!");
            return;
        }
        txt_enemyHP.text = string.Format("{0}/{1}", _data.curHP, _data.curMaxHP);

        txt_enemyName.text = _data.configData.name;
        txt_enemyInfo.text = string.Format("ATK:{0}", _data.configData.atk);

        img_enemyHpBar.DOKill(true);
        if (_isAnim)
        {
            
            float duration = 0.2f;
            img_enemyHpBar.DOFillAmount(_data.curHP * 1f / _data.curMaxHP, duration);
        }
        else
        {
            img_enemyHpBar.fillAmount = _data.curHP * 1f / _data.curMaxHP;
        }
    }

    #endregion
}
