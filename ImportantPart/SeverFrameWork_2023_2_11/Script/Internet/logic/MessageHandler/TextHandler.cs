using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

partial class MessageHandler
{
    public static void OnMessageGetText(ClientState clientState,MessageBase messageBase)
    {
        MessageGetText messageGetText = (MessageGetText)messageBase;
        Player player = clientState.player;
        if(player == null)
        {
            messageGetText.result = MessageBase.State.fail;
            return;
        }
        messageGetText.text = player.playerDatabase.text;
        messageGetText.result = MessageBase.State.success;
        player.Send(messageGetText);
    }

    public static void OnMessageSaveText(ClientState clientState,MessageBase messageBase)
    {
        MessageSaveText messageSaveText = (MessageSaveText)messageBase;
        Player player = clientState.player;
        if (player == null)
        {
            messageSaveText.result = MessageBase.State.fail;
            return;
        }
        player.playerDatabase.text = messageSaveText.text;
        messageSaveText.result = MessageBase.State.success;
        player.Send(messageSaveText);
    }

}



