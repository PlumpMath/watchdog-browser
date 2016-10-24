using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WatchdogBrowser.Config;
using WatchdogBrowser.Models;

namespace WatchdogBrowser.ViewModel {
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase {

        SitesConfig config = new SitesConfig();

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel() {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}            
            config.Ready += Config_Ready;
            config.Initialize();
            //CreateMockupTabs();
        }

        private void Config_Ready(object sender, CustomEventArgs.ConfigReadyEventArgs e) {
            var sitesList = e.Sites;

            var prepTabs = new List<TabItemModel>();
            foreach(var site in sitesList) {
                var prepTab = new TabItemModel();
                prepTab.Title = site.Name;
                prepTab.Url = site.Mirrors[0];
                prepTab.Closeable = false;
                prepTab.Close += TabClosed;
                Tabs.Add(prepTab);
            }
        }

        private void TabClosed(object sender, System.EventArgs e) {
            if (Tabs.Count == 1) {
                App.Current.Shutdown();
            } else {
                Tabs.Remove((TabItemModel)sender);
                RaisePropertyChanged(nameof(Tabs));

                Debug.WriteLine(Tabs.Count);
            }
        }

        private ObservableCollection<TabItemModel> tabs = new ObservableCollection<TabItemModel>();

        public ObservableCollection<TabItemModel> Tabs {
            get {
                return tabs;
            }
            set {
                tabs = value;
                RaisePropertyChanged(nameof(Tabs));
            }
        }

        private TabItemModel selectedTab;
        public TabItemModel SelectedTab {
            get {
                return selectedTab;
            }
            set {
                selectedTab = value;
                Debug.WriteLine(selectedTab?.Title);
                RaisePropertyChanged(nameof(SelectedTab));
            }
        }


        private void CreateMockupTabs() {
            Tabs.Add(new TabItemModel { Title = "������� ����", Url = "https://github.com/", Closeable = true });
            //Tabs.Add(new TabItemModel { Title = "��������", Url = "http://yandex.ru/", Closeable = true });
            //Tabs.Add(new TabItemModel { Title = "�����", Url = "http://bash.im/", Closeable = true });
            //Tabs.Add(new TabItemModel { Title = "������ ���", Url = "http://9gag.com/", Closeable = true });
            foreach (var tab in Tabs) {
                tab.Close += (sender, args) => {
                    if (Tabs.Count == 1) {
                        App.Current.Shutdown();
                    } else {
                        Tabs.Remove((TabItemModel)sender);
                        RaisePropertyChanged(nameof(Tabs));

                        Debug.WriteLine(Tabs.Count);
                    }
                };
            }
            Debug.WriteLine(Tabs.Count);
            RaisePropertyChanged(nameof(Tabs));
        }
    }
}