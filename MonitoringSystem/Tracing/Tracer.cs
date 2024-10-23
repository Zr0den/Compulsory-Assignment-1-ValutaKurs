using MonitoringSystem.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;


namespace MonitoringSystem.Tracing
{
    public class Tracer :IDisposable
    {
        private readonly Logger _logger;
        public readonly TraceService _traceService;
        private readonly string _methodName;
        private readonly int _traceId;
        private readonly int _spanId;
        private readonly int? _parentId;

        public bool IsRoot { get; set; }
        public int TraceId => _traceId;
        public int SpanId => _spanId;
        public int? ParentId => _parentId;

        //TraceService is a singleton service that keeps track of TraceId, SpanId and ongoing traces for ParentId
        //TraceId is only incremented whenever a root span is disposed
        //SpanId is incremented whenever a new trace is spun up
        //ParentId uses the TraceId of the top element of the ActiveTraces Stack.
        //This is probably not robust enough in a truly distributed, asynchronous system?
        public Tracer(Logger logger, string methodName)
        {
            _traceService = TraceService.GetInstance();
            _logger = logger;
            _methodName = methodName;

            _traceId = _traceService.GetTraceId();
            _spanId = _traceService.GetSpanId();

            IsRoot = _traceService.ActiveTraces.Count == 0;
            _traceService.ActiveTraces.TryPeek(out Tracer? parent);
            _parentId = parent?.SpanId;

            Start();
        }

        private void Start()
        {
            _logger.Log(LogLevel.Debug, $"TraceId: {_traceId}, SpanId: {_spanId}, ParentId: {_parentId}, Entering {_methodName}");
            _traceService.ActiveTraces.Push(this);
        }

        public void Dispose()
        {
            _logger.Log(LogLevel.Debug, $"TraceId: {_traceId}, SpanId: {_spanId}, ParentId: {_parentId}, Exiting {_methodName}");
            _traceService.ActiveTraces.Pop();
            if (IsRoot) 
            {
                _traceService.IncrementTraceId();
            }
            GC.SuppressFinalize(this);
        }
    }
}
