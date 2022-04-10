using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    private void Awake()
    {
        Instance = this;
        transform.position = new Vector3(-2.7f, 0.05f, -10);
    }

    /// <summary>
    /// 开始移动
    /// </summary>
    public void StartMove(UnityAction action)
    {
        //一开始往右，然后回归，回归到终点时调用传进来的委托方法
        MoveForLevelStart(() => MoveForLevelStartBack(action));
    }

    /// <summary>
    /// 关卡开始时的摄像机移动
    /// </summary>
    private void MoveForLevelStart(UnityAction action)
    {
        StartCoroutine(DoMove(3.0f, action));
    }

    /// <summary>
    /// 关卡开始时的摄像机回归
    /// </summary>
    private void MoveForLevelStartBack(UnityAction action)
    {
        StartCoroutine(DoMove(-2.7f, action));
    }

    IEnumerator DoMove(float targetPosX, UnityAction action)
    {
        //获取目标
        Vector3 target = new Vector3(targetPosX, transform.position.y, -10);
        //获取标准化的移动方向
        Vector2 dir = (target - transform.position).normalized;
        //如果我离目标点比较远，就一直移动
        while(Vector2.Distance(target,transform.position)>0.1f)
        {
            yield return new WaitForSeconds(0.01f);
            transform.Translate(dir * 0.1f);
        }
        if (action != null) 
        {
            action();
        }
    }
}
