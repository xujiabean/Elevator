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
		private Queue<Request> _requests;
        private Thread _thread;
		public bool IsOverWeight { get; private set; }
		public Direction CurrentDirection { get; private set; }

		public Elevator Elev { get; private set; }
		public int Identifier { get; private set; }

		public ElevatorController(ElevatorsManager manager)
		{
			_manager = manager;
			_requests = new Queue<Request>();
            _thread = new Thread(Run);
			Elev = new Elevator();
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
			if(arg!=null)
			{
				var anyDup = _requests.Where(a => a.Floor == arg.Floor && a.Direction == arg.Direction);
				if (anyDup != null && anyDup.Any()) return;
				_requests.Enqueue(arg);
			}
			if (CurrentDirection == Direction.Up)
				_requests.OrderBy(x => x.Floor);
			else if (CurrentDirection == Direction.Down)
				_requests.OrderByDescending(x => x.Floor);
			else
			{
				CurrentDirection = arg == null ? Direction.Up : arg.Direction;
			}
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
				if (_requests.Count() > 0)
				{
					if (Elev.CurrentFloor == _manager.TopFloor) CurrentDirection = Direction.Down;
					else if (Elev.CurrentFloor == 1) CurrentDirection = Direction.Up;
					ProcessRequest(null);
					var req = _requests.Dequeue();
					Console.WriteLine(string.Format("ElevatorController {0} finished Request {1}, {2}, {3} ", Identifier, req.Floor, req.Direction, req.Time));
				}
				Thread.Sleep(10000);
                //Console.WriteLine("Run ElevatorController " + Identifier + " Resume");
            }
        }
	}
}
