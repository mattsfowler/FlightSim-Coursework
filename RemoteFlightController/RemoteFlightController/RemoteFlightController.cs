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

/*
 AUTHOR:        Matthew Fowler
 STUDENT ID:    17025958
 EMAIL:         17025958@students.southwales.ac.uk
*/

namespace RemoteFlightController
{
    /*used to send data from the controller to the simulator*/
    public struct ControlsUpdate
    {
        public double Throttle;
        public double ElevatorPitch;
    }

    /*used to recieve data from the simulator*/
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


    /*These delegates define the types of function that can handle certain events.
     For example, any function that handles sending data to the simulator will
     need to take a ControlsUpdate as a parameter.*/
    public delegate void UpdateSentHandler(ControlsUpdate controlsUpdate);
    public delegate void UpdateRecievedHandler(TelemetryUpdate telemetryUpdate);
    public delegate void ErrorRecievedHandler(string message);

    /// <summary>
    /// When the connect button is pressed, a worker thread is spun off that
    /// listens for incomming JSON data
    /// </summary>
    public partial class RemoteFlightController : Form
    {
        public event UpdateSentHandler ControlUpdateSent;
        public event UpdateRecievedHandler TelemetryRecieved;
        public event ErrorRecievedHandler ErrorRecieved;

        public Thread listenerThread;
        public TcpClient client;
        public NetworkStream stream;
        public TelemetryUpdate currentTelem;

        public RemoteFlightController()
        {
            //required for Windows Form Apps
            InitializeComponent();

            //contains the last telemetry data recieved (used to work out if repeated data has been sent)
            currentTelem = new TelemetryUpdate();

            //assign concrete methods to handle relevent events:
            ControlUpdateSent += new UpdateSentHandler(onUpdateSent);
            TelemetryRecieved += new UpdateRecievedHandler(onTelemetryRecieved);
            ErrorRecieved += new ErrorRecievedHandler(onErrorRecieved);

            txtServerPort.Text = "9999";    //default port number
            //automatically get this machine's IP address, and load it into the textbox:
            IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] address = ipHostEntry.AddressList;
            txtServerIP.Text = address[4].ToString();
        }

        /* -------------------- HELPER METHODS -------------------- */

        /*A helper method that adds the last telemetry data to the form's table*/
        public void UpdateTable(TelemetryUpdate data)
        {
            //using the Insert(0, ... method instead of the Add(... method adds new rows to the top
            //of the grid instead of the bottom, meaning the most recent data is always visable.
            dgvInputData.Rows.Insert(0, new object[] { data.Altitude,
                                                data.Speed,
                                                data.Pitch,
                                                data.VerticleSpeed,
                                                data.Throttle,
                                                data.ElevatorPitch,
                                                data.WarningCode});

            //if the error code is 0, it means there are no errors, so reset the error display
            if (data.WarningCode == 0)
            {
                lblErrorDisplay.ForeColor = Color.Black;
                lblErrorDisplay.Text = "(no errors)";
            }
        }

