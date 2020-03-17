using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism;

using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Ioc;
using MyCensus.Models;
using MyCensus.Services;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;

using System.Diagnostics;
using System.Net;
using System.Net.Http;

using MyCensus.Models.Census;
using MyCensus.ViewModels.Base;
using MyCensus.Helpers;

namespace MyCensus.ViewModels
{


    public class Restaurant_Card_Page2ViewModel : ViewModelBase, IActiveAware
    {

        private new INavigationService _navigationService;
        private new IDialogService _dialogService;


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


        public RestaurantBusinessInfo _businessInfo;
        public RestaurantBusinessInfo BusinessInfo
        {
            get
            {
                return _businessInfo;
            }
            set
            {
                _businessInfo = value;
                RaisePropertyChanged(() => BusinessInfo);
            }
        }


        private string _ChargePerson_Tip { get; set; }
        private string _Telphone_Tip { get; set; }
        private string _PrivateRoomes_Tip { get; set; }
        private string _TableNumber_Tip { get; set; }
        private string _Seats_Tip { get; set; }
        private string _TopContacts_Tip { get; set; }
        private string _TopContactPhone_Tip { get; set; }
        private string _Position_Tip { get; set; }
        private string _ShowcaseNum_Tip { get; set; }
   


        public string ChargePerson_Tip
        {
            get
            {
                return _ChargePerson_Tip;
            }
            set
            {
                _ChargePerson_Tip = value;
                RaisePropertyChanged(() => ChargePerson_Tip);
            }
        }
        public string Telphone_Tip
        {
            get
            {
                return _Telphone_Tip;
            }
            set
            {
                _Telphone_Tip = value;
                RaisePropertyChanged(() => Telphone_Tip);
            }
        }
        public string PrivateRoomes_Tip
        {
            get
            {
                return _PrivateRoomes_Tip;
            }
            set
            {
                _PrivateRoomes_Tip = value;
                RaisePropertyChanged(() => PrivateRoomes_Tip);
            }
        }
        public string TableNumber_Tip
        {
            get
            {
                return _TableNumber_Tip;
            }
            set
            {
                _TableNumber_Tip = value;
                RaisePropertyChanged(() => TableNumber_Tip);
            }
        }
        public string Seats_Tip
        {
            get
            {
                return _Seats_Tip;
            }
            set
            {
                _Seats_Tip = value;
                RaisePropertyChanged(() => Seats_Tip);
            }
        }
        public string TopContacts_Tip
        {
            get
            {
                return _TopContacts_Tip;
            }
            set
            {
                _TopContacts_Tip = value;
                RaisePropertyChanged(() => TopContacts_Tip);
            }
        }
        public string TopContactPhone_Tip
        {
            get
            {
                return _TopContactPhone_Tip;
            }
            set
            {
                _TopContactPhone_Tip = value;
                RaisePropertyChanged(() => TopContactPhone_Tip);
            }
        }
        public string Position_Tip
        {
            get
            {
                return _Position_Tip;
            }
            set
            {
                _Position_Tip = value;
                RaisePropertyChanged(() => Position_Tip);
            }
        }
        public string ShowcaseNum_Tip
        {
            get
            {
                return _ShowcaseNum_Tip;
            }
            set
            {
                _ShowcaseNum_Tip = value;
                RaisePropertyChanged(() => ShowcaseNum_Tip);
            }
        }



