using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System;
using System.Diagnostics;
using GalaSoft.MvvmLight.Command;

namespace MyWpfProducerConsumerPattern
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        const int MAX_DEVICES = 8;
        const int COLUMN = 3;

        private int _transactionCounter = 0;
        private int _deviceCounter = 0;

        private Random _random = new Random();

        #region INotifyPropertyChanged Properties
        private ObservableCollection<Device> devices;
        public ObservableCollection<Device> Devices
        {
            get { return devices; }
            set { SetField(ref devices, value, nameof(Devices)); }
        }
        private string text;
        public string Text
        {
            get { return text; }
            set { SetField(ref text, value, nameof(Text)); }
        }
        #endregion

        public RelayCommand<object> ButtonJob1 { get; private set; }
        public RelayCommand<object> ButtonJob2 { get; private set; }
        public RelayCommand<object> ButtonJob3 { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Devices = new ObservableCollection<Device>();

            // Put the window in the lower right corner of the screen
            int screenX = (int)System.Windows.SystemParameters.PrimaryScreenWidth;
            int screenY = (int)System.Windows.SystemParameters.PrimaryScreenHeight;
            Left = screenX - Width - 10;
            Top = screenY - Height - 10 - 40;

            ButtonJob1 = new RelayCommand<object>(ButtonJob1CF);
            ButtonJob2 = new RelayCommand<object>(ButtonJob2CF);
            ButtonJob3 = new RelayCommand<object>(ButtonJob3CF);
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~MainWindow()
        {
        }

        /******************************/
        /*       Button Events        */
        /******************************/
        #region Button Events

        /// <summary>
        /// Button_Add_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            AddDevice();
            TiledChildWindows();
        }
        private void AddDevice()
        {
            if (Devices.Count > MAX_DEVICES - 1) { Console.Beep(); return; }

            Device d = new Device(GetRandomBrush2(),++_deviceCounter);
            Devices.Add(d);
        }

        /// <summary>
        /// Button_ClearAll_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_ClearAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var d in Devices)
                d.Dispose();
            Devices.Clear();
        }

        /// <summary>
        /// Button_TiledWindows_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_TiledWindows_Click(object sender, RoutedEventArgs e)
        {
            TiledChildWindows();
        }
        private void TiledChildWindows()
        {
            int screenX = (int)System.Windows.SystemParameters.PrimaryScreenWidth;
            int screenY = (int)System.Windows.SystemParameters.PrimaryScreenHeight;

            int column = COLUMN;
            int x = 0;
            int y = 0;
            int xSpace = 10;
            int ySpace = 10;
            for (int i = 1; i <= Devices.Count; i++)
            {
                Devices[i - 1].Tmow.Left = xSpace + Devices[i - 1].Tmow.Width * x + xSpace;
                Devices[i - 1].Tmow.Top = ySpace + Devices[i - 1].Tmow.Height * y + ySpace;
                x++;
                if (i % column == 0)
                {
                    y++;
                    x = 0;
                }
            }
        }

        /// <summary>
        /// Button_CascateWindows_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_CascateWindows_Click(object sender, RoutedEventArgs e)
        {
            CascateChildWindows();
        }
        private void CascateChildWindows()
        {
            int screenX = (int)System.Windows.SystemParameters.PrimaryScreenWidth;
            int screenY = (int)System.Windows.SystemParameters.PrimaryScreenHeight;

            int x = 0;
            int y = 0;
            int xSift = 30;
            int ySift = 30;
            for (int i = 1; i <= Devices.Count; i++)
            {
                Devices[i - 1].Tmow.Left = xSift + xSift * x;
                Devices[i - 1].Tmow.Top = ySift + ySift * y;
                x++;
                y++;
            }
        }

        /// <summary>
        /// Button_Close_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #region Dynamic ListView Buttons
        /// <summary>
        /// ButtonJob1CF
        /// </summary>
        /// <param name="obj"></param>
        public void ButtonJob1CF(object obj)
        {
            var sc = obj as Device;
            sc.JobQueueActions.Add(new JobAction { Action = JobAction.JobActionTypes.Action_I, TransactionId = ++_transactionCounter});
            Debug.WriteLine(String.Format("Job #1 Id={0}", sc.Description));
        }

        /// <summary>
        /// ButtonJob2CF
        /// </summary>
        /// <param name="obj"></param>
        public void ButtonJob2CF(object obj)
        {
            var sc = obj as Device;
            sc.JobQueueActions.Add(new JobAction { Action = JobAction.JobActionTypes.Action_II, TransactionId = ++_transactionCounter });
            Debug.WriteLine(String.Format("Job #2 Id={0}", sc.Description));
        }

        /// <summary>
        /// ButtonJob3CF
        /// </summary>
        /// <param name="obj"></param>
        public void ButtonJob3CF(object obj)
        {
            var sc = obj as Device;
            sc.JobQueueActions.Add(new JobAction { Action = JobAction.JobActionTypes.Action_III, TransactionId = ++_transactionCounter });
            Debug.WriteLine(String.Format("Job #3 Id={0}", sc.Description));
        }
        #endregion

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

        /// <summary>
        /// Window_Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            foreach (var d in Devices)
                d.Tmow.Close();
        }

        #endregion
        /******************************/
        /*      Other Functions       */
        /******************************/
        #region Other Functions

            
        
        /// <summary>
        /// GetRandomBrush
        /// </summary>
        /// <returns></returns>
        private Brush GetRandomBrush()
        {
            SolidColorBrush rc = Brushes.Black;

            switch(Random(1, 141))
            {
                case 1: rc = Brushes.AliceBlue; break;
                case 2: rc = Brushes.PaleGoldenrod; break;
                case 3: rc = Brushes.Orchid; break;
                case 4: rc = Brushes.OrangeRed; break;
                case 5: rc = Brushes.Orange; break;
                case 6: rc = Brushes.OliveDrab; break;
                case 7: rc = Brushes.Olive; break;
                case 8: rc = Brushes.OldLace; break;
                case 9: rc = Brushes.Navy; break;
                case 0: rc = Brushes.NavajoWhite; break;
                case 11: rc = Brushes.Moccasin; break;
                case 12: rc = Brushes.MistyRose; break;
                case 13: rc = Brushes.MintCream; break;
                case 14: rc = Brushes.MidnightBlue; break;
                case 15: rc = Brushes.MediumVioletRed; break;
                case 16: rc = Brushes.MediumTurquoise; break;
                case 17: rc = Brushes.MediumSpringGreen; break;
                case 18: rc = Brushes.MediumSlateBlue; break;
                case 19: rc = Brushes.LightSkyBlue; break;
                case 20: rc = Brushes.LightSlateGray; break;
                case 21: rc = Brushes.LightSteelBlue; break;
                case 22: rc = Brushes.LightYellow; break;
                case 23: rc = Brushes.Lime; break;
                case 24: rc = Brushes.LimeGreen; break;
                case 25: rc = Brushes.PaleGreen; break;
                case 26: rc = Brushes.Linen; break;
                case 27: rc = Brushes.Maroon; break;
                case 28: rc = Brushes.MediumAquamarine; break;
                case 29: rc = Brushes.MediumBlue; break;
                case 30: rc = Brushes.MediumOrchid; break;
                case 31: rc = Brushes.MediumPurple; break;
                case 32: rc = Brushes.MediumSeaGreen; break;
                case 33: rc = Brushes.Magenta; break;
                case 34: rc = Brushes.PaleTurquoise; break;
                case 35: rc = Brushes.PaleVioletRed; break;
                case 36: rc = Brushes.PapayaWhip; break;
                case 37: rc = Brushes.SlateGray; break;
                case 38: rc = Brushes.Snow; break;
                case 39: rc = Brushes.SpringGreen; break;
                case 40: rc = Brushes.SteelBlue; break;
                case 41: rc = Brushes.Tan; break;
                case 42: rc = Brushes.Teal; break;
                case 43: rc = Brushes.SlateBlue; break;
                case 44: rc = Brushes.Thistle; break;
                case 45: rc = Brushes.Transparent; break;
                case 46: rc = Brushes.Turquoise; break;
                case 47: rc = Brushes.Violet; break;
                case 48: rc = Brushes.Wheat; break;
                case 49: rc = Brushes.White; break;
                case 50: rc = Brushes.WhiteSmoke; break;
                case 51: rc = Brushes.Tomato; break;
                case 52: rc = Brushes.LightSeaGreen; break;
                case 53: rc = Brushes.SkyBlue; break;
                case 54: rc = Brushes.Sienna; break;
                case 55: rc = Brushes.PeachPuff; break;
                case 56: rc = Brushes.Peru; break;
                case 57: rc = Brushes.Pink; break;
                case 58: rc = Brushes.Plum; break;
                case 59: rc = Brushes.PowderBlue; break;
                case 60: rc = Brushes.Purple; break;
                case 61: rc = Brushes.Silver; break;
                case 62: rc = Brushes.Red; break;
                case 63: rc = Brushes.RoyalBlue; break;
                case 64: rc = Brushes.SaddleBrown; break;
                case 65: rc = Brushes.Salmon; break;
                case 66: rc = Brushes.SandyBrown; break;
                case 67: rc = Brushes.SeaGreen; break;
                case 68: rc = Brushes.SeaShell; break;
                case 69: rc = Brushes.RosyBrown; break;
                case 70: rc = Brushes.Yellow; break;
                case 71: rc = Brushes.LightSalmon; break;
                case 72: rc = Brushes.LightGreen; break;
                case 73: rc = Brushes.DarkRed; break;
                case 74: rc = Brushes.DarkOrchid; break;
                case 75: rc = Brushes.DarkOrange; break;
                case 76: rc = Brushes.DarkOliveGreen; break;
                case 77: rc = Brushes.DarkMagenta; break;
                case 78: rc = Brushes.DarkKhaki; break;
                case 79: rc = Brushes.DarkGreen; break;
                case 80: rc = Brushes.DarkGray; break;
                case 81: rc = Brushes.DarkGoldenrod; break;
                case 82: rc = Brushes.DarkCyan; break;
                case 83: rc = Brushes.DarkBlue; break;
                case 84: rc = Brushes.Cyan; break;
                case 85: rc = Brushes.Crimson; break;
                case 86: rc = Brushes.Cornsilk; break;
                case 87: rc = Brushes.CornflowerBlue; break;
                case 88: rc = Brushes.Coral; break;
                case 89: rc = Brushes.Chocolate; break;
                case 90: rc = Brushes.AntiqueWhite; break;
                case 91: rc = Brushes.Aqua; break;
                case 92: rc = Brushes.Aquamarine; break;
                case 93: rc = Brushes.Azure; break;
                case 94: rc = Brushes.Beige; break;
                case 95: rc = Brushes.Bisque; break;
                case 96: rc = Brushes.DarkSalmon; break;
                case 97: rc = Brushes.Black; break;
                case 98: rc = Brushes.Blue; break;
                case 99: rc = Brushes.BlueViolet; break;
                case 100: rc = Brushes.Brown; break;
                case 101: rc = Brushes.BurlyWood; break;
                case 102: rc = Brushes.CadetBlue; break;
                case 103: rc = Brushes.Chartreuse; break;
                case 104: rc = Brushes.BlanchedAlmond; break;
                case 105: rc = Brushes.DarkSeaGreen; break;
                case 106: rc = Brushes.DarkSlateBlue; break;
                case 107: rc = Brushes.DarkSlateGray; break;
                case 108: rc = Brushes.HotPink; break;
                case 109: rc = Brushes.IndianRed; break;
                case 110: rc = Brushes.Indigo; break;
                case 111: rc = Brushes.Ivory; break;
                case 112: rc = Brushes.Khaki; break;
                case 113: rc = Brushes.Lavender; break;
                case 114: rc = Brushes.Honeydew; break;
                case 115: rc = Brushes.LavenderBlush; break;
                case 116: rc = Brushes.LemonChiffon; break;
                case 117: rc = Brushes.LightBlue; break;
                case 118: rc = Brushes.LightCoral; break;
                case 119: rc = Brushes.LightCyan; break;
                case 120: rc = Brushes.LightGoldenrodYellow; break;
                case 121: rc = Brushes.LightGray; break;
                case 122: rc = Brushes.LawnGreen; break;
                case 123: rc = Brushes.LightPink; break;
                case 124: rc = Brushes.GreenYellow; break;
                case 125: rc = Brushes.Gray; break;
                case 126: rc = Brushes.DarkTurquoise; break;
                case 127: rc = Brushes.DarkViolet; break;
                case 128: rc = Brushes.DeepPink; break;
                case 129: rc = Brushes.DeepSkyBlue; break;
                case 130: rc = Brushes.DimGray; break;
                case 131: rc = Brushes.DodgerBlue; break;
                case 132: rc = Brushes.Green; break;
                case 133: rc = Brushes.Firebrick; break;
                case 134: rc = Brushes.ForestGreen; break;
                case 135: rc = Brushes.Fuchsia; break;
                case 136: rc = Brushes.Gainsboro; break;
                case 137: rc = Brushes.GhostWhite; break;
                case 138: rc = Brushes.Gold; break;
                case 139: rc = Brushes.Goldenrod; break;
                case 140: rc = Brushes.FloralWhite; break;
                case 141: rc = Brushes.YellowGreen; break;
            }

            return rc;
        }

        /// <summary>
        /// StringToBrush
        /// </summary>
        /// Convert a string to a SolidColorBrush e.g 
        /// "Balck" --> public static SolidColorBrush Black { get; }
        /// <param name="color"></param>
        /// <returns></returns>
        private Brush GetRandomBrush2()
        {
            var converter = new System.Windows.Media.BrushConverter();
            int rMin = 0x00;
            int rMax = 0xFF;
            string color = String.Format("#{0}{1}{2}{3}",Random(0xFF, 0xFF).ToString("X2"), Random(rMin, rMax).ToString("X2"), Random(rMin, rMax).ToString("X2"), Random(rMin, rMax).ToString("X2"));

            var brush = (Brush)converter.ConvertFromString(color);
            return brush;
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
