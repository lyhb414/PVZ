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
    /// Key是预制体，Value是具体的object
    /// </summary>
    private Dictionary<GameObject, List<GameObject>> poolDataDic = new Dictionary<GameObject, List<GameObject>>();

    /// <summary>
    /// 获取物体
    /// </summary>
    /// <returns></returns>
    public GameObject GetObj(GameObject prefab)
    {
        GameObject obj = null;
        //如果缓存池数据字典中有这个预制体资源
        //并且这种资源还有obj
        if (poolDataDic.ContainsKey(prefab) && poolDataDic[prefab].Count > 0) 
        {
            //返回list中的第一个obj
            obj = poolDataDic[prefab][0];
            //移除list中的第一个
            poolDataDic[prefab].RemoveAt(0);
        }
        //没有这种资源
        else
        {
            //实例化一个，然后传过去
            obj= GameObject.Instantiate(prefab);
        }
        //传出去之前，让他显示
        obj.SetActive(true);
        //让其没有父物体
        obj.transform.SetParent(null);
        return obj;
    }

    /// <summary>
    /// 把物体放进缓存池
    /// </summary>
    /// <param name="obj"></param>
    public void PushObj(GameObject prefab, GameObject obj)
    {
        //判断有没有根目录
        if (poolObj == null)
        {
            poolObj = new GameObject("PoolObj");
        }

        //判断字典中有没有预制体
        if(poolDataDic.ContainsKey(prefab))
        {
            //把物体放进去
            poolDataDic[prefab].Add(obj);
        }
        //字典中没有
        else
        {
            //创建这个预制体的缓存池
            poolDataDic.Add(prefab, new List<GameObject>() { obj });
        }
        //如果根目录下没有预制体命名的子物体
        if (poolObj.transform.Find(prefab.name) == null)
        {
            new GameObject(prefab.name).transform.SetParent(poolObj.transform);
        }

        //隐藏
        obj.SetActive(false);
        //设置父物体
        obj.transform.SetParent(poolObj.transform.Find(prefab.name));
    }

    /// <summary>
    /// 清空所有数据
    /// </summary>
    public void Clear()
    {
        poolDataDic.Clear();
    }
}
