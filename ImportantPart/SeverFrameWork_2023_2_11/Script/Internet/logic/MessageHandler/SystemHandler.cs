using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

partial class MessageHandler
{
    public static void OnMessagePING(ClientState c ,MessageBase messageBase)
    {
        // Console.WriteLine("[服务器]：接收PING,来自"+c.socket.RemoteEndPoint.ToString());
        c.lastPingTime = NetManager.GetTimeStamp();
        MessagePONG messagePONG = new MessagePONG();
        NetManager.Send(c, messagePONG);

    }

    public static void OnMessageRegister(ClientState clientState , MessageBase messageBase)
    {
        MessageRegister messageRegister = (MessageRegister)messageBase;

        if(DatabaseManager.Register(messageRegister.accountID,messageRegister.password))
        {
            DatabaseManager.CreatePlayer(messageRegister.accountID);
            messageRegister.result = MessageBase.State.success;
        }
        else
        {
            messageRegister.result = MessageBase.State.fail;
        }
        NetManager.Send(clientState , messageRegister);
     

    }

    public static void OnMessageLogin(ClientState clientState,MessageBase messageBase)
    {
        MessageLogin messageLogin = (MessageLogin)messageBase;

        if(!DatabaseManager.CheckPassword(messageLogin.accountID,messageLogin.password))
        {
            messageLogin.result = MessageBase.State.fail;
            NetManager.Send(clientState , messageLogin);
            return;
        }

        if(clientState.player != null)
        {
            messageLogin.result = MessageBase.State.fail;
            NetManager.Send(clientState,messageLogin);
            return;
        }

        if(PlayerManager.IsOnline(messageLogin.accountID))
        {
            Player other = PlayerManager.GetPlayer(messageLogin.accountID);
            MessageKick messageKick = new MessageKick();
            messageKick.reason = 0;
            other.Send(messageKick);
            NetManager.Close(other.clientState);
        }

        PlayerDatabase playerDatabase = DatabaseManager.GetPlayerDatabase(messageLogin.accountID);
        if(playerDatabase == null)
        {
            messageLogin.result = MessageBase.State.fail;
            NetManager.Send(clientState, messageLogin);
            return;
        }

        Player player = new Player(clientState);
        player.accountID = messageLogin.accountID;
        player.playerDatabase = playerDatabase;
        PlayerManager.AddPlayer(messageLogin.accountID, player);
        clientState.player = player;

        messageLogin.result = MessageBase.State.success;
        player.Send(messageLogin);
    }

    public static void OnMessageConstraintSleep(ClientState clientState, MessageBase messageBase)
    {
        string password = "Sleep123456";
        MessageConstraintSleep messageConstraintSleep = (MessageConstraintSleep)messageBase;
        if(messageConstraintSleep.password == password)
        {
            messageConstraintSleep.result = MessageBase.State.success;
            NetManager.Send(clientState, messageConstraintSleep);
            NetManager.Sleep();
        }
        else
        {
            messageConstraintSleep.result = MessageBase.State.fail;
            NetManager.Send(clientState, messageConstraintSleep);
        }

    }

}



