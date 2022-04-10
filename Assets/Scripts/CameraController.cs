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
    /// ��ʼ�ƶ�
    /// </summary>
    public void StartMove(UnityAction action)
    {
        //һ��ʼ���ң�Ȼ��ع飬�ع鵽�յ�ʱ���ô�������ί�з���
        MoveForLevelStart(() => MoveForLevelStartBack(action));
    }

    /// <summary>
    /// �ؿ���ʼʱ��������ƶ�
    /// </summary>
    private void MoveForLevelStart(UnityAction action)
    {
        StartCoroutine(DoMove(3.0f, action));
    }

    /// <summary>
    /// �ؿ���ʼʱ��������ع�
    /// </summary>
    private void MoveForLevelStartBack(UnityAction action)
    {
        StartCoroutine(DoMove(-2.7f, action));
    }

    IEnumerator DoMove(float targetPosX, UnityAction action)
    {
        //��ȡĿ��
        Vector3 target = new Vector3(targetPosX, transform.position.y, -10);
        //��ȡ��׼�����ƶ�����
        Vector2 dir = (target - transform.position).normalized;
        //�������Ŀ���Ƚ�Զ����һֱ�ƶ�
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
