using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using RLFreestyle_v3.MVVM.Model;

namespace RLFreestyle_v3.MVVM.Client
{
	class Client
	{
		public static Client instance;

		public static int dataBufferSize = 4096;

		public static string ip = "127.0.0.1";
		public static int port = 13000;
		public int myIndex = -1;
		public TCP tcp;

		private delegate void PacketHandler(Packet _packet);
		private static Dictionary<int, PacketHandler> packetHandlers;

		public Client()
		{
			instance = this;
			tcp = new TCP();
		}
		public void ConnectToServer()
		{
			InitClientData();

			tcp.Connect();
		}

		public class TCP
		{
			public TcpClient socket;

			private NetworkStream stream;
			private Packet receivedData;
			private byte[] receiveBuffer;

			public void Connect()
			{
				socket = new TcpClient
				{
					ReceiveBufferSize = dataBufferSize,
					SendBufferSize = dataBufferSize
				};

				receiveBuffer = new byte[dataBufferSize];
				socket.BeginConnect(ip,port,ConnectCallback,socket);
			}
			private void ConnectCallback(IAsyncResult _AR)
			{
				socket.EndConnect(_AR);
				if (!socket.Connected) return;
				stream = socket.GetStream();

				receivedData = new Packet();

				stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
			}
			private void ReceiveCallback(IAsyncResult _AR)
			{
				try
				{
					int _byteLength = stream.EndRead(_AR);
					if (_byteLength <= 0) return;

					byte[] _data = new byte[_byteLength];
					Array.Copy(receiveBuffer, _data, _byteLength);

					receivedData.Reset(HandleData(_data));
					stream.BeginRead(receiveBuffer, 0, dataBufferSize, new AsyncCallback(ReceiveCallback), null);
				}
				catch (Exception e)
				{
					Console.WriteLine($"Error: {e}");
					Disconnect();
				}
			}
			public void SendData(Packet _packet)
			{
				try
				{
					if (socket != null)
					{
						stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
					}

				}
				catch(Exception e)
				{
					Console.WriteLine($"Error sending data: {e}");
				}
			}
			private bool HandleData(byte[] _data)
			{
				int _packetLength = 0;
				receivedData.SetBytes(_data);
				if(receivedData.UnreadLength() >= 4)
				{
					_packetLength = receivedData.ReadInt();
					if (_packetLength <= 0) return true;
				}
				while(_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
				{
					byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
					ThreadManager.ExecuteOnMainThread(() =>
					{
						using (Packet _packet = new Packet(_packetBytes))
						{
							int _packetId = _packet.ReadInt();
							packetHandlers[_packetId](_packet);
						}
					});
					
					_packetLength = 0;
					if(receivedData.UnreadLength() >= 4)
					{
						_packetLength = receivedData.ReadInt();
						if (_packetLength <= 0) return true;
					}
				}

				return _packetLength <= 1;
			}

			public void Disconnect()
			{
				socket.Close();
			}
		}
		public void Disconnect()
		{
			tcp.Disconnect();
		}
		private void InitClientData()
		{
			packetHandlers = new Dictionary<int, PacketHandler>()
			{
				{(int)ServerPackets.welcome, ClientHandle.Welcome },
				{(int)ServerPackets.getPoints, ClientHandle.GetPoints },
				{(int)ServerPackets.addPlayer, ClientHandle.AddPlayer },
				{(int)ServerPackets.updatePlayer, ClientHandle.UpdatePlayer },
				{(int)ServerPackets.removePlayer, ClientHandle.RemovePlayer },
				{(int)ServerPackets.removeShot, ClientHandle.RemoveShot },
				{(int)ServerPackets.addShot, ClientHandle.AddShot },
				{(int)ServerPackets.setXml, ClientHandle.SetXml },
				{(int)ServerPackets.resetPlayer, ClientHandle.ResetPlayer },
				{(int)ServerPackets.setPlayerColor, ClientHandle.SetPlayerColor },
				{(int)ServerPackets.setLua, ClientHandle.SetLua },
				{(int)ServerPackets.saveGame, ClientHandle.SaveGame }
			};
			Console.WriteLine("Initialized packets...");
		}
	}
	
}
