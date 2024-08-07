using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using InitDataBaseTool.Events;
using InitDataBaseTool.Models;
using InitDataBaseTool.Views;
using MySql.Data.MySqlClient;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace InitDataBaseTool.ViewModels
{
    public class MainViewModel:BindableBase
    {

        public readonly IEventAggregator _eventAggregator;
        public readonly IDialogService _dialogService;
        public readonly IDialogParameters _dialogParameters;
        private string _selectedItemObject;

        private string _title;

        public string Title
        {
            get => _title ?? (_title = string.Empty);
            set => SetProperty(ref _title, value);
        }

        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string SelectedItemObject
        {
            get => _selectedItemObject;
            set => SetProperty(ref _selectedItemObject, value);
        }

        private ObservableCollection<string> _tableList;

        public ObservableCollection<string> TableList
        {
            get => _tableList;
            set => SetProperty(ref _tableList, value);
        }
        public MainViewModel(IEventAggregator eventAggregator, IDialogService dialogService, IDialogParameters dialogParameters)
        {
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _dialogParameters = dialogParameters;
            _eventAggregator.GetEvent<DataBaseEvent>().Subscribe(ExecuteTransmit);

            TableList=new ObservableCollection<string>();
            OpenLocalDataBaseCommand = new DelegateCommand<string>(QueryLocalDataBase);
            OpenConnCommand = new DelegateCommand(ExecuteOpenConn);
            BackServerViewCommand = new DelegateCommand(async ()=>await ExecuteBackPrev());

            Name = "123";
        }

        private void QueryLocalDataBase(string title)
        {

            if (title =="Mysql")
            {
                ConnectionMysql();
            }
            else if (title == "SqlServer")
            {
                ConnectionSqlserver();
            }         
        }

        private void ConnectionSqlserver()
        {
            // 定义连接字符串
            string connectionString = "Server=.;Integrated Security=True";

            // 创建SqlConnection对象
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // 打开数据库连接
                    connection.Open();

                    // 列出所有数据库
                    using (SqlCommand cmd = new SqlCommand("SELECT name FROM sys.databases WHERE state_desc = 'ONLINE'", connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string databaseName = reader.GetString(0);
                                TableList.Add(databaseName);
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // 显示错误消息
                    MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ConnectionMysql()
        {
            string connectionString = "server=localhost;uid=root;pwd=1314;";

            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();

                // 连接成功后，获取所有数据库
                // 查询所有数据库
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SHOW DATABASES;";
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string databaseName = reader.GetString(0);
                    TableList.Add(databaseName);
                }

                MessageBox.Show("连接成功", "标题", MessageBoxButton.OKCancel, MessageBoxImage.Asterisk);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("ex.Message", "连接失败", MessageBoxButton.OKCancel, MessageBoxImage.Error);

            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        private void ExecuteOpenConn()
        {
            if (SelectedItemObject!=null)
            {
                _dialogParameters.Add("dataBaseName", SelectedItemObject);
                _dialogService.ShowDialog("ConnMysqlSetDialog", _dialogParameters, action =>
                {

                });
            }
            else
            {
                MessageBox.Show("没有选定数据库","警告",MessageBoxButton.OKCancel,MessageBoxImage.Information);
            }
          
        }

        private void ExecuteTransmit(DataBaseManager manager)
        {
           
            _dialogParameters.Add("manager",manager);
            Title = manager.DataBaseType;
        }

        public ICommand OpenLocalDataBaseCommand { get; set; }
        public ICommand OpenConnCommand { get; set; }
        public ICommand BackServerViewCommand { get; set; }

      

        private async Task ExecuteBackPrev()
        {
            // 关闭当前窗口 
            Process.Start(Process.GetCurrentProcess().ProcessName);
            Application.Current.Shutdown();

            await Task.Run(() =>
            {
                // 关闭当前窗口
                Task.Delay(TimeSpan.FromSeconds(1));
                // 在STA线程上打开登录窗口
                Application.Current.Dispatcher.Invoke(() =>
                {
                    SelectServerView view = new SelectServerView();
                    view.Show();
                }, DispatcherPriority.Normal, CancellationToken.None);

            });
        }
    }
}
