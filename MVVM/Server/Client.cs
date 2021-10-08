using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using RLFreestyle_v3.MVVM.Model;

namespace RLFreestyle_v3.MVVM.Server
{
	class Client
	{
		public static int dataBufferSize = 4096;
		public bool connected = false;
		public TCP tcp;
		public int id;

		public Client(int id)
		{
			this.id = id;
			tcp = new TCP(id);
		}

		public class TCP
		{
			public TcpClient socket;
			private NetworkStream stream;
			private Packet receivedData;
			private byte[] receiveBuffer;
			private readonly int id;

			public TCP(int id)
			{
				this.id = id;
			}

			public void Connect(TcpClient _socket)
			{
				socket = _socket;
				socket.ReceiveBufferSize = dataBufferSize;
				socket.SendBufferSize = dataBufferSize;

				stream = socket.GetStream();
				receivedData = new Packet();
				receiveBuffer = new byte[dataBufferSize];

				stream.BeginRead(receiveBuffer, 0, dataBufferSize, new AsyncCallback(ReceiveCallback), null);
			}

			public void SendData(Packet _packet)
			{
				try
				{
					if (socket != null) stream.BeginWrite(_packet.ToArray(),0,_packet.Length(),null,null);
				}
				catch(Exception e)
				{
					Console.WriteLine($"Error sending data: {e}");
				}
			}


			private void ReceiveCallback(IAsyncResult _AR)
			{
				try
				{
					int _byteLength = stream.EndRead(_AR);
					if (_byteLength <= 0) {
						Server.clients[id].Disconnect();
						return;
					}
					

					byte[] _data = new byte[_byteLength];
					Array.Copy(receiveBuffer, _data, _byteLength);

					receivedData.Reset(HandleData(_data));
					stream.BeginRead(receiveBuffer, 0, dataBufferSize, new AsyncCallback(ReceiveCallback), null);


				}
				catch(Exception e)
				{
					Console.WriteLine($"Error: {e}");
					Server.clients[id].Disconnect();
				}
			}
			private bool HandleData(byte[] _data)
			{
				int _packetLength = 0;
				receivedData.SetBytes(_data);
				if (receivedData.UnreadLength() >= 4)
				{
					_packetLength = receivedData.ReadInt();
					if (_packetLength <= 0) return true;
				}
				while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
				{
					byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
					ThreadManager.ExecuteOnMainThread(() =>
					{
						using (Packet _packet = new Packet(_packetBytes))
						{
							int _packetId = _packet.ReadInt();
							Server.packetHandlers[_packetId](id,_packet);
						}
					});
					
					_packetLength = 0;
					if (receivedData.UnreadLength() >= 4)
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
				stream = null;
				receivedData = null;
				receiveBuffer = null;
				socket = null;
			}
		}

		public void Disconnect()
		{
			try
			{
				Console.WriteLine($"{tcp.socket.Client.RemoteEndPoint} has disconnected {id}");
			}
			catch
			{

			}
			connected = false;
			tcp.Disconnect();
		}
	}
}
