﻿using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Geared;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFScadaDashboard.DashboardConfigClasses;
using WPFScadaDashboard.DashboardDataPointClasses;
using WPFScadaDashboard.DataFetchers;

namespace WPFScadaDashboard.DashboardUserControls
{
    /// <summary>
    /// Interaction logic for LinePlotCellUC.xaml
    /// </summary>
    public partial class LinePlotCellUC : UserControl, ICellUC, INotifyPropertyChanged
    {
        // Declare the event
        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public LinePlotCellConfig LinePlotCellConfig_;
        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public Func<double, string> XFormatter { get; set; }
        public DateTime StartDateTime_ { get; set; }
        public double MeasPeriodSecs_ { get; set; } = 1;

        public string ZoomHeader
        {
            get
            {
                if (MyChart.Zoom == ZoomingOptions.Xy)
                {
                    return "Zoom (XY)";
                }
                else if (MyChart.Zoom == ZoomingOptions.Y)
                {
                    return "Zoom (Y)";
                }
                else if (MyChart.Zoom == ZoomingOptions.X)
                {
                    return "Zoom (X)";
                }
                else
                {
                    return "Zoom (Off)";
                }
            }
        }

        public string PanHeader
        {
            get
            {
                if (MyChart.Pan == PanningOptions.Xy)
                {
                    return "Pan (XY)";
                }
                else if (MyChart.Pan == PanningOptions.Y)
                {
                    return "Pan (Y)";
                }
                else if (MyChart.Pan == PanningOptions.X)
                {
                    return "Pan (X)";
                }
                else
                {
                    return "Pan (Off)";
                }
            }
        }

        // Send Messages to Dashboard using this event handler
        public event EventHandler<DashBoardEventArgs> Changed;

        protected virtual void OnChanged(DashBoardEventArgs e)
        {
            Changed?.Invoke(this, e);
        }

        private void DoInitialWireUp()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection();

            YFormatter = value => String.Format("{0:0.###}", value);
            XFormatter = delegate (double value)
            {
                if (StartDateTime_.Ticks != 0)
                {
                    DateTime timeStamp;
                    timeStamp = StartDateTime_.AddSeconds(value * MeasPeriodSecs_);
                    return timeStamp.ToString("HH:mm:ss.fff\ndd-MMM-yy");
                }
                return value.ToString();
            };
            MyChart.LegendLocation = LegendLocation.Top;

            // MyChart.DataTooltip = new ChartToolTipUC();

            DataContext = this;
            FetchAndPlotData();
        }

        // constructor
        public LinePlotCellUC()
        {
            DoInitialWireUp();
        }

        // constructor
        public LinePlotCellUC(LinePlotCellConfig linePlotCellConfig)
        {
            LinePlotCellConfig_ = linePlotCellConfig;
            DoInitialWireUp();
        }

        // Implementing Interface
        public IDashboardCellConfig GetDashboardCellConfig()
        {
            return LinePlotCellConfig_;
        }

        public string CellTitle { get { return LinePlotCellConfig_.Name_; } set { LinePlotCellConfig_.Name_ = value; } }

        public int RowIndex { get { return LinePlotCellConfig_.CellPosition_.RowIndex_; } }

        public int ColumnIndex { get { return LinePlotCellConfig_.CellPosition_.ColIndex_; } }

        public int RowSpan { get { return LinePlotCellConfig_.CellPosition_.RowSpan_; } }

        public int ColumnSpan { get { return LinePlotCellConfig_.CellPosition_.ColSpan_; } }

        public string CellHeight
        {
            get
            {
                if (LinePlotCellConfig_.GetHeightMode() == LinePlotCellConfig.VariableHeightMode)
                {
                    return "";
                }
                return LinePlotCellConfig_.Height_.ToString();
            }
        }

        public string CellWidth
        {
            get
            {
                if (LinePlotCellConfig_.GetWidthMode() == LinePlotCellConfig.VariableWidthMode)
                {
                    return "";
                }
                return LinePlotCellConfig_.Width_.ToString();
            }
        }

        public double CellMinHeight { get { return LinePlotCellConfig_.MinHeight_; } set { LinePlotCellConfig_.MinHeight_ = value; } }

        public double CellMinWidth { get { return LinePlotCellConfig_.MinWidth_; } set { LinePlotCellConfig_.MinWidth_ = value; } }

        public string CellHorizontalAlignment { get { return LinePlotCellConfig_.HorizontalAlignment_; } set { LinePlotCellConfig_.SetHorizontalAlignment(value); } }

        public string CellVerticalAlignment { get { return LinePlotCellConfig_.VerticalAlignment_; } set { LinePlotCellConfig_.SetVerticalAlignment(value); } }

