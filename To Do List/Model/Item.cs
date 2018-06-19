//  Author: Eric A. Ens
// Purpose: Task object definition
//    Date: June 19 2018
using System.ComponentModel;


namespace To_Do_List.Model
{
    public class Item : INotifyPropertyChanged
    {
        string _TaskName;
        public string TaskName
        {
            get
            {
                return _TaskName;
            }
            set
            {
                if (_TaskName != value)
                {
                    _TaskName = value;
                    RaisePropertyChanged("TaskName");
                }
            }
        }

        string _Description;
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    RaisePropertyChanged("Description");
                }
            }
        }

        string _DueDate;
        public string DueDate
        {
            get
            {
                return _DueDate;
            }
            set
            {
                if (_DueDate != value)
                {
                    _DueDate = value;
                    RaisePropertyChanged("DueDate");
                }
            }
        }

        string _Priority;
        public string Priority
        {
            get
            {
                return _Priority;
            }
            set
            {
                if (_Priority != value)
                {
                    _Priority = value;
                    RaisePropertyChanged("Priority");
                }
            }
        }

        string _Status;
        public string Status
        {
            get
            {
                return _Status;
            }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    RaisePropertyChanged("Status");
                }
            }
        }

        public Item(string name, string desc, string due, string priority, string status) 
        {
            _TaskName = name;
            _Description = desc;
            _DueDate = due;
            _Priority = priority;
            _Status = status;
        }

        void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
