using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class OperateBar : MonoBehaviour
{
    public GameObject actionGO;
    public GameObject actionArrow;
    public Transform actionPN;

    [HideInInspector]
    public float barWidth = 500;

    private int curActionNum;

    private List<GameObject> actionGOsUsingList = new List<GameObject>();
    private Stack<GameObject> actionGOsPool = new Stack<GameObject>();

    private float[] cacheProgresses;

    public float nextTargetProgress = 1f;
    public float curArrowSpeed = 1f;
    private Vector3 startPos;

    private bool isInitActionNodes = false;

    public Color[] colors = new Color[]
    {
        new Color(90/255f,180f/255,1f),
        new Color(207/255f,31f/255,25/255f),
        //new Color(142/255f,142/255f,142f/255f),
        new Color(124/255f,82/255,142/255f),
    };

    public void SetupOperateView(int _actionNum, string[] _actionNames, float[] _progresses)
    {
        if (_actionNames == null || _progresses == null) return;
        if (_actionNames.Length != _progresses.Length) return;
        if (_actionNum != _actionNames.Length) return;

        if (actionPN == null)
        {
            Debug.LogErrorFormat("could not find PN");
            return;
        }
#if UNITY_EDITOR
        barWidth = actionPN.GetComponent<RectTransform>().rect.width;
#endif
        cacheProgresses = _progresses;
        for (int i = 0;i < _actionNum;i++)
        {
            GameObject go = null;
            if (isInitActionNodes)
            {
                go = actionGOsUsingList[i];
            }
            else
            {
                go = GetActionFromPool();
                go.name = string.Format("actionBar_{0}",i);
                actionGOsUsingList.Add(go);
                if(i == _actionNum - 1)
                {
                    isInitActionNodes = true;
                }
            }
            if (go != null)
            {
                var ci = i;
                go.transform.SetParent(actionPN);
                go.transform.localScale = Vector3.one;
                go.GetComponent<Image>().color = colors[ci];
                go.transform.GetChild(0).GetComponent<Text>().text = _actionNames[i];
                var nWidth = barWidth * _progresses[i];
                var oRect = go.GetComponent<RectTransform>().rect;
                //go.GetComponent<RectTransform>().sizeDelta = new Vector2(nWidth, oRect.height);
                go.GetComponent<RectTransform>().sizeDelta = new Vector2(nWidth, 0);

                GameObject lastGo = null;
                float startX = 0;
                if (i - 1 >= 0)
                {
                    lastGo = actionGOsUsingList[i - 1];
                    startX = lastGo.GetComponent<RectTransform>().rect.width + lastGo.transform.localPosition.x;
                }
                else
                {
                    startX -= barWidth / 2f;
                }
                Vector3 nPos = new Vector3(startX,0);
                //Debug.LogFormat("bar pos :{0}={1}", i, startX);
                go.transform.localPosition = nPos;
            }
        }
    }

    GameObject GetActionFromPool()
    {
        GameObject go = null;
        if (actionGOsPool.Count > 0)
        {
            go = actionGOsPool.Pop();
        }
        else
        {
            if(actionGO == null)
            {
                Debug.LogErrorFormat("could not find go to Instantiate");
                return null;
            }
            go = GameObject.Instantiate<GameObject>(actionGO);
        }
        go.SetActive(true);
        return go;
    }

    void AfterActionEnd()
    {
        
    }

    void ReturnActionToPool(GameObject go)
    {
        if(go != null)
        {
            actionGOsPool.Push(go);
        }
    }

    private void OnEnable()
    {
#if UNITY_EDITOR
        SetTargetProgress(true);
#endif
    }

    public void SetTargetProgress(bool _start = true)
    {
        if (_start)
        {
            actionArrow.transform.localPosition = new Vector3(0-barWidth / 2f, 35, 0);
            actionArrow.transform.SetSiblingIndex(199);
        }

        actionArrow.transform.DOKill();
        startPos = actionArrow.transform.localPosition;
        var targetPosX = barWidth * (nextTargetProgress - 0.5f);
        var duration = Mathf.Abs((targetPosX - startPos.x) / curArrowSpeed);
        actionArrow.transform.DOLocalMoveX(targetPosX, duration)
            .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear)
            .SetAutoKill(true).onComplete = MoveTargetPosCallback;
        //Debug.LogFormat("StartPos:{0},targetPosX:{1},w:{2},d:{3}", startPos, targetPosX, barWidth, duration);
    }

    void MoveTargetPosCallback()
    {
        //actionArrow.transform.DOKill();
    }

    int JudgeOperateActionResult()
    {
        if(cacheProgresses == null)
        {
            Debug.LogErrorFormat("cacheProgresses is null");
            return 1;
        }
        var arrowPosX = actionArrow.transform.localPosition.x + barWidth * 0.5f;
        var curRate = arrowPosX / barWidth;
        for(int i = 0;i < cacheProgresses.Length; i++)
        {
            var item = cacheProgresses[i];
            if (curRate < item)
            {
                return i;
            }
            curRate -= item;
        }
        return cacheProgresses.Length - 1;
    }

    private void Update()
    {
        if (BattleManager.Instance.canPlayerOperate)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                actionArrow.transform.DOKill();
                int res = JudgeOperateActionResult();
                BattleManager.Instance.PlayerOperateCallback(res);
            }
        }
    }
}
