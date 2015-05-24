// MvxAndroidSetupSingleton.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Droid.Platform
{
    public class MvxAndroidSetupSingleton
        : MvxSingleton<MvxAndroidSetupSingleton>
    {
        private static readonly object LockObject = new object();
        private MvxAndroidSetup _setup;
        private bool _initialized;
        private bool _initializationStarted;
        private IMvxAndroidSplashScreenActivity _currentSplashScreen;

        public virtual void EnsureInitialized()
        {
            lock (LockObject)
            {
                if (_initialized)
                    return;

                if (_initializationStarted)
                {
                    Mvx.Warning("Multiple Initialize calls made for MvxAndroidSetupSingleton singleton");
                    throw new MvxException("Multiple initialize calls made");
                }

                _initializationStarted = true;
            }

            _setup.Initialize();

            lock (LockObject)
            {
                _initialized = true;
                if (_currentSplashScreen != null)
                {
                    Mvx.Warning("Current splash screen not null during direct initialization - not sure this should ever happen!");
                    _currentSplashScreen.InitializationComplete();
                }
            }
        }

        public virtual void RemoveSplashScreen(IMvxAndroidSplashScreenActivity splashScreen)
        {
            lock (LockObject)
            {
                _currentSplashScreen = null;
            }
        }
        public virtual void InitializeFromSplashScreen(IMvxAndroidSplashScreenActivity splashScreen)
        {
            lock (LockObject)
            {
                _currentSplashScreen = splashScreen;

                if (_initializationStarted)
                {
                    if (_initialized)
                    {
                        _currentSplashScreen.InitializationComplete();
                        return;
                    }

                    return;
                }

                _initializationStarted = true;
            }

            _setup.InitializePrimary();

#if !DOT42
            WaitCallback action = ignored =>
#else
            Action action = () =>
#endif
            {
                _setup.InitializeSecondary();
                lock (LockObject)
                {
                    _initialized = true;
                    if (_currentSplashScreen != null)
                        _currentSplashScreen.InitializationComplete();
                }
            };
#if !DOT42
            ThreadPool.QueueUserWorkItem(action);
#else
            RunInBackground(action);
#endif
        }

#if DOT42
        private static async void RunInBackground(Action action)
        {
            try
            {
                await Task.Run(action);
            }
            catch(Exception ex)
            {
                // TODO: find out why exceptions don't propagate to the main thread.
                MvxTrace.Error("{0}: {1}\n{2}", ex.GetType().Name, ex.Message, ex.StackTrace);
                throw;
            }
            
        }
#endif

        public static MvxAndroidSetupSingleton EnsureSingletonAvailable(Context applicationContext)
        {
            if (Instance != null)
                return Instance;

            lock (LockObject)
            {
                if (Instance != null)
                    return Instance;

                var instance = new MvxAndroidSetupSingleton();
                instance.CreateSetup(applicationContext);
                return Instance;
            }
        }

        private MvxAndroidSetupSingleton()
        {
        }

        protected virtual void CreateSetup(Context applicationContext)
        {
            var setupType = FindSetupType();
            if (setupType == null)
            {
                throw new MvxException("Could not find a Setup class for application");
            }

            try
            {
                _setup = (MvxAndroidSetup)Activator.CreateInstance(setupType, applicationContext);
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap("Failed to create instance of {0}", setupType.FullName);
            }
        }

        protected virtual Type FindSetupType()
        {
#if !DOT42
            var query = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                        from type in assembly.ExceptionSafeGetTypes()
                        where type.Name == "Setup"
                        where typeof (MvxAndroidSetup).IsAssignableFrom(type)
                        select type;
            return query.FirstOrDefault();
#else
            foreach (var type in Assembly.GetEntryAssembly().ExceptionSafeGetTypes())
            {
                //Console.WriteLine(type.FullName);
                if (type.Name == "Setup" && typeof (MvxAndroidSetup).IsAssignableFrom(type))
                    return type;
            }
            return null;
#endif
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                lock (LockObject)
                {
                    _currentSplashScreen = null;
                }
            }
            base.Dispose(isDisposing);
        }
    }
}