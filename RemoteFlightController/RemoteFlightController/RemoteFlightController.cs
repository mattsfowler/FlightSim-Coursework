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


    public partial class RemoteFlightController : Form
    {
        public event UpdateSentHandler ControlUpdateSent;
        public event UpdateRecievedHandler TelemetryRecieved;
        public event ErrorRecievedHandler ErrorRecieved;

        public RemoteFlightController CurrentForm;
        public Thread listenerThread;
        public TcpClient client;
        public NetworkStream stream;
        public TelemetryUpdate currentTelem;

        public RemoteFlightController()
        {
            InitializeComponent();
            txtServerPort.Text = "9999";

            IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] address = ipHostEntry.AddressList;
            txtServerIP.Text = address[4].ToString();

            currentTelem = new TelemetryUpdate();

            ControlUpdateSent += new UpdateSentHandler(onUpdateSent);
            TelemetryRecieved += new UpdateRecievedHandler(onTelemetryRecieved);
            ErrorRecieved += new ErrorRecievedHandler(onErrorRecieved);
        }

        public void UpdateTable(TelemetryUpdate data)
        {
            dgvInputData.Rows.Insert(0, new object[] { data.Altitude,
                                                data.Speed,
                                                data.Pitch,
                                                data.VerticleSpeed,
                                                data.Throttle,
                                                data.ElevatorPitch,
                                                data.WarningCode});

            if (data.WarningCode == 0)
            {
                lblErrorDisplay.ForeColor = Color.Black;
                lblErrorDisplay.Text = "(no errors)";
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
                listenerThread = new Thread(new ParameterizedThreadStart(ConnectionLoop));
                listenerThread.Start(new string[] { txtServerIP.Text, txtServerPort.Text });
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
                listenerThread.Abort();
                //client.Close(); <--- when the thread is aborted, it closes automatically (I think, check this properly)
            }
        }

        private void trbThrottleControl_Scroll(object sender, EventArgs e)
        {
            ControlsUpdate update = new ControlsUpdate();
            update.Throttle = (trbThrottleControl.Value / 10f);
            update.ElevatorPitch = (trbPitch.Value / 10f);
            ControlUpdateSent.Invoke(update);
        }

        private void trbPitch_Scroll(object sender, EventArgs e)
        {
            ControlsUpdate update = new ControlsUpdate();
            update.Throttle = (trbThrottleControl.Value / 10f);
            update.ElevatorPitch = (trbPitch.Value / 10f);
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
                txtJSON.Text = message;

                TelemetryUpdate update = serializer.Deserialize<TelemetryUpdate>(message);
                TelemetryRecieved.Invoke(update);
            }
        }

        private void onUpdateSent(ControlsUpdate controlsUpdate)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new UpdateSentHandler(onUpdateSent), controlsUpdate);
            }
            else
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string stringData = serializer.Serialize(controlsUpdate);
                byte[] rawData = Encoding.ASCII.GetBytes(stringData);

                dgvSentData.Rows.Insert(0, new object[] { controlsUpdate.Throttle,
                                        controlsUpdate.ElevatorPitch }
                );
                stream.Write(rawData, 0, rawData.Length);
            }
        }

        private void onTelemetryRecieved(TelemetryUpdate telemetryUpdate)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new UpdateRecievedHandler(onTelemetryRecieved), telemetryUpdate);
            }
            else
            {
                if ((currentTelem.Altitude != telemetryUpdate.Altitude)
                 || (currentTelem.ElevatorPitch != telemetryUpdate.ElevatorPitch)
                 || (currentTelem.Pitch != telemetryUpdate.Pitch)
                 || (currentTelem.Speed != telemetryUpdate.Speed)
                 || (currentTelem.Throttle != telemetryUpdate.Throttle)
                 || (currentTelem.VerticleSpeed != telemetryUpdate.VerticleSpeed)
                 || (currentTelem.WarningCode != telemetryUpdate.WarningCode))
                {
                    UpdateTable(telemetryUpdate);
                }

                if (telemetryUpdate.WarningCode != 0)
                {
                    string message = "Unknown warning!";

                    if (telemetryUpdate.WarningCode == 1)
                    {
                        message = "WARNING: Low altitude!";
                    }
                    else if (telemetryUpdate.WarningCode == 2)
                    {
                        message = "WARNING: Stall risk!";
                    }

                    ErrorRecieved.Invoke(message);
                }
                else
                {
                    lblErrorDisplay.ForeColor = Color.Black;
                    lblErrorDisplay.Text = "(no errors)";
                }

                currentTelem = telemetryUpdate;
            }
        }

        private void onErrorRecieved(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ErrorRecievedHandler(onErrorRecieved), message);
            }
            else
            {
                lblErrorDisplay.ForeColor = Color.Red;
                lblErrorDisplay.Text = message;
            }
        }
    }
}
