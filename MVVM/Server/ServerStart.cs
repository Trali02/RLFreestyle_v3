using RLFreestyle_v3.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RLFreestyle_v3.MVVM.Server
{
	class ServerStart
	{
		public static bool isRunning = false;
		public static void Start()
		{
			isRunning = true;

			Thread mainThread = new Thread(new ThreadStart(MainThread));
			mainThread.Start();

			Server.Start(13000);
		}
		public static void Stop()
		{
			isRunning = false;
			Server.Stop();
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
