using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class MyMath
{
    #region 字段
    int _counterFrom_0 = 0;

    #endregion

    #region 属性
    /// <summary>
    /// 每调用一次，自加一并返回
    /// </summary>
    public int CounterFrom_0
    {
        get
        {
            _counterFrom_0 ++;
            return _counterFrom_0;
        }
    }

    #endregion

    #region 方法
    /// <summary>
    /// 生成随机整数,左闭右开（短时间内会一直生成相同的值）
    /// </summary>
    public static int RandomIntNumber(int min , int max)
    {
        return new Random().Next(min, max);
    }




    #endregion









}