        private int _step;
        public int Step
        {
            get { return _step; }
            set
            {
                this.SetProperty(ref _step, value);
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


        private List<string> _endPointType;
        /// <summary>
        /// 终端类型(S超高档餐饮、A高档餐饮、B中档餐饮、C大众餐饮、D低档餐饮)
        /// </summary>
        public List<string> EndPointType
        {
            get
            {
                return _endPointType;
            }
            set
            {
                _endPointType = value;
                RaisePropertyChanged(() => EndPointType);
            }
        }

        private string _endPointTypeSelectedItem;
        public string EndPointTypeSelectedItem
        {
            get => _endPointTypeSelectedItem;
            set { _endPointTypeSelectedItem = value; OnPropertyChanged(); }
        }


        //
        private List<string> _perConsumptions;
        /// <summary>
        ///  人均消费（50元以下，50-100元，100元以上）
        /// </summary>
        public List<string> PerConsumptions
        {
            get
            {
                return _perConsumptions;
            }
            set
            {
                _perConsumptions = value;
                RaisePropertyChanged(() => PerConsumptions);
            }
        }

        private string _perConsumptionsSelectedItem;
        public string PerConsumptionsSelectedItem
        {
            get => _perConsumptionsSelectedItem;
            set { _perConsumptionsSelectedItem = value; OnPropertyChanged(); }
        }


        private bool _isChain;
        /// <summary>
        /// 是否连锁
        /// </summary>
        public bool IsChain
        {
            get
            {
                return _isChain;
            }
            set
            {
                _isChain = value;
                RaisePropertyChanged(() => IsChain);
            }
        }


        private List<string> _characteristics;
        /// <summary>
        /// 经营特色(炒菜/火锅/烧烤/小吃（面馆）快餐/西餐（含日韩料理/自助餐/其它)
        /// </summary>
        public List<string> Characteristics
        {
            get
            {
                return _characteristics;
            }
            set
            {
                _characteristics = value;
                RaisePropertyChanged(() => Characteristics);
            }
        }
        private string _characteristicsSelectedItem;
        public string CharacteristicsSelectedItem
        {
            get => _characteristicsSelectedItem;
            set { _characteristicsSelectedItem = value; OnPropertyChanged(); }
        }

        private bool _isAgreement;
        /// <summary>
        ///  是否协议店
        /// </summary>
        public bool IsAgreement
        {
            get
            {
                return _isAgreement;
            }
            set
            {
                _isAgreement = value;
                RaisePropertyChanged(() => IsAgreement);
            }
        }


        private List<string> _modeOfCooperation;
        /// <summary>
        /// 合作方式(专营保量、专营不保量、混营保量、混营不保量)
        /// </summary>
        public List<string> ModeOfCooperation
        {
            get
            {
                return _modeOfCooperation;
            }
            set
            {
                _modeOfCooperation = value;
                RaisePropertyChanged(() => ModeOfCooperation);
            }
        }
        private string _modeOfCooperationSelectedItem;
        public string ModeOfCooperationSelectedItem
        {
            get => _modeOfCooperationSelectedItem;
            set { _modeOfCooperationSelectedItem = value; OnPropertyChanged(); }
        }

        //ModeOfCooperationSelectedItemLast
        private int _modeOfCooperationSelectedItemLast;
        public int ModeOfCooperationSelectedItemLast
        {
            get => _modeOfCooperationSelectedItemLast;
            set { _modeOfCooperationSelectedItemLast = value; OnPropertyChanged(); }
        }

        private List<string> _endPointStates;
        /// <summary>
        /// 终端状态(专销、强势混销、弱势混销、空白)
        /// </summary>
        public List<string> EndPointStates
        {
            get
            {
                return _endPointStates;
            }
            set
            {
                _endPointStates = value;
                RaisePropertyChanged(() => EndPointStates);
            }
        }
        private string _endPointStatesSelectedItem;
        public string EndPointStatesSelectedItem
        {
            get => _endPointStatesSelectedItem;
            set { _endPointStatesSelectedItem = value; OnPropertyChanged(); }
        }




        //public IList<DropdownClass> MyList { get; set; } = new ObservableCollection<DropdownClass>();
        private DropdownClass _selectedItem;
        private int _selectedIndex;
        public DropdownClass SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; OnPropertyChanged(); }
        }


