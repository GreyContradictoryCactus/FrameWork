using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;


class MyTime
{
    public static float StringShortTimeToFloatTime(string stringTime)
    {
        string[] stringTimeList = stringTime.Split(':');
        float floatTime = float.Parse(stringTimeList[0]) * 60 + float.Parse(stringTimeList[1]);
        return floatTime;
    }

}

