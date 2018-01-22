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
        public Form1()
        {
            InitializeComponent();
        }
        private FilterInfoCollection webcam;
        private VideoCaptureDevice cam;
        private VideoCaptureDevice cam2;
        MotionDetector motdet;
        private SerialPort serialPort1 = new SerialPort();
        int pos2 = 90;
        
       
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
            cam.Start();

            



        }

        void cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bit = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = bit;

        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.InitialDirectory = @"c:\Picture";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image.Save(saveFileDialog1.FileName);
            }
        }



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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

         
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                MessageBox.Show("Test");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
             
                 pos2 = pos2 - 3;
                Byte pos = Convert.ToByte(pos2);
                serialPort1.Write(new byte[] { pos }, 0, 1);
                label2.Text = Convert.ToString(pos);

            }
            catch
            {
                MessageBox.Show("Error: Input Has to Be an Integer Between 5 and 173");
                
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {

                pos2 = pos2 + 3;
                Byte pos = Convert.ToByte(pos2);
                serialPort1.Write(new byte[] { pos }, 0, 1);
                label2.Text = Convert.ToString(pos);

            }
            catch
            {
                MessageBox.Show("Error: Input Has to Be an Integer Between 5 and 173");

            }
        }

        private void videoSourcePlayer1_NewFrame(object sender, ref Bitmap image)
        {
            motdet.ProcessFrame(image);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
            videoSourcePlayer1.Visible = false;
            cam2.Stop();
            cam.Start();

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            videoSourcePlayer1.Visible = true;
            cam2.Start();
            cam.Stop();
        }
    }
}
