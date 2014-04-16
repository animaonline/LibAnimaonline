using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Animaonline.Events;

namespace LibAnimaonline.Console
{
    public partial class Ui : Form
    {
        public Ui()
        {
            InitializeComponent();

            tickers.Add(new Ticker(1));
            tickers.Add(new Ticker(2));
            tickers.Add(new Ticker(3));

            tickerUpdateEvent = new SmartEvent<Tuple<int, string, int>>();

            tickerUpdateEvent.Subscribe(new Action<SmartEventSubscriber<Tuple<int, string, int>>, Tuple<int, string, int>>((subscriber, value) => Invoke(new MethodInvoker(() =>
            {
                switch (value.Item1)
                {
                    case 1:
                        label1.Text = value.Item2 + " - " + value.Item3;
                        break;
                    case 2:
                        label2.Text = value.Item2 + " - " + value.Item3;
                        break;
                    case 3:
                        label3.Text = value.Item2 + " - " + value.Item3;
                        break;
                }
            }))));
        }

        private bool isStarted;

        private List<Ticker> tickers = new List<Ticker>();

        private void buttonToggle_Click(object sender, EventArgs e)
        {
            if (!isStarted)
            {
                isStarted = true;
                buttonToggle.Text = "Stop";

                tickers.ForEach((t) => t.Start());
            }
            else
            {
                isStarted = false;
                buttonToggle.Text = "Start";

                tickers.ForEach((t) => t.Stop());
            }
        }

        private static SmartEvent<Tuple<int, string, int>> tickerUpdateEvent;

        class Ticker
        {
            public Ticker(int id)
            {
                this.ID = id;
            }

            public int ID { get; set; }

            private Thread tickerThread;

            private Random rnd = new Random();

            private void doWork()
            {
                while (true)
                {
                    var rndVal = rnd.Next(4000);

                    tickerUpdateEvent.Trigger(new Tuple<int, string, int>(this.ID, "XASD", rndVal));

                    Thread.Sleep(rndVal);
                }
            }

            public void Start()
            {
                if (tickerThread != null && tickerThread.IsAlive)
                    throw new Exception("Thread already running");

                tickerThread = new Thread(doWork);

                tickerThread.Start();
            }

            public void Stop()
            {
                if (tickerThread != null)
                {
                    tickerThread.Abort();
                    tickerThread = null;
                }
            }
        }
    }
}
