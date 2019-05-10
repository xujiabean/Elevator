using ElevatorsSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElevatorsSystem
{
	public class Simulation
	{
		private int _floor;
		private Thread _thread;
		private Random _rd;
		private ElevatorsManager _manager;
		public Simulation(ElevatorsManager manager, int floor)
		{
			_floor = floor;
			_thread = new Thread(Run);
			_rd = new Random();
			_manager = manager;
		}
		public void Start()
		{
			_thread.Start();
		}
		private void Run()
		{
			Console.WriteLine("Simulate Floor Button Behavior");
			while (true)
			{
				Request request = new Request();
				int idx = _rd.Next(0, _floor);
				request.Floor = idx;
				if (idx == 0) request.Direction = Direction.Up;
				else if (idx == _floor) request.Direction = Direction.Down;
				else
				{
					int dir = _rd.Next(0, 10);
					request.Direction = dir < 5 ? Direction.Down : Direction.Up;
				}
				request.Time = DateTime.Now;
				_manager.AddRequest(request);
				//Console.WriteLine(string.Format("Add request {0}, {1}, {2}", request.Time, request.Floor, request.Direction));
				int min = _rd.Next(1, 100);
				Thread.Sleep(min*100);
			}
		}
	}
}
