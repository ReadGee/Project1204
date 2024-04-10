using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPF_Application
{
    internal class Settings
    {
        //Enum
        public enum TypeWorkForms
        {
            None,
            Add,
            Insert
        }


        //Special static
        public static string SQLiteConnected = @"Data Source=|DataDirectory|\Database_Bank.db;Version=3;";
        public static TypeWorkForms FormOperationType = TypeWorkForms.None;


        //Forms
        public static MainWindow mainWindow = new MainWindow();
        public static AddOrInsertData addInsertData = new AddOrInsertData();
        public static Control_database controlDatabaseWindow = new Control_database();

        //Class'es
        public static class ImportDataBetweenForms
        {
            public static int ID { get; set; }
            public static string FIO { get; set; }
            public static DateTime DateOfBirth { get; set; }
            public static string Gender { get; set; }
            public static string Address { get; set; }
            public static string PlaceOfBirth { get; set; }
            public static string INN { get; set; }
            public static string InsuranceNumber { get; set; }
            public static string Phone { get; set; }
            public static string FamilyStatus { get; set; }
            public static string AdditionalInformation { get; set; }
            public static string PlaceOfWork { get; set; }
            public static string PollingStationNumber { get; set; }

            public static void SetSelectedData(DataGrid dataGrid)
            {
                if (dataGrid.SelectedItem != null)
                {
                    DataRowView row = (DataRowView)dataGrid.SelectedItem;
                    ID = Convert.ToInt32(row["id"]);
                    FIO = row["FIO"].ToString();
                    DateOfBirth = Convert.ToDateTime(row["Date_Birth"]);
                    Gender = row["Gender"].ToString();
                    Address = row["Adress"].ToString();
                    PlaceOfBirth = row["Place_Birth"].ToString();
                    INN = row["INN"].ToString();
                    InsuranceNumber = row["Insurance_number"].ToString();
                    Phone = row["Phone"].ToString();
                    FamilyStatus = row["Family_status"].ToString();
                    AdditionalInformation = row["Additional_information"].ToString();
                    PlaceOfWork = row["Place_Work"].ToString();
                    PollingStationNumber = row["Polling_station_number"].ToString();
                }
            }
        }
    }
}
