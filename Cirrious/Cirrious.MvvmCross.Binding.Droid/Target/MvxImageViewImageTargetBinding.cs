// MvxImageViewImageTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;
using Android.Widget;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Binding.Droid.Target
{
    public class MvxImageViewImageTargetBinding
        : MvxBaseStreamImageViewTargetBinding
    {
        public MvxImageViewImageTargetBinding(ImageView imageView)
            : base(imageView)
        {
        }

        public override Type TargetType
        {
            get { return typeof (string); }
        }

        protected override Stream GetStream(object value)
        {
            if (value == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Null value passed to ImageView binding");
                return null;
            }

            var stringValue = value as string;
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Empty value passed to ImageView binding");
                return null;
            }

            var drawableResourceName = GetImageAssetName(stringValue);
            var assetStream = AndroidGlobals.ApplicationContext.Assets.Open(drawableResourceName);
#if !DOT42 
            return assetStream;
#else  // TODO: make Dot42 either provide an implicit conversion operator, or return the correct 
       //       stream in the first place.
            return new JavaInputStreamWrapper(assetStream);
#endif
        }

        private static string GetImageAssetName(string rawImage)
        {
            return rawImage.TrimStart('/');
        }
    }
}