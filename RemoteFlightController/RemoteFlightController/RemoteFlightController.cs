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
using System.Web;
using System.Web.Script.Serialization;

namespace RemoteFlightController
{
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

    public delegate ControlsUpdate GetControlsHandler();


    public partial class frmRemoteFlightController : Form
    {
        public RemoteFlightController Simulator;

        public frmRemoteFlightController()
        {
            InitializeComponent();
            Simulator = new RemoteFlightController(this);
        }

        public void UpdateTable(TelemetryUpdate data)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new UpdateRecievedHandler(UpdateTable), data);
            }
            else
            {
                dgvInputData.Rows.Insert(0, new object[] { data.Altitude,
                                                 data.Speed,
                                                 data.Pitch,
                                                 data.VerticleSpeed,
                                                 data.Throttle,
                                                 data.ElevatorPitch});
            }
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
                trbPitch.Enabled = true;
                trbThrottleControl.Enabled = true;
                btnConnectDisconnect.Text = "Disconnect";
                Simulator.Connect(txtServerIP.Text, txtServerPort.Text);
            }
            else if (btnConnectDisconnect.Text == "Disconnect")
            {
                txtServerIP.Enabled = true;
                txtServerPort.Enabled = true;
                trbPitch.Enabled = false;
                trbPitch.Value = 0;
                trbThrottleControl.Enabled = false;
                trbThrottleControl.Value = 0;
                btnConnectDisconnect.Text = "Connect";
                Simulator.Disconnect();
            }
        }
    }


    public class RemoteFlightController
    {
        public event UpdateSentHandler ControlUpdateSent;
        public event UpdateRecievedHandler TelemetryRecieved;
        public event ErrorRecievedHandler ErrorRecieved;

        public frmRemoteFlightController CurrentForm;
        public Thread listenerThread;
        public TcpClient client;
        public NetworkStream stream;
        public ControlsUpdate currentControls;

        public RemoteFlightController(frmRemoteFlightController CurrentForm)
        {
            this.CurrentForm = CurrentForm;

            ControlUpdateSent += new UpdateSentHandler(onUpdateSent);
            TelemetryRecieved += new UpdateRecievedHandler(onTelemetryRecieved);
            ErrorRecieved += new ErrorRecievedHandler(onErrorRecieved);
        }

        public void Connect(string ipAddress, string port)
        {
            listenerThread = new Thread(new ParameterizedThreadStart(ConnectionLoop));
            listenerThread.Start(new string[] { ipAddress, port });
        }

        public void Disconnect()
        {
            listenerThread.Abort();
            //client.Close(); <--- when the thread is aborted, it closes automatically (I think, check this properly)
        }

        private void ConnectionLoop(object address)
        {
            string ip = ((string[])address)[0];
            string port = ((string[])address)[1];

            client = new TcpClient();
            client.Connect(ip, int.Parse(port));
            stream = client.GetStream();

            int bufferSize = 256;
            byte[] buffer = new byte[bufferSize];
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            while (true)
            {
                int numBytes = stream.Read(buffer, 0, bufferSize);
                string message = Encoding.ASCII.GetString(buffer, 0, numBytes);

                TelemetryUpdate update = serializer.Deserialize<TelemetryUpdate>(message);
                TelemetryRecieved.Invoke(update);
            }
        }

        private void onUpdateSent(ControlsUpdate controlsUpdate)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string stringData = serializer.Serialize(controlsUpdate);
            byte[] rawData = Encoding.ASCII.GetBytes(stringData);

            stream.Write(rawData, 0, rawData.Length);
        }

        private void onTelemetryRecieved(TelemetryUpdate telemetryUpdate)
        {
            CurrentForm.UpdateTable(telemetryUpdate);
        }

        private void onErrorRecieved(string errormessage)
        {

        }
    }
}
