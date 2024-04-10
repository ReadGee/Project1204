using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static WPF_Application.Settings;
using Microsoft.Office.Interop.Word;
using static WPF_Application.Control_database;


namespace WPF_Application
{
    /// <summary>
    /// Логика взаимодействия для Control_database.xaml
    /// </summary>
    public partial class Control_database : System.Windows.Window
    {
        public Control_database()
        {
            InitializeComponent();
            LoadData();
        }
        private List<DataItemFromDataGrid> DataItems { get; set; }

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }


        private void LoadData()
        {
            try
            {
                string connectionString = Settings.SQLiteConnected;
                string query = "SELECT * FROM Anketa";
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection);
                    System.Data.DataTable dataTable = new System.Data.DataTable();
                    adapter.Fill(dataTable);
                    dataGrid.ItemsSource = dataTable.DefaultView;
                }
                DataItems = ImportDataFromGrid(dataGrid);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }

        private void Button_Add(object sender, RoutedEventArgs e)
        {
            Settings.FormOperationType = Settings.TypeWorkForms.Add;

            AddOrInsertData addInsertData = new AddOrInsertData();
            bool? result = addInsertData.ShowDialog();
            if (result == true)
            {
                MessageBox.Show("Данные успешно добавлены.");
                LoadData();
            }
            else
            {
                MessageBox.Show("Добавление данных отменено.");
            }
            Settings.FormOperationType = Settings.TypeWorkForms.None;
        }
        private void Button_Ins(object sender, RoutedEventArgs e)
        {
            Settings.FormOperationType = Settings.TypeWorkForms.Insert;
            Settings.ImportDataBetweenForms.SetSelectedData(dataGrid);
            AddOrInsertData addInsertData = new AddOrInsertData();
            bool? result = addInsertData.ShowDialog();
            if (result == true)
            {
                MessageBox.Show("Данные успешно изменены.");
                LoadData();
            }
            else
            {
                MessageBox.Show("Изменение данных отменено.");
            }
            Settings.FormOperationType = Settings.TypeWorkForms.None;
        }
        private void Button_Upd(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
                
        private void Button_Del(object sender, RoutedEventArgs e)
        {
            try
            {
                string connectionString = Settings.SQLiteConnected;
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();


                    if (dataGrid.SelectedItem != null)
                    {

                        DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
                        string selectedId = selectedRow["ID"].ToString();


                        string query = "DELETE FROM [Anketa] WHERE id = @Id";
                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Id", selectedId);
                            command.ExecuteNonQuery();
                        }


                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Выберите строку для удаления.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }
        private List<DataItemFromDataGrid> ImportDataFromGrid(DataGrid dataGrid)
        {
            List<DataItemFromDataGrid> importedData = new List<DataItemFromDataGrid>();

            foreach (var item in dataGrid.Items)
            {
                if (item is DataRowView row)
                {
                    DataItemFromDataGrid dataItem = new DataItemFromDataGrid
                    {
                        id = Convert.ToInt32(row["id"]),
                        FIO = row["FIO"].ToString(),
                        Date_Birth = Convert.ToDateTime(row["Date_Birth"]),
                        Gender = row["Gender"].ToString(),
                        Adress = row["Adress"].ToString(),
                        Place_Birth = row["Place_Birth"].ToString(),
                        INN = row["INN"].ToString(),
                        Insurance_number = row["Insurance_number"].ToString(),
                        Phone = row["Phone"].ToString(),
                        Family_status = row["Family_status"].ToString(),
                        Additional_information = row["Additional_information"].ToString(),
                        Place_Work = row["Place_Work"].ToString(),
                        Polling_station_number = row["Polling_station_number"].ToString()
                    };

                    importedData.Add(dataItem);
                }
            }

            return importedData;
        }

        private void Create_report_1(object sender, RoutedEventArgs e)
        {
            string selectedPollingStationNumber = Nomer_Ychastok_TB.Text.Trim();

            List<DataItemFromDataGrid> filteredItems = GetFilteredData(selectedPollingStationNumber);

            if (filteredItems.Count > 0)
            {
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                Document doc = wordApp.Documents.Add();

                Paragraph title = doc.Content.Paragraphs.Add();
                title.Range.Text = "Отчет по избирательному участку " + selectedPollingStationNumber;
                title.Range.Font.Bold = 0;
                title.Range.Font.Size = 8;
                title.Format.SpaceAfter = 10;
                title.Range.InsertParagraphAfter();

                Table dataTable = doc.Tables.Add(doc.Range(), filteredItems.Count + 1, 13); 

                dataTable.Cell(1, 1).Range.Text = "ID";
                dataTable.Cell(1, 2).Range.Text = "ФИО";
                dataTable.Cell(1, 3).Range.Text = "Дата рождения";
                dataTable.Cell(1, 4).Range.Text = "Пол";
                dataTable.Cell(1, 5).Range.Text = "Адрес";
                dataTable.Cell(1, 6).Range.Text = "Место рождения";
                dataTable.Cell(1, 7).Range.Text = "ИНН";
                dataTable.Cell(1, 8).Range.Text = "Номер страховки";
                dataTable.Cell(1, 9).Range.Text = "Телефон";
                dataTable.Cell(1, 10).Range.Text = "Семейное положение";
                dataTable.Cell(1, 11).Range.Text = "Дополнительная информация";
                dataTable.Cell(1, 12).Range.Text = "Место работы";
                dataTable.Cell(1, 13).Range.Text = "Номер избирательного участка";

                for (int i = 0; i < filteredItems.Count; i++)
                {
                    var item = filteredItems[i];
                    dataTable.Cell(i + 2, 1).Range.Text = item.id.ToString();
                    dataTable.Cell(i + 2, 2).Range.Text = item.FIO;
                    dataTable.Cell(i + 2, 3).Range.Text = item.Date_Birth.ToString();
                    dataTable.Cell(i + 2, 4).Range.Text = item.Gender;
                    dataTable.Cell(i + 2, 5).Range.Text = item.Adress;
                    dataTable.Cell(i + 2, 6).Range.Text = item.Place_Birth;
                    dataTable.Cell(i + 2, 7).Range.Text = item.INN;
                    dataTable.Cell(i + 2, 8).Range.Text = item.Insurance_number;
                    dataTable.Cell(i + 2, 9).Range.Text = item.Phone;
                    dataTable.Cell(i + 2, 10).Range.Text = item.Family_status;
                    dataTable.Cell(i + 2, 11).Range.Text = item.Additional_information;
                    dataTable.Cell(i + 2, 12).Range.Text = item.Place_Work;
                    dataTable.Cell(i + 2, 13).Range.Text = item.Polling_station_number;
                }

                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.Filter = "Документ Word (*.docx)|*.docx";
                if (saveFileDialog.ShowDialog() == true)
                {
                    doc.SaveAs2(saveFileDialog.FileName);
                    wordApp.Visible = true;
                }
                else
                {
                    doc.Close();
                    wordApp.Quit();
                }
            }
            else
            {
                MessageBox.Show("Не найдено записей для выбранного номера избирательного участка.");
            }
        }

        private List<DataItemFromDataGrid> GetFilteredData(string pollingStationNumber)
        {
            return DataItems.Where(item => item.Polling_station_number == pollingStationNumber).ToList();
        }

        private List<DataItemFromDataGrid> GetFilteredData_FamilyStatus(string Family_status)
        {
            return DataItems.Where(item => item.Family_status == Family_status).ToList();
        }

        public class DataItemFromDataGrid
        {
            public int id { get; set; }
            public string FIO { get; set; }
            public DateTime Date_Birth { get; set; }
            public string Gender { get; set; }
            public string Adress { get; set; }
            public string Place_Birth { get; set; }
            public string INN { get; set; }
            public string Insurance_number { get; set; }
            public string Phone { get; set; }
            public string Family_status { get; set; }
            public string Additional_information { get; set; }
            public string Place_Work { get; set; }
            public string Polling_station_number { get; set; }
        }

        private void Create_report_2(object sender, RoutedEventArgs e)
        {
            string Family_status = CBFamaly.SelectedItem.ToString();

            List<DataItemFromDataGrid> filteredItems = GetFilteredData_FamilyStatus(Family_status);

            if (filteredItems.Count > 0)
            {
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                Document doc = wordApp.Documents.Add();

                Paragraph title = doc.Content.Paragraphs.Add();
                title.Range.Text = "Отчет по избирательному участку " + Family_status;
                title.Range.Font.Bold = 0;
                title.Range.Font.Size = 8;
                title.Format.SpaceAfter = 10;
                title.Range.InsertParagraphAfter();

                Table dataTable = doc.Tables.Add(doc.Range(), filteredItems.Count + 1, 13);

                dataTable.Cell(1, 1).Range.Text = "ID";
                dataTable.Cell(1, 2).Range.Text = "ФИО";
                dataTable.Cell(1, 3).Range.Text = "Дата рождения";
                dataTable.Cell(1, 4).Range.Text = "Пол";
                dataTable.Cell(1, 5).Range.Text = "Адрес";
                dataTable.Cell(1, 6).Range.Text = "Место рождения";
                dataTable.Cell(1, 7).Range.Text = "ИНН";
                dataTable.Cell(1, 8).Range.Text = "Номер страховки";
                dataTable.Cell(1, 9).Range.Text = "Телефон";
                dataTable.Cell(1, 10).Range.Text = "Семейное положение";
                dataTable.Cell(1, 11).Range.Text = "Дополнительная информация";
                dataTable.Cell(1, 12).Range.Text = "Место работы";
                dataTable.Cell(1, 13).Range.Text = "Номер избирательного участка";

                for (int i = 0; i < filteredItems.Count; i++)
                {
                    var item = filteredItems[i];
                    dataTable.Cell(i + 2, 1).Range.Text = item.id.ToString();
                    dataTable.Cell(i + 2, 2).Range.Text = item.FIO;
                    dataTable.Cell(i + 2, 3).Range.Text = item.Date_Birth.ToString();
                    dataTable.Cell(i + 2, 4).Range.Text = item.Gender;
                    dataTable.Cell(i + 2, 5).Range.Text = item.Adress;
                    dataTable.Cell(i + 2, 6).Range.Text = item.Place_Birth;
                    dataTable.Cell(i + 2, 7).Range.Text = item.INN;
                    dataTable.Cell(i + 2, 8).Range.Text = item.Insurance_number;
                    dataTable.Cell(i + 2, 9).Range.Text = item.Phone;
                    dataTable.Cell(i + 2, 10).Range.Text = item.Family_status;
                    dataTable.Cell(i + 2, 11).Range.Text = item.Additional_information;
                    dataTable.Cell(i + 2, 12).Range.Text = item.Place_Work;
                    dataTable.Cell(i + 2, 13).Range.Text = item.Polling_station_number;
                }

                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.Filter = "Документ Word (*.docx)|*.docx";
                if (saveFileDialog.ShowDialog() == true)
                {
                    doc.SaveAs2(saveFileDialog.FileName);
                    wordApp.Visible = true;
                }
                else
                {
                    doc.Close();
                    wordApp.Quit();
                }
            }
            else
            {
                MessageBox.Show("Не найдено записей для выбранного номера избирательного участка.");
            }
        }
    }
}
