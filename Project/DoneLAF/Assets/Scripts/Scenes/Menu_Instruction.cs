using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Menu_Instruction : MonoBehaviour
{
    private Transform inTrans;
    // Start is called before the first frame update
    void Start()
    {
        inTrans = this.gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void showpanel()
    {
        if(this.isActiveAndEnabled)
        {
            inTrans.DOLocalMoveY(0,0.5f, false);
        }
    }
    public void hidepanel()
    {
        if(this.isActiveAndEnabled)
        {
            inTrans.DOLocalMoveY(-540,0.5f, false);
        }
    }
}
