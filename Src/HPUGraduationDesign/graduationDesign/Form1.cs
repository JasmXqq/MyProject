using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace graduationDesign
{
    public partial class Form1 : Form
    {
        // ############################################################################
        WGLAV5.DEMO demo = new WGLAV5.DEMO();
        Graphics graphics;
        Pen pen;
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            if (this.AddLayer.Checked)
            {
                demo.OnPaint(this.Handle, this.Size.Width, this.Size.Height);
                System.Threading.Thread.Sleep(100); // 10 msec ==> 100 frames per second max.
                Invalidate(false); // Force OnPaint() to get called again.
            }
            if (this.AddDrillHole.Checked)
            {
                demo.OnPaintDrill(this.Handle, this.Size.Width, this.Size.Height);
                System.Threading.Thread.Sleep(100); // 10 msec ==> 100 frames per second max.
                Invalidate(false); // Force OnPaint() to get called again.
            }
            if (this.AddModel.Checked)
            {
                demo.OnPaint(this.Handle, this.Size.Width, this.Size.Height);
                System.Threading.Thread.Sleep(100); // 10 msec ==> 100 frames per second max.
                Invalidate(false); // Force OnPaint() to get called again.
            }
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //Graphics aGraphics = e.Graphics;
            //aGraphics.FillRectangle(Brushes.White, 0, 0, this.Size.Width, this.Size.Height);
            //Override System.Windows.Forms.Control OnPaintBackground()
            //method.  No GDI painting means no flicker in OpenGL rendering.
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        //重写按键事件
        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            demo.OnKeyDown(e);

        }
        public Form1()
        {
            InitializeComponent();
        }
        private void DrillHole_MouseMove(object sender, MouseEventArgs e)
        {
            DrillHole.ForeColor = Color.DeepPink;
        }

        private void DrillHole_MouseLeave(object sender, EventArgs e)
        {
            DrillHole.ForeColor = Color.Black;
        }

        private void Layers_MouseMove(object sender, MouseEventArgs e)
        {
            Layers.ForeColor = Color.DeepPink;
        }

        private void Layers_MouseLeave(object sender, EventArgs e)
        {
            Layers.ForeColor = Color.Black;
        }
        private void CancelChecked(object sender)
        {
            for (int i = 0; i < menuStrip1.Items.Count; i++)
            {
                foreach (ToolStripMenuItem tsmi1 in ((ToolStripMenuItem)menuStrip1.Items[i]).DropDownItems)
                {
                    if (!tsmi1.Name.Equals(((ToolStripMenuItem)sender).Name))
                    {
                        tsmi1.Checked = false;
                    }
                    else
                    {
                        tsmi1.Checked = !tsmi1.Checked;
                    }
                }
            }
        }

        private void AddLayer_Click(object sender, EventArgs e)
        {
            this.CancelChecked(sender);
        }

        private void AddDrillHole_Click(object sender, EventArgs e)
        {
            this.CancelChecked(sender);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            graphics = this.CreateGraphics();
            pen = new Pen(Color.Blue, 2);           
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.AddLayer.Checked)
            {
                demo.OnPaintLayer(this.Handle, this.Size.Width, this.Size.Height);
                System.Threading.Thread.Sleep(100); // 10 msec ==> 100 frames per second max.
                Invalidate(false); // Force OnPaint() to get called again.
            }
            if (this.AddDrillHole.Checked)
            {
                demo.OnPaintDrill(this.Handle, this.Size.Width, this.Size.Height);
                System.Threading.Thread.Sleep(100); // 10 msec ==> 100 frames per second max.
                Invalidate(false); // Force OnPaint() to get called again.
            }
            if(this.AddModel.Checked)
            {
                demo.OnPaint(this.Handle, this.Size.Width, this.Size.Height);
                System.Threading.Thread.Sleep(100); // 10 msec ==> 100 frames per second max.
                Invalidate(false); // Force OnPaint() to get called again.
            }
        }

        private void AddModel_Click(object sender, EventArgs e)
        {
            this.CancelChecked(sender);
        }

    }
}
