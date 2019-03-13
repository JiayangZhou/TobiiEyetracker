using System;
using System.Collections.Generic;
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
using System.Threading;
using Tobii.Research;

namespace EyeTackingA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Uri trackerUri = new Uri("tet-tcp://172.28.195.1/");
        private static IEyeTracker myTracker = EyeTrackingOperations.GetEyeTracker(trackerUri);
        private static ScreenBasedCalibration calibration = new ScreenBasedCalibration(myTracker);

        public MainWindow()
        {
            InitializeComponent();
            

        }
        
        private void FindTackers()
        {
            //Uri trackerUri = new Uri("tet-tcp://172.28.195.1/");
            //IEyeTracker myTracker = EyeTrackingOperations.GetEyeTracker(trackerUri);
            //IEyeTracker myTracker = EyeTrackingOperations.FindAllEyeTrackers().First();           
            
        }


        private void CalibrateEnter_Click(object sender, RoutedEventArgs e)
        {
            CalibrationMode();

        }
        private void CalibrationMode()
        {
            calibration.EnterCalibrationMode();
            var pointsToCalibrate = new NormalizedPoint2D[] {
                new NormalizedPoint2D(0.5f, 0.5f),
                new NormalizedPoint2D(0.1f, 0.1f),
                new NormalizedPoint2D(0.1f, 0.9f),
                new NormalizedPoint2D(0.9f, 0.1f),
                new NormalizedPoint2D(0.9f, 0.9f),
            };

            foreach (var point in pointsToCalibrate)
            {
                //this.Dispatcher.Invoke(() =>
                //{
                //    Connect.Text = $"Please look at {point.X},{point.Y}.";
                //});
                //CoPoint.Text = $"Please look at {point.X},{point.Y}.";

                //ThreadPool.QueueUserWorkItem((o) =>
                //{
                    
                //});
                Dispatcher.Invoke((Action)(() => Connect.Text = $"Please look at {point.X},{point.Y}."));
                System.Threading.Thread.Sleep(1000);

                CalibrationStatus status = calibration.CollectData(point);
                if (status != CalibrationStatus.Success)
                {
                    calibration.CollectData(point);
                }
            }
            CalibrationResult calibrationResult = calibration.ComputeAndApply();
            CoPoint.Text = $"Compute and apply returned {calibrationResult.Status} and collected at {calibrationResult.CalibrationPoints.Count} points.";
            Thread.Sleep(1000);
            //calibration.DiscardData(new NormalizedPoint2D(0.1f, 0.1f));
            //calibration.CollectData(new NormalizedPoint2D(0.1f, 0.1f));
            //calibrationResult = calibration.ComputeAndApply();

            calibration.LeaveCalibrationMode();
        }

        private void CalibrateLeave_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (myTracker != null)
            {
                GazeData(myTracker);
            }
            
        }
        private void GazeData(IEyeTracker eyeTracker)
        {
            // Start listening to gaze data.
            eyeTracker.GazeDataReceived += EyeTracker_GazeDataReceived;
            // Wait for some data to be received.
            System.Threading.Thread.Sleep(2000);
            // Stop listening to gaze data.
            eyeTracker.GazeDataReceived -= EyeTracker_GazeDataReceived;
        }
        private void EyeTracker_GazeDataReceived(object sender, GazeDataEventArgs e)
        {
            //this.Dispatcher.Invoke(() =>
            //{
            //    Connect.Text = $"Got gaze data with {e.LeftEye.GazeOrigin.Validity} left eye origin at point ({e.LeftEye.GazeOrigin.PositionInUserCoordinates.X}," +
            //               $" {e.LeftEye.GazeOrigin.PositionInUserCoordinates.Y}, {e.LeftEye.GazeOrigin.PositionInUserCoordinates.Z}) in the user coordinate system.";
            //});

        }


    }
}
