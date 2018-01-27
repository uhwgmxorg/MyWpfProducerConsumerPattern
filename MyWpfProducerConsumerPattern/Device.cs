using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MyWpfProducerConsumerPattern
{
    public class Device : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ThreadMonitorOutputWindow Tmow { get; set; }

        #region INotifyPropertyChanged Properties
        private string description;
        public string Description
        {
            get { return description; }
            set { SetField(ref description, value, nameof(Description)); }
        }
        private Brush idColor;
        public Brush IdColor
        {
            get { return idColor; }
            set { SetField(ref idColor, value, nameof(IdColor)); }
        }
        private string returnCode;
        public string ReturnCode
        {
            get { return returnCode; }
            set { SetField(ref returnCode, value, nameof(ReturnCode)); }
        }
        #endregion

        public BlockingCollection<JobAction> JobQueueActions { get; set; } = new BlockingCollection<JobAction>();

        public ConcurrentQueue<JobResult> jobResponse;
        public ConcurrentQueue<JobResult> JobResponse
        {
            get { return jobResponse; }
            set { SetField(ref jobResponse, value, nameof(JobResponse));}
        }


        private System.Collections.ObjectModel.ObservableCollection<string> queueDiplayList;
        public System.Collections.ObjectModel.ObservableCollection<string> QueueDiplayList
        {
            get { return queueDiplayList; }
            set { SetField(ref queueDiplayList, value, nameof(QueueDiplayList)); }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public Device(Brush color, int counter)
        {
            // Initialize member vars
            JobResponse = new ConcurrentQueue<JobResult>(); QueueDiplayList = new System.Collections.ObjectModel.ObservableCollection<string>();
            Description = string.Format("Device Id #{0}", counter);
            IdColor = color;

            // Create the output window for this device and show it
            Tmow = new ThreadMonitorOutputWindow(IdColor);
            Tmow.Show();
            Tmow.InfoHeadline = Description;

            // We put our devices action in a Lamda function 
            Task task = Task.Factory.StartNew(() =>
            {
                int c = 0;
                Tmow.AddMessage(string.Format("Stat {0}", Description));

                while (true)
                {
                    // Wait until the main thread orders an action
                    var action = JobQueueActions.Take();
                    Tmow.AddMessage(string.Format("----------------------- Start Action -------------------------"));
                    Tmow.AddMessage(string.Format("Get Job Order No {0} Job Type is {1}", ++c, action.Action));
                    Tmow.AddMessage(string.Format("with TransactionId {0}", action.TransactionId));

                    // and do something
                    Tmow.AddMessage(string.Format("do something ....."));
                    Thread.Sleep(500);

                    // Create the Job Result
                    var response = new JobResult { TransactionId = action.TransactionId };
                    // and put the result in a queue, so that the main thread gets a response, identified by its transaction id                        
                    ReturnCode = String.Format("{0} {1} {2}", c,action.Action.ToString(), response.Result.ToString());
                    JobResponse.Enqueue(response); Application.Current.Dispatcher.BeginInvoke(new Action(() => this.QueueDiplayList.Add(ReturnCode)));
                    Tmow.AddMessage(string.Format("Job Response Code is {0} and TransactionId is {1}", response.Result,response.TransactionId));

                    // then go ahead
                }
            });
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Device()
        {
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Tmow.Close();
        }

        /******************************/
        /*      Other Functions       */
        /******************************/
        #region Other Functions

        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        private void OnPropertyChanged(string p)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
        }

        #endregion
    }

    #region Help Classes
    public class JobAction
    {
        public enum JobActionTypes
        {
            Action_I = 1,
            Action_II,
            Action_III
        }

        public int TransactionId { get; set; }
        public JobActionTypes Action { get; set; }
    }

    public class JobResult
    {
        public enum JobResultTypes
        {
            Result_I = 1,
            Result_II,
            Result_III,
            Result_IV,
            Result_V,
            Result_VI,
            Result_VII,
            Result_VIII,
            Result_IX,
            Result_X
        }

        private Random _random = new Random();

        public int TransactionId { get; set; }
        public JobResultTypes Result { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public JobResult()
        {
            GetRandomResult();
        }

        /// <summary>
        /// GetRandomResult
        /// </summary>
        /// <returns></returns>
        public JobResultTypes GetRandomResult()
        {
            Result = (JobResultTypes)_random.Next(1, 10);
            return Result;
        }

        /// <summary>
        /// Random
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private int Random(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
    #endregion
}
