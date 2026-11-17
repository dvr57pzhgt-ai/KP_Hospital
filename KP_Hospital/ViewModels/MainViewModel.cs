using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KP_Hospital.Data;
using KP_Hospital.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace KP_Hospital.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly AppDbContext _context;

        private ObservableCollection<Patient> _patients;
        public ObservableCollection<Patient> Patients
        {
            get => _patients;
            set => SetProperty(ref _patients, value);
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public MainViewModel()
        {
            _context = new AppDbContext();
            _patients = new ObservableCollection<Patient>();
            LoadPatients();
        }

        private void LoadPatients()
        {
            var list = _context.Patients.ToList();
            Patients.Clear();
            foreach (var p in list)
            {
                Patients.Add(p);
            }
        }

        private void Search()
        {
            var query = _context.Patients.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                query = query.Where(p => p.FullName.Contains(SearchText));
            }

            Patients.Clear();
            foreach (var p in query.ToList())
            {
                Patients.Add(p);
            }
        }
        /// <summary>
        /// Команда для открытия окна добавления нового пациента.
        /// </summary>
      
        private void OpenAddPatientWindow()
        {
            // Создаем экземпляр окна добавления пациента
            var patientWindow = new KP_Hospital.Views.PatientWindow();

            // ShowDialog() открывает окно и ждет, пока оно не закроется.
            // Если в окне была нажата кнопка "Сохранить" (DialogResult = true), 
            // то мы обновляем таблицу на главном окне.
            if (patientWindow.ShowDialog() == true)
            {
                LoadPatients(); // Перезагружаем список из БД
            }
        }
    }
}