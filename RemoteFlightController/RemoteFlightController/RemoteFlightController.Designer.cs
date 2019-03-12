namespace RemoteFlightController
{
    partial class frmRemoteFlightController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtServerIP = new System.Windows.Forms.TextBox();
            this.lblServerIP = new System.Windows.Forms.Label();
            this.btnConnectDisconnect = new System.Windows.Forms.Button();
            this.grpConnection = new System.Windows.Forms.GroupBox();
            this.lblServerPort = new System.Windows.Forms.Label();
            this.txtServerPort = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.grpPlaneControls = new System.Windows.Forms.GroupBox();
            this.lblPitch = new System.Windows.Forms.Label();
            this.lblThrottle = new System.Windows.Forms.Label();
            this.trbPitch = new System.Windows.Forms.TrackBar();
            this.trbThrottleControl = new System.Windows.Forms.TrackBar();
            this.dgvInputData = new System.Windows.Forms.DataGridView();
            this.Altitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Speed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pitch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VerticleSpeed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Throttle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ElevatorPitch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ErrorCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblErrorDisplay = new System.Windows.Forms.Label();
            this.dgvSentData = new System.Windows.Forms.DataGridView();
            this.SentPitch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SentThrottle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblSentData = new System.Windows.Forms.Label();
            this.lblDataRecieved = new System.Windows.Forms.Label();
            this.grpConnection.SuspendLayout();
            this.grpPlaneControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbPitch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbThrottleControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInputData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSentData)).BeginInit();
            this.SuspendLayout();
            // 
            // txtServerIP
            // 
            this.txtServerIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtServerIP.Location = new System.Drawing.Point(115, 22);
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.Size = new System.Drawing.Size(161, 21);
            this.txtServerIP.TabIndex = 0;
            // 
            // lblServerIP
            // 
            this.lblServerIP.AutoSize = true;
            this.lblServerIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServerIP.Location = new System.Drawing.Point(6, 25);
            this.lblServerIP.Name = "lblServerIP";
            this.lblServerIP.Size = new System.Drawing.Size(106, 15);
            this.lblServerIP.TabIndex = 1;
            this.lblServerIP.Text = "Connection (IPv4):";
            // 
            // btnConnectDisconnect
            // 
            this.btnConnectDisconnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnectDisconnect.Location = new System.Drawing.Point(428, 20);
            this.btnConnectDisconnect.Name = "btnConnectDisconnect";
            this.btnConnectDisconnect.Size = new System.Drawing.Size(114, 23);
            this.btnConnectDisconnect.TabIndex = 2;
            this.btnConnectDisconnect.Text = "Connect";
            this.btnConnectDisconnect.UseVisualStyleBackColor = true;
            this.btnConnectDisconnect.Click += new System.EventHandler(this.btnConnectDisconnect_Click);
            // 
            // grpConnection
            // 
            this.grpConnection.Controls.Add(this.lblServerPort);
            this.grpConnection.Controls.Add(this.txtServerPort);
            this.grpConnection.Controls.Add(this.lblServerIP);
            this.grpConnection.Controls.Add(this.btnConnectDisconnect);
            this.grpConnection.Controls.Add(this.txtServerIP);
            this.grpConnection.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpConnection.Location = new System.Drawing.Point(12, 42);
            this.grpConnection.Name = "grpConnection";
            this.grpConnection.Size = new System.Drawing.Size(558, 63);
            this.grpConnection.TabIndex = 3;
            this.grpConnection.TabStop = false;
            this.grpConnection.Text = "Server Connection";
            // 
            // lblServerPort
            // 
            this.lblServerPort.AutoSize = true;
            this.lblServerPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServerPort.Location = new System.Drawing.Point(287, 25);
            this.lblServerPort.Name = "lblServerPort";
            this.lblServerPort.Size = new System.Drawing.Size(32, 15);
            this.lblServerPort.TabIndex = 4;
            this.lblServerPort.Text = "Port:";
            // 
            // txtServerPort
            // 
            this.txtServerPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtServerPort.Location = new System.Drawing.Point(322, 22);
            this.txtServerPort.Name = "txtServerPort";
            this.txtServerPort.Size = new System.Drawing.Size(100, 21);
            this.txtServerPort.TabIndex = 3;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(297, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(300, 29);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "Remote Flight Controller";
            // 
            // grpPlaneControls
            // 
            this.grpPlaneControls.Controls.Add(this.lblPitch);
            this.grpPlaneControls.Controls.Add(this.lblThrottle);
            this.grpPlaneControls.Controls.Add(this.trbPitch);
            this.grpPlaneControls.Controls.Add(this.trbThrottleControl);
            this.grpPlaneControls.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpPlaneControls.Location = new System.Drawing.Point(12, 130);
            this.grpPlaneControls.Name = "grpPlaneControls";
            this.grpPlaneControls.Size = new System.Drawing.Size(206, 165);
            this.grpPlaneControls.TabIndex = 5;
            this.grpPlaneControls.TabStop = false;
            this.grpPlaneControls.Text = "Controls";
            // 
            // lblPitch
            // 
            this.lblPitch.AutoSize = true;
            this.lblPitch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPitch.Location = new System.Drawing.Point(101, 26);
            this.lblPitch.Name = "lblPitch";
            this.lblPitch.Size = new System.Drawing.Size(95, 15);
            this.lblPitch.TabIndex = 7;
            this.lblPitch.Text = "Elevator Pitch";
            // 
            // lblThrottle
            // 
            this.lblThrottle.AutoSize = true;
            this.lblThrottle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThrottle.Location = new System.Drawing.Point(18, 26);
            this.lblThrottle.Name = "lblThrottle";
            this.lblThrottle.Size = new System.Drawing.Size(56, 15);
            this.lblThrottle.TabIndex = 1;
            this.lblThrottle.Text = "Throttle";
            // 
            // trbPitch
            // 
            this.trbPitch.Enabled = false;
            this.trbPitch.Location = new System.Drawing.Point(115, 44);
            this.trbPitch.Maximum = 50;
            this.trbPitch.Minimum = -50;
            this.trbPitch.Name = "trbPitch";
            this.trbPitch.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trbPitch.Size = new System.Drawing.Size(45, 104);
            this.trbPitch.TabIndex = 6;
            this.trbPitch.Scroll += new System.EventHandler(this.trbPitch_Scroll);
            // 
            // trbThrottleControl
            // 
            this.trbThrottleControl.Enabled = false;
            this.trbThrottleControl.Location = new System.Drawing.Point(21, 44);
            this.trbThrottleControl.Maximum = 1000;
            this.trbThrottleControl.Name = "trbThrottleControl";
            this.trbThrottleControl.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trbThrottleControl.Size = new System.Drawing.Size(45, 104);
            this.trbThrottleControl.TabIndex = 0;
            this.trbThrottleControl.Scroll += new System.EventHandler(this.trbThrottleControl_Scroll);
            // 
            // dgvInputData
            // 
            this.dgvInputData.AllowUserToAddRows = false;
            this.dgvInputData.AllowUserToDeleteRows = false;
            this.dgvInputData.AllowUserToResizeColumns = false;
            this.dgvInputData.AllowUserToResizeRows = false;
            this.dgvInputData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInputData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Altitude,
            this.Speed,
            this.Pitch,
            this.VerticleSpeed,
            this.Throttle,
            this.ElevatorPitch,
            this.ErrorCode});
            this.dgvInputData.Enabled = false;
            this.dgvInputData.Location = new System.Drawing.Point(275, 151);
            this.dgvInputData.Name = "dgvInputData";
            this.dgvInputData.Size = new System.Drawing.Size(746, 330);
            this.dgvInputData.TabIndex = 6;
            // 
            // Altitude
            // 
            this.Altitude.HeaderText = "Altitude";
            this.Altitude.Name = "Altitude";
            this.Altitude.ReadOnly = true;
            // 
            // Speed
            // 
            this.Speed.HeaderText = "Speed";
            this.Speed.Name = "Speed";
            this.Speed.ReadOnly = true;
            // 
            // Pitch
            // 
            this.Pitch.HeaderText = "Pitch";
            this.Pitch.Name = "Pitch";
            this.Pitch.ReadOnly = true;
            // 
            // VerticleSpeed
            // 
            this.VerticleSpeed.HeaderText = "VerticleSpeed";
            this.VerticleSpeed.Name = "VerticleSpeed";
            this.VerticleSpeed.ReadOnly = true;
            // 
            // Throttle
            // 
            this.Throttle.HeaderText = "Throttle";
            this.Throttle.Name = "Throttle";
            this.Throttle.ReadOnly = true;
            // 
            // ElevatorPitch
            // 
            this.ElevatorPitch.HeaderText = "ElevatorPitch";
            this.ElevatorPitch.Name = "ElevatorPitch";
            // 
            // ErrorCode
            // 
            this.ErrorCode.HeaderText = "Warnings";
            this.ErrorCode.Name = "ErrorCode";
            // 
            // lblErrorDisplay
            // 
            this.lblErrorDisplay.AutoSize = true;
            this.lblErrorDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrorDisplay.Location = new System.Drawing.Point(603, 83);
            this.lblErrorDisplay.Name = "lblErrorDisplay";
            this.lblErrorDisplay.Size = new System.Drawing.Size(94, 22);
            this.lblErrorDisplay.TabIndex = 7;
            this.lblErrorDisplay.Text = "(no errors)";
            // 
            // dgvSentData
            // 
            this.dgvSentData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSentData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SentThrottle,
            this.SentPitch});
            this.dgvSentData.Location = new System.Drawing.Point(12, 321);
            this.dgvSentData.Name = "dgvSentData";
            this.dgvSentData.Size = new System.Drawing.Size(246, 160);
            this.dgvSentData.TabIndex = 8;
            // 
            // SentPitch
            // 
            this.SentPitch.HeaderText = "Elevator Pitch";
            this.SentPitch.Name = "SentPitch";
            // 
            // SentThrottle
            // 
            this.SentThrottle.HeaderText = "Throttle";
            this.SentThrottle.Name = "SentThrottle";
            // 
            // lblSentData
            // 
            this.lblSentData.AutoSize = true;
            this.lblSentData.Location = new System.Drawing.Point(12, 302);
            this.lblSentData.Name = "lblSentData";
            this.lblSentData.Size = new System.Drawing.Size(116, 13);
            this.lblSentData.TabIndex = 9;
            this.lblSentData.Text = "Data Sent to Simulator:";
            // 
            // lblDataRecieved
            // 
            this.lblDataRecieved.AutoSize = true;
            this.lblDataRecieved.Location = new System.Drawing.Point(272, 130);
            this.lblDataRecieved.Name = "lblDataRecieved";
            this.lblDataRecieved.Size = new System.Drawing.Size(151, 13);
            this.lblDataRecieved.TabIndex = 10;
            this.lblDataRecieved.Text = "Data Recieved from Simulator:";
            // 
            // frmRemoteFlightController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1063, 506);
            this.Controls.Add(this.lblDataRecieved);
            this.Controls.Add(this.lblSentData);
            this.Controls.Add(this.dgvSentData);
            this.Controls.Add(this.lblErrorDisplay);
            this.Controls.Add(this.dgvInputData);
            this.Controls.Add(this.grpPlaneControls);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.grpConnection);
            this.Name = "frmRemoteFlightController";
            this.Text = "Remote Flight Controller";
            this.grpConnection.ResumeLayout(false);
            this.grpConnection.PerformLayout();
            this.grpPlaneControls.ResumeLayout(false);
            this.grpPlaneControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbPitch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbThrottleControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInputData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSentData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtServerIP;
        private System.Windows.Forms.Label lblServerIP;
        private System.Windows.Forms.Button btnConnectDisconnect;
        private System.Windows.Forms.GroupBox grpConnection;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblServerPort;
        private System.Windows.Forms.TextBox txtServerPort;
        private System.Windows.Forms.GroupBox grpPlaneControls;
        private System.Windows.Forms.Label lblThrottle;
        private System.Windows.Forms.TrackBar trbThrottleControl;
        private System.Windows.Forms.Label lblPitch;
        private System.Windows.Forms.TrackBar trbPitch;
        private System.Windows.Forms.DataGridView dgvInputData;
        private System.Windows.Forms.Label lblErrorDisplay;
        private System.Windows.Forms.DataGridViewTextBoxColumn Altitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn Speed;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pitch;
        private System.Windows.Forms.DataGridViewTextBoxColumn VerticleSpeed;
        private System.Windows.Forms.DataGridViewTextBoxColumn Throttle;
        private System.Windows.Forms.DataGridViewTextBoxColumn ElevatorPitch;
        private System.Windows.Forms.DataGridViewTextBoxColumn ErrorCode;
        private System.Windows.Forms.DataGridView dgvSentData;
        private System.Windows.Forms.DataGridViewTextBoxColumn SentThrottle;
        private System.Windows.Forms.DataGridViewTextBoxColumn SentPitch;
        private System.Windows.Forms.Label lblSentData;
        private System.Windows.Forms.Label lblDataRecieved;
    }
}

