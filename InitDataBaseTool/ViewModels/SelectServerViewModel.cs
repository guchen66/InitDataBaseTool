using InitDataBaseTool.Events;
using InitDataBaseTool.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InitDataBaseTool.ViewModels
{
    public class SelectServerViewModel:BindableBase
    {
        private string _version;

        public string Version
        {
            get => _version;
            set => SetProperty(ref _version,value);
        }

        private DataBaseManager _dataBaseManager;

        public DataBaseManager DataBaseManager
        {
            get => _dataBaseManager;
            set => SetProperty(ref _dataBaseManager, value);
        }

        public readonly IEventAggregator _eventAggregator;

        public SelectServerViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            OpenServerCommand = new DelegateCommand<string>(Execute);
            InitData();
        }

        private void Execute(string dataBaseType)
        {
            DataBaseManager dataBaseManager = new DataBaseManager();
            dataBaseManager.DataBaseType = dataBaseType;
            _eventAggregator.GetEvent<DataBaseEvent>().Publish(dataBaseManager);         



            var activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            activeWindow.DialogResult = true;
        }

        public ICommand OpenServerCommand { get; set; }
        private void InitData()
        {
            Version= Application.ResourceAssembly.GetName().Version.ToString();
        }
    }
}
