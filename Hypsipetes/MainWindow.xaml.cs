using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MarkdownSharp;
using System.IO;

namespace Hypsipetes
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private TextModifier _modifier = new TextModifier();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _modifier;
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void changeTitle_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new TitleInputDialogBox();
            dialog.Owner = this;
            dialog.NewWindowTitle = this.Title;
            dialog.ShowDialog();

            var result = dialog.DialogResult ?? false;

            if(result)
            {
                this.Title = dialog.NewWindowTitle;
                WriteLog("Changed title -> " + dialog.NewWindowTitle);
            }
        }

        private void about_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new About();
            dialog.Owner = this;
            dialog.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WriteLog("Start application");

            try
            {
                _modifier.Holder.Load();
        }
            catch(IOException)
            {
                WriteLog("Failed to load memo content");
            }
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                _modifier.Holder.Save();
            }
            catch (IOException)
            {
                WriteLog("Failed to save memo content");
            }
        }

        private void changeCheckbox_Click(object sender, RoutedEventArgs e)
        {
            var m = sender as MenuItem;
            if (!m.IsChecked)
            {
                displayFront.Content = "常に手前に表示";
                WriteLog("Changed display -> 常に手前に表示");
            }
            else
            {
                displayFront.Content = "常に前向きに生きる";
                WriteLog("Changed display -> 常に前向きに生きる");
            }
        }

        private void generateCyclic_Click(object sender, RoutedEventArgs e)
        {
            if (!IsValid(cyclicGenerator))
                return;

            try
            {
                var result = _modifier.Cyclic.Modify();
                Clipboard.SetText(result);
            }
            catch(FormatException)
            {
                WriteLog("Failed generate -> CyclicModifier");
                MessageBox.Show("フォーマットが不正です");
            }
            catch(ArgumentNullException)
            {
                WriteLog("Failed generate -> CyclicModifier");
                MessageBox.Show("フォーマットが不正です");
            }
        }

        private void generateFix_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = _modifier.Fix.Modify();
                Clipboard.SetText(result);
            }
            catch(NullReferenceException)
            {
                WriteLog("Failed generate -> FixModifier");
                MessageBox.Show("フォーマットが不正です");
            }
            catch(ArgumentNullException)
            {
                WriteLog("Failed generate -> FixModifier");
                MessageBox.Show("フォーマットが不正です");
            }
        }

        private void generateFromMarkdown_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _modifier.Holder.GenerateHtml();
                _modifier.Holder.Save();
                WriteLog("Generate HTML content");
            }
            catch(IOException)
            {
                WriteLog("Failed to save HTML content");
            }
        }

        private void openHtml_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(
                System.IO.Path.Combine(
                _modifier.Holder.SavePath, _modifier.Holder.SaveHtmlName));
        }

        #region LogWriter

        private void WriteLog(string msg)
        {
            log.Text = log.Text + DateTime.Now.ToString("yyyy/MM/dd - HH:mm") + " " + msg + Environment.NewLine;
        }

        #endregion

        #region Validator

        bool IsValid(DependencyObject node)
        {
            // Check if dependency object was passed
            if (node != null)
            {
                // Check if dependency object is valid.
                // NOTE: Validation.GetHasError works for controls that have validation rules attached 
                bool isValid = !Validation.GetHasError(node);
                if (!isValid)
                {
                    // If the dependency object is invalid, and it can receive the focus,
                    // set the focus
                    if (node is IInputElement) Keyboard.Focus((IInputElement)node);
                    return false;
                }
            }

            // If this dependency object is valid, check all child dependency objects
            foreach (object subnode in LogicalTreeHelper.GetChildren(node))
            {
                if (subnode is DependencyObject)
                {
                    // If a child dependency object is invalid, return false immediately,
                    // otherwise keep checking
                    if (IsValid((DependencyObject)subnode) == false) return false;
                }
            }

            // All dependency objects are valid
            return true;
        }

#endregion

    }
}
