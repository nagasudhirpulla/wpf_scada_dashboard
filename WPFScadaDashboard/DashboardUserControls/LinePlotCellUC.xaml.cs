using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Geared;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

        public void UpdateCellPosition()
        {
            //todo update width, height and respective modes also
            OnPropertyChanged("RowIndex");
            OnPropertyChanged("ColumnIndex");
            OnPropertyChanged("RowSpan");
            OnPropertyChanged("ColumnSpan");
            OnPropertyChanged("CellWidth");
            OnPropertyChanged("CellHeight");
            OnPropertyChanged("CellMinWidth");
            OnPropertyChanged("CellMinHeight");
            OnPropertyChanged("CellHorizontalAlignment");
            OnPropertyChanged("CellVerticalAlignment");
        }

        public void DeleteCell()
        {
            // Send message to dashboard to delete this cell
            OnChanged(new DashBoardEventArgs(DashboardUC.DeleteCellMessageTypeStr, LinePlotCellConfig_.Name_));
        }

        // ***Declare a System.Threading.CancellationTokenSource.
        CancellationTokenSource cts;

        public LinePlotCellConfig LinePlotCellConfig_ { get; set; }
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
        public event EventHandler<EventArgs> Changed;

        protected virtual void OnChanged(EventArgs e)
        {
            Changed?.Invoke(this, e);
        }

        private async void DoInitialWireUp()
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
            await FetchAndPlotData();
            ResetAxes();
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
                if (LinePlotCellConfig_.HeightMode_ == LinePlotCellConfig.VariableHeightMode)
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
                if (LinePlotCellConfig_.WidthMode_ == LinePlotCellConfig.VariableWidthMode)
                {
                    return "";
                }
                return LinePlotCellConfig_.Width_.ToString();
            }
        }

        public double CellMinHeight { get { return LinePlotCellConfig_.MinHeight_; } set { LinePlotCellConfig_.MinHeight_ = value; } }

        public double CellMinWidth { get { return LinePlotCellConfig_.MinWidth_; } set { LinePlotCellConfig_.MinWidth_ = value; } }

        public string CellHorizontalAlignment { get { return LinePlotCellConfig_.HorizontalAlignment_; } set { LinePlotCellConfig_.HorizontalAlignment_ = value; } }

        public string CellVerticalAlignment { get { return LinePlotCellConfig_.VerticalAlignment_; } set { LinePlotCellConfig_.VerticalAlignment_ = value; } }

        public async Task FetchAndPlotData()
        {
            // stop running fetch tasks
            if (cts != null)
            {
                //cts.Cancel();
                return;
            }

            // ***Instantiate the CancellationTokenSource.
            cts = new CancellationTokenSource();

            // clear the current series of the plot
            SeriesCollection.Clear();

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
                    List<ScadaPointResult> results = new List<ScadaPointResult>();

                    try
                    {
                        // ***Send a token to carry the message if cancellation is requested.
                        results = await scadaFetcher.FetchHistoricalPointDataAsync(scadaTimeseriesPnt, cts.Token);

                    }
                    // *** If cancellation is requested, an OperationCanceledException results.
                    catch (OperationCanceledException)
                    {
                        AddLinesToConsole("Existing Fetch task cancelled");
                    }
                    catch (Exception e)
                    {
                        AddLinesToConsole($"Error in running fetch task: {e.Message}");
                    }


                    if (results == null)
                    {
                        // handle the null results by printing in the console
                        AddLinesToConsole("No values returned after fetching");
                        results = new List<ScadaPointResult>();
                    }

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
                }
            }

            // ***Set the CancellationTokenSource to null when the download is complete.
            cts = null;
        }

        public void ResetAxes()
        {
            // If no line series are present, then use Double.NaN for resetting the axis
            if (SeriesCollection == null || SeriesCollection.Count == 0)
            {
                MyChart.AxisX[0].MinValue = double.NaN;
                MyChart.AxisX[0].MaxValue = double.NaN;
                MyChart.AxisY[0].MinValue = double.NaN;
                MyChart.AxisY[0].MaxValue = double.NaN;
            }
            else
            {
                // get the first sample of all the lineseries, add +-10 for Y max/min and , lineSeries length for X axis max/min
                double maxYVal = double.NaN;
                double minYVal = double.NaN;
                double numXSamples = 100;
                // find the number of samples present
                numXSamples = SeriesCollection.ElementAt(0).Values.Count;
                for (int i = 0; i < SeriesCollection.Count; i++)
                {
                    //double maxSeriesVal = ((LiveCharts.Geared.GearedValues<float>)SeriesCollection.ElementAt(i).Values).Max();
                    //double minSeriesVal = ((LiveCharts.Geared.GearedValues<float>)SeriesCollection.ElementAt(i).Values).Min();
                    LiveCharts.Geared.GearedValues<double> SeriesValues = ((LiveCharts.Geared.GearedValues<double>)SeriesCollection.ElementAt(i).Values);
                    if (SeriesValues.Count < 1)
                    {
                        continue;
                    }
                    double maxSeriesVal = SeriesValues.Max();
                    if (double.IsNaN(maxSeriesVal))
                    {
                        maxSeriesVal = SeriesValues.First();
                    }

                    double minSeriesVal = SeriesValues.Min();
                    if (double.IsNaN(minSeriesVal))
                    {
                        minSeriesVal = SeriesValues.First();
                    }

                    if (double.IsNaN(minYVal))
                    {
                        minYVal = minSeriesVal;
                    }

                    if (double.IsNaN(maxYVal))
                    {
                        maxYVal = maxSeriesVal;
                    }

                    if (!double.IsNaN(minSeriesVal) && minYVal > minSeriesVal)
                    {
                        minYVal = minSeriesVal;
                    }
                    if (!double.IsNaN(maxSeriesVal) && maxYVal < maxSeriesVal)
                    {
                        maxYVal = maxSeriesVal;
                    }
                }
                double valDiff = 0;
                if (!double.IsNaN(maxYVal) && !double.IsNaN(minYVal))
                {
                    valDiff = maxYVal - minYVal;
                    //MyChart.AxisX[0].SetRange(-numXSamples * 0.1, numXSamples * 1.1);
                    //MyChart.AxisY[0].SetRange(minYVal - valDiff * 0.1, maxYVal + valDiff * 0.1);
                    MyChart.AxisX[0].MinValue = -numXSamples * 0.1;
                    MyChart.AxisX[0].MaxValue = numXSamples * 1.1;
                    MyChart.AxisY[0].MinValue = minYVal - valDiff * 0.1;
                    MyChart.AxisY[0].MaxValue = maxYVal + valDiff * 0.1;
                }
                else
                {
                    MyChart.AxisX[0].MinValue = double.NaN;
                    MyChart.AxisX[0].MaxValue = double.NaN;
                    MyChart.AxisY[0].MinValue = double.NaN;
                    MyChart.AxisY[0].MaxValue = double.NaN;
                }
            }
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

        private async void FetchBtn_Click(object sender, RoutedEventArgs e)
        {
            await FetchAndPlotData();
            ResetAxes();
            AddLinesToConsole("Data fetched");
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            ResetAxes();
            AddLinesToConsole("Reset axes done");
        }

        private void ConfigPosition_Click(object sender, RoutedEventArgs e)
        {
            // todo create a cell position config window
            OnChanged(new CellPosChangeReqArgs());
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            // https://www.flaticon.com/free-icon/rubbish-bin-delete-button_60761#term=delete&page=1&position=9
            DeleteCell();
        }

        private async void ConfigDataPoints_Click(object sender, RoutedEventArgs e)
        {
            // todo create configure data points window to get the result and update changes after add/edit/remove points
            DataPointsConfigWindow dataPointsConfigWindow = new DataPointsConfigWindow(LinePlotCellConfig_);
            if (dataPointsConfigWindow.ShowDialog() == true)
            {
                LinePlotCellConfig_.TimeSeriesPoints_ = new List<IDashboardTimeSeriesPoint>(dataPointsConfigWindow.dataPointsVM.dashboardTimeSeriesPoints);
                await FetchAndPlotData();
            }
        }

        public async void RefreshCell()
        {
            await FetchAndPlotData();
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
