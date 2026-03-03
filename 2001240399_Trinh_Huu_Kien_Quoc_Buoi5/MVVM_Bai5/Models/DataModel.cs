using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace MVVM_Bai5.Models
{
    public static class DataModel
    {
        public static void Save(string path, ObservableCollection<Student> students)
        {
            try
            {
                string json = JsonConvert.SerializeObject(students, Formatting.Indented);
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu file: " + ex.Message);
            }
        }

        public static void Load(string path, ObservableCollection<Student> students)
        {
            try
            {
                if (!File.Exists(path))
                    return;

                string json = File.ReadAllText(path);
                var list = JsonConvert.DeserializeObject<ObservableCollection<Student>>(json);

                students.Clear();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        students.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load file: " + ex.Message);
            }
        }
    }
}
