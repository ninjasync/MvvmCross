// MvxViewVisibleBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Views;
using Cirrious.MvvmCross.Binding.ExtensionMethods;

namespace Cirrious.MvvmCross.Binding.Droid.Target
{
    public class MvxViewVisibleBinding
        : MvxBaseViewVisibleBinding
    {
        public MvxViewVisibleBinding(object target)
            : base(target)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
#if !DOT42
            ((View)target).Visibility =  value.ConvertToBoolean() ? ViewStates.Visible : ViewStates.Gone;
#else
            ((View)target).Visibility = value.ConvertToBoolean() ? View.VISIBLE : View.GONE;
#endif
        }
    }
}