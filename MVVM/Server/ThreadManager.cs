﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RLFreestyle_v3.MVVM.Server
{
	class ThreadManager
	{
		private static readonly List<Action> executeOnMainThread = new List<Action>();
		private static readonly List<Action> executeCopiedOnMainThread = new List<Action>();
		private static bool actionToExecuteOnMainThread = false;

		

		/// <summary>Sets an action to be executed on the main thread.</summary>
		/// <param name="_action">The action to be executed on the main thread.</param>
		public static void ExecuteOnMainThread(Action _action)
		{
			if (_action == null)
			{
				Console.WriteLine("No action to execute on main thread!");
				return;
			}

			lock (executeOnMainThread)
			{
				executeOnMainThread.Add(_action);
				actionToExecuteOnMainThread = true;
			}
			UpdateMain();
		}
		public static void UpdateMain()
		{
			if (actionToExecuteOnMainThread)
			{
				executeCopiedOnMainThread.Clear();
				lock (executeOnMainThread)
				{
					executeCopiedOnMainThread.AddRange(executeOnMainThread);
					executeOnMainThread.Clear();
					actionToExecuteOnMainThread = false;
				}

				for (int i = 0; i < executeCopiedOnMainThread.Count; i++)
				{
					executeCopiedOnMainThread[i]();
				}
			}
		}

	}
}