        public void FetchAndPlotData()
        {
            // clear the current series of the plot
            SeriesCollection.Clear();
            double minXVal = 0;
            double maxXVal = double.NaN;
            double minYVal = double.NaN;
            double maxYVal = double.NaN;

            // get each point data and bind it to the Series Collection
            // iterate through each timeseries point in the config
            for (int i = 0; i < LinePlotCellConfig_.TimeSeriesPoints_.Count; i++)
            {
                IDashboardTimeSeriesPoint pnt = LinePlotCellConfig_.TimeSeriesPoints_.ElementAt(i);
                if (pnt.GetType().FullName == typeof(DashboardScadaTimeSeriesPoint).FullName)
                {
                    // handle if the point is a scada point
                    ScadaFetcher scadaFetcher = new ScadaFetcher();
                    DashboardScadaTimeSeriesPoint scadaTimeseriesPnt = (DashboardScadaTimeSeriesPoint)pnt;

                    // fetch the results from the data fetcher
                    List<ScadaPointResult> results = scadaFetcher.FetchHistoricalPointDataTest(scadaTimeseriesPnt);

                    // Get Plot Values from ScadaResults
                    List<double> plotVals = new List<double>();
                    for (int resCounter = 0; resCounter < results.Count; resCounter++)
                    {
                        ScadaPointResult res = results[resCounter];
                        plotVals.Add(res.Val_);
                    }

                    // change the plot values spacing in secs and the starting dateTime
                    if (i == 0)
                    {
                        MeasPeriodSecs_ = scadaTimeseriesPnt.FetchPeriodSecs_;
                        if (plotVals.Count > 0)
                        {
                            StartDateTime_ = new DateTime(results[0].ResultTime_.Ticks);
                        }
                    }

                    // Add the Data Point fetch results to the Plot lines Collection
                    SeriesCollection.Add(new GLineSeries() { Title = scadaTimeseriesPnt.ScadaPoint_.Name_, Values = new GearedValues<double>(plotVals), PointGeometry = null, Fill = Brushes.Transparent, StrokeThickness = 1, LineSmoothness = 0 });

                    // update min max Y Vals, max X val
                    double tempVal = plotVals.Count;
                    if (double.IsNaN(maxXVal) || maxXVal < tempVal)
                    {
                        maxXVal = tempVal;
                    }

                    tempVal = plotVals.Max();
                    if (double.IsNaN(maxYVal) || maxYVal < tempVal)
                    {
                        maxYVal = tempVal;
                    }

                    tempVal = plotVals.Min();
                    if (double.IsNaN(minYVal) || minYVal > tempVal)
                    {
                        minYVal = tempVal;
                    }
                }
            }
            // Set Axes limits
            MyChart.AxisX[0].MinValue = minXVal;
            MyChart.AxisX[0].MaxValue = maxXVal;
            MyChart.AxisY[0].MinValue = minYVal;
            MyChart.AxisY[0].MaxValue = maxYVal;
        }

        private void ResetAxes()
        {
            MyChart.AxisX[0].MinValue = double.NaN;
            MyChart.AxisX[0].MaxValue = double.NaN;
            MyChart.AxisY[0].MinValue = double.NaN;
            MyChart.AxisY[0].MaxValue = double.NaN;
            AddLinesToConsole("Reset Axis done...");
        }

        private void Zoom_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mItem = sender as MenuItem;
            mItem.IsChecked = true;
            foreach (MenuItem item in (mItem.Parent as MenuItem).Items)
            {
                if (item.Tag != mItem.Tag)
                {
                    item.IsChecked = false;
                }
            }
            switch (mItem.Tag.ToString())
            {
                case "ZXY":
                    MyChart.Zoom = ZoomingOptions.Xy;
                    AddLinesToConsole("Zoom mode set to XY");
                    break;
                case "ZX":
                    MyChart.Zoom = ZoomingOptions.X;
                    AddLinesToConsole("Zoom mode set to X");
                    break;
                case "ZY":
                    MyChart.Zoom = ZoomingOptions.Y;
                    AddLinesToConsole("Zoom mode set to Y");
                    break;
                case "ZOff":
                    MyChart.Zoom = ZoomingOptions.None;
                    AddLinesToConsole("Zoom mode set to Off");
                    break;
            }
            OnPropertyChanged("ZoomHeader");
        }

        private void AddLinesToConsole(string v)
        {
            // todo send events
            OnChanged(new DashBoardEventArgs(DashboardUC.ConsoleMessageTypeStr, v, LinePlotCellConfig_.Name_));
        }

        private void Pan_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mItem = sender as MenuItem;
            mItem.IsChecked = true;
            foreach (MenuItem item in (mItem.Parent as MenuItem).Items)
            {
                if (item.Tag != mItem.Tag)
                {
                    item.IsChecked = false;
                }
            }
            switch (mItem.Tag.ToString())
            {
                case "PXY":
                    MyChart.Pan = PanningOptions.Xy;
                    AddLinesToConsole("Pan mode set to XY");
                    break;
                case "PX":
                    MyChart.Pan = PanningOptions.X;
                    AddLinesToConsole("Pan mode set to X");
                    break;
                case "PY":
                    MyChart.Pan = PanningOptions.Y;
                    AddLinesToConsole("Pan mode set to Y");
                    break;
                case "POff":
                    MyChart.Pan = PanningOptions.None;
                    AddLinesToConsole("Pan mode set to None");
                    break;
            }
            OnPropertyChanged("PanHeader");
        }

        private void FetchBtn_Click(object sender, RoutedEventArgs e)
        {
            FetchAndPlotData();
            AddLinesToConsole("Data fetched");
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            ResetAxes();
        }
    }
}

/*
 todo handle x and y formatters
 todo keep customizable legend locations by binding
 todo handle zoom, pan, disable Animations, hoverable by binding so that they can be defined in config
 todo use labels for each time series since the the plots may be aligned but the timestamps may be different
 
 The constraints faced here is that since we are using geared values, the periodicity of all the fetched series should be same.
     */
