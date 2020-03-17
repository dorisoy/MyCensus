using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism;

using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Ioc;

using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Collections.ObjectModel;

using MyCensus.Services;
using MyCensus.Models.Census;
using MyCensus.ViewModels.Base;
using MyCensus.Helpers;
using System.Reflection;
using MyCensus.DataServices.Interfaces;
using Newtonsoft.Json;
using System.Windows.Input;
using Xamarin.Forms;
using Acr.UserDialogs;
using MyCensus.Cache;

namespace MyCensus.ViewModels
{

    public class Restaurant_Card_Page3ViewModel : ViewModelBase, IActiveAware
    {

        private new INavigationService _navigationService;
        private new IDialogService _dialogService;
        private ISaveAndLoad _saveAndLoadService;
        private readonly ICensusService _censusService;
        private readonly ICacheManager _cacheManager;

        #region Property

        public new event PropertyChangedEventHandler PropertyChanged;
        protected new virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion

        private int _step;
        public int Step
        {
            get { return _step; }
            set
            {
                this.SetProperty(ref _step, value);
            }
        }

        /// <summary>
        /// 餐饮商品信息
        /// </summary>
        private ObservableCollection<SalesProduct> _products = new ObservableCollection<SalesProduct>();
        public ObservableCollection<SalesProduct> Products
        {
            get
            {
                return _products;
            }

            set
            {
                _products = value;
                RaisePropertyChanged(() => Products);
            }
        }


        private string _action;
        public string Action
        {
            get { return _action; }
            set
            {
                this.SetProperty(ref _action, value);
            }
        }


        private Dictionary<int, string> _steps;
        public Dictionary<int, string> Steps
        {
            get { return _steps; }
            set
            {
                this.SetProperty(ref _steps, value);
            }
        }


        private Restaurant _restaurant;
        public Restaurant Restaurant
        {
            get
            {
                return _restaurant;
            }
            set
            {
                _restaurant = value;
                RaisePropertyChanged(() => Restaurant);
            }
        }


        public Restaurant_Card_Page3ViewModel(INavigationService navigationService,
            ISaveAndLoad saveAndLoadService,
            ICensusService censusService,
            ICacheManager cacheManager,
            IDialogService dialogService) : base(navigationService, dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _saveAndLoadService = saveAndLoadService;
            _censusService = censusService;
            _cacheManager = cacheManager;

            //普查步骤
            var steps = new Dictionary<int, string> { { 0, "基础信息" }, { 1, "经营信息" }, { 2, "销售信息" } };
            Steps = steps;
            Step = 3;
            Title = "添加餐饮终端普查-销售信息";
        }


        /// <summary>
        /// 添加商品信息
        /// </summary>
        private DelegateCommand<string> _newAddCmd;
        public DelegateCommand<string> NewAddCmd
        {
            get
            {
                if (_newAddCmd == null)
                {
                    _newAddCmd = new DelegateCommand<string>(async (r) =>
                    {
                        try
                        {
                            var parameter = new NavigationParameters() {
                                { "data", "Restaurant" },
                                { "action", "add" }
                            };
                            await _navigationService.NavigateAsync("AddProducts", parameter);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.Print(ex.Message);
                        }
                    });
                }
                return _newAddCmd;
            }
        }


        /// <summary>
        /// 删除商品
        /// </summary>
        public ICommand DelItemCmd => new Command<SalesProduct>(async (item) => { await DelItem(item); });
        private async Task DelItem(SalesProduct item)
        {
            if (item == null)
            {
                _dialogService.LongAlert("没找到商品信息!");
                return;
            }
            await DelProduct(item.GuId);
        }


        private async Task DelProduct(string pid)
        {
            try
            {
                var fileName = "products_restaurant.json";

                if (string.IsNullOrEmpty(pid))
                    _saveAndLoadService.DelFile(fileName);
                else
                {
                    var products = new List<SalesProduct>();
                    var read = _saveAndLoadService.LoadText(fileName);
                    if (!string.IsNullOrEmpty(read))
                    {
                        products = JsonConvert.DeserializeObject<List<SalesProduct>>(read);
                        var curProduct = products.Select(p => p).Where(p => p.GuId.ToString() == pid).First();
                        if (curProduct != null)
                        {
                            products.Remove(curProduct);
                            //save
                            _saveAndLoadService.SaveText(fileName, JsonConvert.SerializeObject(products));
                        }
                    }
                    Products = new ObservableCollection<SalesProduct>(products);

                }
                await LoadData();
            }
            catch (Exception ex) when (ex is WebException || ex is HttpRequestException)
            {
                //await _dialogService.ShowAlertAsync("Communication error", "Error", "Ok");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in: {ex}");
            }
        }

        private async Task DelAllProduct()
        {
            try
            {
                var fileName = "products_restaurant.json";
                _saveAndLoadService.DelFile(fileName);
                await LoadData();
            }
            catch (Exception ex) when (ex is WebException || ex is HttpRequestException)
            {
                //await _dialogService.ShowAlertAsync("Communication error", "Error", "Ok");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in: {ex}");
            }
        }


