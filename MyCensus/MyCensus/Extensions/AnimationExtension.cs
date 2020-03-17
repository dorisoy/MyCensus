using MyCensus.Animations.Base;
using System;
using Xamarin.Forms;

namespace MyCensus.Extensions
{
    /// <summary>
    /// 动画效果扩展
    /// </summary>
    public static class AnimationExtension
    {
        public static async void Animate(this VisualElement visualElement, AnimationBase animation, Action onFinishedCallback = null)
        {
            animation.Target = visualElement;
            await animation.Begin();
            onFinishedCallback?.Invoke();
        }
    }
}