//  Author: Eric A. Ens
// Purpose: (optionally) create db with test data for use with To Do List
//    Date: June 19 2018
using System;
using System.Windows;
using System.Windows.Media;
using System.Data.SQLite;
using System.IO;

namespace To_Do_List.ViewModel
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
           CreateList(); //un comment and run once to utilize
        }

        private void CreateList()  // used to create a blank db with a set of test data
        {
            if (File.Exists("To_Do_List.sqlite"))
                return;


            SQLiteConnection.CreateFile("To_Do_List.sqlite");

            SQLiteConnection db_Connection = new SQLiteConnection("Data Source=To_Do_List.sqlite;Version=3;");
            db_Connection.Open();

            //create db
            string sql = "create table list (taskname varchar(20), description varchar(100), duedate date, priority varchar(10), status varchar(10) )";

            SQLiteCommand command = new SQLiteCommand(sql, db_Connection);
            command.ExecuteNonQuery();

            sql = "insert into list (taskname, description, duedate, priority, status) values ('Groceries', 'Milk, Eggs, Butter', '2018-06-19', 'Low', 'OPEN')";

            command = new SQLiteCommand(sql, db_Connection);
            command.ExecuteNonQuery();

            sql = "insert into list (taskname, description, duedate, priority, status) values ('Laundry', 'Press clothes for work', '2016-05-15', 'Low', 'CLOSED')";

            command = new SQLiteCommand(sql, db_Connection);
            command.ExecuteNonQuery();

            sql = "insert into list (taskname, description, duedate, priority, status) values ('Mow Lawn', 'refuel mower', '2018-06-20', 'High', 'OPEN')";

            command = new SQLiteCommand(sql, db_Connection);
            command.ExecuteNonQuery();

            db_Connection.Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
