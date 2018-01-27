using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace MyWpfProducerConsumerPattern
{
    /// <summary>
    /// Interaktionslogik für ThreadMonitorOutputWindow.xaml
    /// </summary>
    public partial class ThreadMonitorOutputWindow : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        Brush MessageColor { get; set; }

        #region INotifyPropertyChanged Properties
        private string infoHeadline;
        public string InfoHeadline
        {
            get { return infoHeadline; }
            set { SetField(ref infoHeadline, value, nameof(InfoHeadline)); }
        }
        private Brush headlineColor;
        public Brush HeadlineColor
        {
            get { return headlineColor; }
            set { SetField(ref headlineColor, value, nameof(HeadlineColor)); }
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ThreadMonitorOutputWindow(Brush color)
        {
            InitializeComponent();
            DataContext = this;

            MessageColor = color;
            HeadlineColor = color;
        }

        /******************************/
        /*       Button Events        */
        /******************************/
        #region Button Events

        #endregion
        /******************************/
        /*      Menu Events          */
        /******************************/
        #region Menu Events

        #endregion
        /******************************/
        /*      Other Events          */
        /******************************/
        #region Other Events

        #endregion
        /******************************/
        /*      Other Functions       */
        /******************************/
        #region Other Functions
        
        /// <summary>
        /// AddMessage
        /// </summary>
        /// <param name="message"></param>
        /// <param name="color"></param>
        public void AddMessage(string message)
        {
            mlbl_ListBoxLog.ListBoxLogMessageAdd(message, MessageColor);
        }

        /// <summary>
        /// SetField
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
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
}
