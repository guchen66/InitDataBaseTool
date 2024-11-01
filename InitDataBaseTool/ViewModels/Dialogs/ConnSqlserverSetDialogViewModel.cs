﻿using InitDataBaseTool.Models;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitDataBaseTool.ViewModels.Dialogs
{
    public class ConnSqlserverSetDialogViewModel : BindableBase, IDialogAware
    {
        public string Title => SetTitle;


        public string SetTitle { get; set; }

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            DataBaseManager dataBaseManager = parameters.GetValue<DataBaseManager>("manager");
            SetTitle = dataBaseManager.DataBaseType;
        }
    }
}