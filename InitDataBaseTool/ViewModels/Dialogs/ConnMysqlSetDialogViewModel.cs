using InitDataBaseTool.Models;
using MySql.Data.MySqlClient;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitDataBaseTool.ViewModels.Dialogs
{
    public class ConnMysqlSetDialogViewModel : BindableBase, IDialogAware
    {
        public string Title => SetTitle;

        public string SetTitle { get; set; }

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        private string _connName;

        public string ConnName
        {
            get => _connName;
            set => SetProperty(ref _connName, value);
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            DataBaseManager dataBaseManager = parameters.GetValue<DataBaseManager>("manager");
            ConnName=parameters.GetValue<string>("dataBaseName");
           
            SetTitle = dataBaseManager.DataBaseType;
        }


        public ConnMysqlSetDialogViewModel()
        {
            TestConnCommand = new DelegateCommand(TestConn);
            CancelCommand = new DelegateCommand<string>(ExecuteCancel);
            ConfirmCommand = new DelegateCommand<string>(ExecuteConfirm);
        }

        public ICommand TestConnCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        private void ExecuteConfirm(string obj)
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }

        private void ExecuteCancel(string obj)
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
        }

        public void TestConn()
        {
           
        }

         
    }
}
