using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

using ShakeGestures;
using AmCharts;
using System.Collections.ObjectModel;

namespace RuningApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Конструктор
        public MainPage()
        {
            InitializeComponent();

            // Задайте для контекста данных элемента управления listbox пример данных
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Загрузка данных для элементов ViewModel
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
            this.DataContext = this;
        }

        public ShakeType CurrentShakeType
        {
            get { return (ShakeType)GetValue(CurrentShakeTypeProperty); }
            set { SetValue(CurrentShakeTypeProperty, value); }
        }

        public static readonly DependencyProperty CurrentShakeTypeProperty =
            DependencyProperty.Register("CurrentShakeType", typeof(ShakeType), typeof(MainPage), new PropertyMetadata(ShakeType.X));

        private void RecordGestures_Click(object sender, RoutedEventArgs e)
        {
            ShakeGesturesHelper.Instance.ShakeGesture += new EventHandler<ShakeGestureEventArgs>(Instance_ShakeGesture);

            // optional, set parameters
            ShakeGesturesHelper.Instance.MinimumRequiredMovesForShake = 1;
            ShakeGesturesHelper.Instance.WeakMagnitudeWithoutGravitationThreshold = 0.05;

            // start shake detection
            ShakeGesturesHelper.Instance.Active = true;
        }

        private void Instance_ShakeGesture(object sender, ShakeGestureEventArgs e)
        {
            Dispatcher.BeginInvoke(
                () =>
                {
                    double x_type = 0, y_type = 0, z_type = 0;

                    if (e.shakeType.MaxValues.minx == 10000)
                    {
                        e.shakeType.MaxValues.minx = 0;
                    };
                    if (e.shakeType.MaxValues.miny == 10000)
                    {
                        e.shakeType.MaxValues.miny = 0;
                    };
                    if (e.shakeType.MaxValues.minz == 10000)
                    {
                        e.shakeType.MaxValues.minz = 0;
                    };


                    if (e.shakeType.MaxValues.maxx == -10000)
                    {
                        e.shakeType.MaxValues.maxx = 0;
                    };
                    if (e.shakeType.MaxValues.maxy == -10000)
                    {
                        e.shakeType.MaxValues.maxy = 0;
                    };
                    if (e.shakeType.MaxValues.maxz == -10000)
                    {
                        e.shakeType.MaxValues.maxz = 0;
                    };


                    switch (e.shakeType.ShakeTypeStr.ToString())
                    {
                        case "X":
                            if ((Math.Abs(e.shakeType.MaxValues.maxx))>(Math.Abs(e.shakeType.MaxValues.minx))) {
                                x_type = (Math.Abs(e.shakeType.MaxValues.maxx));
                            }
                            else {
                                x_type = (Math.Abs(e.shakeType.MaxValues.minx));
                            };
                            break;
                        case "Y":
                            if ((Math.Abs(e.shakeType.MaxValues.maxy))>(Math.Abs(e.shakeType.MaxValues.miny))) {
                                y_type = (Math.Abs(e.shakeType.MaxValues.maxy));
                            }
                            else {
                                y_type = (Math.Abs(e.shakeType.MaxValues.miny));
                            };
                            break;
                        case "Z":
                            if ((Math.Abs(e.shakeType.MaxValues.maxz))>(Math.Abs(e.shakeType.MaxValues.minz))) {
                                z_type = (Math.Abs(e.shakeType.MaxValues.maxz));
                            }
                            else {
                                z_type = (Math.Abs(e.shakeType.MaxValues.minz));
                            };
                            break;
                        default:                            
                            break;
                    }
                    if ((x_type == 10000) && (x_type==-10000)) {
                        x_type = 0;
                    };
                    if ((z_type == 10000) && (z_type == -10000))
                    {
                        z_type = 0;
                    };
                    if ((y_type == 10000) && (y_type == -10000))
                    {
                        y_type = 0;
                    };

                    _distance = _distance + Double.Parse(this.StepLength.Text);
                    this.DistanceText.Text = _distance.ToString("F");

                    _data.Add(new CoordDataItem() { time = DateTime.Now.Minute + ":" + DateTime.Now.Second, X = x_type, Y = y_type, Z = z_type });
                    this.ShakeLog.Items.Add(DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + "  " + e.shakeType.ShakeTypeStr.ToString() + "  " + x_type.ToString() + "  " + y_type.ToString() + "  " + z_type.ToString());
                });
            //System.Diagnostics.Debug.WriteLine(DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond + "  " + e.ShakeType);
        }

        public ObservableCollection<CoordDataItem> Data { get { return _data; } }

        public string Distance { get { return _distance.ToString("F"); } }

        private double _distance = 0;

        private ObservableCollection<CoordDataItem> _data = new ObservableCollection<CoordDataItem>()
        { 
        };

        public class CoordDataItem
        {
            public string time { get; set; }
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
        }

        private void StopGestures_Click(object sender, RoutedEventArgs e)
        {
            ShakeGesturesHelper.Instance.Active = false;
        }

    }
}