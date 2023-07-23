using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{

    #region 初始化

    static Dictionary<string,BasePool> Pools = new Dictionary<string,BasePool>();

    [RuntimeInitializeOnLoadMethod]
    public static void Init()
    {
        LogManager.Info("[PoolManager]:Init", "FrameworkManagerInit");
    }
    #endregion

    #region 对象池相关
    public static void InitNewPool(string poolName,BasePool pool)
    {
        if(Pools.ContainsKey(poolName))
        {
            Debug.LogError("[PoolManager]:对象池重复！");
            return;
        }
        Pools[poolName] = pool;

    }
    public static int GetPoolObjectCount(string poolName)
    {
        return Pools[poolName].GetPoolObjectCount();
    }

    public static void PutInPool(string poolName,GameObject o)
    {
        Pools[poolName].PutIn(o);
    }

    public static GameObject FetchFromPool(string poolName)
    {
        return Pools[poolName].FetchOut();
    }

    public static void CreatObject(string poolName,int count)
    {
        Pools[poolName].CreatObject(count);
    }

    public static void DeleteObject(string poolName,int count)
    {
        Pools[poolName].DeleteObject(count);
    }

    public static void DeleteAllObject(string poolName)
    {
        Pools[poolName].DeleteAllObject();
    }

    public static void RemovePool(string poolName)
    {
        Pools[poolName].DeleteAllObject();
        Pools.Remove(poolName);
    }

    #endregion




}
