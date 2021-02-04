namespace graduationDesign
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
       

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.Layers = new System.Windows.Forms.ToolStripMenuItem();
            this.AddLayer = new System.Windows.Forms.ToolStripMenuItem();
            this.DrillHole = new System.Windows.Forms.ToolStripMenuItem();
            this.AddDrillHole = new System.Windows.Forms.ToolStripMenuItem();
            this.Model = new System.Windows.Forms.ToolStripMenuItem();
            this.AddModel = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Layers,
            this.DrillHole,
            this.Model});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(664, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // Layers
            // 
            this.Layers.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddLayer});
            this.Layers.Name = "Layers";
            this.Layers.Size = new System.Drawing.Size(68, 21);
            this.Layers.Text = "地层模型";
            this.Layers.MouseLeave += new System.EventHandler(this.Layers_MouseLeave);
            this.Layers.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Layers_MouseMove);
            // 
            // AddLayer
            // 
            this.AddLayer.Name = "AddLayer";
            this.AddLayer.Size = new System.Drawing.Size(152, 22);
            this.AddLayer.Text = "添加标志层";
            this.AddLayer.Click += new System.EventHandler(this.AddLayer_Click);
            // 
            // DrillHole
            // 
            this.DrillHole.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddDrillHole});
            this.DrillHole.Name = "DrillHole";
            this.DrillHole.Size = new System.Drawing.Size(68, 21);
            this.DrillHole.Text = "钻孔模型";
            this.DrillHole.MouseLeave += new System.EventHandler(this.DrillHole_MouseLeave);
            this.DrillHole.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DrillHole_MouseMove);
            // 
            // AddDrillHole
            // 
            this.AddDrillHole.Name = "AddDrillHole";
            this.AddDrillHole.Size = new System.Drawing.Size(152, 22);
            this.AddDrillHole.Text = "添加钻孔模型";
            this.AddDrillHole.Click += new System.EventHandler(this.AddDrillHole_Click);
            // 
            // Model
            // 
            this.Model.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddModel});
            this.Model.Name = "Model";
            this.Model.Size = new System.Drawing.Size(56, 21);
            this.Model.Text = "总模型";
            // 
            // AddModel
            // 
            this.AddModel.Name = "AddModel";
            this.AddModel.Size = new System.Drawing.Size(152, 22);
            this.AddModel.Text = "添加模型";
            this.AddModel.Click += new System.EventHandler(this.AddModel_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 488);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "长平矿-三维建模";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem Layers;
        private System.Windows.Forms.ToolStripMenuItem AddLayer;
        private System.Windows.Forms.ToolStripMenuItem DrillHole;
        private System.Windows.Forms.ToolStripMenuItem AddDrillHole;
        private System.Windows.Forms.ToolStripMenuItem Model;
        private System.Windows.Forms.ToolStripMenuItem AddModel;

    }
}

