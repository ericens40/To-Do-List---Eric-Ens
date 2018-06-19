//  Author: Eric A. Ens
// Purpose: MainWindow ViewModel
//    Date: June 19 2018
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using To_Do_List.Helpers;
using To_Do_List.Model;

namespace To_Do_List.ViewModel
{
    class ViewModelMain : ViewModelBase
    {
        public ItemList listData; // The business object (cached list object)

        ObservableCollection<Item> _ToDoList;
        public ObservableCollection<Item> ToDoList
        {
            get
            {
                _ToDoList = new ObservableCollection<Item>(listData.GetItems());//Update cached data to reflect DB
                return _ToDoList;
            }
        }

        private static ViewModelMain _instance = new ViewModelMain();
        public static ViewModelMain Instance { get { return _instance; } }


        string _TextPropertyName;
        public string TextPropertyName
        {
            get { return _TextPropertyName; }
            set
            {
                if (_TextPropertyName != value)
                {
                    _TextPropertyName = value;
                    RaisePropertyChanged("TextProperyName");
                }
            }
        }

        string _TextPropertyDescription;
        public string TextPropertyDescription
        {
            get { return _TextPropertyDescription; }
            set
            {
                if (_TextPropertyDescription != value)
                {
                    _TextPropertyDescription = value;
                    RaisePropertyChanged("TextPropertyDescription");
                }
            }
        }

        string _TextPropertyDueDate;
        public string TextPropertyDueDate
        {
            get { return _TextPropertyDueDate; }
            set
            {
                if (_TextPropertyDueDate != value)
                {
                    _TextPropertyDueDate = value;
                    RaisePropertyChanged("TextPropertyDueDate");
                }
            }
        }

        string _TextPropertyPriority;
        public string TextPropertyPriority
        {
            get { return _TextPropertyPriority; }
            set
            {
                if (_TextPropertyPriority != value)
                {
                    _TextPropertyPriority = value;
                    RaisePropertyChanged("TextPropertyPriority");
                }
            }
        }
    
        string _TextPropertyStatus;
        public string TextPropertyStatus
        {
            get { return _TextPropertyStatus; }
            set
            {
                if (_TextPropertyStatus != value)
                {
                    _TextPropertyStatus = value;
                    RaisePropertyChanged("TextPropertyStatus");
                }
            }
        }

    


        object _SelectedItem;
        public object SelectedItem//currently selected item from datagrid
        {
            get { return _SelectedItem; }
            set
            {
                if (_SelectedItem != value)
                {
                    _SelectedItem = value;
                    if (_SelectedItem != null)
                    {
                        TextPropertyName = (_SelectedItem as Item).TaskName;//populate GUI text fields with proper data
                        TextPropertyDescription = (_SelectedItem as Item).Description.ToString();
                        TextPropertyDueDate = (_SelectedItem as Item).DueDate.ToString();
                        TextPropertyPriority = (_SelectedItem as Item).Priority.ToString();
                        TextPropertyStatus = (_SelectedItem as Item).Status.ToString();
                    }

                    RaisePropertyChanged("SelectedItem");
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                    {
                        RaisePropertyChanged("SelectedItem");
                        RaisePropertyChanged("TextPropertyName");
                        RaisePropertyChanged("TextPropertyDescription");
                        RaisePropertyChanged("TextPropertyDueDate");
                        RaisePropertyChanged("TextPropertyPriority");
                        RaisePropertyChanged("TextPropertyStatus");
                    }));
                }
            }
        }

        int _SelectedIndex;
        public int SelectedIndex//index of currently selected item from datagrid
        {
            get { return _SelectedIndex; }
            set
            {
                if (_SelectedIndex != value)
                {
                    _SelectedIndex = value;

                    RaisePropertyChanged("SelectedIndex");
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                    {
                        RaisePropertyChanged("SelectedIndex");
                    }));
                }
            }
        }

        BindingGroup _UpdateBindingGroup;
        public BindingGroup UpdateBindingGroup
        {
            get
            { return _UpdateBindingGroup; }
            set
            {
                if (_UpdateBindingGroup != value)
                {
                    _UpdateBindingGroup = value;
                    RaisePropertyChanged("UpdateBindingGroup");
                }
            }
        }

        public RelayCommand AddItemCommand { get; set; } // Add record to Database
        public RelayCommand DeleteItemCommand { get; set; } // Remove record from Database
        public RelayCommand EditItemCommand { get; set; } // Edit an existing record in Database

        public ViewModelMain()
        {
            listData = new ItemList();//default c'tor loads existing data
            listData.ItemChanged += new EventHandler(listData_ItemChanged);

            AddItemCommand = new RelayCommand(AddItem); // Add record to Database
            DeleteItemCommand = new RelayCommand(DeleteItem); // Remove record from Database
            EditItemCommand = new RelayCommand(DoUpdate); // Edit an existing record in Database

            UpdateBindingGroup = new BindingGroup { Name = "Group1" };
        }

        void listData_ItemChanged(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                RaisePropertyChanged("ToDoList");
            }));
        }

        void DoUpdate(object param)//update selected item with new values.
        {
            if (TextPropertyDueDate.Length == 10 && TextPropertyName.Length != 0)
            {
                if (SelectedItem != null)
                {
                    listData.EditItem(new Item(TextPropertyName, TextPropertyDescription, TextPropertyDueDate, TextPropertyPriority, TextPropertyStatus), SelectedIndex);


                    RaisePropertyChanged("ToDoList"); // Update the list from the data source
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                    {
                        RaisePropertyChanged("ToDoList");
                    }));
                }
            }
            else
            {
                MessageBox.Show("Please ensue a task name has been entered as well as a valid date(YYYY-MM-DD)");
            }
        }

        void AddItem(object parameter)
        {
            if (TextPropertyDueDate.Length == 10 && TextPropertyName.Length != 0)
            {
                SelectedItem = null; // Unselects last selection. Essential, as assignment below won't clear other control's SelectedItems
                var item = new Item(TextPropertyName, TextPropertyDescription, TextPropertyDueDate, TextPropertyPriority, TextPropertyStatus);
                if (TextPropertyName != null)
                {
                    listData.AddItem(item);
                    RaisePropertyChanged("ToDoList"); // Update the list from the data source
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                    {
                        RaisePropertyChanged("ToDoList");
                    }));
                }

                SelectedItem = item;
            }
            else
            {
                MessageBox.Show("Please ensue a task name has been entered as well as a valid date(YYYY-MM-DD)");
            }

        }

        void DeleteItem(object parameter)//deletes a selected item from colleciton
        {
            var item = SelectedItem as Item;
            if (SelectedIndex != -1)
            {
                listData.DeleteItem(item);
                RaisePropertyChanged("ToDoList"); // Update the list from the data source
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    RaisePropertyChanged("ToDoList");
                }));
            }
            else
                SelectedItem = null; // Simply discard the new object
        }

    }
}
