using System;
using System.Windows;
using Microsoft.Phone.Controls;
using ShakeGestures;

namespace ShakeGesturesSample
{
    public partial class MainPage : PhoneApplicationPage
    {
        #region CurrentShakeType dependency property
        
        public ShakeType CurrentShakeType
        {
            get { return (ShakeType)GetValue(CurrentShakeTypeProperty); }
            set { SetValue(CurrentShakeTypeProperty, value); }
        }

        #endregion

        public static readonly DependencyProperty CurrentShakeTypeProperty =
            DependencyProperty.Register("CurrentShakeType", typeof(ShakeType), typeof(MainPage), new PropertyMetadata(ShakeType.X));

        public MainPage()
        {
            InitializeComponent();

            // register shake event
            ShakeGesturesHelper.Instance.ShakeGesture += new EventHandler<ShakeGestureEventArgs>(Instance_ShakeGesture);

            // optional, set parameters
            ShakeGesturesHelper.Instance.MinimumRequiredMovesForShake = 5;

            // start shake detection
            ShakeGesturesHelper.Instance.Active = true;

            // run shake Z simulation
            //ShakeGesturesHelper.Instance.Simulate(ShakeType.Z);
        }

        private void Instance_ShakeGesture(object sender, ShakeGestureEventArgs e)
        {
            _lastUpdateTime.Dispatcher.BeginInvoke(
                () =>
                {
                    _lastUpdateTime.Text = DateTime.Now.ToString();
                    string CurrentShakeType1 = e.shakeType.ShakeTypeStr.ToString();
                });
            System.Diagnostics.Debug.WriteLine(DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond + "  " + e.shakeType.ShakeTypeStr.ToString() + "  " + e.shakeType.ShakeValue.ToString());
        }
    }
}