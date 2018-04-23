using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors.Popup;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace WindowsApplication34
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gridControl1.DataSource = CreateTable(5);

        }
        private DataTable CreateTable(int RowCount)
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add("Name", typeof(string));
            tbl.Columns.Add("ID", typeof(int));
            tbl.Columns.Add("Number", typeof(int));
            tbl.Columns.Add("Date", typeof(DateTime));
            for (int i = 0; i < RowCount; i++)
                tbl.Rows.Add(new object[] { String.Format("Name{0}", i), i, 3 - i, DateTime.Now.AddDays(i) });
            return tbl;
        }

        VistaPopupDateEditForm dateEditForm;

        void OnEditDateModified(object sender, EventArgs e) 
        {
            int rowHandle = Convert.ToInt32( dateEditForm.Tag );
            gridView1.SetRowCellValue(rowHandle, gridView1.Columns["Date"], dateEditForm.Calendar.DateTime); 
            dateEditForm.HidePopupForm();
        }

        private void gridView1_MouseDown(object sender, MouseEventArgs e)
        {
            GridHitInfo hitInfo = gridView1.CalcHitInfo(e.Location);
            ClearForm();
            if (e.Button == MouseButtons.Right && (hitInfo.HitTest == GridHitTest.RowCell || hitInfo.HitTest == GridHitTest.Row))
            {
                DateEdit dateEdit = new DateEdit();
                dateEdit.EditValue = gridView1.GetRowCellValue(hitInfo.RowHandle, gridView1.Columns["Date"]);
                dateEditForm = new VistaPopupDateEditForm(dateEdit);
                dateEditForm.Calendar.EditDateModified += new EventHandler(OnEditDateModified);

                dateEditForm.ClosePopup();
                dateEditForm.Tag = hitInfo.RowHandle;
                dateEditForm.Location = PointToScreen(e.Location);
                dateEditForm.Size = dateEditForm.CalcFormSize();
                dateEditForm.ShowPopupForm();
            }
        }

        private void ClearForm()
        {
            if (dateEditForm != null)
            {
                dateEditForm.Calendar.EditDateModified -= new EventHandler(OnEditDateModified);
                dateEditForm.ClosePopup();
                dateEditForm.Dispose();
            }
        }
    }
}