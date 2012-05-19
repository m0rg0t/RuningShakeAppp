using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ShakeGestures
{

    public class MaxVal
    {
        public MaxVal()
        {
            this.maxx = -10000; this.maxy = -10000; this.maxz = -10000;
            this.minx = 10000; this.miny = 10000; this.minz = 10000;
        }

        public double maxx { get; set; }
        public double maxy { get; set; }
        public double maxz { get; set; }
        public double minx { get; set; }
        public double miny { get; set; }
        public double minz { get; set; }
    }

    public enum ShakeType
    {
        X = 0,
        Y = 1,
        Z = 2,
    }
    public class ShakeTypeExtended
    {
        public ShakeTypeExtended()
        {

        }
        public MaxVal MaxValues { get; set; }
        public int ShakeValue
        {
            get;
            set;
        }
        public string ShakeTypeStr { get; set; }
        public enum ShakeType
        {
            X = 0,
            Y = 1,
            Z = 2,
        }
    }
}
