using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElevatorsSystem.Model
{
	public abstract class Button
	{
       // private Thread _thread;
        public int Indicator;
        public Button()
        {
           // _thread = new Thread(() => Run());
        }
		public virtual void PlaceRequest(Request req) { }
		public void Illuminate() { }
		public void DisIlluminate() { }
        public void Start()
        {
            //_thread.Start();
        }
        protected abstract void Run();
    }
}
