using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    #region GameSceneState
    public enum GAME_SCENE_STATE
    {
        MENU_BASE = 0,
        BATTLE = 1,
    }

    private static string[] SceneNames = new string[]
    {
        "MenuScene",
        "BattleScene",
    };

    #endregion

    private static GameManager _instance = null;
    public static GameManager Instance
    {
        get {
            if(_instance == null)
            {
                _instance = new GameManager();
                _instance.Init();
            }
            return _instance;
        }
    }

    private bool isInited = false;
    private GAME_SCENE_STATE scene_state = GAME_SCENE_STATE.MENU_BASE;

    private void Init()
    {
        if (isInited) return;
        //
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.currentScene.StartsWith("Battle"))
        {
            scene_state = GAME_SCENE_STATE.BATTLE;
        }

#endif


        isInited = true;
    }

    public void ChangeScene(GAME_SCENE_STATE _targetScene)
    {
        if (scene_state == _targetScene) return;

        scene_state = _targetScene;
        SceneManager.LoadScene(SceneNames[(int)_targetScene]);
        

    }

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Ext/跳转到开始场景 %g")]
    public static void GotoMenuScene()
    {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/MenuScene.unity");
    }
    [UnityEditor.MenuItem("Ext/跳转到战斗场景 &g")]
    public static void GotoBattleScene()
    {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/BattleScene.unity");
    }
#endif

}
