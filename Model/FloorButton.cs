using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

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
        public override void PlaceRequest(Request req)
		{
            _req = req;
		}
        protected override void Run()
        {
            //Console.WriteLine("Run FloorButton " + Identifier);
            while(true)
            {
				if (_req == null || _req.Time.Equals(default(DateTime)))
					continue;
				else
				{
					Console.WriteLine(string.Format("New request {0}, {1}, {2}", _req.Time, _req.Floor, _req.Direction));
					_manager.AllocateRequest(_req);
					ResetRequest();
				}
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
