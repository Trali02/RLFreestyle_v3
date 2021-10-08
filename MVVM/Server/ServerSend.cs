using RLFreestyle_v3.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RLFreestyle_v3.MVVM.Server
{
	class ServerSend
	{
		public static void SendTCPData(int _toClient,Packet _packet)
		{
			_packet.WriteLength();
			if(Server.clients[_toClient].connected) Server.clients[_toClient].tcp.SendData(_packet);
		}
		public static void SendTCPDataToAll(Packet _packet)
		{
			_packet.WriteLength();
			foreach (Client client in Server.clients) client.tcp.SendData(_packet);
		}
		public static void SendTCPDataToAll(int _exceptClient, Packet _packet)
		{
			if (_exceptClient < 0)
			{
				SendTCPDataToAll(_packet);
			} 
			else
			{
				_packet.WriteLength();
				for (int i = 0; i < Server.clients.Count; i++)
				{
					if (i != _exceptClient) Server.clients[i].tcp.SendData(_packet);
				}
			}
		}
		public static void Welcome(int _toClient, string _msg)
		{
			using (Packet _packet = new Packet((int)ServerPackets.welcome))
			{
				_packet.Write(_msg);
				_packet.Write(_toClient);

				SendTCPData(_toClient,_packet);
				if(_toClient != 0)
				{
					foreach(Player p in Player.Players)
					{
						Packet packet = new Packet((int)ServerPackets.addPlayer);
						packet.Write(p);
						SendTCPData(_toClient, packet);
					}
				}
			}
		}
	}
}
