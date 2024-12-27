using System;
using System.Drawing;
using System.Windows.Forms;

namespace pomodoro
{
    public partial class Form1 : Form
    {
        //Add timer manually 
        private System.Windows.Forms.Timer timer;
        private TimeSpan worktime = TimeSpan.FromMinutes(25);
        private TimeSpan breaktime = TimeSpan.FromMinutes(5);
        private TimeSpan remainingTime;
        private bool isWorkTime = true;
        private bool isRunning = false;

        public Form1()
        {
            InitializeComponent();
            InitializeTimer();
            remainingTime = worktime;
            UpdateTimeDisplay();
            timer.Stop();
            this.Text = "Pomodoro Counter"; 
            this.FormClosing += Form1_FormClosing; // Bind the FormClosing event
        }

        private void InitializeTimer()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += TimerTick;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (remainingTime.TotalSeconds > 0)
            {
                remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds(1));
                UpdateTimeDisplay();
            }
            else
            {
                timer.Stop();
                isRunning = false;
                btnStartStop.Text = "Start";
                MessageBox.Show(isWorkTime ? "Work time is over! Take a break." : "Break time is over! Back to work.");
                isWorkTime = !isWorkTime;
                remainingTime = isWorkTime ? worktime : breaktime;
                UpdateTimeDisplay();
            }
        }

        private void UpdateTimeDisplay()
        {
            lblTime.Text = remainingTime.ToString(@"mm\:ss");
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                timer.Stop();
                btnStartStop.Text = "Start";
            }
            else
            {
                timer.Start();
                btnStartStop.Text = "Stop";
            }
            isRunning = !isRunning;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            timer.Stop();
            isRunning = false;
            isWorkTime = true;
            remainingTime = worktime;
            btnStartStop.Text = "Start";
            UpdateTimeDisplay();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isRunning && isWorkTime) // Only show warning if timer is running and in run mode
            {
                DialogResult result = MessageBox.Show(
                    "Your runtime is not over, are you sure you want to close?",
                    "Warning",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.No)
                {
                    e.Cancel = true; // Block shutdown
                }
            }
            else if (isRunning && !isWorkTime) // Don't show warning when on break
            {
                // Close directly when on break
                e.Cancel = false;
            }
        }
    }
}
