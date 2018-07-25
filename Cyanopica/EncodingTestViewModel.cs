using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;

namespace Cyanopica
{
    public class EncodingTestViewModel : INotifyPropertyChanged
    {
        public string SourceText {set; get;}

        private string _destText;
        public string DestText
        {
            set
            {
                if(value != _destText)
                {
                    _destText = value;
                    NotifyPropertyChanged("DestText");
                }
            }

            get { return _destText; }
        }

        private string _hexDump;
        public string HexDump
        {
            set
            {
                if (value != _hexDump)
                {
                    _hexDump = value;
                    NotifyPropertyChanged("HexDump");
                }
            }

            get { return _hexDump; }
        }

        private int _encodingCodePage;
        public int EncodingCodePage
        {
            set
            {
                if (value != _encodingCodePage)
                {
                    _encodingCodePage = value;
                    NotifyPropertyChanged("EncodingScheme");
                }
            }

            get { return _encodingCodePage; }
        }

        private bool _isEncodingBestFitFallback;
        public bool IsEncodingBestFitFallback
        {
            set
            {
                if (value != _isEncodingBestFitFallback)
                {
                    _isEncodingBestFitFallback = value;
                    NotifyPropertyChanged("IsEncodingBestFitFallback");
                }
            }

            get { return _isEncodingBestFitFallback; }
        }

        public string EncoderReplacementText { set; get; }

        private int _decodingCodePage;
        public int DecodingCodePage
        {
            set
            {
                if (value != _decodingCodePage)
                {
                    _decodingCodePage = value;
                    NotifyPropertyChanged("DecodingScheme");
                }
            }

            get { return _decodingCodePage; }
        }

        private bool _isDecodingBestFitFallback;
        public bool IsDecodingBestFitFallback
        {
            set
            {
                if (value != _isDecodingBestFitFallback)
                {
                    _isDecodingBestFitFallback = value;
                    NotifyPropertyChanged("IsDecodingBestFitFallback");
                }
            }

            get { return _isDecodingBestFitFallback; }
        }

        public string DecoderReplacementText { set; get; }

        private IEnumerable<EncodingInfo> _usableEncoding;
        public IEnumerable<EncodingInfo> UsableEncoding
        {
            private set
            {
                if (!Object.Equals(value, _usableEncoding))
                {
                    _usableEncoding = value;
                    NotifyPropertyChanged("UsableEncoding");
                }
            }

            get { return _usableEncoding; }
        }

        public ICommand Conversion { set; get; }

        public EncodingTestViewModel()
        {
            _usableEncoding = Encoding.GetEncodings();
            Conversion = new ConversionCommand(this);
            IsEncodingBestFitFallback = true;
            IsDecodingBestFitFallback = true;
        }

        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion
    }

    public class ConversionCommand : ICommand
    {
        private EncodingTestViewModel _view;

        public ConversionCommand(EncodingTestViewModel view)
        {
            _view = view;
            _view.PropertyChanged += (sender, e) => NotifyCanExecuteChanged();
        }

        public bool CanExecute(object parameter)
        {
            return _view.EncodingCodePage != 0 && _view.DecodingCodePage != 0;
        }

        public void Execute(object parameter)
        {
            var encoder = EncodingTester.CreateEncoding(_view.EncodingCodePage, _view.IsEncodingBestFitFallback, _view.EncoderReplacementText);
            var decoder = EncodingTester.CreateEncoding(_view.DecodingCodePage, _view.IsDecodingBestFitFallback, _view.DecoderReplacementText);

            try
            {
                _view.DestText = EncodingTester.EncodeAndDecode(encoder, decoder, _view.SourceText);
                _view.HexDump = EncodingTester.Hexdump(encoder, _view.SourceText);
            }
            catch(ArgumentException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "再帰的なフォールバックの検出",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        #region NotifyCanExecuteChanged
        public event EventHandler CanExecuteChanged;
        private void NotifyCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, null);
        }
        #endregion
    }
}
