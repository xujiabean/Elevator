using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorsSystem.Model;

namespace ElevatorsSystem
{
	class Program
	{
		static void Main(string[] args)
		{
			int nElevators = int.Parse(args[1]);
			int floors = int.Parse(args[3]);

			ElevatorsManager manager = ElevatorsManager.GetInstance();
			manager.InitializeElevators(nElevators, floors);

			Simulation simulation = new Simulation(manager, floors);
			simulation.Start();
		}
	}
}
