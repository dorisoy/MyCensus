using Acr.UserDialogs;
using MyCensus.DataServices.Interfaces;
using MyCensus.Helpers;
using MyCensus.Models;
using MyCensus.Models.Census;
using MyCensus.Services;
using MyCensus.Services.Interfaces;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism;
using Prism.Commands;
using Prism.Navigation;
using Stormlion.PhotoBrowser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;


namespace MyCensus.ViewModels
{

    public class Restaurant_Card_Page1ViewModel : ViewModelBase, IActiveAware
    {

        private new INavigationService _navigationService;
        private new IDialogService _dialogService;
        private ICensusService _censusService;
        private IMediaPickerService _mediaPickerService;
        private IImageEditor _imageEditor;
        private MediaFile _mediaFile;

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

        public Restaurant_Card_Page1ViewModel(INavigationService navigationService,
             ICensusService censusService,
             IImageEditor imageEditor,
            IMediaPickerService mediaPickerService,
            IDialogService dialogService) : base(navigationService, dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _censusService = censusService;
            _mediaPickerService = mediaPickerService;
            _imageEditor = imageEditor;

            //普查步骤
            var steps = new Dictionary<int, string> { { 0, "基础信息" }, { 1, "经营信息" }, { 2, "销售信息" } };
            Steps = steps;
            Step = 1;

            Title = "添加餐饮终端普查-基本信息";


            //
            GlobalSettings.EventDate = DateTime.Now;

            EndPointNumber_Tip = "请定义终端编号";
            SaleRegion_Tip = "请输入所在大区";
            SalesDepartment_Tip = "请输入业务部";
            City_Tip = "请确定你所在城市";
            DistrictOrCounty_Tip = "请输入你所在区/县";
            CityOrTown_Tip = "请输入城区/乡镇";
            EndPointStorsName_Tip = "请输入终端店名";
            EndPointTelphone_Tip = "固话，填\"区号+号码\"(不加\"-\")，如0293248123";
            EndPointAddress_Tip = "终端地址（详细街道/门牌号）";

            Restaurant = new Restaurant()
            {
                UserId = Settings.UserId.ToString(),
                Latitude = GlobalSettings.CurrtntCoordinate != null ? GlobalSettings.CurrtntCoordinate.Latitude : 0,
                Longitude = GlobalSettings.CurrtntCoordinate != null ? GlobalSettings.CurrtntCoordinate.Longitude : 0,
                Location = GlobalSettings.CurrentAddComp != null ? GlobalSettings.CurrentAddComp.City : ""
            };

            BaseInfo = new RestaurantBaseInfo()
            {
                EndPointNumber = string.Format("{0}_{1}", "CY", CommonHelper.GetTimeStamp(DateTime.Now, 13)),
                SaleRegion = Settings.SaleRegion,
                SalesDepartment = Settings.SalesDepartment
            };
        }



        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("action"))
            {
                Action = (string)parameters["action"];
                if (Action.ToLower() == "edit")
                {
                    Title = "编辑餐饮终端普查-基础信息";
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

                            if (string.IsNullOrEmpty(ThumbnailPhotoUrl))
                            {
                                await TakePhotograph();
                                return;
                            }

                            if (string.IsNullOrEmpty(BaseInfo.EndPointNumber))
                            {
                                EndPointNumber_Tip = "请定义终端编号";
                                valid = true;
                            }

                            if (string.IsNullOrEmpty(BaseInfo.SaleRegion))
                            {
                                SaleRegion_Tip = "请输入所在大区";
                                valid = true;
                            }

                            if (string.IsNullOrEmpty(BaseInfo.SalesDepartment))
                            {
                                SalesDepartment_Tip = "请输入业务部";
                                valid = true;
                            }

                            if (string.IsNullOrEmpty(BaseInfo.City))
                            {
                                City_Tip = "请确定你所在城市";
                                valid = true;
                            }

                            if (string.IsNullOrEmpty(BaseInfo.DistrictOrCounty))
                            {
                                DistrictOrCounty_Tip = "请输入你所在区/县";
                                valid = true;
                            }

                            if (string.IsNullOrEmpty(BaseInfo.CityOrTown))
                            {
                                CityOrTown_Tip = "请输入城区/乡镇";
                                valid = true;
                            }

                            if (string.IsNullOrEmpty(BaseInfo.EndPointStorsName))
                            {
                                EndPointStorsName_Tip = "请输入终端店名";
                                valid = true;
                            }

                            if (string.IsNullOrEmpty(BaseInfo.EndPointTelphone))
                            {
                                EndPointTelphone_Tip = "请输入营业电话";
                                valid = true;
                            }
                            else
                            {
                                if (!CommonHelper.IsPhoneNo(BaseInfo.EndPointTelphone, true))
                                {
                                    EndPointTelphone_Tip = "请输入营业电话格式有误,如:029888888,091688888,13002900000";
                                    valid = true;
                                }
                            }
                          

                            if (string.IsNullOrEmpty(BaseInfo.EndPointAddress))
                            {
                                EndPointAddress_Tip = "请输入终端地址（详细街道/门牌号）";
                                valid = true;
                            }

                            if (valid)
                            {
                                _dialogService.LongAlert("录入验证失败,请确认必填项目并检查格式.");
                                return;
                            }

                            if (GlobalSettings.CurrentAddComp == null)
                            {
                                _dialogService.LongAlert("获取不到终端坐标,请重新尝试定位!");
                                return;
                            }

                            if (MyCensus.Utils.MathHelper.IsNotDouble(Restaurant.Latitude.ToString()) || MyCensus.Utils.MathHelper.IsNotDouble(Restaurant.Longitude.ToString()))
                            {
                                _dialogService.LongAlert("获取不到终端坐标,请重新尝试定位!");
                                return;
                            }

                            Restaurant.BaseInfo = BaseInfo;
                            Restaurant.DoorheadPhotos.Add(new DoorheadPhoto() { StoragePath = ThumbnailPhotoUrl });

                            var parameter = new NavigationParameters() {
                                { "data", Restaurant },
                                { "action",Action}
                            };
                            //await _dialogService.ShowAlertAsync("提示", "抱歉,功能稍后开放...", "取消");
                            await _navigationService.NavigateAsync("Restaurant_Card_Page2", parameter);
                        }
                        catch (Exception ex)
                        {
                            _dialogService.LongAlert("系统错误，请反馈Bug！");
                        }
                    });
                }
                return _nextWFCmd;
            }
        }


        /// <summary>
        /// 餐饮
        /// </summary>
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


        /// <summary>
        /// 基本信息绑定
        /// </summary>
        private RestaurantBaseInfo _baseInfo;
        public RestaurantBaseInfo BaseInfo
        {
            get
            {
                return _baseInfo;
            }
            set
            {
                _baseInfo = value;
                RaisePropertyChanged(() => BaseInfo);
            }
        }



        private string _EndPointNumber_Tip { get; set; }
        private string _SaleRegion_Tip { get; set; }
        private string _SalesDepartment_Tip { get; set; }
        private string _City_Tip { get; set; }
        private string _DistrictOrCounty_Tip { get; set; }
        private string _CityOrTown_Tip { get; set; }
        private string _EndPointStorsName_Tip { get; set; }
        private string _EndPointTelphone_Tip { get; set; }
        private string _EndPointAddress_Tip { get; set; }

        public string EndPointNumber_Tip
        {
            get
            {
                return _EndPointNumber_Tip;
            }
            set
            {
                _EndPointNumber_Tip = value;
                RaisePropertyChanged(() => EndPointNumber_Tip);
            }
        }
        public string SaleRegion_Tip
        {
            get
            {
                return _SaleRegion_Tip;
            }
            set
            {
                _SaleRegion_Tip = value;
                RaisePropertyChanged(() => SaleRegion_Tip);
            }
        }
        public string SalesDepartment_Tip
        {
            get
            {
                return _SalesDepartment_Tip;
            }
            set
            {
                _SalesDepartment_Tip = value;
                RaisePropertyChanged(() => SalesDepartment_Tip);
            }
        }
        public string City_Tip
        {
            get
            {
                return _City_Tip;
            }
            set
            {
                _City_Tip = value;
                RaisePropertyChanged(() => City_Tip);
            }
        }
        public string DistrictOrCounty_Tip
        {
            get
            {
                return _DistrictOrCounty_Tip;
            }
            set
            {
                _DistrictOrCounty_Tip = value;
                RaisePropertyChanged(() => DistrictOrCounty_Tip);
            }
        }
        public string CityOrTown_Tip
        {
            get
            {
                return _CityOrTown_Tip;
            }
            set
            {
                _CityOrTown_Tip = value;
                RaisePropertyChanged(() => CityOrTown_Tip);
            }
        }
        public string EndPointStorsName_Tip
        {
            get
            {
                return _EndPointStorsName_Tip;
            }
            set
            {
                _EndPointStorsName_Tip = value;
                RaisePropertyChanged(() => EndPointStorsName_Tip);
            }
        }
        public string EndPointTelphone_Tip
        {
            get
            {
                return _EndPointTelphone_Tip;
            }
            set
            {
                _EndPointTelphone_Tip = value;
                RaisePropertyChanged(() => EndPointTelphone_Tip);
            }
        }
        public string EndPointAddress_Tip
        {
            get
            {
                return _EndPointAddress_Tip;
            }
            set
            {
                _EndPointAddress_Tip = value;
                RaisePropertyChanged(() => EndPointAddress_Tip);
            }
        }



        /// <summary>
        /// 缩略图
        /// </summary>
        private ImageSource _thumbnailPhoto;
        public ImageSource ThumbnailPhoto
        {
            get
            {
                return _thumbnailPhoto;
            }
            set
            {
                _thumbnailPhoto = value;
                RaisePropertyChanged(() => ThumbnailPhoto);
            }
        }


        private string _thumbnailPhotoUrl;
        public string ThumbnailPhotoUrl
        {
            get
            {
                return _thumbnailPhotoUrl;
            }
            set
            {
                _thumbnailPhotoUrl = value;
                RaisePropertyChanged(() => ThumbnailPhotoUrl);
            }
        }


        /// <summary>
        /// 拍照上传
        /// </summary>
        public ICommand CameraPhotoCmd => new Command<string>(async (id) => { await CameraPhoto(id); });
        private async Task CameraPhoto(string pid)
        {
            await TakePhotograph();
        }


        /// <summary>
        /// 预览缩略图
        /// </summary>
        public ICommand ShowViewerCmd => new Command<string>((url) => { ShowViewer(url); });
        private void ShowViewer(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                using (UserDialogs.Instance.Loading("下载中..."))
                {
                    new PhotoBrowser
                    {
                        Photos = new List<Photo>
                        {
                            new Photo
                            {
                                URL = url,
                                Title = ""
                            }
                        },
                        ActionButtonPressed = (index) =>
                        {
                            //Debug.WriteLine($"Clicked {index}");
                        }
                    }.Show();
                }
            }
        }

        private async Task TakePhotograph()
        {
            try
            {
                await CrossMedia.Current.Initialize();

                /*
                 if (Plugin.Media.CrossMedia.Current.IsTakePhotoSupported && CrossMedia.Current.IsCameraAvailable)
                    {
                            var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions() { SaveToAlbum=false, SaveMetaData=false});

                            if (photo != null)
                                return ImageSource.FromStream(() => { return photo.GetStream(); });
                            else
                                return null;

                    }
                    else
                    {
                        return null;
                    }
                 */

                if (!CrossMedia.Current.IsTakePhotoSupported || !CrossMedia.Current.IsCameraAvailable)
                {
                    await _dialogService.ShowAlertAsync("抱歉,没有检测到相机...", "提示", "取消");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        SaveToAlbum = true,
                        //存储目录
                        Directory = "Demo"
                    });

                if (file == null)
                    return;

                //裁切图片481X480
                var imageEditor = await _imageEditor.CreateImageAsync(CommonHelper.StreamToBytes(file.GetStream()));
                imageEditor.Resize(480, 480);
                var convertStream = CommonHelper.BytesToStream(imageEditor.ToJpeg());
                imageEditor.Dispose();

                //上传图片
                using (UserDialogs.Instance.Loading("上传图片中..."))
                {
                    var content = new MultipartFormDataContent();
                    content.Add(new StreamContent(convertStream), "\"file\"", $"\"{file.Path}\"");
                    var httpClient = new HttpClient();
                    var uploadServiceBaseAddress = "http://www.jshcrb.com:9100/document/reomte/fileupload/HRXHJS";
                    //var uploadServiceBaseAddress = "http://192.168.1.42:9100/document/reomte/fileupload/HRXHJS";
                    var httpResponseMessage = await httpClient.PostAsync(uploadServiceBaseAddress, content);
                    //RemotePathLabel.Text = await httpResponseMessage.Content.ReadAsStringAsync();
                    //new { Success = "上传失败,文件不存在!", MD5="" }
                    var result = await httpResponseMessage.Content.ReadAsStringAsync();
                    var uploadResult = new UploadResult();

                    if (!string.IsNullOrEmpty(result))
                    {
                        uploadResult = JsonConvert.DeserializeObject<UploadResult>(result);
                    }

                    if (uploadResult != null)
                    {
                        //ThumbnailPhotoUrl = "http://192.168.1.42:9100/HRXHJS/document/image/" + uploadResult.Id + "";
                        ThumbnailPhotoUrl = "http://www.jshcrb.com:9100/HRXHJS/document/image/" + uploadResult.Id + "";
                    }

                    _dialogService.LongAlert(uploadResult != null ? uploadResult.Success : "未知错误!");

                    ThumbnailPhoto = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        //string base64Str = "";
                        //using (MemoryStream memStream = new MemoryStream())
                        //{
                        //    stream.CopyToAsync(memStream);
                        //    base64Str = Convert.ToBase64String(memStream.ToArray());
                        //}
                        //System.Diagnostics.Debug.Print("base64Str-------->" + base64Str);
                        if (file != null)
                            file.Dispose();
                        return stream;
                    });
                };

            }
            catch (Exception ex)
            {
                //ex.Message = "Unable to get file location. This most likely means that the file provider information is not set in your Android Manifest file. Please check documentation on how to set this up in your project."
                //"Unable to get file location. This most likely means that the file provider information is not set in your Android Manifest file. Please check documentation on how to set this up in your project."
                await _dialogService.ShowAlertAsync(ex.Message, "提示", "取消");
            }
        }




        /// <summary>
        /// 自动定位
        /// </summary>
        public ICommand AutoPositionCmd => new Command<string>(async (id) => { await AutoPosition(id); });
        private async Task AutoPosition(string pid)
        {
            //string imageAsBase64 = await _mediaPickerService.PickImageAsBase64String();
            //await _dialogService.ShowAlertAsync("抱歉,功能稍后开放...", "提示", "取消");
            using (UserDialogs.Instance.Loading("位置获取中..."))
            {
                await Task.Delay(1000);
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (GlobalSettings.CurrentAddComp == null)
                    {
                        _dialogService.LongAlert("你需要前往位置导航先定位下你坐标!");
                    }

                    Restaurant.Latitude = GlobalSettings.CurrtntCoordinate != null ? GlobalSettings.CurrtntCoordinate.Latitude : 0;
                    Restaurant.Longitude = GlobalSettings.CurrtntCoordinate != null ? GlobalSettings.CurrtntCoordinate.Longitude : 0;
                    Restaurant.Location = GlobalSettings.CurrentAddComp != null ? GlobalSettings.CurrentAddComp.City : "";
                    BaseInfo.City = GlobalSettings.CurrentAddComp != null ? GlobalSettings.CurrentAddComp.City : "";
                    BaseInfo.DistrictOrCounty = GlobalSettings.CurrentAddComp != null ? GlobalSettings.CurrentAddComp.District : "";
                    BaseInfo.CityOrTown = GlobalSettings.CurrentAddComp != null ? GlobalSettings.CurrentAddComp.Street : "";
                    BaseInfo.EndPointAddress = GlobalSettings.CurrentAddComp != null ? GlobalSettings.CurrentAddComp.Address : "";
                    this.BaseInfo = BaseInfo;
                });
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
