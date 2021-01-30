using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneBattle : MonoBehaviour
{
    public Button btnExitGame;

    // Start is called before the first frame update
    void Start()
    {
        if (btnExitGame != null)
        {
            btnExitGame.onClick.AddListener(OnBtnExitGameClick);
        }
    }

    void OnBtnExitGameClick()
    {
        GameManager.Instance.ChangeScene(GameManager.GAME_SCENE_STATE.MENU_BASE);
    }

}
