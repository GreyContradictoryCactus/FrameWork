using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 玩家
/// </summary>
public class Player
{
    public string name = "";
    public string accountID = "";

    public ClientState clientState;

    public PlayerEphemeral playerEphemeral;
    public PlayerDatabase playerDatabase;



    public Player(ClientState clientState)
    {
        this.clientState = clientState;
    }

    public void Send(MessageBase messageBase)
    {
        NetManager.Send(clientState, messageBase);
    }

}

