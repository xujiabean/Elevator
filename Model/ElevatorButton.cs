﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorsSystem.Model
{
	public class ElevatorButton: Button
	{
		public Direction Direction;
		public override void PlaceRequest(Request req)
		{
			base.PlaceRequest(req);
		}
        protected override void Run()
        {
            
        }
	}
}
