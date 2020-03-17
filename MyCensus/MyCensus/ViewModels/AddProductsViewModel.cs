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

using MyCensus.Services;
using MyCensus.Models.Census;
using MyCensus.ViewModels.Base;
using MyCensus.Helpers;
using MyCensus.DataServices.Interfaces;
using Newtonsoft.Json;
using System.Windows.Input;
using Xamarin.Forms;
using MyCensus.Views;
using MyCensus.Validations;
using MyCensus.Cache;


namespace MyCensus.ViewModels
{
    public class AddProductsViewModel : ViewModelBase, IActiveAware
    {
        private new INavigationService _navigationService;
        private new IDialogService _dialogService;
        private ISaveAndLoad _saveAndLoadService;
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



        private string _brandSelectedItem;
        public string BrandSelectedItem
        {
            get => _brandSelectedItem;
            set { _brandSelectedItem = value;
                OnPropertyChanged(); }
        }

        private List<string> _brand;
        /// <summary>
        /// 品牌(终端所销售的啤酒品牌)青岛啤酒、雪花啤酒、Budweiser百威、其他
        /// </summary>
        public List<string> Brand
        {
            get
            {
                return _brand;
            }
            set
            {
                _brand = value;
                RaisePropertyChanged(() => Brand);
            }
        }



        private string _packingFormSelectedItem;
        public string PackingFormSelectedItem
        {
            get => _packingFormSelectedItem;
            set { _packingFormSelectedItem = value;
                OnPropertyChanged(); }
        }
        private List<string> _packingForm;
        /// <summary>
        /// 包装形式(瓶、听、桶)(所售啤酒产品的最小包装形式)
        /// </summary>
        public List<string> PackingForm
        {
            get
            {
                return _packingForm;
            }
            set
            {
                _packingForm = value;
                RaisePropertyChanged(() => PackingForm);
            }
        }



        private string _specificationSelectedItem;
        public string SpecificationSelectedItem
        {
            get => _specificationSelectedItem;
            set { _specificationSelectedItem = value; OnPropertyChanged(); }
        }
        private List<string> _specification;
        public List<string> Specification
        {
            get
            {
                return _specification;
            }
            set
            {
                _specification = value;
                RaisePropertyChanged(() => PackingForm);
            }
        }




        private string _channelAttributesSelectedItem;
        public string ChannelAttributesSelectedItem
        {
            get => _channelAttributesSelectedItem;
            set { _channelAttributesSelectedItem = value; OnPropertyChanged(); }
        }
        private List<string> _channelAttributes;
        /// <summary>
        /// 渠道属性((一批、二批、其他)（销售啤酒产品给此终端的渠道商在其代理的啤酒产品渠道中的层级，包括一批、二批和其他（一批、二批之外））
        /// </summary>
        public List<string> ChannelAttributes
        {
            get
            {
                return _channelAttributes;
            }
            set
            {
                _channelAttributes = value;
                RaisePropertyChanged(() => ChannelAttributes);
            }
        }



        public AddProductsViewModel(INavigationService navigationService,
            ISaveAndLoad saveAndLoadService,
            ICacheManager cacheManager,
            IDialogService dialogService) : base(navigationService, dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _cacheManager = cacheManager;
            _saveAndLoadService = saveAndLoadService;
            Title = "添加商品";

            //初始配置
            InitSettting();
            //
            _productName = new ValidatableObject<string>();
            _annualSales = new ValidatableObject<string>();
            _productProvider = new ValidatableObject<string>();

            AddValidations();

        }


