using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MessageKick:MessageBase
{
    public MessageKick()
    {
        protoName = "MessageKick";
    }

    /// <summary>
    /// -1默认值，0代表他人登录
    /// </summary>
    public int reason = -1; 



}
