Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraEditors.Popup
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo

Namespace WindowsApplication34
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			gridControl1.DataSource = CreateTable(5)

		End Sub
		Private Function CreateTable(ByVal RowCount As Integer) As DataTable
			Dim tbl As New DataTable()
			tbl.Columns.Add("Name", GetType(String))
			tbl.Columns.Add("ID", GetType(Integer))
			tbl.Columns.Add("Number", GetType(Integer))
			tbl.Columns.Add("Date", GetType(DateTime))
			For i As Integer = 0 To RowCount - 1
				tbl.Rows.Add(New Object() { String.Format("Name{0}", i), i, 3 - i, DateTime.Now.AddDays(i) })
			Next i
			Return tbl
		End Function

		Private dateEditForm As VistaPopupDateEditForm

		Private Sub OnEditDateModified(ByVal sender As Object, ByVal e As EventArgs)
			Dim rowHandle As Integer = Convert.ToInt32(dateEditForm.Tag)
			gridView1.SetRowCellValue(rowHandle, gridView1.Columns("Date"), dateEditForm.Calendar.DateTime)
			dateEditForm.HidePopupForm()
		End Sub

		Private Sub gridView1_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles gridView1.MouseDown
			Dim hitInfo As GridHitInfo = gridView1.CalcHitInfo(e.Location)
			ClearForm()
			If e.Button = MouseButtons.Right AndAlso (hitInfo.HitTest = GridHitTest.RowCell OrElse hitInfo.HitTest = GridHitTest.Row) Then
				Dim dateEdit As New DateEdit()
				dateEdit.EditValue = gridView1.GetRowCellValue(hitInfo.RowHandle, gridView1.Columns("Date"))
				dateEditForm = New VistaPopupDateEditForm(dateEdit)
				AddHandler dateEditForm.Calendar.EditDateModified, AddressOf OnEditDateModified

				dateEditForm.ClosePopup()
				dateEditForm.Tag = hitInfo.RowHandle
				dateEditForm.Location = PointToScreen(e.Location)
				dateEditForm.Size = dateEditForm.CalcFormSize()
				dateEditForm.ShowPopupForm()
			End If
		End Sub

		Private Sub ClearForm()
			If dateEditForm IsNot Nothing Then
				RemoveHandler dateEditForm.Calendar.EditDateModified, AddressOf OnEditDateModified
				dateEditForm.ClosePopup()
				dateEditForm.Dispose()
			End If
		End Sub
	End Class
End Namespace