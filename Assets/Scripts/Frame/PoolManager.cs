using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    private static PoolManager instance;

    public static PoolManager Instance 
    {
        get
        {
            if(instance==null)
            {
                instance = new PoolManager();
            }
            return instance;
        }
    }

    private GameObject poolObj;

    /// <summary>
    /// Key��Ԥ���壬Value�Ǿ����object
    /// </summary>
    private Dictionary<GameObject, List<GameObject>> poolDataDic = new Dictionary<GameObject, List<GameObject>>();

    /// <summary>
    /// ��ȡ����
    /// </summary>
    /// <returns></returns>
    public GameObject GetObj(GameObject prefab)
    {
        GameObject obj = null;
        //�������������ֵ��������Ԥ������Դ
        //����������Դ����obj
        if (poolDataDic.ContainsKey(prefab) && poolDataDic[prefab].Count > 0) 
        {
            //����list�еĵ�һ��obj
            obj = poolDataDic[prefab][0];
            //�Ƴ�list�еĵ�һ��
            poolDataDic[prefab].RemoveAt(0);
        }
        //û��������Դ
        else
        {
            //ʵ����һ����Ȼ�󴫹�ȥ
            obj= GameObject.Instantiate(prefab);
        }
        //����ȥ֮ǰ��������ʾ
        obj.SetActive(true);
        //����û�и�����
        obj.transform.SetParent(null);
        return obj;
    }

    /// <summary>
    /// ������Ž������
    /// </summary>
    /// <param name="obj"></param>
    public void PushObj(GameObject prefab, GameObject obj)
    {
        //�ж���û�и�Ŀ¼
        if (poolObj == null)
        {
            poolObj = new GameObject("PoolObj");
        }

        //�ж��ֵ�����û��Ԥ����
        if(poolDataDic.ContainsKey(prefab))
        {
            //������Ž�ȥ
            poolDataDic[prefab].Add(obj);
        }
        //�ֵ���û��
        else
        {
            //�������Ԥ����Ļ����
            poolDataDic.Add(prefab, new List<GameObject>() { obj });
        }
        //�����Ŀ¼��û��Ԥ����������������
        if (poolObj.transform.Find(prefab.name) == null)
        {
            new GameObject(prefab.name).transform.SetParent(poolObj.transform);
        }

        //����
        obj.SetActive(false);
        //���ø�����
        obj.transform.SetParent(poolObj.transform.Find(prefab.name));
    }

    /// <summary>
    /// �����������
    /// </summary>
    public void Clear()
    {
        poolDataDic.Clear();
    }
}
