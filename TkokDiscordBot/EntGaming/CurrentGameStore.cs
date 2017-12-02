using System;
using System.ComponentModel;
using TkokDiscordBot.EntGaming.Dto;

namespace TkokDiscordBot.EntGaming
{
    internal class CurrentGameStore
    {
        private LobbyStatus _status;
        public DateTime TrackingDate;

        /// <summary>
        ///     Status of current Game or null (if game doesn't exist).
        /// </summary>
        public LobbyStatus Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));

                    TrackingDate = DateTime.Now;
                }
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler LobbyStatusChanged;

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = LobbyStatusChanged;
            handler?.Invoke(this, e);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}