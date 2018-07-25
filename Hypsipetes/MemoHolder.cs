using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using MarkdownSharp;

namespace Hypsipetes
{
    public class MemoHolder : INotifyPropertyChanged
    {
        private string _text;

        public string Text 
        {
            set 
            {
                if (_text != value)
                {
                    this._text = value;
                    NotifyPropertyChanged("Text");
                }
            }
            get { return this._text; }
        }
        public string SavePath { set; get; }
        public string SaveName { set; get; }
        public string SaveHtmlName { set; get; }

        public MemoHolder()
        {
            SavePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Hypsipetes");
            SaveName = "memo.txt";
            SaveHtmlName = "memo.html";
            Text = string.Empty;
        }

        public void Load()
        {
            if (!File.Exists(Path.Combine(SavePath, SaveName)))
                return;
            
            using(var sr = new StreamReader(Path.Combine(SavePath, SaveName)))
            {
                Console.WriteLine("read");
                Text = sr.ReadToEnd();
            }
        }

        public void Save()
        {
            GenerateSaveDirecotryIfAbsent();
            
            using(var sw = new StreamWriter(Path.Combine(SavePath, SaveName), false))
            {
                sw.Write(Text);
            }
        }

        public void GenerateHtml()
        {
            var processor = new Markdown();
            var markdown = processor.Transform(Text);
            var content = Properties.Resources.HttpHeader + markdown + Properties.Resources.HttpFooter;

            GenerateSaveDirecotryIfAbsent();
            SaveCSSIfAbsent();

            using (var sw = new StreamWriter(Path.Combine(SavePath, SaveHtmlName), false))
            {
                sw.Write(content);
            }
        }

        private void GenerateSaveDirecotryIfAbsent()
        {
            if (!Directory.Exists(SavePath))
                Directory.CreateDirectory(SavePath);
        }

        private void SaveCSSIfAbsent()
        {
            GenerateSaveDirecotryIfAbsent();

            var css = "github-markdown.css";
            if(!File.Exists(Path.Combine(SavePath, css)))
            {
                using (var sw = new StreamWriter(Path.Combine(SavePath, css), false))
                {
                    sw.Write(Properties.Resources.GithubMarkdownCSS);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
    }
}