        public Restaurant_Card_Page2ViewModel(INavigationService navigationService, IDialogService dialogService) : base(navigationService, dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;

            //普查步骤
            var steps = new Dictionary<int, string> { { 0, "基础信息" }, { 1, "经营信息" }, { 2, "销售信息" } };
            Steps = steps;
            Step = 2;
            Title = "添加餐饮终端普查-经营信息";

            //初始枚举
            InitSettting();


            //MyList.Add(new DropdownClass { Id = 1, Name = "S超高档餐饮" });
            //MyList.Add(new DropdownClass { Id = 2, Name = "A高档餐饮" });
            //MyList.Add(new DropdownClass { Id = 3, Name = "B中档餐饮" });


            PrivateRoomes_Tip = "请输入包厢数";
            TableNumber_Tip = "请输入桌台数";
            Seats_Tip = "请输入座位数";
            TopContacts_Tip = "请输入关键人(可选)";
            TopContactPhone_Tip = "请输入关键人电话(可选)";
            Position_Tip = "请输入关键人职位(可选)";
            ShowcaseNum_Tip = "请输入本品展柜投入数量";
            ChargePerson_Tip = "请输入负责人";
            Telphone_Tip = "请输入负责人电话";

            BusinessInfo = new RestaurantBusinessInfo()
            {
            };
        }


