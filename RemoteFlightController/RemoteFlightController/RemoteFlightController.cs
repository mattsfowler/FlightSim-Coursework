using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace RemoteFlightController
{
    public partial class frmRemoteFlightController : Form
    {
        public RemoteFlightController Simulator;
        public Thread workerThread;

        public frmRemoteFlightController()
        {
            InitializeComponent();
            Simulator = new RemoteFlightController();
        }

        private void RemoteFlightController_Load(object sender, EventArgs e)
        {
            
        }

        private void btnConnectDisconnect_Click(object sender, EventArgs e)
        {
            if (btnConnectDisconnect.Text == "Connect")
            {
                txtServerIP.Enabled = false;
                txtServerPort.Enabled = false;
                btnConnectDisconnect.Text = "Disconnect";

                workerThread = new Thread(new ThreadStart(ConnectToSimulator));

                workerThread.Start();
            }
            else if (btnConnectDisconnect.Text == "Disconnect")
            {
                workerThread.Abort();
            }
        }

        public void ConnectToSimulator()
        {
            string ip = txtServerIP.Text;
            string port = txtServerPort.Text;

            TcpClient client = new TcpClient();
            client.Connect(ip, int.Parse(port));
            NetworkStream stream = client.GetStream();

            client.Close();
        }
    }


    public struct ControlsUpdate
    {
        public double Throttle;
        public double ElevatorPitch;
    }

    public struct TelemetryUpdate
    {
        public double Altitude;         //Altitude in ft.
        public double Speed;            //Plane's speed in Knts.
        public double Pitch;            //Plane's pitch in degrees relative to horizon. Positive is planes pointing upwards, negative plane points downwards;
        public double VerticleSpeed;    //Plane's vertical speed in Feet per minute.
        public double Throttle;         //Current throttle setting as a percentage
        public double ElevatorPitch;    //Current Elevator Pitch in degrees. Positive creates upwards lift, negative downwards.
        public int WarningCode;         //Warning code: 0 - No Warnings; 1 -  Too Low (less than 1000ft); 2 - Stall.
    }


    public delegate void UpdateSentHandler(ControlsUpdate controlsUpdate);
    public delegate void UpdateRecievedHandler(TelemetryUpdate telemetryUpdate);
    public delegate void ErrorRecievedHandler(string errormessage);


    public class RemoteFlightController
    {
        public event UpdateSentHandler ControlUpdateSent;
        public event UpdateRecievedHandler TelemetryRecieved;
        public event ErrorRecievedHandler ErrorRecieved;

        public Thread workerThread;
        public TcpClient client;
        public NetworkStream stream;

        public RemoteFlightController()
        {
            ControlUpdateSent += new UpdateSentHandler(onUpdateSent);
            TelemetryRecieved += new UpdateRecievedHandler(onTelemetryRecieved);
            ErrorRecieved += new ErrorRecievedHandler(onErrorRecieved);
        }

        private void onUpdateSent(ControlsUpdate controlsUpdate)
        {

        }

        private void onTelemetryRecieved(TelemetryUpdate telemetryUpdate)
        {

        }

        private void onErrorRecieved(string errormessage)
        {

        }
    }
}
