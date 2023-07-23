using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;


class MyCode
{

    #region 中文/UFT-8
    public static string FromChineseToUFT8(string chineseString)
    {
        return HttpUtility.UrlEncode(chineseString, Encoding.UTF8);
    }


    public static string FromUFT8ToChinese(string uft8String)
    {
        return HttpUtility.UrlDecode(uft8String, Encoding.UTF8);
    }

    #endregion

    #region unicode转中文
    public static string FromUnicodeToChinese(string unicodeString)
    {

        return Regex.Unescape(unicodeString);

    }

    #endregion

    #region unicode转UFT-8
    public static string FromUnicodeToUFT8(string unicodeString)
    {

        return FromChineseToUFT8(FromUnicodeToChinese(unicodeString));

    }

    #endregion

    #region 双倍斜杠
    public static string DoubleCharacter_gang(string unicode)
    {
        string outStr = "";
        foreach(char s in unicode)
        {
            string sN = s.ToString();
            if( sN == "\\")
            {
                outStr += "\\\\"; 
            }
            else
            {
                outStr += sN;
            }
        }

        return outStr;
    }
    #endregion


}

