using System;
using System.Activities;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace Extent.ExtentHtmlReporter.Activities
{
    /// <summary>
    /// Extent.ExtentHtmlReporter AsyncTaskCodeActivity with result <T>
    /// </summary>
    public abstract class AsyncTaskCodeActivity<T> : AsyncCodeActivity, IDisposable
    {
        public AsyncTaskCodeActivity()
        {

        }
        protected CancellationTokenSource _cancellationTokenSource;
        protected bool _tokenDisposed = false;

        protected override void Cancel(AsyncCodeActivityContext context)
        {
            if (!_tokenDisposed)
            {
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource?.Dispose();

                _tokenDisposed = true;
            }

            base.Cancel(context);
        }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
        }

        protected override IAsyncResult BeginExecute(AsyncCodeActivityContext context, AsyncCallback callback, object state)
        {
            if (!_tokenDisposed)
            {
                _cancellationTokenSource?.Dispose();
            }

            _cancellationTokenSource = new CancellationTokenSource();
            _tokenDisposed = false;

            var property = context.DataContext.GetProperties()[TestcaseScope.TestcaseContainerPropertyTag];
            var servicesTestcase = property.GetValue(context.DataContext) as Testcase;

            TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>(state);
            Task<T> task = ExecuteAsync(context, _cancellationTokenSource.Token, servicesTestcase);

            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    taskCompletionSource.TrySetException(t.Exception.InnerException);
                }
                else if (t.IsCanceled || _cancellationTokenSource.IsCancellationRequested)
                {
                    taskCompletionSource.TrySetCanceled();
                }
                else
                {
                    taskCompletionSource.TrySetResult(t.Result);
                }

                callback?.Invoke(taskCompletionSource.Task);
            });

            return taskCompletionSource.Task;
        }

        protected override void EndExecute(AsyncCodeActivityContext context, IAsyncResult result)
        {
            Task<T> task = (Task<T>)result;

            if (task.IsFaulted)
            {
                ExceptionDispatchInfo.Capture(task.Exception.InnerException).Throw();
            }
            if (task.IsCanceled)
            {
                context.MarkCanceled();
            }

            if (!_tokenDisposed)
            {
                _cancellationTokenSource?.Dispose();

                _tokenDisposed = true;
            }


        }

        protected abstract Task<T> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken, Testcase client);

        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (!_tokenDisposed)
                    {
                        _cancellationTokenSource.Dispose();

                        _tokenDisposed = true;
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~AsyncTaskCodeActivity() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion

    }

    /// <summary>
    /// Extent.ExtentHtmlReporter AsyncTaskCodeActivity without result
    /// </summary>
    public abstract class AsyncTaskCodeActivity : AsyncCodeActivity, IDisposable
    {

        protected AsyncTaskCodeActivity()
        {

        }
        private CancellationTokenSource _cancellationTokenSource;
        private bool _tokenDisposed = false;

        protected override void Cancel(AsyncCodeActivityContext context)
        {
            if (!_tokenDisposed)
            {
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource?.Dispose();

                _tokenDisposed = true;
            }

            base.Cancel(context);
        }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
        }

        protected override IAsyncResult BeginExecute(AsyncCodeActivityContext context, AsyncCallback callback, object state)
        {
            if (!_tokenDisposed)
            {
                _cancellationTokenSource?.Dispose();
            }

            _cancellationTokenSource = new CancellationTokenSource();
            _tokenDisposed = false;

            var property = context.DataContext.GetProperties()[TestcaseScope.TestcaseContainerPropertyTag];
            var servicesTestcase = property.GetValue(context.DataContext) as Testcase;

            TaskCompletionSource<Action<AsyncCodeActivityContext>> taskCompletionSource = new TaskCompletionSource<Action<AsyncCodeActivityContext>>(state);
            Task<Action<AsyncCodeActivityContext>> task = ExecuteAsync(context, _cancellationTokenSource.Token, servicesTestcase);

            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    taskCompletionSource.TrySetException(t.Exception.InnerException);
                }
                else if (t.IsCanceled || _cancellationTokenSource.IsCancellationRequested)
                {
                    taskCompletionSource.TrySetCanceled();
                }
                else
                {
                    taskCompletionSource.TrySetResult(t.Result);
                }

                callback?.Invoke(taskCompletionSource.Task);
            });

            return taskCompletionSource.Task;
        }

        protected override void EndExecute(AsyncCodeActivityContext context, IAsyncResult result)
        {
            Task<Action<AsyncCodeActivityContext>> task = (Task<Action<AsyncCodeActivityContext>>)result;

            if (task.IsFaulted)
            {
                ExceptionDispatchInfo.Capture(task.Exception.InnerException).Throw();
            }
            if (task.IsCanceled)
            {
                context.MarkCanceled();
            }

            task.Result?.Invoke(context);

            if (!_tokenDisposed)
            {
                _cancellationTokenSource?.Dispose();

                _tokenDisposed = true;
            }
        }

        protected abstract Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken, Testcase client);

        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (!_tokenDisposed)
                    {
                        _cancellationTokenSource.Dispose();

                        _tokenDisposed = true;
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~AsyncTaskCodeActivity() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}