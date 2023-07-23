using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servers
{
    class Program
    {
        static void Main(string[] args)
        {

            if (!DatabaseManager.Connect("game", "127.0.0.1", 3306, "root", ""))
            {
                return;
            }

            NetManager.StartLoop("127.0.0.1", 8888);

        }
    }
}

#region 测试
/*
             
DatabaseManager.Register("m2", "123456");
DatabaseManager.CreatePlayer("m2");
PlayerDatabase playerDatabase = DatabaseManager.GetPlayerDatabase("m2");
playerDatabase.text =  "你好";
DatabaseManager.UpdatePlayerDatabase("m2",playerDatabase);

*/
#endregion