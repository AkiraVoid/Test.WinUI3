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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Test
{
    public sealed partial class PersonItem : UserControl
    {
        public PersonItem()
        {
            this.InitializeComponent();
        }


        public Person Person
        {
            get => (Person)GetValue(PersonProperty);
            set => SetValue(PersonProperty, value);
        }

        // Using a DependencyProperty as the backing store for Person.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PersonProperty = DependencyProperty.Register(
            nameof(Person),
            typeof(Person),
            typeof(PersonItem),
            new(null));
    }
}