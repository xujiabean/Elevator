using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorsSystem.Model
{
    public class ElevatorsManager : IManager
	{
		private static ElevatorsManager _manager;
		private ProduceConsume _pc;

		private ElevatorsManager()
		{
			_pc = ProduceConsume.GetInstance(CanProcess);
		}

		public static ElevatorsManager GetInstance()
		{
			if (_manager == null)
			{
				_manager = new ElevatorsManager();

			}
			return _manager;
		}

		private IList<ElevatorController> _controllers;
        private IList<FloorButton> _floorButtons;
		public void InitializeElevators(int nElevators, int floors)
		{
			_controllers = new List<ElevatorController>();
			for (int i = 0; i < nElevators; i++)
			{
                var controller = new ElevatorController(_manager, i);
                controller.Start();
                _controllers.Add(controller);
			}
            _floorButtons = new List<FloorButton>();
            for (int i = 0; i < floors; i++)
            {
                var floor = new FloorButton(_manager, i);
				floor.Direction = Direction.Up;
				floor.Start();
                _floorButtons.Add(floor);
				floor = new FloorButton(_manager, i);
				floor.Direction = Direction.Down;
				floor.Start();
				_floorButtons.Add(floor);
			}
		}

		public int TopFloor => _floorButtons.Count;

		private bool CanProcess(Request arg)
		{
			int floor = arg.Floor;

			var idlers = _controllers.Where(x => x.IsIdle()).ToList();
			if (idlers.Count() > 1)
			{
				int index = GetShortestDistanceIndex(floor, idlers);
				Console.WriteLine(string.Format("Assign elevator {0}, Request {1}", index, arg.id));
				idlers[index].ProcessRequest(arg);
			}
			else if (idlers.Count() == 1)
			{
				Console.WriteLine(string.Format("Assign elevator {0}, Request {1}", idlers[0].Identifier, arg.id));
				idlers[0].ProcessRequest(arg);
			}
			else
			{
				if(arg.Direction == Direction.Up)
                    idlers = _controllers.Where(x => x.CurrentDirection == arg.Direction && !(x.IsOverWeight) && arg.Floor >= x.Elev.CurrentFloor).ToList();
                else if (arg.Direction == Direction.Down)
                    idlers = _controllers.Where(x => x.CurrentDirection == arg.Direction && !(x.IsOverWeight) && arg.Floor <= x.Elev.CurrentFloor).ToList();
				if (idlers.Any())
				{
					int index = GetShortestDistanceIndex(floor, idlers);
					Console.WriteLine(string.Format("Assign elevator {0}, Request {1}", index, arg.id));
					idlers[index].ProcessRequest(arg);
				}
				else
				{
					return false;
				}
			}
			return true;
		}

		private int GetShortestDistanceIndex(int floor, List<ElevatorController> idlers)
		{
			int min = int.MaxValue;
			int index = -1;
			for (int i = 0; i < idlers.Count(); i++)
			{
				if (Math.Abs(idlers[i].Elev.CurrentFloor - floor) < min)
					index = i;
			}
			return index;
		}

		public void AllocateRequest(Request request) 
        {
            _pc.AddRequest(request);
        }

        public void AddRequest(Request request)
		{
			var floorBotton = _floorButtons.First(a => a.Identifier == request.Floor && a.Direction == request.Direction);
			floorBotton.PlaceRequest(request);
		}
        public void RemoveRequest(Request request) { }
		public void EnableStrategy() { }

		public void Update(string msg)
		{
			throw new NotImplementedException();
		}
	}
}
