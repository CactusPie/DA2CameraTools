using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DragonAge2CameraTools.ViewLogic.ViewModels;

namespace DragonAge2CameraTools.Wpf.Controls
{
    public partial class KeyBindingButtons : UserControl
    {
        public static readonly DependencyProperty KeyBindingsProperty = 
            DependencyProperty.Register(nameof(KeyBindings), typeof(IEnumerable<KeyBindingViewmodel>), typeof(KeyBindingButtons));
        
        public static readonly DependencyProperty BindKeysCommandProperty = 
            DependencyProperty.Register(nameof(BindKeysCommand), typeof(ICommand), typeof(KeyBindingButtons));
        
        public IEnumerable<KeyBindingViewmodel> KeyBindings
        {
            get => GetValue(KeyBindingsProperty) as IEnumerable<KeyBindingViewmodel>;
            set => SetValue(KeyBindingsProperty, value);
        }
        
        public ICommand BindKeysCommand
        {
            get => GetValue(BindKeysCommandProperty) as ICommand;
            set => SetValue(BindKeysCommandProperty, value);
        }

        public KeyBindingButtons()
        {
            InitializeComponent();
        }
    }
}