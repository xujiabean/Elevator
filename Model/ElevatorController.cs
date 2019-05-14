using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ElevatorsSystem.Model
{
	public class ElevatorController
	{
		private ElevatorsManager _manager;
		private List<Request> _requests;
        private Thread _thread;
		private static object _lock;
		public bool IsOverWeight { get; private set; }
		public Direction CurrentDirection { get; private set; }

		public Elevator Elev { get; private set; }
		public int Identifier { get; private set; }

		public ElevatorController(ElevatorsManager manager)
		{
			_manager = manager;
			_requests = new List<Request>();
            _thread = new Thread(Run);
			Elev = new Elevator();
			_lock = new object();
		}

		public ElevatorController(ElevatorsManager manager, int id) : this(manager)
		{
			Identifier = id;
		}

		public bool IsIdle()
		{
			return _requests.Count == 0;
		}
		

		public void ShutDownElevator() { }
		public void StartElevator() { }
		public void Reset() { }

		internal void ProcessRequest(Request arg)
		{
			if (arg != null)
			{
				lock (_lock)
				{
					if (CurrentDirection == Direction.Idle) CurrentDirection = arg.Direction;
					var anyDup = _requests.Where(a => a.Floor == arg.Floor && a.Direction == arg.Direction);
					if (anyDup != null && anyDup.Any()) return;
					_requests.Add(arg);
				}
			}
			//if (CurrentDirection == Direction.Up)
			//	OrderQueue(_requests.OrderBy(x => x.Floor));
			//else if (CurrentDirection == Direction.Down)
			//	OrderQueue(_requests.OrderByDescending(x => x.Floor));
			//else
			//{
			//	CurrentDirection = arg == null ? Direction.Up : arg.Direction;
			//}
		}

		private void OrderQueue(IOrderedEnumerable<Request> list)
		{
			List<Request> tempQueue = new List<Request>();
			foreach(var item in list)
			{
				tempQueue.Add(item);
			}
			_requests = tempQueue;
		}
        public void Start()
        {
            _thread.Start();
        }
        protected void Run()
        {
            Console.WriteLine("Run ElevatorController " + Identifier);
            while (true)
			{
				Console.WriteLine("ElevatorController " + Identifier + ", "+Elev.CurrentFloor +", "+ CurrentDirection);
				if (_requests.Count == 0)
				{
					CurrentDirection = Direction.Idle;
					Elev.CurrentFloor = 0;
					Thread.Sleep(10000);
				}
				else
				{
					lock (_lock)
					{
						var anyMatch = _requests.Where(a => a.Floor == Elev.CurrentFloor);
						if (anyMatch.Any())
						{
							for(int i=0;i<anyMatch.Count();i++)
							{
								Request item = anyMatch.ElementAt(i);
								_requests.Remove(item);
								item.CallBack();
								//Console.WriteLine(string.Format("ElevatorController {0} finished Request {1}, {2}, {3} ({4}).", Identifier, req.Floor, req.Direction, req.Time, req.Id));
							}
						}
					}

					if (Elev.CurrentFloor == _manager.TopFloor) CurrentDirection = Direction.Down;
					else if (Elev.CurrentFloor == 0) CurrentDirection = Direction.Up;

					if (CurrentDirection == Direction.Down) Elev.CurrentFloor -= 1;
					else if (CurrentDirection == Direction.Up) Elev.CurrentFloor += 1;
				}
				Thread.Sleep(1000);
                //Console.WriteLine("Run ElevatorController " + Identifier + " Resume");
            }
        }
	}
}
