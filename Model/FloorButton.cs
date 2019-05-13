using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElevatorsSystem.Model
{
	public class FloorButton: Button
	{
        private ElevatorsManager _manager;
        private Request _req = new Request();
        public Direction Direction;
		public int Identifier { get; private set; }
		public FloorButton(ElevatorsManager manager, int id)
        {
            _manager = manager;
			Identifier = id;
		}
        public override void PlaceRequest(Request newreq)
		{
			if (_req == null || _req.Time.Equals(default(DateTime)))
			{
				_req = newreq;
				WaitRequestToBeFinished();
			}
			else Console.WriteLine(string.Format("Duplicate Request. Cur: {0}, {1}, ({4}). New: {2}, {3}, ({5}).", _req.Direction, _req.Floor, newreq.Direction, newreq.Floor, _req.id, newreq.id));
		}

		private void WaitRequestToBeFinished()
		{
			_manager.AllocateRequest(_req);
			var waitTask = Task.Run(() =>
				{
					while (!_req.IsDone) { }
					Console.WriteLine(string.Format("Finish request {0}, {1}, {2} ({3})", _req.Floor, _req.Direction, _req.Time, _req.id));
					ResetRequest();
				});
		}
		protected override void Run()
        {
            //Console.WriteLine("Run FloorButton " + Identifier);
            while(true)
            {
				//if (_req == null || _req.Time.Equals(default(DateTime)))
				//	continue;
				//else if(_req.IsDone)
				//{
				//	Console.WriteLine(string.Format("Finish request {0}, {1}, {2}", _req.Time, _req.Floor, _req.Direction));
				//	ResetRequest();
				//}
				//else
				//{
				//	Console.WriteLine(string.Format("New request {0}, {1}, {2}", _req.Time, _req.Floor, _req.Direction));
				//	 _manager.AllocateRequest(_req);
				//}
                //Thread.Sleep(20000);
                //Console.WriteLine($"Run FloorButton {Identifier} Resume");
            }
        }
		private void ResetRequest()
		{
			_req = new Request();
		}
	}
}
