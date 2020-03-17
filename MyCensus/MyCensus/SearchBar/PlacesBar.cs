//
// PlacesBar.cs
//
// Author:
//       Alex Smith <alex@duriancode.com>
//
// Copyright (c) 2017 (c) Alexander Smith
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyCensus.PlacesSearchBar
{
    /// <summary>
    /// Places retrieved event handler.
    /// </summary>
    public delegate void PlacesRetrievedEventHandler(object sender, AutoCompleteResult result);

    /// <summary>
    /// Places bar.
    /// </summary>
    public class PlacesBar : SearchBar
    {
        /// <summary>
        /// Backing store for the Type property.
        /// </summary>
        public static readonly BindableProperty PlaceTypeProperty = BindableProperty.Create(nameof(Type), typeof(PlaceType), typeof(PlacesBar), PlaceType.All, BindingMode.OneWay, (BindableProperty.ValidateValueDelegate)null, (BindableProperty.BindingPropertyChangedDelegate)null, (BindableProperty.BindingPropertyChangingDelegate)null, (BindableProperty.CoerceValueDelegate)null, (BindableProperty.CreateDefaultValueDelegate)null);

        /// <summary>
        /// Backing store for the Bias property.
        /// </summary>
        public static readonly BindableProperty LocationBiasProperty = BindableProperty.Create(nameof(Bias), typeof(LocationBias), typeof(PlacesBar), (object)null, BindingMode.OneWay, (BindableProperty.ValidateValueDelegate)null, (BindableProperty.BindingPropertyChangedDelegate)null, (BindableProperty.BindingPropertyChangingDelegate)null, (BindableProperty.CoerceValueDelegate)null, (BindableProperty.CreateDefaultValueDelegate)null);

        /// <summary>
        /// Backing store for the Components property.
        /// </summary>
        public static readonly BindableProperty ComponentsProperty = BindableProperty.Create(nameof(Components), typeof(Components), typeof(PlacesBar), (object)null, BindingMode.OneWay, (BindableProperty.ValidateValueDelegate)null, (BindableProperty.BindingPropertyChangedDelegate)null, (BindableProperty.BindingPropertyChangingDelegate)null, (BindableProperty.CoerceValueDelegate)null, (BindableProperty.CreateDefaultValueDelegate)null);

        /// <summary>
        /// Backing store for the ApiKey property.
        /// </summary>
        public static readonly BindableProperty ApiKeyProperty = BindableProperty.Create(nameof(ApiKey), typeof(string), typeof(PlacesBar), string.Empty, BindingMode.OneWay, (BindableProperty.ValidateValueDelegate)null, (BindableProperty.BindingPropertyChangedDelegate)null, (BindableProperty.BindingPropertyChangingDelegate)null, (BindableProperty.CoerceValueDelegate)null, (BindableProperty.CreateDefaultValueDelegate)null);

        /// <summary>
        /// Backing store for the MinimumSearchText property.
        /// </summary>
        public static readonly BindableProperty MinimumSearchTextProperty = BindableProperty.Create(nameof(MinimumSearchText), typeof(int), typeof(PlacesBar), 2, BindingMode.OneWay, (BindableProperty.ValidateValueDelegate)null, (BindableProperty.BindingPropertyChangedDelegate)null, (BindableProperty.BindingPropertyChangingDelegate)null, (BindableProperty.CoerceValueDelegate)null, (BindableProperty.CreateDefaultValueDelegate)null);

        #region Property accessors
        /// <summary>
        /// Gets or sets the place type.
        /// </summary>
        /// <value>The type.</value>
        public PlaceType Type
        {
            get
            {
                return (PlaceType)this.GetValue(PlacesBar.PlaceTypeProperty);
            }
            set
            {
                this.SetValue(PlacesBar.PlaceTypeProperty, (object)value);
            }
        }

        /// <summary>
        /// Gets or sets the location bias.
        /// </summary>
        /// <value>The bias.</value>
        public LocationBias Bias
        {
            get
            {
                return (LocationBias)this.GetValue(PlacesBar.LocationBiasProperty);
            }
            set
            {
                this.SetValue(PlacesBar.LocationBiasProperty, (object)value);
            }
        }

        /// <summary>
        /// Gets or sets the components
        /// </summary>
        public Components Components
        {
            get
            {
                return (Components)this.GetValue(PlacesBar.ComponentsProperty);
            }
            set
            {
                this.SetValue(PlacesBar.ComponentsProperty, (object)value);
            }
        }

        /// <summary>
        /// Gets or sets the API key.
        /// </summary>
        /// <value>The API key.</value>
        public string ApiKey
        {
            get
            {
                return (string)this.GetValue(PlacesBar.ApiKeyProperty);

            }
            set
            {
                this.SetValue(PlacesBar.ApiKeyProperty, (object)value);
            }
        }

        /// <summary>
        /// Gets or sets the minimum search text.
        /// </summary>
        /// <value>The minimum search text.</value>
        public int MinimumSearchText
        {
            get
            {
                return (int)this.GetValue(PlacesBar.MinimumSearchTextProperty);

            }
            set
            {
                this.SetValue(PlacesBar.MinimumSearchTextProperty, (object)value);
            }
        }
        #endregion

        /// <summary>
        /// The places retrieved handler.
        /// </summary>
        public event PlacesRetrievedEventHandler PlacesRetrieved;

        protected virtual void OnPlacesRetrieved(AutoCompleteResult e)
        {
            PlacesRetrievedEventHandler handler = PlacesRetrieved;
            handler?.Invoke(this, e);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DurianCode.PlacesSearchBar.PlacesBar"/> class.
        /// </summary>
        public PlacesBar()
        {
            TextChanged += OnTextChanged;
        }

        /// <summary>
        /// Handles changes to search text.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue) && e.NewTextValue.Length >= MinimumSearchText)
            {
                var predictions = await GetPlaces(e.NewTextValue);

                if (PlacesRetrieved != null && predictions != null)
                    OnPlacesRetrieved(predictions);
                else
                    OnPlacesRetrieved(new AutoCompleteResult());
            }
            else
            {
                OnPlacesRetrieved(new AutoCompleteResult());
            }
        }

        /// <summary>
        /// Calls the Google Places API to retrieve autofill suggestions
        /// </summary>
        /// <returns>The places.</returns>
        /// <param name="newTextValue">New text value.</param>
        async Task<AutoCompleteResult> GetPlaces(string newTextValue)
        {
            try
            {
                await Task.Delay(500);
                var items = new List<AutoCompletePrediction>
                {
                     new AutoCompletePrediction{ Brand="雪花", Grade="高档及以上", ProductName="脸谱花脸"},
                     new AutoCompletePrediction{ Brand="雪花", Grade="高档及以上", ProductName="脸谱花旦"},
                     new AutoCompletePrediction{ Brand="雪花", Grade="高档及以上", ProductName="金标纯生"},
                     new AutoCompletePrediction{ Brand="雪花", Grade="高档及以上", ProductName="概念纯生"},
                     new AutoCompletePrediction{ Brand="雪花", Grade="中档及中档高", ProductName="概念勇闯天涯"},
                     new AutoCompletePrediction{ Brand="雪花", Grade="中档及中档高", ProductName="勇闯天涯superX"},
                     new AutoCompletePrediction{ Brand="雪花", Grade="中档及中档高", ProductName="勇闯天涯"},
                     new AutoCompletePrediction{ Brand="雪花", Grade="中档及中档高", ProductName="干啤"},
                     new AutoCompletePrediction{ Brand="雪花", Grade="中档低", ProductName="原汁麦"},
                     new AutoCompletePrediction{ Brand="雪花", Grade="中档低", ProductName="自然之美"},
                     new AutoCompletePrediction{ Brand="雪花", Grade="中档低", ProductName="冰菓野刺梨"},
                     new AutoCompletePrediction{ Brand="雪花", Grade="其他", ProductName="清爽"},
                     new AutoCompletePrediction{ Brand="雪花", Grade="其他", ProductName="金威果园菠萝味"},
                     new AutoCompletePrediction{ Brand="雪花", Grade="其他", ProductName="金威果园橙味"},
                     new AutoCompletePrediction{ Brand="雪花", Grade="其他", ProductName="金威果园酸梅汤"},
                     new AutoCompletePrediction{ Brand="青岛", Grade="高档及以上", ProductName="鸿运当头"},
                     new AutoCompletePrediction{ Brand="青岛", Grade="高档及以上", ProductName="奥古特"},
                     new AutoCompletePrediction{ Brand="青岛", Grade="高档及以上", ProductName="汉斯红狼"},
                     new AutoCompletePrediction{ Brand="青岛", Grade="高档及以上", ProductName="黑啤"},
                     new AutoCompletePrediction{ Brand="青岛", Grade="高档及以上", ProductName="纯生"},
                     new AutoCompletePrediction{ Brand="青岛", Grade="高档及以上", ProductName="纯生"},
                     new AutoCompletePrediction{ Brand="青岛", Grade="中档及中档高", ProductName="全麦白啤"},
                     new AutoCompletePrediction{ Brand="青岛", Grade="中档及中档高", ProductName="汉斯纯生"},
                     new AutoCompletePrediction{ Brand="青岛", Grade="中档及中档高", ProductName="经典"},
                     new AutoCompletePrediction{ Brand="青岛", Grade="中档及中档高", ProductName="1903"},
                     new AutoCompletePrediction{ Brand="青岛", Grade="中档及中档高", ProductName="冰醇"},
                     new AutoCompletePrediction{ Brand="青岛", Grade="中档及中档高", ProductName="9度"},
                     new AutoCompletePrediction{ Brand="青岛", Grade="中档低", ProductName="汉斯醉美"},
                     new AutoCompletePrediction{ Brand="青岛", Grade="中档低", ProductName="9度"},
                     new AutoCompletePrediction{ Brand="青岛", Grade="其他", ProductName="汉斯干啤"},
                     new AutoCompletePrediction{ Brand="青岛", Grade="其他", ProductName="汉斯小木屋"},
                     new AutoCompletePrediction{ Brand="百威", Grade="高档及以上", ProductName="红百威"},
                     new AutoCompletePrediction{ Brand="百威", Grade="高档及以上", ProductName="百威大瓶"},
                     new AutoCompletePrediction{ Brand="百威", Grade="中档及中档高", ProductName="哈啤小麦王"},
                     new AutoCompletePrediction{ Brand="百威", Grade="中档及中档高", ProductName="哈啤冰纯"},
                     new AutoCompletePrediction{ Brand="其它", Grade="高档及以上", ProductName="红乌苏"},
                     new AutoCompletePrediction{ Brand="其它", Grade="高档及以上", ProductName="绿乌苏"},
                     new AutoCompletePrediction{ Brand="其它", Grade="其他", ProductName="蓝马果乐"},
                     new AutoCompletePrediction{ Brand="其它", Grade="其他", ProductName="蓝马野刺梨"},
                     new AutoCompletePrediction{ Brand="其它", Grade="其他", ProductName="蓝马小橙橙"},
                     new AutoCompletePrediction{ Brand="其它", Grade="其他", ProductName="蓝马酸梅汤"}
                };

                //Detalls

                items.ForEach(p =>
                {
                    p.Detalls = $"{p.Brand},{p.Grade}";
                });

                var tempItem = items;
                tempItem = tempItem.Where(p => p.ProductName.StartsWith(newTextValue) || p.Grade.StartsWith(newTextValue) || p.Brand.StartsWith(newTextValue)).ToList();
                if (tempItem.Count == 0)
                    tempItem = items;

                var result = new AutoCompleteResult() { Status = "", AutoCompletePlaces = tempItem };

                //JsonConvert.DeserializeObject<AutoCompleteResult>(result);

                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("PlacesBar HTTP issue: {0} {1}", ex.Message, ex);
                return null;
            }
        }

    }
}
