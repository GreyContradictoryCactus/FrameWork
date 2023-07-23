using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



class PlayerManager
{
    /// <summary>
    /// 玩家在线列表
    /// </summary>
    static Dictionary<string,Player> players = new Dictionary<string,Player>();

    /// <summary>
    /// 玩家是否在线
    /// </summary>
    public static bool IsOnline(string accountID)
    {
        return players.ContainsKey(accountID);
    }

    /// <summary>
    /// 通过账户ID获取玩家
    /// </summary>
    public static Player GetPlayer(string accountID)
    {
        if(IsOnline(accountID))
        {
            return players[accountID];
        }
        return null;
    }

    public static void AddPlayer(string accountID,Player player)
    {
        players[accountID] = player;
    }

    public static void RemovePlayer(string accountID)
    {
        players.Remove(accountID);
    }




}