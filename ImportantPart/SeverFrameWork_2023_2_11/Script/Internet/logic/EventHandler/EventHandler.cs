using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


partial class EventHandler
{

    public static void OnClose(ClientState c)
    {
        Console.WriteLine("[服务器]：客户端" + c.socket.RemoteEndPoint.ToString() + "关闭");

        if(c.player != null)
        {
            DatabaseManager.UpdatePlayerDatabase(c.player.accountID, c.player.playerDatabase);
            PlayerManager.RemovePlayer(c.player.accountID);
        }
    }

    public static void OnTimer()
    {
        NetManager.CheakPing();
        NetManager.CheakSleep();
    }


}
