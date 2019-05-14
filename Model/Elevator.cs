using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorsSystem.Model
{
	public class Elevator
	{
		public int CurrentFloor;
		public int MaxWeight;
		public void MoveUp() { }
		public void MoveDown() { }
		public void OpenDoor() { }
		public void CloseDoor() { }

		public Elevator()
		{
			CurrentFloor = 0;
		}
	}
}
