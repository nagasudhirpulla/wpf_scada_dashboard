using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFScadaDashboard.DashboardDataPointClasses;
using WPFScadaDashboard.JSONConverters;

namespace WPFScadaDashboard.DashboardConfigClasses
{
    public class LinePlotCellConfig : IDashboardCellConfig
    {
        public const string cellType = "Time_Series_Line_Plot";
        // Implemening Interface
        public string VizType_ { get { return cellType; } set { } }
        public const string AbsoluteWidthMode = "absolute";
        public const string AbsoluteHeightMode = "absolute";
        public const string VariableWidthMode = "variable";
        public const string VariableHeightMode = "variable";
        public const string AlignmentModeStretch = "Stretch";
        public const string AlignmentModeCenter = "Center";
        public const string AlignmentModeTop = "Top";
        public const string AlignmentModeBottom = "Bottom";
        public const string AlignmentModeLeft = "Left";
        public const string AlignmentModeRight = "Right";
        private string widthMode = VariableWidthMode;
        private string heightMode = VariableHeightMode;
        private string horizontalAlignment = AlignmentModeStretch;
        private string verticalAlignment = AlignmentModeStretch;

        public string Name_ { get; set; } = "Line Plot Cell";

        public double MinWidth_ { get; set; } = 0;

        public double MinHeight_ { get; set; } = 0;

        public double Width_ { get; set; } = 100;

        public double Height_ { get; set; } = 100;

        public string WidthMode_
        {
            get { return widthMode; }
            set
            {
                if (value != AbsoluteWidthMode && value != VariableWidthMode)
                {
                    widthMode = VariableWidthMode;
                    return;
                }
                widthMode = value;
            }
        }

        public string HeightMode_
        {
            get { return heightMode; }
            set
            {
                if (value != AbsoluteHeightMode && value != VariableHeightMode)
                {
                    heightMode = VariableHeightMode;
                    return;
                }
                heightMode = value;
            }
        }

        public string HorizontalAlignment_
        {
            get { return horizontalAlignment; }
            set
            {
                if (value != AlignmentModeStretch && value != AlignmentModeLeft && value != AlignmentModeRight && value != AlignmentModeCenter)
                {
                    horizontalAlignment = AlignmentModeStretch;
                    return;
                }
                horizontalAlignment = value;
            }
        }

        public string VerticalAlignment_
        {
            get { return verticalAlignment; }
            set
            {
                if (value != AlignmentModeStretch && value != AlignmentModeTop && value != AlignmentModeBottom && value != AlignmentModeCenter)
                {
                    verticalAlignment = AlignmentModeStretch;
                    return;
                }
                verticalAlignment = value;

            }
        }

        public double CellWidth_ { get { return Width_; } set { Width_ = value; } }

        public double CellHeight_ { get { return Height_; } set { Height_ = value; } }

        // Cell Position
        public DashboardCellPosition CellPosition_ { get; set; } = new DashboardCellPosition();

        // Data Points of the plots that are used for fetching data for line plots
        [JsonConverter(typeof(DashboardTimeSeriesPointConverter))]
        public List<IDashboardTimeSeriesPoint> TimeSeriesPoints_ { get; set; } = new List<IDashboardTimeSeriesPoint>();

        // constructor
        public LinePlotCellConfig()
        {
        }

        // Implementing Interface
        public List<IDataPoint> GetCellDataPoints()
        {
            List<IDataPoint> dataPoints = new List<IDataPoint>();
            for (int i = 0; i < TimeSeriesPoints_.Count; i++)
            {
                dataPoints.Add(TimeSeriesPoints_.ElementAt(i).GetDataPoint());
            }
            return dataPoints;
        }

    }
}
