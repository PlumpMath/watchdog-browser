﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WatchdogBrowser {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            //CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, CloseTab));
        }

        //private void CloseTab(object sender, ExecutedRoutedEventArgs e) {
        //    if(uxTabs.Cou)
        //    throw new NotImplementedException();
        //}
    }
}
