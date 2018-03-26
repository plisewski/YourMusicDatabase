using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace YourMusicDatabase.ViewModel
{
    using Model;
    using System.Windows;
    using System.Windows.Input;

    public class MusicAlbumsViewModel : INotifyPropertyChanged
    {
        private MusicAlbumsModel albumsModel = new MusicAlbumsModel();

        #region Properties
        public string Artist
        {
            get { return albumsModel.Artist; }
            set
            {
                albumsModel.Artist = value;
                OnPropertyChanged(nameof(Artist));
            }
        }

        public string AlbumTitle
        {
            get { return albumsModel.AlbumTitle; }
            set
            {
                albumsModel.AlbumTitle = value;
                OnPropertyChanged(nameof(AlbumTitle));
            }
        }

        public Genre AlbumGenre
        {
            get { return albumsModel.AlbumGenre; }
            set
            {
                albumsModel.AlbumGenre = value;
                OnPropertyChanged(nameof(AlbumGenre));
            }
        }

        public IEnumerable<Genre> GenreValues
        {
            get { return Enum.GetValues(typeof(Genre)).Cast<Genre>(); }
        }

        public DateTime ReleaseDate
        {
            get { return albumsModel.ReleaseDate; }
            set
            {
                albumsModel.ReleaseDate = value;
                OnPropertyChanged(nameof(ReleaseDate));
            }
        }

        public DateTime AddedDate
        {
            get { return albumsModel.AddedDate; }
            set
            {
                albumsModel.AddedDate = value;
                OnPropertyChanged(nameof(ReleaseDate));
            }
        }
        #endregion

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        #endregion

        #region commands
        private ICommand _create;
        public ICommand Create
        {
            get
            {
                if (_create == null)
                    _create = new RelayCommand(
                        p =>
                        {
                            //for testing purposes only
                            AddedDate = DateTime.Now;
                            MessageBox.Show("Artist: " + Artist + "; Title: " + AlbumTitle + "; Genre: " + AlbumGenre + 
                                "; Release Date: " + ReleaseDate.ToShortDateString() + "; Added Date " + AddedDate.ToShortDateString());
                        },
                        p =>
                        {
                            return !string.IsNullOrEmpty(Artist) && !string.IsNullOrEmpty(AlbumTitle) && ReleaseDate < DateTime.Now && ReleaseDate.Year > 1;
                        }
                );
                return _create;
            }
        }

        #endregion
    }
}
