using RLFreestyle_v3.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RLFreestyle_v3.MVVM.Client
{
	class ClientSend
	{
		public static void SendTCPData(Packet _packet)
		{
			_packet.WriteLength();
			Client.instance.tcp.SendData(_packet);
		}
		#region Packets
		public static void WelcomeReceived()
		{
			using(Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
			{
				_packet.Write(Client.instance.myIndex);
				SendTCPData(_packet);
			}
		}
		#endregion
	}
}
