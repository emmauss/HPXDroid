using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace HappyPandaXDroid.Core
{
    public class RequestToken
    {
        public event EventHandler<EventArgs> FinishedCallback;

        public event EventHandler<EventArgs> FailedCallback;

        public Action<RequestToken> ProcessResult;

        public CancellationToken CancellationToken;

        public ManualResetEvent RequestResetEvent { get; private set; }

        public RequestMode Mode { get; private set; } = RequestMode.Callback;

        public object Result { get; private set; }

        public object Request { get; set; }

        public bool IsPaused { get; set; }

        public bool Failed { get; set; }

        public List<object> Args { get; set; }

        public RequestToken(CancellationToken cancellationToken)
        {
            CancellationToken = cancellationToken;
        }

        public RequestToken(ManualResetEvent resetEvent, CancellationToken cancellationToken) : this(cancellationToken)
        {
            RequestResetEvent = resetEvent;

            Mode = RequestMode.Locked;
        }

        public enum RequestMode
        {
            Locked,
            Callback
        }

        public void SetResult(object result)
        {
            Result = result;
        }

        public void OnFinished()
        {
            ProcessResult?.Invoke(this);

            if (CancellationToken.IsCancellationRequested)
                return;

            FinishedCallback?.Invoke(this, new ExtraEventArgs(Args));
        }

        public void OnFailed()
        {
            FailedCallback?.Invoke(this, null);
        }

        public void Queue()
        {
            Net.RequestHandler.Queue(this);

            Net.RequestHandler.Start();
        }

        public class ExtraEventArgs : EventArgs
        {
            public object[] ExtraArgs { get; set; }

            public ExtraEventArgs(params object[] Args)
            {
                ExtraArgs = Args;
            }
        }
    }
}