using System;
using System.Collections.Generic;
using SimbirSoftTestTask.Util;

namespace SimbirSoftTestTask.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        private string _url;

        private Dictionary<string, int> _statistic;

        private bool _isUrlValid;

        private RelayCommand _getStatisticRelayCommand;

        public string Url
        {
            get => _url;
            set
            {
                IsUrlValid = Uri.TryCreate(value, UriKind.Absolute, out Uri uriResult)
                             && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                _url = value;
                OnPropertyChanged(nameof(Url));
            }
        }

        public Dictionary<string, int> Statistic
        {
            get => _statistic;

            set
            {
                _statistic = value;
                OnPropertyChanged(nameof(Statistic));
            }
        }

        public bool IsUrlValid
        {
            get => _isUrlValid;
            set
            {
                _isUrlValid = value;
                OnPropertyChanged(nameof(IsUrlValid));
            }
        }
        
        public RelayCommand GetStatisticRelayCommand =>
            _getStatisticRelayCommand ?? (_getStatisticRelayCommand = new RelayCommand(obj =>
            {
                Statistic = Model.Model.GetStatistic(Url);
            }));
    }
}