using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism;

using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Ioc;
using MyCensus.Services;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

using System.Diagnostics;
using System.Net;
using System.Net.Http;

using MyCensus.Models.Census;
using MyCensus.ViewModels.Base;
using MyCensus.Helpers;


namespace MyCensus.ViewModels
{
	public class Tradition_Card_Page2ViewModel : ViewModelBase, IActiveAware
    {

        private new INavigationService _navigationService;
        private new IDialogService _dialogService;


        private Tradition _tradition;
        public Tradition Tradition
        {
            get
            {
                return _tradition;
            }
            set
            {
                _tradition = value;
                RaisePropertyChanged(() => Tradition);
            }
        }


        public TraditionBusinessInfo _businessInfo;
        public TraditionBusinessInfo BusinessInfo
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
        /// 终端类型(现代小零售(X)/名烟名酒店(M)/传统小零售(T)/批零店（P）)
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



        private bool _isChain;
        /// <summary>
        /// 是否连锁
        /// </summary>
        public bool IsChain {
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
        /// 合作方式(专营保量、专营不保量、混营保量、混营不保量)是协议店时必填，非协议店不填
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
        public List<string> EndPointStates {
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


       



        public Tradition_Card_Page2ViewModel(INavigationService navigationService, IDialogService dialogService) : base(navigationService, dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;

            //普查步骤
            var steps = new Dictionary<int, string> { { 0, "基础信息" }, { 1, "经营信息" }, { 2, "销售信息" } };
            Steps = steps;
            Step = 2;
            Title = "添加传统终端普查-经营信息";


            //初始枚举
            InitSettting();
            SelectedIndex = 1;

            ChargePerson_Tip = "请输入负责人";
            Telphone_Tip = "请输入负责人电话";
            BusinessInfo = new TraditionBusinessInfo()
            {
            };
        }



        public void InitSettting()
        {
            var setting = GlobalSettings.TraditionSetting;
            if (setting != null)
            {
                EndPointType = setting.EndPointType.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                ModeOfCooperation = setting.ModeOfCooperation.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                EndPointStates = setting.EndPointStates.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            else
            {
                EndPointType = new List<string>() { "请选择", "现代小零售(X)", "名烟名酒店(M)", "传统小零售(T)", "批零店（P）" };
                ModeOfCooperation = new List<string>() { "请选择", "专营保量", "专营不保量", "混营保量", "混营不保量", "无" };
                EndPointStates = new List<string>() { "请选择", "专销", "强势混销", "弱势混销", "无" };
            }

            ModeOfCooperationSelectedItemLast = ModeOfCooperation.Count - 1;
        }



        private int _selectedIndex;
        private string _selectedItem;
        private Color _textColor = Color.Red;
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                if (_selectedIndex != value)
                {
                    _selectedIndex = value;
                    //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedIndex)));
                    RaisePropertyChanged(() => SelectedIndex);
                }
            }
        }
        public string SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItem)));
                }
            }
        }

       
        public List<string> List2 { get; private set; }

        public Color TextColor
        {
            get
            {
                return _textColor;
            }
            set
            {
                if (_textColor != value)
                {
                    _textColor = value;
                    //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TextColor)));
                    RaisePropertyChanged(() => TextColor);
                }
            }
        }

        //public ICommand ChangeTextColorCommand
        //{
        //    get;
        //}

        void ChangeColor()
        {
            if (TextColor == Color.Red)
            {
                TextColor = Color.Pink;
            }
            else
            {
                TextColor = Color.Red;
            }
        }



        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("data") || parameters.ContainsKey("action"))
            {
                Action = (string)parameters["action"];
                Tradition = (Tradition)parameters["data"];

                if (string.IsNullOrEmpty(Tradition.BaseInfo.EndPointNumber))
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

                            if (string.IsNullOrEmpty(ModeOfCooperationSelectedItem) || ModeOfCooperationSelectedItem == "请选择")
                                valid = true;

                            if (string.IsNullOrEmpty(EndPointStatesSelectedItem) || EndPointStatesSelectedItem == "请选择")
                                valid = true;

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
                            else
                            {
                                if (!CommonHelper.IsPhoneNo(BusinessInfo.Telphone, true))
                                {
                                    Telphone_Tip = "电话格式有误,如:029888888,091688888,13002900000";
                                    valid = true;
                                }
                            }


                            if (valid)
                            {
                                _dialogService.LongAlert("录入验证失败,请确认必填项目并检查格式.");
                                return;
                            }

                            BusinessInfo.IsChain = this.IsChain;
                            BusinessInfo.IsAgreement = this.IsAgreement;
                            BusinessInfo.EndPointType = EndPointTypeSelectedItem;
                            BusinessInfo.ModeOfCooperation = ModeOfCooperationSelectedItem;
                            BusinessInfo.EndPointStates = EndPointStatesSelectedItem;

                            Tradition.BusinessInfo = BusinessInfo;

                            var parameter = new NavigationParameters() {
                                { "data", Tradition },
                                { "action",Action}
                            };
                            await _navigationService.NavigateAsync("Tradition_Card_Page3", parameter);
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
                BusinessInfo = new TraditionBusinessInfo()
                {
                    Telphone = Tradition.BaseInfo != null ? Tradition.BaseInfo.EndPointTelphone : "",
                    ChargePerson = Tradition.BusinessInfo != null ? Tradition.BusinessInfo.ChargePerson : ""
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
