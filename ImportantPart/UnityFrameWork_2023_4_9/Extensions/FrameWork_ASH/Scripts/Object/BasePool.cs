using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePool 
{
    protected GameObject poolObject;

    protected int MaxObjectCount = 99;

    protected Queue<GameObject> poolObjects = new Queue<GameObject>();

    public BasePool(GameObject o) 
    {
        OnPutIn(o);
        poolObject = o;
    }

    /// <summary>
    /// ��д��ȡ��ʱ����
    /// </summary>
    protected virtual void OnFetchOut(GameObject poolObject)
    {

    }

    /// <summary>
    /// ��д������ʱ����
    /// </summary>
    protected virtual void OnPutIn(GameObject poolObject)
    {

    }

    public void PutIn(GameObject poolObject)
    {
        //������ӵ������ޣ��ݻٶ�������
        if(poolObjects.Count > MaxObjectCount)
        {
            Object.Destroy(poolObject);
            return;
        }

        OnPutIn(poolObject);
        poolObjects.Enqueue(poolObject);
        return;
    }

    public GameObject FetchOut()
    {
        GameObject o;
        LogManager.Test(poolObjects.Count.ToString());
        if (poolObjects.Count <= 0)
        {
            o = Object.Instantiate(poolObject);
            OnFetchOut(o);
            return o;
        }
        o = poolObjects.Dequeue();
        OnFetchOut(o);
        return o;
    }

    public void CreatObject(int count)
    {
        int MaxCount = MaxObjectCount - poolObjects.Count;
        for(int i = 0; (i < count)&&(i < MaxCount); i++)
        {
            poolObjects.Enqueue(Object.Instantiate(poolObject));
        }
        return;
    }

    public void DeleteObject(int count)
    {
        if(count >= poolObjects.Count )
        {
            DeleteAllObject();
            return;
        }

        for(int i = 0; i < count; i++)
        {
            Object.Destroy(poolObjects.Dequeue());
        }
        return;
    }

    public void DeleteAllObject()
    {
        int count = poolObjects.Count;
        for (int i = 0; i < count ; i++)
        {
            Object.Destroy(poolObjects.Dequeue());
        }
        return;
    }


    public int GetPoolObjectCount()
    {
        return poolObjects.Count;
    }


}
