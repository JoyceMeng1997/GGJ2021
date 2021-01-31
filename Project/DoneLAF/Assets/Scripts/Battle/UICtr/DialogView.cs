using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogView : MonoBehaviour
{

    public Text txt_title = null;
    public Text txt_cancel;
    public Text txt_confirm;

    public UnityAction cancelCallback = null;
    public UnityAction confirmCallback = null;

    public Button btnConfirm;
    public Button btnCancel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ResetCallbacks()
    {
        cancelCallback = null;
        confirmCallback = null;
    }

    public void SetCancelCallback(UnityAction _action)
    {
        cancelCallback = _action;
        btnCancel.onClick.AddListener(()=> {
            BattleManager.Instance.HideDialog();
            cancelCallback();
        });

    }

    public void SetConfirmCallback(UnityAction _action)
    {
        confirmCallback = _action;
        btnConfirm.onClick.AddListener(()=> {
            BattleManager.Instance.HideDialog();
            confirmCallback();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
