using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneMenu : MonoBehaviour
{

    public Button btnStartGame;

    // Start is called before the first frame update
    void Start()
    {
        if(btnStartGame != null)
        {
            btnStartGame.onClick.AddListener(OnBtnStartGameClick);
        }
    }

    void OnBtnStartGameClick()
    {
        GameManager.Instance.ChangeScene(GameManager.GAME_SCENE_STATE.BATTLE);
    }
    
}
