using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reflection;
using System.Text;

namespace JFront.ViewModels
{
    class SettingsViewModel : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ReactiveProperty<string> SignInUrl { get; } = new ReactiveProperty<string>(Properties.Settings.Default.SIGN_IN_URL);
        public ReactiveProperty<string> OrganizationCode { get; } = new ReactiveProperty<string>(Properties.Settings.Default.ORGANIZATION_CODE);
        public ReactiveProperty<string> UserCode { get; } = new ReactiveProperty<string>(Properties.Settings.Default.USER_CODE);
        public ReactiveProperty<string> OrganizationPass { get; } = new ReactiveProperty<string>(Properties.Settings.Default.ORGANIZATION_PASS);
        public ReactiveProperty<string> UserPass { get; } = new ReactiveProperty<string>(Properties.Settings.Default.USER_PASS);
        public ReactiveProperty<bool> CloseWindow { get; } = new ReactiveProperty<bool>(false);

        public ReactiveCommand Save { get; } = new ReactiveCommand();
        public ReactiveCommand Cancel { get; } = new ReactiveCommand();

        public SettingsViewModel()
        {
            Save.Subscribe(_ =>
            {
                Properties.Settings.Default.SIGN_IN_URL = SignInUrl.Value;
                Properties.Settings.Default.ORGANIZATION_CODE = OrganizationCode.Value;
                Properties.Settings.Default.USER_CODE = UserCode.Value;
                Properties.Settings.Default.ORGANIZATION_PASS = OrganizationPass.Value;
                Properties.Settings.Default.USER_PASS = UserPass.Value;
                Properties.Settings.Default.Save();
                Close();
            });

            Cancel.Subscribe(_ =>
            {
                Close();
            });
        }

        public void Dispose()
        {
            // ReactiveProperty、ReactiveCommandをDispose
            foreach (PropertyInfo propertyInfo in this.GetType().GetProperties())
            {
                var instance = propertyInfo.GetGetMethod().Invoke(this, null);
                var dispose = instance.GetType().GetMethod("Dispose");
                dispose?.Invoke(instance, null);
            }
        }

        private void Close()
        {
            CloseWindow.Value = true;
        }
    }
}
