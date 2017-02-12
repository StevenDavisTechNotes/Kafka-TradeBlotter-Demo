using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Linq;
using System.Threading;
using KafktaListener;
using KafktaListener.Repositories;
using Newtonsoft.Json;
using TickingViewSvc_Net.Models;

namespace TickingViewSvc_Net.Services
{
    public interface IViewEngine
    {
        Exposure[] GetPositionView();
    }

    public class ViewEngine : IViewEngine
    {
        public ViewEngine(CancellationToken cancellationToken)
        {
            exposures = new List<Exposure>();

            var exposuresListener = new KafkaSpout("Exposures", cancellationToken);
            exposuresListener.WhenMessageReceived
                .Select(message => JsonConvert.DeserializeObject<Exposure[]>(message.Text))
                .Subscribe(exposures =>
                    {
                        lock (this.exposures)
                        {
                            this.exposures.Clear();
                            this.exposures.AddRange(exposures);
                        }
                    },
                    ex => Console.WriteLine($"Got Exception {ex.Message}"),
                    cancellationToken);
        }

        private List<Exposure> exposures;

        public Exposure[] GetPositionView()
        {
            lock (exposures)
            {
                return exposures.ToArray();
            }
        }

    }
}
