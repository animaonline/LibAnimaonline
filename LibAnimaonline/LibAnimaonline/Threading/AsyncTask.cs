using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Animaonline.Threading
{
    public abstract class AsyncTask
    {
        public AsyncTask()
        {
            this.CancellationToken = new CancellationToken();
        }

        public virtual void OnPreExecute()
        {

        }

        public CancellationToken CancellationToken { get; set; }

        public virtual void OnPostExecute(object result)
        {

        }

        public abstract object DoInBackground();

        private Task runningTask;

        public void Cancel()
        {
            CancellationToken.
        }

        public void Execute()
        {
            OnPreExecute();

            object taskResult = null;

            runningTask = Task.Factory.StartNew(() =>
            {
                taskResult = DoInBackground();
            }).ContinueWith((t) => OnPostExecute(taskResult));
        }
    }
}
