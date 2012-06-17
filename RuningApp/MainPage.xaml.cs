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
using GART;
using GART.Data;

using Matrix = Microsoft.Xna.Framework.Matrix;
using Point = System.Windows.Point;
using GART.Controls;
using System.Device.Location;

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
            ShakeGesturesHelper.Instance.WeakMagnitudeWithoutGravitationThreshold = 0.04;

            // start shake detection
            ShakeGesturesHelper.Instance.Active = true;
        }

        public int StepsCount { get { return _stepsCount; } set { _stepsCount = value; } }
        private int _stepsCount = 0;

        private void Instance_ShakeGesture(object sender, ShakeGestureEventArgs e)
        {
            
            Dispatcher.BeginInvoke(
                () =>
                {
                    //this.MovingSounds.Play();
                    //clear uncorrect values
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

                    //set max value
                    switch (e.shakeType.ShakeTypeStr.ToString())
                    {
                        case "X":
                            if ((Math.Abs(e.shakeType.MaxValues.maxx))>(Math.Abs(e.shakeType.MaxValues.minx))) {
                                x_type = (Math.Abs(e.shakeType.MaxValues.maxx));
                            }
                            else {
                                x_type = (Math.Abs(e.shakeType.MaxValues.minx));
                            };
                            //this.MovingSoundsX.Source = new Uri("/RuningApp;component/sounds/knock1.wav");
                            //this.MovingSoundsX.Volume = x_type;
                            this.MovingSoundsX.Play();
                            break;
                        case "Y":
                            if ((Math.Abs(e.shakeType.MaxValues.maxy))>(Math.Abs(e.shakeType.MaxValues.miny))) {
                                y_type = (Math.Abs(e.shakeType.MaxValues.maxy));
                            }
                            else {
                                y_type = (Math.Abs(e.shakeType.MaxValues.miny));
                            };
                            this.MovingSoundsY.Play();
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
                    /////////////////////////////
                    //clear values
                    /////////////////////////////
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
                    //update distance

                    CoordDataItem sitem = new CoordDataItem()
                    {
                        time = DateTime.Now,
                        timestr = DateTime.Now.Minute + ":" + DateTime.Now.Second,
                        X = x_type,
                        Y = y_type,
                        Z = z_type,
                        GeoLocation = ARDisplay.Location,
                        step = _stepsCount
                    };

                    try {
                        //((_data.Last() as CoordDataItem).time.Second == sitem.time.Second) && 

                        if (((_data.Last() as CoordDataItem).time.Second == sitem.time.Second) 
                            && ((_data.Last() as CoordDataItem).time.Minute == sitem.time.Minute))
                        {
                            //nothing
                    }
                    else {
                        _stepsCount++;
                        _distance = _distance + Double.Parse(this.StepLength.Text);
                        this.DistanceText.Text = _distance.ToString("F");
                        this.StepsText.Text = _stepsCount.ToString();

                        if ((_stepsCount % 10) == 0)
                        {
                            this.AddLabel(sitem);
                            //_data.Add(sitem);
                        };

                        _data.Add(sitem);
                    };
                    }
                    catch {
                        _stepsCount++;
                        _distance = _distance + Double.Parse(this.StepLength.Text);
                        this.DistanceText.Text = _distance.ToString("F");

                        this.StepsText.Text = _stepsCount.ToString();

                        _data.Add(sitem);
                    };                    

                    
                    string out_label = DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + "  " + e.shakeType.ShakeTypeStr.ToString() + "  " + x_type.ToString() + "  " + y_type.ToString() + "  " + z_type.ToString();
                    
                    this.ShakeLog.Items.Add(DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + "  " + e.shakeType.ShakeTypeStr.ToString() + "  " + x_type.ToString() + "  " + y_type.ToString() + "  " + z_type.ToString());

                    //ARDisplay.ARItems = _data;
                });            
        }

        private Random rand = new Random();

        private void AddLabel(CoordDataItem in_item)
        {
            // We'll use the specified text for the content and we'll let 
            // the system automatically project the item into world space
            // for us based on the Geo location.

            ARDisplay.ARItems.Add(in_item);
        }

        private void AddNearbyLabels()
        {
            // Start with the current location
            GeoCoordinate current = ARDisplay.Location;

            // We'll add three Labels
            for (int i = 0; i < 6; i++)
            {
                // Create a new location based on the users location plus
                // a random offset.
                GeoCoordinate offset = new GeoCoordinate(
                    current.Latitude + ((double)rand.Next(-160, 160)) / 100000,
                    current.Longitude + ((double)rand.Next(-160, 160)) / 100000);

                CoordDataItem sitem = new CoordDataItem()
                {
                    time = DateTime.Now,
                    timestr = DateTime.Now.Minute + ":" + DateTime.Now.Second,
                    X = 0,
                    Y = 0,
                    Z = 0,
                    GeoLocation = offset,
                    step = i
                };

                AddLabel(sitem);
            }
        }

        /// <summary>
        /// Switches between rottaing the Heading Indicator or rotating the Map to the current heading.
        /// </summary>
        private void SwitchHeadingMode()
        {
            if (HeadingIndicator.RotationSource == RotationSource.AttitudeHeading)
            {
                HeadingIndicator.RotationSource = RotationSource.North;
                OverheadMap.RotationSource = RotationSource.AttitudeHeading;
            }
            else
            {
                OverheadMap.RotationSource = RotationSource.North;
                HeadingIndicator.RotationSource = RotationSource.AttitudeHeading;
            }
        }

        public ObservableCollection<ARItem> Data { get { return _data; } }
        public string Distance { get { return _distance.ToString("F"); } }
        private double _distance = 0;
        private ObservableCollection<ARItem> _data = new ObservableCollection<ARItem>()
        { 
        };

        private void AddLocationsMenu_Click(object sender, EventArgs e)
        {
            AddNearbyLabels();
        }

        private void ClearLocationsMenu_Click(object sender, EventArgs e)
        {
            ARDisplay.ARItems.Clear();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Stop AR services
            ARDisplay.StopServices();
            //ARDisplayHeading.StopServices();
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Start AR services
            ARDisplay.StartServices();
            //ARDisplayHeading.StartServices();

            base.OnNavigatedTo(e);
        }

        private void HeadingButton_Click(object sender, System.EventArgs e)
        {
            UIHelper.ToggleVisibility(HeadingIndicator);
        }

        private void MapButton_Click(object sender, System.EventArgs e)
        {
            UIHelper.ToggleVisibility(OverheadMap);
        }

        private void RotateButton_Click(object sender, System.EventArgs e)
        {
            SwitchHeadingMode();
        }

        private void ThreeDButton_Click(object sender, System.EventArgs e)
        {
            UIHelper.ToggleVisibility(WorldView);
        }

        public class CoordDataItem: ARItem
        {
            public DateTime time { get; set; }
            public string timestr { get; set; }
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
            public int step { get; set; }
            //GeoCoordinate current = ARDisplay.Location;
            //public GeoCoordinate GeoLocation { get; set; }
        }

        private void StopGestures_Click(object sender, RoutedEventArgs e)
        {
            ShakeGesturesHelper.Instance.Active = false;
        }

        private void AddCurrentLocationsMenu_Click(object sender, EventArgs e)
        {
            GeoCoordinate current = ARDisplay.Location;
            CoordDataItem sitem = new CoordDataItem()
            {
                time = DateTime.Now,
                timestr = DateTime.Now.Minute + ":" + DateTime.Now.Second,
                X = 0,
                Y = 0,
                Z = 0,
                GeoLocation = current,
                step = 0
            };

            AddLabel(sitem);
        }

    }
}