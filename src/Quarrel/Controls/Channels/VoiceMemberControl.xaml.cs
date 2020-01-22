﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DiscordAPI.Models;
using Quarrel.ViewModels.Models.Bindables;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Quarrel.Controls.Channels
{
    public sealed partial class VoiceMemberControl : UserControl
    {
        public BindableVoiceUser ViewModel => DataContext as BindableVoiceUser;
        public VoiceMemberControl()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) =>
            {
                this.Bindings.Update();
            };
        }
    }
}
