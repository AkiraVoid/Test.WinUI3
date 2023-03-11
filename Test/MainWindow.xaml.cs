// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Playback;
using Windows.Media.SpeechSynthesis;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Test
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private async Task SpeakAsync(string language, string text)
        {
            var player = new MediaPlayer();
            var synth = new SpeechSynthesizer();
            synth.Voice = SpeechSynthesizer.AllVoices.FirstOrDefault(v => v.Language == language) ??
                SpeechSynthesizer.DefaultVoice;
            var source = await synth.SynthesizeTextToStreamAsync(text);
            synth.Dispose();
            player.MediaEnded += (sender, _) =>
            {
                sender.Dispose();
                source.Dispose();
            };
            player.SetStreamSource(source);
            player.Play();
        }


        private void OnClick(object _sender, RoutedEventArgs _e)
        {
            SpeakAsync("en-US", "Test 1").Wait();
            SpeakAsync("en-US", "Test 2").Wait();
        }
    }
}