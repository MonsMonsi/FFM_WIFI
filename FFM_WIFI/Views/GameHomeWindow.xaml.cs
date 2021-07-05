﻿using FFM_WIFI.Models.DataContext;
using FFM_WIFI.ViewModels;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace FFM_WIFI.Views
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameHomeWindow : Window
    {
        private GameHomeViewModel ghvm;
        public GameHomeWindow(User user1, User user2, Player[] teamUser1, Player[] teamUser2, Season season, League league)
        {
            InitializeComponent();
            ghvm = new GameHomeViewModel(this, user1, user2, teamUser1, teamUser2, season, league);
            this.DataContext = ghvm;
        }
    }
}
