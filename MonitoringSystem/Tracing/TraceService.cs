using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringSystem.Tracing
{
    public class TraceService
    {
        private static TraceService instance;
        private int TraceId { get; set; }
        private int SpanId { get; set; }
        public Stack<Tracer> ActiveTraces { get; set; }
        private TraceService()
        {
            TraceId = 1;
            SpanId = 1;
            ActiveTraces = new Stack<Tracer>();
        }

        public static TraceService GetInstance()
        {
            if (instance == null)
            {
                instance = new TraceService();
            }
            return instance;

        }

        public int GetSpanId() 
        {
            int spanId = SpanId;
            SpanId++;

            return spanId; 
        }

        public int GetTraceId() 
        { 
            return TraceId; 
        }

        public void IncrementTraceId()
        {
            TraceId++;
        }
    }
}
