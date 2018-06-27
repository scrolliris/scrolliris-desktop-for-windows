using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Wetzikon {
    /// <summary>
    /// </summary>
    public sealed partial class MainPage : Page {
        public MainPage() {
            this.InitializeComponent();

            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            coreTitleBar.LayoutMetricsChanged +=
                (s, e) => UpdateAppTitleBar(s, null);

            Window.Current.CoreWindow.SizeChanged +=
                (s, e) => UpdateAppTitleBar(s, e);
            Window.Current.SetTitleBar(AppTitleBar);
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e) {
        }

        private void NavView_ItemInvoked(
            NavigationView sender, NavigationViewItemInvokedEventArgs e) {
        }

        private void NavView_Navigate(NavigationViewItem item) {
        }

        private void NavView_BackRequested(
            NavigationView sender, NavigationViewBackRequestedEventArgs e) {
        }

        private void MenuButton_Checked(object sender, RoutedEventArgs e) {
            this.NavView.IsPaneOpen = !NavView.IsPaneOpen;
        }

        private void BackInvoked(
            KeyboardAccelerator sender,
            KeyboardAcceleratorInvokedEventArgs e) {
            On_BackRequested();
        }

        private bool On_BackRequested() {
            return true;
        }

        private bool On_Navigated(object sender, NavigationEventArgs e) {
            return true;
        }

        private void UpdateAppTitleBar(
            object sender, WindowSizeChangedEventArgs e) {
            this.NavView.IsPaneOpen = (bool)MenuButton.IsChecked;

            var isFullScreen = (
                ApplicationView.GetForCurrentView().IsFullScreenMode);
            var leftMargin = 49 + (
                isFullScreen ? 0 : CoreApplication.GetCurrentView()
                    .TitleBar.SystemOverlayLeftInset);
            this.AppTitleBar.Margin = new Thickness(leftMargin, 0, 0, 0);
        }
    }
}
