using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorsSystem.Model
{
	public class Request
	{
		public Direction Direction;
		public int Floor;
		public DateTime Time;
		//public bool IsDone;
		public long Id;
		public RequestCallBack CallBack;
	}
}
