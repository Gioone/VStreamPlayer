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

                var resources = Application.Current.Resources.MergedDictionaries;

                foreach (var v in resources)
                {
                    if (v.Source.ToString().StartsWith("languages", StringComparison.OrdinalIgnoreCase))
                    {
                        var strLanguage = v.Source.ToString().Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries)[1]
                            .Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries)[0];
                        foreach (var item in CboLanguage.Items)
                        {
                            if (item.ToString() == strLanguage)
                            {
                                CboLanguage.SelectedItem = item;
                            }
                        }
                    }
                }

                /*
                string strLanguagesPath = Path.Combine(System.Windows.Forms.Application.StartupPath, "Languages");
                string[] strLanguages = Directory.GetFiles(strLanguagesPath, "*.*", SearchOption.TopDirectoryOnly);

                for (int i = 0; i < strLanguages.Length; i++)
                {
                    string strLanguagePath = strLanguages[i];

                    string[] strSplits = strLanguagePath.Split('/');

                    strLanguages[i] = strSplits[strSplits.Length - 1];
                }
                */


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
    }
}