        /// <summary>
        /// 刷新列表
        /// </summary>
        public ICommand RefreshCommand => new Command(RefreshRidesCommand);
        private async void RefreshRidesCommand(object obj)
        {
            await LoadData();
        }



        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("data") || parameters.ContainsKey("action"))
            {
                Action = (string)parameters["action"];
                Restaurant = (Restaurant)parameters["data"];

                if (string.IsNullOrEmpty(Restaurant.BaseInfo.EndPointNumber))
                {
                    _dialogService.ShowAlertAsync("流程操作无效...", "提示", "取消");
                    _navigationService.GoBackAsync();
                }

                if (Action.ToLower() == "edit")
                {
                    Title = "编辑传统终端普查-销售信息";
                }
            }
        }

        /// <summary>
        /// 步骤完成确认
        /// </summary>
        private DelegateCommand<string> _confimWFCmd;
        public DelegateCommand<string> ConfimWFCmd
        {
            get
            {
                if (_confimWFCmd == null)
                {
                    _confimWFCmd = new DelegateCommand<string>(async (r) =>
                    {
                        using (UserDialogs.Instance.Loading("上传数据中..."))
                        {
                            await Task.Delay(1000);

                            try
                            {
                                var products = Products;
                                if (products.Count > 0)
                                {
                                    foreach (var p in products)
                                    {
                                        Restaurant.SalesProducts.Add(new SalesProduct()
                                        {
                                            Brand = p.Brand,
                                            ProductName = p.ProductName,
                                            AnnualSales = p.AnnualSales,
                                            PackingForm = p.ProductProvider,
                                            ProductProvider = p.Brand,
                                            ChannelAttributes = p.ChannelAttributes,
                                            Specification = p.Specification
                                        });
                                    }
                                }
                                else
                                {
                                    await _dialogService.ShowAlertAsync("请添加销售商品信息", "提示", "确定");
                                    return;
                                }

                                //添加跟踪
                                try
                                {
                                    var trackLocation = new TrackLocation()
                                    {
                                        UserId = Restaurant != null ? Restaurant.UserId : Settings.UserId.ToString(),
                                        Start = GlobalSettings.EventDate != null ? GlobalSettings.EventDate : DateTime.Now,
                                        Stop = DateTime.Now,
                                        CensusType = 2,
                                        Duration = (int)DateTime.Now.Subtract(GlobalSettings.EventDate).TotalSeconds,
                                        EndPointNumber = Restaurant.BaseInfo != null ? Restaurant.BaseInfo.EndPointNumber : ""
                                    };

                                    Restaurant.TrackLocation = trackLocation;
                                }
                                catch (Exception)
                                {

                                }

                                //提交数据
                                var result = await _censusService.AddRestaurant(Restaurant);

                                if (result != null)
                                {
                                    _dialogService.LongAlert(result.msg);
                                }

                                try
                                {
                                    //清空临时
                                    await DelAllProduct();
                                }
                                catch (Exception)
                                {
                                }

                            }
                            catch (Exception ex)
                            {
                                await _dialogService.ShowAlertAsync("提交数据失败:" + ex.Message, "Error", "确定");
                                return;
                            }
                        };

                        await _navigationService.NavigateAsync("Networks");
                        await _navigationService.RemoveLastFromBackStackAsync();
                        MessagingCenter.Send<NavigationMessage>(new NavigationMessage() { Paremeter = "Add" }, "GotoHomeToolbarItem");

                        try
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await _cacheManager.DeleteAll();
                            });
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.Print(ex.Message);
                        }

                    });

                }
                return _confimWFCmd;
            }
        }


        /// <summary>
        /// 编辑商品ItemSelectedCommand
        /// </summary>
        public ICommand ItemSelectedCommand => new Command<SalesProduct>(async (item) => { await OnSelectItem(item); });
        private async Task OnSelectItem(SalesProduct item)
        {
            try
            {
                if (item != null)
                {
                    var parameter = new NavigationParameters()
                    {
                        { "data", "Restaurant" },
                        { "productid", item.GuId },
                        { "action", "edit" }
                    };
                    await _navigationService.NavigateAsync("AddProducts", parameter);
                }
                else
                {
                    _dialogService.LongAlert("操作无效！");
                }
            }
            catch (Exception ex)
            {
                _dialogService.LongAlert(ex.Message);
            }
        }



        public event EventHandler IsActiveChanged;
        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                OnActiveTabChangedAsync();
            }
        }
        private async void OnActiveTabChangedAsync()
        {
            if (IsActive)
            {
                await LoadData();
            }
        }


        private async Task LoadData()
        {
            IsBusy = true;

            try
            {
                var fileName = "products_restaurant.json";
                var products = new List<SalesProduct>();
                var read = _saveAndLoadService.LoadText(fileName);
                if (!string.IsNullOrEmpty(read))
                {
                    products = JsonConvert.DeserializeObject<List<SalesProduct>>(read);
                }
                Products = new ObservableCollection<SalesProduct>(products);
            }
            catch (Exception ex) when (ex is WebException || ex is HttpRequestException)
            {
                //await _dialogService.ShowAlertAsync("Communication error", "Error", "Ok");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in: {ex}");
            }

            IsBusy = false;
        }
    }
}
