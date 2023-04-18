using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
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
using Path = System.IO.Path;

namespace VStreamPlayer.MVVM.Views
{
    /// <summary>
    /// SettingsView.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsView : System.Windows.Window
    {
        public SettingsView()
        {
            InitializeComponent();
            LoadLanguages();
            LoadThemes();
        }

        private void LoadLanguages()
        {
            try
            {
                Assembly assembly = Assembly.GetAssembly(GetType());

                string resourcesName = assembly.GetName().Name + ".g";
                ResourceManager rm = new ResourceManager(resourcesName, assembly);

                using (ResourceSet set = rm.GetResourceSet(CultureInfo.CurrentCulture, true, true))
                {
                    foreach (DictionaryEntry item in set)
                    {
                        string strLanguage = item.Key.ToString();
                        
                        if (strLanguage.StartsWith("languages", StringComparison.OrdinalIgnoreCase))
                        {
                            // It's like en-us.baml
                            string[] splits = strLanguage.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);

                            // We get like en-us
                            string strResLanguage = splits[1].Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries)[0];

                            CboLanguage.Items.Add(strResLanguage);
                        }
                    }
                }

                // Set the current selected language
                var resources = Application.Current.Resources.MergedDictionaries;

                foreach (var v in resources)
                {
                    if (v.Source.ToString().StartsWith("languages", StringComparison.OrdinalIgnoreCase))
                    {
                        var strLanguage = v.Source.ToString().Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries)[1]
                            .Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries)[0];
                        foreach (var item in CboLanguage.Items)
                        {
                            if (string.Equals(item.ToString(), strLanguage, StringComparison.OrdinalIgnoreCase))
                            {
                                CboLanguage.SelectedItem = item;
                            }
                        }
                    }
                }


            }
            catch
            {

            }
        }

        private void LoadThemes()
        {
            try
            {
                Assembly assembly = Assembly.GetAssembly(GetType());

                string resourcesName = assembly.GetName().Name + ".g";
                ResourceManager rm = new ResourceManager(resourcesName, assembly);

                using (ResourceSet set = rm.GetResourceSet(CultureInfo.CurrentCulture, true, true))
                {
                    foreach (DictionaryEntry item in set)
                    {
                        string strTheme = item.Key.ToString();
                        
                        if (strTheme.StartsWith("theme", StringComparison.OrdinalIgnoreCase))
                        {
                            // It's like default.baml
                            string[] splits = strTheme.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);

                            // We get like default
                            string strDefault = splits[1].Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries)[0];

                            CboTheme.Items.Add(strDefault);
                        }
                    }
                }

                // Set the current selected theme
                var resources = Application.Current.Resources.MergedDictionaries;

                foreach (var v in resources)
                {
                    if (v.Source.ToString().StartsWith("theme", StringComparison.OrdinalIgnoreCase))
                    {
                        // We got like the "default" string.
                        var strTheme = v.Source.ToString().Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries)[1]
                            .Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries)[0];
                        foreach (var item in CboTheme.Items)
                        {
                            if (string.Equals(item.ToString(), strTheme, StringComparison.OrdinalIgnoreCase))
                            {
                                CboTheme.SelectedItem = item;
                            }
                        }
                    }
                }


            }
            catch
            {

            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CboLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var resources = Application.Current.Resources.MergedDictionaries;

            foreach (var v in resources)
            {
                // Find the language resource.
                if (v.Source.ToString().StartsWith("languages", StringComparison.OrdinalIgnoreCase))
                {
                    string strSelectedLanguage = CboLanguage.SelectedItem.ToString();

                    v.Source = new Uri($"Languages/{strSelectedLanguage}.xaml", UriKind.Relative);

                }
            }
        }

        private void CboTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // TODO: 这里有 BUG
            /*
            var resources = Application.Current.Resources.MergedDictionaries;

            foreach (var v in resources)
            {
                // Find the theme resources.
                if (v.Source.ToString().StartsWith("theme", StringComparison.OrdinalIgnoreCase))
                {
                    string strSelectedTheme = CboLanguage.SelectedItem.ToString();

                    v.Source = new Uri($"Theme/{strSelectedTheme}.xaml", UriKind.Relative);

                }
            }
            */
        }
    }
}
