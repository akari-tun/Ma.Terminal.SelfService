using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Ma.Terminal.SelfService.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Ma.Terminal.SelfService.ViewModel
{
    public abstract class ViewModelBase : ObservableObject, IViewModel
    {
        string _title;
        public string Title
        { 
            get => _title;
            set
            {
                SetProperty(ref _title, value);
            }
        }
        public bool IsShown { get; set; }

        public ViewModelBase()
        {

        }

        public abstract void Initialization();

        public string GetString(string key) => ResourceManager.Instance.GetString(key);

    }
}
