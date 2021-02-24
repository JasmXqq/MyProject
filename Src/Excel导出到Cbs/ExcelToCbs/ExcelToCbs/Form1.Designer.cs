namespace ExcelToCbs
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
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.buttonEdit_ExportExcel = new DevExpress.XtraEditors.ButtonEdit();
            this.simpleButton_OutCbs = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit_ExportExcel.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(24, 51);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(94, 22);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "导入Excel：";
            // 
            // buttonEdit_ExportExcel
            // 
            this.buttonEdit_ExportExcel.Location = new System.Drawing.Point(124, 45);
            this.buttonEdit_ExportExcel.Name = "buttonEdit_ExportExcel";
            this.buttonEdit_ExportExcel.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.buttonEdit_ExportExcel.Size = new System.Drawing.Size(348, 28);
            this.buttonEdit_ExportExcel.TabIndex = 1;
            this.buttonEdit_ExportExcel.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.buttonEdit_ExportExcel_ButtonClick);
            // 
            // simpleButton_OutCbs
            // 
            this.simpleButton_OutCbs.Location = new System.Drawing.Point(478, 42);
            this.simpleButton_OutCbs.Name = "simpleButton_OutCbs";
            this.simpleButton_OutCbs.Size = new System.Drawing.Size(71, 33);
            this.simpleButton_OutCbs.TabIndex = 2;
            this.simpleButton_OutCbs.Text = "导出";
            this.simpleButton_OutCbs.Click += new System.EventHandler(this.simpleButton_OutCbs_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 126);
            this.Controls.Add(this.simpleButton_OutCbs);
            this.Controls.Add(this.buttonEdit_ExportExcel);
            this.Controls.Add(this.labelControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "数据格式转换";
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit_ExportExcel.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ButtonEdit buttonEdit_ExportExcel;
        private DevExpress.XtraEditors.SimpleButton simpleButton_OutCbs;
    }
}

