using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Vision.Motion;
using System.IO;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;

namespace ServoControlCamera
{
    public partial class Form1 : Form
    {
        public bool bt3Click = false;

        // Initializer

        public Form1()
        {
            InitializeComponent();
        }
        private FilterInfoCollection webcam;
        private VideoCaptureDevice cam;
        private VideoCaptureDevice cam2;
        MotionDetector motdet;
        private SerialPort serialPort1 = new SerialPort();
        bool alrm = false;
        bool alrm2 = false;
        // Startup
       
        private void Form1_Load(object sender, System.EventArgs e)
        {

            motdet = new MotionDetector(new TwoFramesDifferenceDetector(), new MotionAreaHighlighting());
            try
            {
                serialPort1.PortName = "com3";
                serialPort1.BaudRate = 9600;
                serialPort1.Open();
            }
            catch
            {
                MessageBox.Show("Make Sure arduino is pluged in if it is reset device and try again");
            }
            webcam = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach(FilterInfo VideoCaptureDevice in webcam)
            {
                comboBox1.Items.Add(VideoCaptureDevice.Name);
            
            }
            comboBox1.SelectedIndex = 0;

            cam = new VideoCaptureDevice(webcam[comboBox1.SelectedIndex].MonikerString);
            cam2 = new VideoCaptureDevice(webcam[comboBox1.SelectedIndex].MonikerString);
            cam.NewFrame += new NewFrameEventHandler(cam_NewFrame);
            videoSourcePlayer1.VideoSource = cam2;
            cam.Stop();
            cam2.Stop();
            cam.Start();
            byte pos = Convert.ToByte(textBox1.Text);
            serialPort1.Write(new byte[] { pos }, 0, 1);




        }

        //camera new frame Picture box

        void cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bit = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = bit;

        }

        // Enter key Function

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                try
                {
                    byte pos = Convert.ToByte(textBox1.Text);

                    serialPort1.Write(new byte[] { pos }, 0, 1);
                    label2.Text = Convert.ToString(pos);
                    textBox1.Text = null;

                }
                catch
                {
                    MessageBox.Show("Error: Input Has to Be an Integer Between 5 and 173");
                }
            }
        }

        // Motion Detection Function

        private void videoSourcePlayer1_NewFrame(object sender, ref Bitmap image)
        {
            motdet.ProcessFrame(image);
            while (true)
            {
                if(alrm == true)
                {
                    MessageBox.Show("we made it");
                }
            }
        }

        //Button 1 (Enter Button)

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                byte pos = Convert.ToByte(textBox1.Text);

                serialPort1.Write(new byte[] { pos }, 0, 1);
                label2.Text = Convert.ToString(pos);

            }
            catch
            {
                MessageBox.Show("Error: Input Has to Be an Integer Between 5 and 173");
            }
        }

        //Button 2 (Take Picture)

        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.InitialDirectory = @"c:\Picture";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image.Save(saveFileDialog1.FileName);
            }
        }

        //Button 3 (Default camera Button)

        private void button3_Click_1(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
            videoSourcePlayer1.Visible = false;
            cam2.Stop();
            cam.Start();
            alrm2 = false;
            label3.Text = "Camera: Default";

        }

        //Button 4 (Motion Detection Button)

        private void button4_Click_1(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            videoSourcePlayer1.Visible = true;
            cam2.Start();
            cam.Stop();
            alrm2 = true;
            label3.Text = "Camera: Motion";
        }

        //Button 5 (Camera off button)

        private void button5_Click(object sender, EventArgs e)
        {
            label3.Text = "Camera: Off";
            cam2.Stop();
            cam.Stop();
            pictureBox1.Visible = false;
            alrm2 = false;
            videoSourcePlayer1.Visible = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        
        void alarm()
        {

                if (alrm == true & alrm2 == true)
                {
                    MessageBox.Show("we made it");
                }

        }

        // alarm on button

        private void button6_Click(object sender, EventArgs e)
        {
            alrm = true;
            label4.Text = "Alarm: On";
        }

        // Alarm off button

        private void button7_Click(object sender, EventArgs e)
        {
            alrm = false;
            label4.Text = "Alarm: off";
            alarm();
        }
    }
}
