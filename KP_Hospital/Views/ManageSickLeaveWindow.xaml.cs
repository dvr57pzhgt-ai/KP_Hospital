using System;
using System.Windows;
using KP_Hospital.Data;
using KP_Hospital.Models;

namespace KP_Hospital.Views
{
    public partial class ManageSickLeaveWindow : Window
    {
        private readonly MedicalRecord _record;

        public ManageSickLeaveWindow(MedicalRecord record)
        {
            InitializeComponent();
            _record = record;
            UpdateStatusText();
        }

        private void UpdateStatusText()
        {
            if (_record.SickLeaveEndDate != null)
                txtStatus.Text = "Статус: Больничный закрыт.";
            else
                txtStatus.Text = $"Статус: Больничный открыт (Продлений: {_record.SickLeaveExtensionDays}).";
        }

        private void BtnCloseSickLeave_Click(object sender, RoutedEventArgs e)
        {
            using var context = new AppDbContext();
            var rec = context.MedicalRecords.Find(_record.RecordId);
            if (rec != null)
            {
                rec.SickLeaveEndDate = DateTime.Now; 
                context.SaveChanges();
            }
            MessageBox.Show("Пациент снят с больничного!");
            this.DialogResult = true;
            this.Close();
        }

        private void BtnExtendSickLeave_Click(object sender, RoutedEventArgs e)
        {
            using var context = new AppDbContext();
            var rec = context.MedicalRecords.Find(_record.RecordId);
            if (rec != null)
            {
                rec.SickLeaveExtensionDays += 7; 
                context.SaveChanges();
            }
            MessageBox.Show("Больничный продлен на 7 дней!");
            this.DialogResult = true;
            this.Close();
        }
    }
}