// MvxAndroidBindingResource.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com
using System;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Binding.Droid.ResourceHelpers
{
    public class MvxAndroidBindingResource
        : MvxSingleton<IMvxAndroidBindingResource>
        , IMvxAndroidBindingResource
    {
        public static void Initialize()
        {
            if (Instance != null)
                return;

            new MvxAndroidBindingResource();
        }

        private MvxAndroidBindingResource()
        {
            var finder = Mvx.Resolve<IMvxAppResourceTypeFinder>();
            var resourceType = finder.Find();
            try
            {
                var id = resourceType.GetNestedType("Id");
                BindingTagUnique = (int) SafeGetFieldValue(id, "MvxBindingTagUnique");

                var styleable = resourceType.GetNestedType("Styleable");

                var mvxControl = styleable.GetNestedType("MvxControl");
                ControlStylableGroupId = (int[]) SafeGetFieldValue(mvxControl, "AllIds", new int[0]);
                TemplateId = (int)SafeGetFieldValue(mvxControl, "MvxTemplate");

                var mvxBinding = styleable.GetNestedType("MvxBinding");
                BindingStylableGroupId = (int[])SafeGetFieldValue(mvxBinding, "AllIds", new int[0]);
                BindingBindId = (int)SafeGetFieldValue(mvxBinding, "MvxBind");
                BindingLangId = (int)SafeGetFieldValue(mvxBinding, "MvxLang");

                var mvxImageView = styleable.GetNestedType("MvxImageView");
                ImageViewStylableGroupId = (int[])SafeGetFieldValue(mvxImageView, "AllIds", new int[0]);
                SourceBindId = (int)SafeGetFieldValue(mvxImageView, "MvxSource");

                var mvxListView = styleable.GetNestedType("MvxListView");
                ListViewStylableGroupId = (int[])SafeGetFieldValue(mvxListView, "AllIds");
                ListItemTemplateId =         (int)mvxListView
                                                     .GetField("MvxItemTemplate")
                                                     .GetValue(null);
                DropDownListItemTemplateId = (int)mvxListView
                                                     .GetField("MvxDropDownItemTemplate")
                                                     .GetValue(null);

                var mvxExpandableListView = styleable.GetNestedType("MvxExpandableListView");
                ExpandableListViewStylableGroupId = (int[])SafeGetFieldValue(mvxExpandableListView, "AllIds", new int[0]);
                GroupItemTemplateId       = (int)mvxExpandableListView
                                                     .GetField("GroupItemTemplate")
                                                     .GetValue(null);
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap(
                    "Error finding resource ids for MvxBinding - please make sure ResourcesToCopy are linked into the executable");
            }
        }

        private static object SafeGetFieldValue(Type styleable, string fieldName)
        {
            return SafeGetFieldValue(styleable, fieldName, 0);
        }

        private static object SafeGetFieldValue(Type styleable, string fieldName, object defaultValue)
        {
            var field = styleable.GetField(fieldName);
            if (field == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Missing stylable field {0}", fieldName);
                return defaultValue;
            }

            return field.GetValue(null);
        }

        public int BindingTagUnique { get; private set; }

        public int[] BindingStylableGroupId { get; private set; }
        public int BindingBindId { get; private set; }
        public int BindingLangId { get; private set; }

        public int[] ControlStylableGroupId { get; private set; }
        public int TemplateId { get; private set; }

        public int[] ImageViewStylableGroupId { get; private set; }
        public int SourceBindId { get; private set; }

        public int[] ListViewStylableGroupId { get; private set; }
        public int ListItemTemplateId { get; private set; }
        public int DropDownListItemTemplateId { get; private set; }

        public int[] ExpandableListViewStylableGroupId { get; private set; }
        public int GroupItemTemplateId { get; private set; }

    }
}
