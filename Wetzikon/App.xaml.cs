using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Wetzikon {
  /// Provides application-specific behavior to supplement the default
  /// Application class.
  sealed partial class App : Application {
    /// Initializes the singleton application object.  This is the first line
    /// of authored code executed, and as such is the logical equivalent of
    /// main() or WinMain().
    public App() {
      this.InitializeComponent();
      this.Suspending += OnSuspending;

#if DEBUG
      SetupLogger();
#endif
    }

    private void SetupLogger(bool internalLoggingEnabled = false) {
      if (internalLoggingEnabled) {
        // C:\Users\<username>\AppData\Local\Packages\<app>\TempState
        StorageFolder tempDir = ApplicationData.Current.TemporaryFolder;
        NLog.Common.InternalLogger.LogToConsole = true;
        NLog.Common.InternalLogger.LogFile = Path.Combine(
          tempDir.Path, "internal.log");
        NLog.Common.InternalLogger.LogLevel = NLog.LogLevel.Trace;
      }
      // NLog
      // C:\Users\<username>\AppData\Local\Packages\<app>\LocalCache
      StorageFolder cacheDir = ApplicationData.Current.LocalCacheFolder;
      var config = new NLog.Config.LoggingConfiguration();
      var target = new NLog.Targets.FileTarget("file") {
        FileName = Path.Combine(cacheDir.Path, "debug.log"),
        Header = "",
        Layout = "" + 
          "${date:universalTime=False:format=dd-MM-yyyy HH\\:mm\\:ss.fff} " +
          "${pad:padding=5:alignmentOnTruncation=left:inner=" +
            "${level:uppercase=True}" +
           "} " +
          "${message}${exception:format=ToString}",
        LineEnding = NLog.Targets.LineEndingMode.LF
      };
      config.AddTarget(target);
      config.AddRuleForAllLevels(target);
      NLog.LogManager.Configuration = config;

      var logger = NLog.LogManager.GetCurrentClassLogger();
      logger.Info("");
    }

    /// Invoked when the application is launched normally by the end user.
    /// Other entry points will be used such as when the application is
    /// launched to open a specific file.
    protected override void OnLaunched(LaunchActivatedEventArgs e) {
    Frame rootFrame = Window.Current.Content as Frame;

    // Do not repeat app initialization when the Window already has content,
    // just ensure that the window is active
    if (rootFrame == null) {
      // Create a Frame to act as the navigation context and navigate to the
      // first page
      rootFrame = new Frame();

      rootFrame.NavigationFailed += OnNavigationFailed;

      if (e.PreviousExecutionState == ApplicationExecutionState.Terminated) {
          //TODO: Load state from previously suspended application
      }

      // Place the frame in the current Window
      Window.Current.Content = rootFrame;
    }

    if (e.PrelaunchActivated == false) {
      if (rootFrame.Content == null) {
        // When the navigation stack isn't restored navigate to the first page,
        // configuring the new page by passing required information as a
        // navigation parameter
        rootFrame.Navigate(typeof(MainPage), e.Arguments);
      }
      // Ensure the current window is active
      Window.Current.Activate();
    }

    // TitleBar
    var viewTitleBar = ApplicationView.GetForCurrentView().TitleBar;
    viewTitleBar.ButtonBackgroundColor = Colors.Transparent;
    viewTitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
    viewTitleBar.ButtonForegroundColor =
      (Color)Resources["SystemBaseHighColor"];
    }

    /// Invoked when Navigation to a certain page fails
    void OnNavigationFailed(object sender, NavigationFailedEventArgs e) {
      throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
    }

    /// Invoked when application execution is being suspended.  Application
    /// state is saved without knowing whether the application will be
    /// terminated or resumed with the contents of memory still intact.
    private void OnSuspending(object sender, SuspendingEventArgs e) {
      var deferral = e.SuspendingOperation.GetDeferral();
      //TODO: Save application state and stop any background activity
      deferral.Complete();
    }
  }

  public static class Log {
    // TODO: Info, Warn, Error, Fatal
    public static void Trace(string text) {
#if DEBUG
      var logger = NLog.LogManager.GetLogger("App");
      var trace = new System.Diagnostics.StackTrace();
      logger.Trace(FormatMessage(trace, text, new object[]{}));
#endif
    }

    public static void Trace(string text, params object[] args) {
#if DEBUG
      var logger = NLog.LogManager.GetLogger("App");
      var trace = new System.Diagnostics.StackTrace();
      logger.Trace(FormatMessage(trace, text, args));
#endif
    }

    public static void Debug(string text) {
#if DEBUG
      var logger = NLog.LogManager.GetLogger("App");
      var trace = new System.Diagnostics.StackTrace();
      logger.Debug(FormatMessage(trace, text, new object[]{}));
#endif
    }

    public static void Debug(string text, params object[] args) {
#if DEBUG
      var logger = NLog.LogManager.GetLogger("App");
      var trace = new System.Diagnostics.StackTrace();
      logger.Debug(FormatMessage(trace, text, args));
#endif
    }

    private static string FormatMessage(
      System.Diagnostics.StackTrace trace, string text, params object[] args) {
      System.Diagnostics.StackFrame frame = trace.GetFrame(1);
      string className = frame.GetMethod().ReflectedType.FullName;
      string methodName = frame.GetMethod().Name;
      string message = String.Format(
        "{0}/{1}: {2}", className, methodName, text);
      return String.Format(message, args);
    }
  }
}
