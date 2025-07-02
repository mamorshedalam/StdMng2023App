using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using StdMng2023App.Services;
using System;
using System.Collections.Generic;
using StdMng2023App.Models;


namespace StdMng2023App
{
    public partial class MainWindow : Window
    {
        private string? _selectedStudentID;
        public MainWindow()
        {
            InitializeComponent();
        }

        // LOAD STUDENT
        private void OnLoadStudentsClick(object? sender, RoutedEventArgs e)
        {
            var service = new StudentService();
            var students = service.GetStudentSummaries();
            StudentList.ItemsSource = students;
        }

        // SELECT STUDENT
        private void OnStudentSelected(object? sender, SelectionChangedEventArgs e)
        {
           if (StudentList.SelectedItem is Student student)
            {
                _selectedStudentID = student.stuID;
                TxtStudentID.Text = student.stuID;
                TxtStudentName.Text = student.name;
                TxtStudentTel.Text = student.tel;
                TxtStudentAddr.Text = student.addr;
                // Set ComboBox selections
                SetComboBoxValue(CmbStudentSex, student.sex);
                SetComboBoxValue(CmbStudentDepName, student.depname);
                SetComboBoxValue(CmbStudentDepName, student.depname);
                SetComboBoxValue(CmbStudentGrade, student.grade);
                SetComboBoxValue(CmbStudentClass, student.@class);
            }
        }
        
        // REFRESH
        private void OnRefresh(object? sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        // ADD STUDENT
        private void OnAddStudentClick(object? sender, RoutedEventArgs e)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(TxtStudentName.Text))
            {
                Console.WriteLine("WARNING: Student Name is required.");
                return;
            }
            // Generate new Student ID
            var service = new StudentService();
            string newStudentID = service.GenerateNewStudentID();

            var student = new Student
            {
                stuID = newStudentID,
                name = TxtStudentName.Text.Trim(),
                sex = GetComboBoxValue(CmbStudentSex),
                depname = GetComboBoxValue(CmbStudentDepName),
                depno = SetDepnoValue(GetComboBoxValue(CmbStudentDepName)),
                grade = GetComboBoxValue(CmbStudentGrade),
                @class = GetComboBoxValue(CmbStudentClass),
                tel = TxtStudentTel.Text.Trim(),
                addr = TxtStudentAddr.Text.Trim()
            };

            service.AddStudent(student);

            Console.WriteLine("SUCCESS: Student added with ID: " + newStudentID);

            // Refresh the list and clear form
            StudentList.ItemsSource = service.GetStudentSummaries();
            ClearForm();
        }

        // UPDATE STUDENT
        private void OnUpdateStudentClick(object? sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_selectedStudentID))
            {
                Console.WriteLine("WARNING: No student selected.");
                return;
            }
            // Basic validation
            if (string.IsNullOrWhiteSpace(TxtStudentName.Text))
            {
                Console.WriteLine("WARNING: Student Name is required.");
                return;
            }

            var updatedStudent = new Student
            {
                stuID = _selectedStudentID,
                name = TxtStudentName.Text.Trim(),
                sex = GetComboBoxValue(CmbStudentSex),
                depname = GetComboBoxValue(CmbStudentDepName),
                depno = SetDepnoValue(GetComboBoxValue(CmbStudentDepName)),
                grade = GetComboBoxValue(CmbStudentGrade),
                @class = GetComboBoxValue(CmbStudentClass),
                tel = TxtStudentTel.Text.Trim(),
                addr = TxtStudentAddr.Text.Trim()
            };

            var service = new StudentService();
            service.UpdateStudent(_selectedStudentID, updatedStudent);
            Console.WriteLine($"SUCCESS: Updated student with ID '{_selectedStudentID}'");

            // Refresh the list and clear form
            StudentList.ItemsSource = service.GetStudentSummaries();
            ClearForm();
            _selectedStudentID = null;
        }

        // DELETE STUDENT
        private void OnDeleteStudentClick(object? sender, RoutedEventArgs e)
        {
            if (StudentList.SelectedItem is not Student selectedStudent)
            {
                Console.WriteLine("WARNING: No student selected.");
                return;
            }

            var service = new StudentService();
            service.DeleteStudentById(selectedStudent.stuID);
            Console.WriteLine($"SUCCESS: Deleted student '{selectedStudent.name}' (ID: {selectedStudent.stuID})");

            // Refresh the list and clear form
            StudentList.ItemsSource = service.GetStudentSummaries();
            ClearForm();
            _selectedStudentID = null;
        }

        // HELPER:: clear form
        private void ClearForm()
        {
            TxtStudentID.Text = "";
            TxtStudentName.Text = "";
            CmbStudentSex.SelectedIndex = -1;
            CmbStudentDepName.SelectedIndex = -1;
            CmbStudentGrade.SelectedIndex = -1;
            CmbStudentClass.SelectedIndex = -1;
            TxtStudentTel.Text = "";
            TxtStudentAddr.Text = "";
        }

        // HELPER:: get ComboBox selected value
        private string GetComboBoxValue(ComboBox comboBox)
        {
            if (comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                return selectedItem.Content?.ToString() ?? "";
            }
            return "";
        }

        // HELPER:: set ComboBox selection value
        private void SetComboBoxValue(ComboBox comboBox, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                comboBox.SelectedIndex = -1;
                return;
            }

            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                if (comboBox.Items[i] is ComboBoxItem item && 
                    item.Content?.ToString()?.Equals(value, StringComparison.OrdinalIgnoreCase) == true)
                {
                    comboBox.SelectedIndex = i;
                    return;
                }
            }
            comboBox.SelectedIndex = -1;
        }

        // HELPER:: set depno from depname
        private string SetDepnoValue(string value)
        {
            switch (value)
            {
                case "Chemistry":
                    return "CHS";
                case "Computer Science":
                    return "CS";
                case "Information Systems":
                    return "IS";
                case "Mathematics":
                    return "MA";
                default:
                    return "Unknown";
            }
        }
    }
}
