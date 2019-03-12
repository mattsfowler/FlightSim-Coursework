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
    public delegate void ErrorRecievedHandler(string message);

    public delegate ControlsUpdate GetControlsHandler();


    public partial class frmRemoteFlightController : Form
    {
        public RemoteFlightController Simulator;

        public frmRemoteFlightController()
        {
            InitializeComponent();
            Simulator = new RemoteFlightController(this);
            txtServerPort.Text = "9999";

            IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] address = ipHostEntry.AddressList;
            txtServerIP.Text = address[4].ToString();
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
                                                 data.ElevatorPitch,
                                                 data.WarningCode});
            }
        }

        public void ShowError(string message)
        {
            if (this.InvokeRequired)
            {
                Invoke(new ErrorRecievedHandler(ShowError), message);
            }
            else
            {
                if (message == "(no errors)")
                {
                    lblErrorDisplay.ForeColor = Color.Black;
                }
                else
                {
                    lblErrorDisplay.ForeColor = Color.Red;
                }

                lblErrorDisplay.Text = message;
            }
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

        private void trbThrottleControl_Scroll(object sender, EventArgs e)
        {
            Simulator.ControlDataChanged((trbThrottleControl.Value / 10f), (trbPitch.Value / 10f));
        }

        private void trbPitch_Scroll(object sender, EventArgs e)
        {
            Simulator.ControlDataChanged((trbThrottleControl.Value / 10f), (trbPitch.Value / 10f));
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
        public TelemetryUpdate currentTelem;

        public RemoteFlightController(frmRemoteFlightController CurrentForm)
        {
            this.CurrentForm = CurrentForm;
            currentTelem = new TelemetryUpdate();

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

        public void ControlDataChanged(double throttle, double elevatorPitch)
        {
            ControlsUpdate update = new ControlsUpdate();
            update.Throttle = throttle;
            update.ElevatorPitch = elevatorPitch;
            ControlUpdateSent.Invoke(update);
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
            if ((currentTelem.Altitude != telemetryUpdate.Altitude)
             || (currentTelem.ElevatorPitch != telemetryUpdate.ElevatorPitch)
             || (currentTelem.Pitch != telemetryUpdate.Pitch)
             || (currentTelem.Speed != telemetryUpdate.Speed)
             || (currentTelem.Throttle != telemetryUpdate.Throttle)
             || (currentTelem.VerticleSpeed != telemetryUpdate.VerticleSpeed)
             || (currentTelem.WarningCode != telemetryUpdate.WarningCode))
            {
                CurrentForm.UpdateTable(telemetryUpdate);
            }

            if (currentTelem.WarningCode != telemetryUpdate.WarningCode)
            {
                string message = "Unknown warning!";

                if (telemetryUpdate.WarningCode == 0)
                {
                    message = "(no errors)";
                }
                else if (telemetryUpdate.WarningCode == 1)
                {
                    message = "WARNING: Low altitude!";
                }
                else if (telemetryUpdate.WarningCode == 2)
                {
                    message = "WARNING: Stall risk!";
                }

                ErrorRecieved.Invoke(message);
            }

            currentTelem = telemetryUpdate;
        }

        private void onErrorRecieved(string message)
        {
            CurrentForm.ShowError(message);
        }
    }
}