        public void InitSettting()
        {
            var setting = GlobalSettings.RestaurantSetting;
            if (setting != null)
            {
                EndPointType = setting.EndPointType.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                ModeOfCooperation = setting.ModeOfCooperation.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                EndPointStates = setting.EndPointStates.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                Characteristics = setting.Characteristics.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                PerConsumptions = setting.PerConsumptions.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                if(PerConsumptions.Count==0) PerConsumptions = new List<string>() { "请选择", "50元以下", "50-100元", "100元以上" };
                if (EndPointType.Count == 0) EndPointType = new List<string>() { "请选择", "S超高档餐饮", "A高档餐饮", "B中档餐饮", "C大众餐饮", "D低档餐饮" };
                if (Characteristics.Count == 0) Characteristics = new List<string>() { "请选择", "炒菜", "火锅", "烧烤", "小吃（面馆）快餐", "西餐（含日韩料理/自助餐)", "其它" };
                if (ModeOfCooperation.Count == 0) ModeOfCooperation = new List<string>() { "请选择", "专营保量", "专营不保量", "混营保量", "混营不保量", "无" };
                if (EndPointStates.Count == 0) EndPointStates = new List<string>() { "请选择", "专销", "强势混销", "弱势混销", "无" };
                if (PerConsumptions.Count == 0) PerConsumptions = new List<string>() { "请选择", "50元以下", "50-100元", "100元以上" };
            }
            else
            {
                EndPointType = new List<string>() { "请选择", "S超高档餐饮", "A高档餐饮", "B中档餐饮", "C大众餐饮", "D低档餐饮" };
                Characteristics = new List<string>() { "请选择", "炒菜", "火锅", "烧烤", "小吃（面馆）快餐", "西餐（含日韩料理/自助餐)", "其它" };
                ModeOfCooperation = new List<string>() { "请选择", "专营保量", "专营不保量", "混营保量", "混营不保量", "无" };
                EndPointStates = new List<string>() { "请选择", "专销", "强势混销", "弱势混销", "无" };
                PerConsumptions = new List<string>() { "请选择", "50元以下", "50-100元", "100元以上" };
            }

            ModeOfCooperationSelectedItemLast = ModeOfCooperation.Count - 1;
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
                    Title = "编辑传统终端普查-经营信息";
                }
            }
        }


        /// <summary>
        /// 下一步
        /// </summary>
        private DelegateCommand<string> _nextWFCmd;
        public DelegateCommand<string> NextWFCmd
        {
            get
            {
                if (_nextWFCmd == null)
                {
                    _nextWFCmd = new DelegateCommand<string>(async (r) =>
                    {
                        try
                        {

                            var valid = false;
                            if (string.IsNullOrEmpty(EndPointTypeSelectedItem) || EndPointTypeSelectedItem == "请选择")
                                valid = true;

                            if (string.IsNullOrEmpty(CharacteristicsSelectedItem) || CharacteristicsSelectedItem == "请选择")
                                valid = true;

                            if (string.IsNullOrEmpty(ModeOfCooperationSelectedItem) || ModeOfCooperationSelectedItem == "请选择")
                                valid = true;

                            if (string.IsNullOrEmpty(EndPointStatesSelectedItem) || EndPointStatesSelectedItem == "请选择")
                                valid = true;


                            int PrivateRoomes = 0;
                            if (!int.TryParse(BusinessInfo.PrivateRoomes.ToString(), out PrivateRoomes))
                            {
                                PrivateRoomes_Tip = "请输入包厢数";
                                valid = true;
                            }

                            int TableNumber = 0;
                            if (!int.TryParse(BusinessInfo.TableNumber.ToString(), out TableNumber))
                            {
                                TableNumber_Tip = "请输入桌台数";
                                valid = true;
                            }

                            int Seats = 0;
                            if (!int.TryParse(BusinessInfo.Seats.ToString(), out Seats))
                            {
                                Seats_Tip = "请输入座位数";
                                valid = true;
                            }

                            //if (string.IsNullOrEmpty(BusinessInfo.TopContacts))
                            //{
                            //    TopContacts_Tip = "请输入关键人";
                            //    valid = true;
                            //}

                            //if (string.IsNullOrEmpty(BusinessInfo.TopContactPhone))
                            //{
                            //    TopContactPhone_Tip = "请输入关键人电话";
                            //    valid = true;
                            //}
                            //else
                            //{
                            //    if (!CommonHelper.IsPhoneNo(BusinessInfo.TopContactPhone, true))
                            //    {
                            //        TopContactPhone_Tip = "电话格式有误,如:029888888,091688888,13002900000";
                            //        valid = true;
                            //    }
                            //}

                            //if (string.IsNullOrEmpty(BusinessInfo.Position))
                            //{
                            //    Position_Tip = "请输入关键人职位"; valid = true;
                            //}

                            int ShowcaseNum = 0;
                            if (!int.TryParse(BusinessInfo.ShowcaseNum.ToString(), out ShowcaseNum))
                            {
                                ShowcaseNum_Tip = "请输入本品展柜投入数量";
                                valid = true;
                            }

                            if (string.IsNullOrEmpty(BusinessInfo.ChargePerson))
                            {
                                ChargePerson_Tip = "请输入负责人";
                                valid = true;
                            }

                            if (string.IsNullOrEmpty(BusinessInfo.Telphone))
                            {
                                Telphone_Tip = "请输入负责人电话";
                                valid = true;
                            }

                            if (!CommonHelper.IsPhoneNo(BusinessInfo.Telphone, true))
                            {
                                Telphone_Tip = "电话格式有误,如:029888888,091688888,13002900000";
                                valid = true;
                            }

                            if (valid)
                            {
                                _dialogService.LongAlert("录入验证失败,请确认必填项目并检查格式.");
                                return;
                            }

                            BusinessInfo.IsChain = this.IsChain;
                            BusinessInfo.IsAgreement = this.IsAgreement;
                            BusinessInfo.EndPointType = EndPointTypeSelectedItem;
                            BusinessInfo.Characteristics = CharacteristicsSelectedItem;
                            BusinessInfo.ModeOfCooperation = ModeOfCooperationSelectedItem;
                            BusinessInfo.EndPointStates = EndPointStatesSelectedItem;
                            BusinessInfo.PerConsumptions = PerConsumptionsSelectedItem;

                            Restaurant.BusinessInfo = BusinessInfo;

                            var parameter = new NavigationParameters() {
                                { "data", Restaurant },
                                { "action",Action}
                            };
                            await _navigationService.NavigateAsync("Restaurant_Card_Page3", parameter);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.Print(ex.Message);
                        }
                    });
                }
                return _nextWFCmd;
            }
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
                await InitiAsync();
            }
        }

        /// <summary>
        /// 初始绑定
        /// </summary>
        /// <returns></returns>
        public async Task InitiAsync()
        {
            IsBusy = true;
            try
            {
                BusinessInfo = new RestaurantBusinessInfo()
                {
                    Telphone = Restaurant.BaseInfo != null ? Restaurant.BaseInfo.EndPointTelphone : "",
                    ChargePerson = Restaurant.BusinessInfo != null ? Restaurant.BusinessInfo.ChargePerson : ""
                };
            }
            catch (Exception ex) when (ex is WebException || ex is HttpRequestException)
            {
                //await _dialogService.ShowAlertAsync("Communication error", "Error", "Ok");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching profile with exception: {ex}");
            }

            IsBusy = false;

        }
    }
}
