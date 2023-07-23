using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MessageLogin:MessageBase
{
    public MessageLogin()
    {
        protoName = "MessageLogin";
    }

    public string accountID = "";
    public string password = "";


}
