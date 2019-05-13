using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElevatorsSystem.Model
{
	public class ProduceConsume
	{
		private static ProduceConsume _produceConsume;
		private Func<Request, bool> _callBackManager;
		private ConcurrentQueue<Request> _requests;
		private Request _preRequest;
		private Thread _thread;
		private ProduceConsume(Func<Request, bool> callBackManager)
		{
			_requests = new ConcurrentQueue<Request>();
			_callBackManager = callBackManager;
			_thread = new Thread(ConsumeRequest);
			_thread.Start();
		}

		public static ProduceConsume GetInstance(Func<Request, bool> callBackManager)
		{
			if (_produceConsume == null)
			{
				_produceConsume = new ProduceConsume(callBackManager);
			}
			return _produceConsume;
		}
		
		public void AddRequest(Request req)
		{
			if(!CanProcessed(req))
			{
				_requests.Enqueue(req);
				Console.WriteLine(string.Format("Waiting list: {0}, {1}, {2} ({3}).", req.Time, req.Floor, req.Direction, req.id));
			}
				
		}

		private void ConsumeRequest()
		{
			while (true)
			{
				while (_requests.Count > 0)
				{
					Request topReq;
					_requests.TryPeek(out topReq);
					if (CanProcessed(topReq))
					{
						_requests.TryDequeue(out topReq);
						Console.WriteLine(string.Format("Depart Waiting list: {0}, {1}, {2} ({3}).", topReq.Time, topReq.Floor, topReq.Direction, topReq.id));
					}
					Thread.Sleep(1000);
				}
				Thread.Sleep(10000);
			}
		}
		private bool CanProcessed(Request req)
		{
			return _callBackManager(req);
		}
	}
}
