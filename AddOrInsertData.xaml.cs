using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SQLite;

namespace WPF_Application
{
    /// <summary>
    /// Логика взаимодействия для AddOrInsertData.xaml
    /// </summary>
    public partial class AddOrInsertData : Window
    {
        public AddOrInsertData()
        {
            InitializeComponent();        
        }
        private void Window_Activated(object sender, EventArgs e)
        {
            INIT_Form();
        }

        private void INIT_Form()
        {
            if(Settings.FormOperationType == Settings.TypeWorkForms.Add)
            {
                LabelForSwitch.Content = "Добавление";
            }
            else if(Settings.FormOperationType == Settings.TypeWorkForms.Insert)
            {
                LabelForSwitch.Content = "Изменение";
                ImportFromClassInForm();
            }
        }


        private void Click_OK(object sender, RoutedEventArgs e)
        {
            if (Settings.FormOperationType == Settings.TypeWorkForms.Add)
            {
                AddInBase();
            }
            else if (Settings.FormOperationType == Settings.TypeWorkForms.Insert)
            {
                InsertInBase();
            }
        }

        private void Click_Return(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            ClearTextBoxes();
        }
        private void ClearTextBoxes()
        {
            FIO_TB.Text = null;
            DatePic.SelectedDate = null;
            Gender_TB.Text = null;
            Adress_TB.Text = null;
            MestoRoj_TB.Text = null;
            INN_TB.Text = null;
            Strah_TB.Text = null;
            Phone_TB.Text = null;
            Family_TB.Text = null;
            Dop_TB.Text = null;
            MestoRab_TB.Text = null;
            IZB_TB.Text = null;
        }

        private void ImportFromClassInForm()
        {            
            FIO_TB.Text = Settings.ImportDataBetweenForms.FIO;
            DatePic.SelectedDate = Settings.ImportDataBetweenForms.DateOfBirth;
            Gender_TB.Text = Settings.ImportDataBetweenForms.Gender;
            Adress_TB.Text = Settings.ImportDataBetweenForms.Address;
            MestoRoj_TB.Text = Settings.ImportDataBetweenForms.PlaceOfBirth;
            INN_TB.Text = Settings.ImportDataBetweenForms.INN;
            Strah_TB.Text = Settings.ImportDataBetweenForms.InsuranceNumber;
            Phone_TB.Text = Settings.ImportDataBetweenForms.Phone;
            Family_TB.Text = Settings.ImportDataBetweenForms.FamilyStatus;
            Dop_TB.Text = Settings.ImportDataBetweenForms.AdditionalInformation;
            MestoRab_TB.Text = Settings.ImportDataBetweenForms.PlaceOfWork;
            IZB_TB.Text = Settings.ImportDataBetweenForms.PollingStationNumber;
        }

        private void InsertInBase()
        {
            try
            {
                string connectionString = Settings.SQLiteConnected;
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE Anketa " +
                                   "SET FIO = @FIO, " +
                                   "    Date_Birth = @DateOfBirth, " +
                                   "    Gender = @Gender, " +
                                   "    Adress = @Address, " +
                                   "    Place_Birth = @PlaceOfBirth, " +
                                   "    INN = @INN, " +
                                   "    Insurance_number = @InsuranceNumber, " +
                                   "    Phone = @Phone, " +
                                   "    Family_status = @FamilyStatus, " +
                                   "    Additional_information = @AdditionalInformation, " +
                                   "    Place_Work = @PlaceOfWork, " +
                                   "    Polling_station_number = @PollingStationNumber " +
                                   "WHERE id = @ID";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        // Параметры запроса
                        command.Parameters.AddWithValue("@FIO", FIO_TB.Text);
                        command.Parameters.AddWithValue("@DateOfBirth", DatePic.SelectedDate.Value.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@Gender", Gender_TB.Text);
                        command.Parameters.AddWithValue("@Address", Adress_TB.Text);
                        command.Parameters.AddWithValue("@PlaceOfBirth", MestoRoj_TB.Text);
                        command.Parameters.AddWithValue("@INN", INN_TB.Text);
                        command.Parameters.AddWithValue("@InsuranceNumber", Strah_TB.Text);
                        command.Parameters.AddWithValue("@Phone", Phone_TB.Text);
                        command.Parameters.AddWithValue("@FamilyStatus", Family_TB.Text);
                        command.Parameters.AddWithValue("@AdditionalInformation", Dop_TB.Text);
                        command.Parameters.AddWithValue("@PlaceOfWork", MestoRab_TB.Text);
                        command.Parameters.AddWithValue("@PollingStationNumber", IZB_TB.Text);
                        command.Parameters.AddWithValue("@ID", Settings.ImportDataBetweenForms.ID);

                        int rowsAffected = command.ExecuteNonQuery();
                        ClearTextBoxes();
                        if (rowsAffected > 0)
                        {
                            DialogResult = true;
                        }
                        else
                        {
                            DialogResult = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void AddInBase()
        {
            try
            {
                string connectionString = Settings.SQLiteConnected;
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Anketa (FIO, Date_Birth, Gender, Adress, Place_Birth, INN, Insurance_number, Phone, Family_status, Additional_information, Place_Work, Polling_station_number) " +
                               "VALUES (@FIO, @DateOfBirth, @Gender, @Address, @PlaceOfBirth, @INN, @InsuranceNumber, @Phone, @FamilyStatus, @AdditionalInformation, @PlaceOfWork, @PollingStationNumber)";


                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        // Параметры запроса
                        command.Parameters.AddWithValue("@FIO", FIO_TB.Text);
                        command.Parameters.AddWithValue("@DateOfBirth", DatePic.SelectedDate.Value.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@Gender", Gender_TB.Text);
                        command.Parameters.AddWithValue("@Address", Adress_TB.Text);
                        command.Parameters.AddWithValue("@PlaceOfBirth", MestoRoj_TB.Text);
                        command.Parameters.AddWithValue("@INN", INN_TB.Text);
                        command.Parameters.AddWithValue("@InsuranceNumber", Strah_TB.Text);
                        command.Parameters.AddWithValue("@Phone", Phone_TB.Text);
                        command.Parameters.AddWithValue("@FamilyStatus", Family_TB.Text);
                        command.Parameters.AddWithValue("@AdditionalInformation", Dop_TB.Text);
                        command.Parameters.AddWithValue("@PlaceOfWork", MestoRab_TB.Text);
                        command.Parameters.AddWithValue("@PollingStationNumber", IZB_TB.Text);

                        int rowsAffected = command.ExecuteNonQuery();
                        ClearTextBoxes();
                        if (rowsAffected > 0)
                        {
                            DialogResult = true;
                        }
                        else
                        {
                            DialogResult = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        
    }
}
