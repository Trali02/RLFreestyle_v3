using RLFreestyle_v3.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RLFreestyle_v3.MVVM.Client
{
	class ClientStart
	{
		public static bool isRunning = false;
		public static Client client;

		/// <summary>
		/// connects the client with the server
		/// </summary>
		/// <param name="ip">ip of the server</param>
		/// <param name="port">port of the server</param>
		public static void Start(string ip,int port=13000)
		{
			Client.ip = ip;
			Client.port = port;

			client = new Client();

			client.ConnectToServer();

			Thread mainThread = new Thread(new ThreadStart(MainThread));
			mainThread.Start();

			isRunning = true;
		}
		public static void Stop()
		{
			client.Disconnect();
			isRunning = false;
		}

		private static void MainThread()
		{
			Console.WriteLine($"Main thread started. Running at {Constants.TPS} tps.");
			DateTime _nextLoop = DateTime.Now;

			while (isRunning)
			{
				while (_nextLoop < DateTime.Now)
				{
					ThreadManager.UpdateMain();

					_nextLoop = _nextLoop.AddMilliseconds(Constants.MS_PER_TICK);

					if (_nextLoop > DateTime.Now)
					{
						Thread.Sleep(_nextLoop - DateTime.Now);
					}
				}
			}
		}
	}
}
