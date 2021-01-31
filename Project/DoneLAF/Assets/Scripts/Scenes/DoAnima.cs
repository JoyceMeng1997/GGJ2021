using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoAnima : MonoBehaviour
{

    private RectTransform o_trans;
    private float the_X;
    private float the_Y;
    public uint animaType;
    // Start is called before the first frame update
    void Start()
    {

        o_trans = this.GetComponent<RectTransform>();
        the_X = this.GetComponent<RectTransform>().localPosition.x;
  
        the_Y = this.GetComponent<RectTransform>().localPosition.y;
        if (animaType == 1)
        {
            o_trans.DOBlendableLocalMoveBy(new Vector3(0, 10f, 0), 1).SetEase(Ease.InOutBack).SetLoops(-1, LoopType.Yoyo);
        }
        else if(animaType == 2)
        {
            o_trans.DOScale(new Vector3(1.02f,1.02f,1),0.5f).SetEase(Ease.InBack).SetLoops(-1, LoopType.Yoyo);
        }
        else if(animaType ==3)
        {
            o_trans.DOLocalRotate(new Vector3(0,0,3.0f),0.5f).SetEase(Ease.InBack).SetLoops(-1, LoopType.Yoyo);
        }

    }

    // Update is called once per frame
    void Update()
    {
       
        
    }
}
