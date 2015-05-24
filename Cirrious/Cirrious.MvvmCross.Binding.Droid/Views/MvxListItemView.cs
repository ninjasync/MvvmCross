// MvxListItemView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Content;
using Android.Runtime;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    [Register("cirrious.mvvmcross.binding.droid.views.MvxListItemView")]
    public class MvxListItemView
        : MvxBaseListItemView
          , IMvxListItemView
    {
        private readonly int _templateId;

        public MvxListItemView(Context context,
                               IMvxLayoutInflater layoutInflater,
                               object dataContext,
                               int templateId)
            : base(context, layoutInflater, dataContext)
        {
            _templateId = templateId;
            AndroidBindingContext.BindingInflate(templateId, this);
        }

#if !DOT42
		protected MvxListItemView(IntPtr javaReference, JniHandleOwnership transfer)
			: base(javaReference, transfer)
	    {
	    }
#endif

        public int TemplateId
        {
            get { return _templateId; }
        }
    }
}