        /// <summary>
        /// 初始配置
        /// </summary>
        public void InitSettting()
        {
            var setting = GlobalSettings.SalesProductSetting;
            if (setting != null)
            {
                Brand = setting.Brand.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                PackingForm = setting.PackingForm.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                ChannelAttributes = setting.ChannelAttributes.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                Specification = setting.Specification.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            else
            {
                Brand = new List<string>() { "雪花", "青岛", "百威", "其他" };
                PackingForm = new List<string>() { "瓶", "听", "桶", "其他" };
                ChannelAttributes = new List<string>() { "一批", "二批", "其他" };
                Specification = new List<string>() { "12*330ML", "24*330ML", "9*500ML", "12*500ML", "9*620ML", "9*510ML", "9*600ML", "9*508ML", "12*600ML" };
            }
        }


        private int _brandSelectedIndex;
        public int BrandSelectedIndex
        {
            get
            {
                return _brandSelectedIndex;
            }
            set
            {
                if (_brandSelectedIndex != value)
                {
                    _brandSelectedIndex = value;
                    RaisePropertyChanged(() => BrandSelectedIndex);
                }
            }
        }


        private int _packingFormSelectedIndex;
        public int PackingFormSelectedIndex
        {
            get
            {
                return _packingFormSelectedIndex;
            }
            set
            {
                if (_packingFormSelectedIndex != value)
                {
                    _packingFormSelectedIndex = value;
                    RaisePropertyChanged(() => PackingFormSelectedIndex);
                }
            }
        }
   


        private int _channelAttributesSelectedIndex;
        public int ChannelAttributesSelectedIndex
        {
            get
            {
                return _channelAttributesSelectedIndex;
            }
            set
            {
                if (_channelAttributesSelectedIndex != value)
                {
                    _channelAttributesSelectedIndex = value;
                    RaisePropertyChanged(() => ChannelAttributesSelectedIndex);
                }
            }
        }


        private int _specificationSelectedIndex;
        public int SpecificationSelectedIndex
        {
            get
            {
                return _specificationSelectedIndex;
            }
            set
            {
                if (_specificationSelectedIndex != value)
                {
                    _specificationSelectedIndex = value;
                    RaisePropertyChanged(() => SpecificationSelectedIndex);
                }
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

        /// <summary>
        /// 类别
        /// </summary>
        private string _category;
        public string Category
        {
            get { return _category; }
            set
            {
                this.SetProperty(ref _category, value);
            }
        }

        //productid
        private string _productid;
        public string ProductId
        {
            get { return _productid; }
            set
            {
                this.SetProperty(ref _productid, value);
            }
        }


        private ValidatableObject<string> _productName;
        private ValidatableObject<string> _annualSales;
        private ValidatableObject<string> _productProvider;


        public ValidatableObject<string> ProductName
        {
            get
            {
                return _productName;
            }
            set
            {
                _productName = value;
                RaisePropertyChanged(() => ProductName);
            }
        }
        public ValidatableObject<string> AnnualSales
        {
            get
            {
                return _annualSales;
            }
            set
            {
                _annualSales = value;
                RaisePropertyChanged(() => AnnualSales);
            }
        }
        public ValidatableObject<string> ProductProvider
        {
            get
            {
                return _productProvider;
            }
            set
            {
                _productProvider = value;
                RaisePropertyChanged(() => ProductProvider);
            }
        }



        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("data") || parameters.ContainsKey("action"))
            {
                try
                {
                    Action = (string)parameters["action"];
                    Category = (string)parameters["data"] ?? "";
                    ProductId = (string)parameters["productid"] ?? "";
                    switch (Action.ToLower())
                    {
                        case "add":
                            Title = "添加商品";
                            break;
                        case "edit":
                            Title = "编辑商品";
                            break;
                    }
                }
                catch (Exception ex)
                {
                    _dialogService.ShowAlertAsync(ex.Message, "警告", "取消");
                }
            }
        }


        private bool Validate()
        {
            //bool isValidProductName = _productName.Validate();
            bool isValidAnnualSales = _annualSales.Validate();
            bool isValidProductProvider = _productProvider.Validate();
            return isValidAnnualSales && isValidProductProvider;
        }

        private void AddValidations()
        {
            _productName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "商品名称不能为空" });
            _annualSales.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "年销量不能为空" });
            _annualSales.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "提供商不能为空" });
        }


        /// <summary>
        /// 保存商品
        /// </summary>
        private DelegateCommand<string> _saveProductCmd;
        public DelegateCommand<string> SaveProductCmd
        {
            get
            {
                if (_saveProductCmd == null)
                {
                    _saveProductCmd = new DelegateCommand<string>(async (r) =>
                    {
                        var fileName = "products_" + Category.ToLower() + ".json";
                        try
                        {

                            var curBread = string.IsNullOrEmpty(BrandSelectedItem) ? "其他" : BrandSelectedItem;
                            var curChannel = string.IsNullOrEmpty(ChannelAttributesSelectedItem) ? "其他" : ChannelAttributesSelectedItem;
                            var curPcking = string.IsNullOrEmpty(PackingFormSelectedItem) ? "其他" : PackingFormSelectedItem;


                            bool isValid = Validate();

                            if (string.IsNullOrEmpty(ProductName.Value))
                            {
                                _dialogService.LongAlert("商品名称不能为空");
                                return;
                            }
                            if (ProductName.Value.Length <= 4)
                            {
                                _dialogService.LongAlert("商品名称如实填写，领导会审查哦！");
                                return;
                            }
                            if (ProductName.Value.IndexOf(curBread) < 0)
                            {
                                _dialogService.LongAlert("你的商品和品牌不对应？");
                                return;
                            }

                            if (string.IsNullOrEmpty(AnnualSales.Value))
                            {
                                 _dialogService.LongAlert("年销量不能为空");
                                return;
                            }

                            if (AnnualSales.Value.Length <= 4)
                            {
                                _dialogService.LongAlert("年销量如实填写，领导会审查哦！");
                                return;
                            }

                            if (AnnualSales.Value.IndexOf("箱/年") < 0)
                            {
                                _dialogService.LongAlert("年销量如实填写，领导会审查哦！");
                                return;
                            }

                            if (string.IsNullOrEmpty(ProductProvider.Value))
                            {
                                 _dialogService.LongAlert("提供商不能为空");
                                return;
                            }
                            if (ProductProvider.Value.Length <= 4)
                            {
                                _dialogService.LongAlert("提供商如实填写，领导会审查哦！");
                                return;
                            }

                            var products = new List<SalesProduct>();
                            var read = _saveAndLoadService.LoadText(fileName);
                            if (!string.IsNullOrEmpty(read))
                            {
                                products = JsonConvert.DeserializeObject<List<SalesProduct>>(read);
                            }

       
                            //年销量（填写整数 年/箱）4大类品牌（青岛啤酒、雪花啤酒、Budweiser百威、其他）分为四档的容量，高档及以上，中档及中档高，中档低、其他
                            //var curSales = string.Format("{0},{1} 为 {2} 年/箱", curBread, "高档及以上",400);

                            if (!string.IsNullOrEmpty(curBread))
                            {
                                ///添加最近提供商
                                try
                                {
                                    if (!string.IsNullOrEmpty(ProductProvider.Value))
                                    {
                                        var cacheProductProviders = await _cacheManager.Get<List<string>>(GlobalSettings.productProviders_key);
                                        if (cacheProductProviders == null)
                                        {
                                            cacheProductProviders = new List<string>();
                                            cacheProductProviders.Add(ProductProvider.Value);
                                            await _cacheManager.Set<List<string>>(GlobalSettings.productProviders_key, cacheProductProviders);
                                        }
                                        else
                                        {
                                            cacheProductProviders.Add(ProductProvider.Value);
                                            if (cacheProductProviders.Count > 5)
                                            {
                                                cacheProductProviders.RemoveRange(4, 1);
                                            }
                                            await _cacheManager.Set<List<string>>(GlobalSettings.productProviders_key, cacheProductProviders);
                                        }
                                    }
                                }
                                catch (Exception ex) {
                                    _dialogService.ShortAlert(ex.Message);
                                }

                                var tempp = ProductName.Value ?? "";

                                if (Action == "edit")
                                {
              
                                    if (!string.IsNullOrEmpty(ProductId))
                                    {
                                        products.ForEach(p => 
                                        {
                                            if (p.GuId == ProductId)
                                            {
                                                p.Brand = curBread;
                                                p.ProductName = ProductName.Value;
                                                p.AnnualSales = AnnualSales.Value ?? "";
                                                p.ChannelAttributes = curChannel;
                                                p.PackingForm = curPcking;
                                                p.ProductProvider = ProductProvider.Value ?? "";
                                                p.Specification = SpecificationSelectedItem;
                                            }
                                        });

                                        if (products.Count > 0)
                                            _saveAndLoadService.SaveText(fileName, JsonConvert.SerializeObject(products));
                                    }
                                }
                                else
                                {
                                    products.Add(new SalesProduct()
                                    {
                                        Brand = curBread,
                                        ProductName = ProductName.Value,
                                        AnnualSales = AnnualSales.Value ?? "",
                                        ChannelAttributes = curChannel,
                                        PackingForm = curPcking,
                                        ProductProvider = ProductProvider.Value ?? "",
                                        Specification = SpecificationSelectedItem
                                    });

                                    if (products.Count > 0)
                                        _saveAndLoadService.SaveText(fileName, JsonConvert.SerializeObject(products));
                                }
                               
                            }
                            //await _dialogService.ShowAlertAsync("提示", "抱歉,功能稍后开放...", "取消");
                            await _navigationService.GoBackAsync();
                        }
                        catch (Exception ex)
                        {
                            _saveAndLoadService.DelFile(fileName);
                            System.Diagnostics.Debug.Print(ex.Message);
                        }
                    });

                }
                return _saveProductCmd;
            }
        }



        /// <summary>
        /// 扫描条码
        /// </summary>
        public ICommand ScanCodeCommand => new Command(ScanCode);
        private async void ScanCode(object obj)
        {
            //await _dialogService.ShowAlertAsync("Communication error", "Error", "Ok");
            //await _navigationService.NavigateAsync("ScanPage");
            await _navigationService.NavigateAsync("SaoPage");
        }


        //搜索商品
        public ICommand SearchCommand => new Command(Search);
        private async void Search(object obj)
        {
            await _navigationService.NavigateAsync("SearchPage");
        }

        /// <summary>
        /// 年销量动态模板
        /// </summary>
        public ICommand ConvertAnnualSalesCommand => new Command(ConvertAnnualSales);
        private void ConvertAnnualSales(object obj)
        {
            var curBread = string.IsNullOrEmpty(BrandSelectedItem) ? "其他" : BrandSelectedItem;
            var msg = new NavigationMessage() { Paremeter = curBread };
            MessagingCenter.Send(msg, "ConvertAnnualSales");
        }



        public delegate void IsActiveChangedEventHandler(object sender, EventArgs e);
        public event EventHandler IsActiveChanged;
        protected virtual void OnThresholdReached(EventArgs e)
        {
            EventHandler handler = IsActiveChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }


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
                try
                {
                    var cacheProductProviders = await _cacheManager.Get<List<string>>(GlobalSettings.productProviders_key);
                    if (cacheProductProviders.Count > 0)
                        ProductProvider.Value = cacheProductProviders.First();
                }
                catch (Exception ex) { }

                await InitProduct();
            }
        }

        private async Task InitProduct()
        {
            IsBusy = true;
            try
            {
                if (Action == "edit")
                {
                    var fileName = "products_tradition.json";
                    if (Category == "Restaurant")
                    {
                        fileName = "products_restaurant.json";
                    }

                    if (!string.IsNullOrEmpty(ProductId))
                    {
                        var products = new List<SalesProduct>();
                        var read = _saveAndLoadService.LoadText(fileName);
                        if (!string.IsNullOrEmpty(read))
                        {
                            products = JsonConvert.DeserializeObject<List<SalesProduct>>(read);
                        }

                        var curProduct = products.Where(p => p.GuId == ProductId).FirstOrDefault();
                        if (curProduct != null)
                        {

                            BrandSelectedIndex = Brand.IndexOf(curProduct.Brand); 
                            BrandSelectedItem = curProduct.Brand;

                            ChannelAttributesSelectedIndex = ChannelAttributes.IndexOf(curProduct.ChannelAttributes);
                            ChannelAttributesSelectedItem = curProduct.ChannelAttributes;

                            PackingFormSelectedIndex = PackingForm.IndexOf(curProduct.PackingForm);
                            PackingFormSelectedItem = curProduct.PackingForm;

                            SpecificationSelectedIndex = Specification.IndexOf(curProduct.Specification);
                            SpecificationSelectedItem = curProduct.Specification;

                            ProductName.Value = curProduct.ProductName;
                            AnnualSales.Value = curProduct.AnnualSales;
                            ProductProvider.Value = curProduct.ProductProvider;
                        }

                    }
                }
            }
            catch (Exception ex) when (ex is WebException || ex is HttpRequestException)
            {
                //await _dialogService.ShowAlertAsync("Communication error", "Error", "Ok");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in: {ex}");
            }
            finally
            {
                IsBusy = false;
            }
        }


    }
}

