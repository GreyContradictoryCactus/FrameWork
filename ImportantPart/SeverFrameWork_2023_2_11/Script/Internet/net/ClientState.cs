using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
public class ClientState
{
    public Socket socket;
    public ByteArray readBuffer = new ByteArray();

    public long lastPingTime = 0;

    public Player player;

}
