using MyCensus.Models.Enums;
using MyCensus.ViewModels.Base;
using System;

namespace MyCensus.Models
{
    public class MenuItem : ExtendedBindableObject
    {
        private string _title;

        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        private MenuItemType _menuItemType;

        public MenuItemType MenuItemType
        {
            get
            {
                return _menuItemType;
            }

            set
            {
                _menuItemType = value;
                RaisePropertyChanged(() => MenuItemType);
            }
        }

        private string _menuItemIcon;

        public string MenuItemIcon
        {
            get
            {
                return _menuItemIcon;
            }

            set
            {
                _menuItemIcon = value;
                RaisePropertyChanged(() => MenuItemIcon);
            }
        }



        private Type _viewModelType;

        public Type ViewModelType
        {
            get
            {
                return _viewModelType;
            }

            set
            {
                _viewModelType = value;
                RaisePropertyChanged(() => ViewModelType);
            }
        }

        private bool _isEnabled;

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }

            set
            {
                _isEnabled = value;
                RaisePropertyChanged(() => IsEnabled);
            }
        }
    }
}
