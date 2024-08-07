using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using InitDataBaseTool.ViewModels.Dialogs;
using InitDataBaseTool.Views;
using InitDataBaseTool.Views.Dialogs;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Services.Dialogs;

namespace InitDataBaseTool
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : PrismApplication
    {
        
        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }
        protected override void InitializeShell(System.Windows.Window shell)
        {       
            var loginView = Container.Resolve<SelectServerView>();
            loginView.Topmost = true;
            loginView.Activate();
            if (loginView.ShowDialog() == true)
            {
                base.InitializeShell(shell);
            }
            else
            {
                Application.Current?.Shutdown();
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<ConnMysqlSetDialog, ConnMysqlSetDialogViewModel>();
            containerRegistry.RegisterDialog<ConnSqlserverSetDialog, ConnSqlserverSetDialogViewModel>();
            containerRegistry.RegisterScoped<IDialogParameters, DialogParameters>();
        }
    }
}
