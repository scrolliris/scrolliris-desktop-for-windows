using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Wetzikon {
  public sealed partial class MainPage : Page {
    private readonly IList<(string Tag, Type Page)> _pages = new List<(
      string Tag, Type Page)> {
        ("registry", typeof(Registry)),
        ("desktop", typeof(Desktop)),
        ("preferences", typeof(Preferences)),
        ("publications", typeof(Publications)),
    };

    private bool IsPaneOpen {
        get { return !(bool)MenuButton.IsChecked; }
    }

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

    #region Event Handlers
    private void NavView_Loaded(object sender, RoutedEventArgs e) {
      ContentFrame.Navigated += On_Navigated;

      ContentFrame.Navigate(
        typeof(Registry), null, new SuppressNavigationTransitionInfo());
    }

    private void NavView_PaneClosing(
      object sender, NavigationViewPaneClosingEventArgs e) {
      // cancel close
      if (IsPaneOpen) {
        KeepPaneOpen();
        e.Cancel = true;
      }
    }

    private void NavView_DisplayModeChanged(
      object sender, NavigationViewDisplayModeChangedEventArgs e) {
      if (IsPaneOpen) {
        KeepPaneOpen();
      }
    }

    private void NavView_SelectionChanged(
      object sender, NavigationViewSelectionChangedEventArgs e) {
      // pass
    }

    private void NavView_ItemInvoked(
      NavigationView sender, NavigationViewItemInvokedEventArgs e) {

      if (e.IsSettingsInvoked) {
        ContentFrame.Navigate(
          typeof(Settings), null, new DrillInNavigationTransitionInfo());
        return;
      }

      var tag = NavView.MenuItems.OfType<NavigationViewItem>()
        .First(i => e.InvokedItem.Equals(i.Content))
        .Tag.ToString();
      NavView_Navigate(tag);
    }

    private void NavView_Navigate(string navItemTag) {
      var item = _pages.First(p => p.Tag.Equals(navItemTag));
      ContentFrame.Navigate(item.Page);
    }

    private void NavView_BackRequested(
      NavigationView sender, NavigationViewBackRequestedEventArgs e) {
      // pass
    }

    private void MenuButton_Checked(object sender, RoutedEventArgs e) {
      this.NavView.IsPaneOpen = !NavView.IsPaneOpen;
    }

    private void BackInvoked(
      KeyboardAccelerator sender,
      KeyboardAcceleratorInvokedEventArgs e) {
      On_BackRequested();
      e.Handled = true;
    }
    #endregion

    #region Additional Event Handlers
    private bool On_BackRequested() {
      // Do Nothing
      return true;
    }

    private void On_Navigated(object sender, NavigationEventArgs e) {
      if (ContentFrame.SourcePageType == typeof(Settings)) {
        NavView.SelectedItem = (
          NavigationViewItem)NavView.SettingsItem;
      } else {
        var item = _pages.First(p => p.Page == e.SourcePageType);
        NavView.SelectedItem = NavView.MenuItems
          .OfType<NavigationViewItem>()
          .First(i => i.Tag.Equals(item.Tag));
      }
    }
    #endregion

    private void KeepPaneOpen() {
      NavView.SetValue(NavigationView.DisplayModeProperty,
        NavigationViewDisplayMode.Expanded);
    }

    private void UpdateAppTitleBar(
      object sender, WindowSizeChangedEventArgs e) {

      var isFullScreen = (
        ApplicationView.GetForCurrentView().IsFullScreenMode);
      var leftMargin = 49 + (
        isFullScreen ? 0 : CoreApplication.GetCurrentView()
          .TitleBar.SystemOverlayLeftInset);
      this.AppTitleBar.Margin = new Thickness(leftMargin, 0, 0, 0);
    }
  }
}
