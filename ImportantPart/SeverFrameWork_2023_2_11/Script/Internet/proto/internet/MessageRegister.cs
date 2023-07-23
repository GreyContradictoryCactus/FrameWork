using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[System.Serializable]
public class MessageRegister:MessageBase
{
    public MessageRegister()
    {
        protoName = "MessageRegister";
    }

    public string accountID = "";
    public string password = "";


}
