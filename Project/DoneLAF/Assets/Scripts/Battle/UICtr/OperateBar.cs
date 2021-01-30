using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    public float nextTargetProgress = 1f;
    public float curArrowSpeed = 1f;
    private Vector3 startPos;
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
        for (int i = 0;i < _actionNum;i++)
        {
            var go = GetActionFromPool();
            if (go != null)
            {
                go.transform.SetParent(actionPN);
                go.transform.localScale = Vector3.one;

                var nWidth = barWidth * _progresses[i];
                var oRect = go.GetComponent<RectTransform>().rect;
                go.GetComponent<RectTransform>().sizeDelta = new Vector2(nWidth, oRect.height);

                GameObject lastGo = null;
                float startX = 0;
                if (i - 1 > 0)
                {
                    lastGo = actionGOsUsingList[i - 1];
                    startX = lastGo.GetComponent<RectTransform>().rect.width + lastGo.transform.localPosition.x;
                }
                Vector3 nPos = new Vector3(startX,0);
                go.transform.localPosition = nPos;
                actionGOsUsingList.Add(go);
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
        SetTargetProgress();
#endif
    }

    public void SetTargetProgress()
    {
        actionArrow.transform.DOKill();
        startPos = actionArrow.transform.localPosition;
        var targetPosX = barWidth * (nextTargetProgress - 0.5f);
        var duration = Mathf.Abs((targetPosX - startPos.x) / curArrowSpeed);
        actionArrow.transform.DOLocalMoveX(targetPosX, duration).SetAutoKill(true).onComplete = MoveTargetPosCallback;
        //Debug.LogFormat("StartPos:{0},targetPosX:{1},w:{2},d:{3}", startPos, targetPosX, barWidth, duration);
    }

    void MoveTargetPosCallback()
    {
        actionArrow.transform.DOKill();
    }
}
