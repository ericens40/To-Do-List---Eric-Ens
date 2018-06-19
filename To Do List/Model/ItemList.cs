//  Author: Eric A. Ens
// Purpose: Responsible for managing a list of item(task) objects. ensures data is up to date as well as handling Add, Delete, and Updates.
//    Date: June 19 2018
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace To_Do_List.Model
{
    class ItemList
    {
        public event EventHandler ItemChanged;

        public List<Item> _items { get; set; }//list of indv items

        public ItemList()//c'tor
        {
            _items = new List<Item>();
        }

        public List<Item> GetItems()//retrieve data from DB. clears and fills list to reflect data
        {
            SQLiteConnection db_Connection = new SQLiteConnection("Data Source=To_Do_List.sqlite;Version=3;");
            db_Connection.Open();

            string sql = "select * from list";//all tasks
            SQLiteCommand command = new SQLiteCommand(sql, db_Connection);
            SQLiteDataReader reader = command.ExecuteReader();

            _items.Clear();//clear cached tasks

            while (reader.Read())
                _items.Add(new Item(Convert.ToString(reader["taskname"]), Convert.ToString(reader["description"]), Convert.ToString(reader["duedate"]).Split(' ')[0], Convert.ToString(reader["priority"]),Convert.ToString(reader["status"])));
            db_Connection.Close();

            return _items;
        }

        public void AddItem(Item item)//add a new task to db
        {
            //  _items.Add(item);
            SQLiteConnection db_Connection = new SQLiteConnection("Data Source=To_Do_List.sqlite;Version=3;");
            db_Connection.Open();

            using (SQLiteCommand command = new SQLiteCommand(db_Connection))
            {
                command.CommandText =
                    "insert into list(taskname, description, duedate, priority, status) values(:name, :desc, :due, :prio, :stat)";
                command.Parameters.Add("name", DbType.String).Value = item.TaskName;
                command.Parameters.Add("desc", DbType.String).Value = item.Description;
                command.Parameters.Add("due", DbType.String).Value = item.DueDate;
                command.Parameters.Add("prio", DbType.String).Value = item.Priority;
                command.Parameters.Add("stat", DbType.String).Value = item.Status;
                command.ExecuteNonQuery();
            }

            db_Connection.Close();
            OnItemChanged();
        }

        public void DeleteItem(Item item)//remove an existing task from db
        {
            SQLiteConnection db_Connection = new SQLiteConnection("Data Source=To_Do_List.sqlite;Version=3;");
            db_Connection.Open();

            using (SQLiteCommand command = new SQLiteCommand(db_Connection))
            {
                command.CommandText =
                    "delete from list where taskname = :name and description = :desc and duedate = :due and priority = :prio and status = :stat";
                command.Parameters.Add("name", DbType.String).Value = item.TaskName;
                command.Parameters.Add("desc", DbType.String).Value = item.Description;
                command.Parameters.Add("due", DbType.String).Value = item.DueDate;
                command.Parameters.Add("prio", DbType.String).Value = item.Priority;
                command.Parameters.Add("stat", DbType.String).Value = item.Status;
                command.ExecuteNonQuery();
            }

            db_Connection.Close();
            OnItemChanged();
        }

        public void EditItem(Item item, int idx)//edit existing task from db
        {
            SQLiteConnection db_Connection = new SQLiteConnection("Data Source=To_Do_List.sqlite;Version=3;");
            db_Connection.Open();

            using (SQLiteCommand command = new SQLiteCommand(db_Connection))
            {
                command.CommandText =
                    "update list set taskname = :name, description = :desc, duedate = :due, priority = :prio, status = :stat where rowid = :id";
                command.Parameters.Add("name", DbType.String).Value = item.TaskName;
                command.Parameters.Add("desc", DbType.String).Value = item.Description;
                command.Parameters.Add("due", DbType.String).Value = item.DueDate;
                command.Parameters.Add("prio", DbType.String).Value = item.Priority;
                command.Parameters.Add("stat", DbType.String).Value = item.Status;
                command.Parameters.Add("id", DbType.String).Value = idx + 1;
                command.ExecuteNonQuery();
            }

            db_Connection.Close();
        }

        void OnItemChanged()
        {
            if (ItemChanged != null)
                ItemChanged(this, null);
        }

    }
}