        public void UpdateJSONDisplay(TelemetryUpdate data)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            txtJSON.Text = serializer.Serialize(data);
        }

        /*A helper method that takes a string array containing an IP and a port number, and
         continually listens for incomming JSON data, invoking relevent events when it
         does recieve data.*/
        private void ConnectionLoop(object address)
        {
            //convert the generic object input into the two strings it will contain
            string ip = ((string[])address)[0];
            string port = ((string[])address)[1];

            //create a new client object
            client = new TcpClient();
            client.Connect(ip, int.Parse(port));
            //the stream is used to send and recieve bytes
            stream = client.GetStream();

            //set up the serialiser - the stream will recieve raw bytes and so
            byte[] buffer = new byte[256];
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            int numBytes = 0;

            //the loop runs indefinitely and does three things each cycle:
            //1. waits for data to be sent to the network stream, and reads it
            //2. deserialises the data, interpreting it as a TelemetryUpdate structure
            //3. triggers the telemetryRecieved event that deals with the data
            while (true)
            {
                //Read is a blocking method - the thread will pause here until data is recieved
                numBytes = stream.Read(buffer, 0, 256);
                //the data from the stream will be raw bytes - convert it to a string for the serialiser
                string message = Encoding.ASCII.GetString(buffer, 0, numBytes);

                TelemetryUpdate update = serializer.Deserialize<TelemetryUpdate>(message);
                TelemetryRecieved.Invoke(update);
            }
        }

        /* -------------------- FORM EVENT HANDLERS -------------------- */

        /*Functions as both the disconnect and connect button - it starts as connect, but toggles
         each time it is pressed.*/
        private void btnConnectDisconnect_Click(object sender, EventArgs e)
        {
            if (btnConnectDisconnect.Text == "Connect")
            {
                //dissable the text fields so that the user cannot enter a new address once it is connected
                txtServerIP.Enabled = false;
                txtServerPort.Enabled = false;
                //enable the controls so that the user can send updates to the simulator
                trbPitch.Enabled = true;
                trbThrottleControl.Enabled = true;
                //toggle the button to a disconnect button
                btnConnectDisconnect.Text = "Disconnect";
                //spin off a new thread to listen for incomming data (listening is done by a seperate
                //worker thread so that the form does not become unresponsive)
                listenerThread = new Thread(new ParameterizedThreadStart(ConnectionLoop));
                listenerThread.Start(new string[] { txtServerIP.Text, txtServerPort.Text });
            }
            else if (btnConnectDisconnect.Text == "Disconnect")
            {
                //re-enable the text fields so that the user can alter their connection if they want
                txtServerIP.Enabled = true;
                txtServerPort.Enabled = true;
                //dissable and reset the controls to their default positions
                trbPitch.Enabled = false;
                trbPitch.Value = 0;
                trbThrottleControl.Enabled = false;
                trbThrottleControl.Value = 0;
                //toggle the button to a connect button
                btnConnectDisconnect.Text = "Connect";
                //stop the listening thread and close the connection to the simulator
                listenerThread.Abort();
                client.Close();
            }
        }

        /*Each time the control is updated by the user, it triggers the "ControlUpdateSent" event
         (the same event is used by both controls)*/
        private void trbPitch_ThrottleControl_Scroll(object sender, EventArgs e)
        {
            ControlsUpdate update = new ControlsUpdate();
            //the control's values are 10 times larger to allow greater accuracy
            update.Throttle = (trbThrottleControl.Value / 10f);
            update.ElevatorPitch = (trbPitch.Value / 10f);
            //invoke the event to deal with sending updates to the simulator
            ControlUpdateSent.Invoke(update);
        }

        /* -------------------- CONTROLLER EVENT HANDLERS -------------------- */

        /*The InvokeRequired sections are needed for cross-thread method calls: 
         if the method that invoked the method is on a different thread, it
         cannot directly call the method on this thread (it must be invoked
         instead)*/

        /*Deals with sending updates to the simulator in the correct format*/
        private void onUpdateSent(ControlsUpdate controlsUpdate)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new UpdateSentHandler(onUpdateSent), controlsUpdate);
            }
            else
            {
                //the data to send must be sent as a raw byte stream - to do this we
                //convert the data to a JSON string, which is the format that the
                //simulator is expecting.
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string stringData = serializer.Serialize(controlsUpdate);
                byte[] rawData = Encoding.ASCII.GetBytes(stringData);

                //update the display on this form with the data we just sent
                dgvSentData.Rows.Insert(0, new object[] { controlsUpdate.Throttle,
                                        controlsUpdate.ElevatorPitch }
                );

                //send the bytes to the network stream
                stream.Write(rawData, 0, rawData.Length);
            }
        }

        /*Deals with updating the form with the data provided, and with triggering the
         error recieved handler if required*/
        private void onTelemetryRecieved(TelemetryUpdate telemetryUpdate)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new UpdateRecievedHandler(onTelemetryRecieved), telemetryUpdate);
            }
            else
            {
                //in English: if the telemetry of the plane has changed since the last update
                if ((currentTelem.Altitude != telemetryUpdate.Altitude)
                 || (currentTelem.ElevatorPitch != telemetryUpdate.ElevatorPitch)
                 || (currentTelem.Pitch != telemetryUpdate.Pitch)
                 || (currentTelem.Speed != telemetryUpdate.Speed)
                 || (currentTelem.Throttle != telemetryUpdate.Throttle)
                 || (currentTelem.VerticleSpeed != telemetryUpdate.VerticleSpeed)
                 || (currentTelem.WarningCode != telemetryUpdate.WarningCode))
                {
                    //add the recieved telemetry to the table, and the raw JSON string to a textbox
                    UpdateTable(telemetryUpdate);
                    UpdateJSONDisplay(telemetryUpdate);
                }

                //if the simulator sent a warning code other that 0, we need to handle the error
                if (telemetryUpdate.WarningCode != 0)
                {
                    //default message - will be displayed if a warning code other that 1 or 2 was given
                    string message = "Unknown warning!";

                    //set the message to the appropriate value based on the error code
                    if (telemetryUpdate.WarningCode == 1)
                    {
                        message = "WARNING: Low altitude!";
                    }
                    else if (telemetryUpdate.WarningCode == 2)
                    {
                        message = "WARNING: Stall risk!";
                    }

                    //trigger the error handling event to deal with the error
                    ErrorRecieved.Invoke(message);
                }

                currentTelem = telemetryUpdate;
            }
        }

        /*Deals with any errors recieved from the simulator - displays the provided
         message to the form*/
        private void onErrorRecieved(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ErrorRecievedHandler(onErrorRecieved), message);
            }
            else
            {
                //no actual processing here - just set the error display to the message given
                lblErrorDisplay.ForeColor = Color.Red;
                lblErrorDisplay.Text = message;
            }
        }
    }
}
