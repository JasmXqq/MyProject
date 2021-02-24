using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ExcelToCbs
{
    public partial class Form1 : Form
    {
        private List<Mdl> _mdls = new List<Mdl>();

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonEdit_ExportExcel_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            using (var openFile = new OpenFileDialog())
            {
                openFile.Multiselect = false;
                openFile.Filter = @"Excel File(*.xls;*.xlsx)|*.xls;*.xlsx";
                openFile.Title = @"请选择要导入的数据文件";
                if (DialogResult.OK == openFile.ShowDialog())
                {
                    buttonEdit_ExportExcel.Text = openFile.FileName;
                    if (Import(openFile.FileName))
                    {
                        MessageBox.Show(@"完成数据导入", @"提示!");
                    }                  
                }
            }
        }

        private bool Import(string path)
        {
            _mdls.Clear();
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                IWorkbook workbook;
                var extension = Path.GetExtension(path);
                if (".xlsx".Equals(extension, StringComparison.CurrentCultureIgnoreCase))
                {
                    workbook = new XSSFWorkbook(fs);
                }
                else
                {
                    workbook = new HSSFWorkbook(fs);
                }

                if (workbook.NumberOfSheets > 0)
                {
                    var sheetAt0 = workbook.GetSheetAt(0);
                    var cou = sheetAt0.LastRowNum;
                    for (var i = 1; i < cou + 1; i++)
                    {
                        var mdl = new Mdl();
                        var rowHeader = sheetAt0.GetRow(i);

                        mdl.X = rowHeader.GetCell(0).NumericCellValue;
                        mdl.Y = rowHeader.GetCell(1).NumericCellValue;
                        mdl.Z = rowHeader.GetCell(2).NumericCellValue;
                        mdl.Value = rowHeader.GetCell(3).NumericCellValue;

                        _mdls.Add(mdl);
                    }
                }
            }
            return true;
        }

        private void simpleButton_OutCbs_Click(object sender, EventArgs e)
        {
            using (var saveFile = new SaveFileDialog())
            {
                saveFile.Filter = @"Cbs(*.cbs)|*.cbs";
                saveFile.Title = @"请选择要导出的文件路径";
                if (DialogResult.OK == saveFile.ShowDialog())
                {
                    var strPath = saveFile.FileName;
                    var sw = new StreamWriter(strPath, false, System.Text.Encoding.Default);
                    var strText = "";
                    strText = "点测深属性建模" + "\r\n" + 4 + "\r\n" + _mdls.Count + "\r\n" + "Xlocation" + "\r\n" + "Ylocation" + "\r\n" + "Zlocation" + "\r\n" + "适宜性";
                    sw.WriteLine(strText);
                    foreach (var item in _mdls)
                    {
                        strText = "";

                        strText = strText + item.X * 100 + "\t";
                        strText = strText + item.Y * 100 + "\t";
                        strText = strText + item.Z + "\t";
                        strText = strText + item.Value;
                        sw.WriteLine(strText);
                    }
                    sw.Close();
                    MessageBox.Show(@"数据导出完成",@"提示!");
                }
            }
        }

        public class Mdl
        {
            public double X { get; set; }

            public double Y { get; set; }

            public double Z { get; set; }

            public double Value { get; set; }
        }
    }
}
