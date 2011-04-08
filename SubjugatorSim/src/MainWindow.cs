using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SubjugatorSim.Entities;
using SubjugatorSim.src;
using SubjugatorSim.src.Xml;
using Math = Mogre.Math;

namespace SubjugatorSim
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(State state) : this()
        {
            State = state;
            SuspendLayout();
            ClientSize = new Size(Screen.PrimaryScreen.Bounds.Right, Screen.PrimaryScreen.Bounds.Bottom);
            ResumeLayout(false);
            WindowState = FormWindowState.Maximized;
        }
        public State State { get; set; }

        public void Init()
        {
            State.SceneWorld = new World(State);
            propertyTreeGrid.Bind(State);

            KeyUp += KeyUpHandler;
        }

        public void FocusOnRenderWindow()
        {
            propertyTreeGrid.Enabled = false;
            Focus();
            propertyTreeGrid.Enabled = true;
        }

        public bool IsRenderWindowFocused
        {
            get { return Focused && !propertyTreeGrid.Focused; }
        }

        private void KeyUpHandler(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F1:
                    State.MainWindow.ToggleFullScreen();
                    break;
                case Keys.F2:
                    State.ControlState = State.ControlState.Next();
                    break;
                case Keys.F3:
                    State.CameraManager.Camera2.Pitch(Math.HALF_PI);
                    break;
                case Keys.F4:
                    State.CameraManager.ToggleViewport();
                    break;
            }
        }


        public void ToggleFullScreen()
        {
            if (FormBorderStyle != FormBorderStyle.None)
            {
                Visible = false;
                WindowState = FormWindowState.Normal;
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
                Visible = true;
                TopMost = true;
            }
            else
            {
                FormBorderStyle = FormBorderStyle.Sizable;
            }
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                State.SceneWorld.Load(openFileDialog.FileName);
                propertyTreeGrid.Bind(State);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == DialogResult.OK) 
                State.SceneWorld.Save(saveFileDialog.FileName);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void UpdatePropertyTreeGrid()
        {
            propertyTreeGrid.PropertyGrid.Refresh();
        }
    }
}
