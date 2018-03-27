using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace YourMusicDatabase.ViewModel
{
    #region usings

    using Model;
    using System.Windows;
    using System.Windows.Input;

    #endregion

    public class MusicAlbumViewModel : INotifyPropertyChanged
    {
        #region Fields

        private MusicAlbumModel albumModel = new MusicAlbumModel();        
        private const string filePath = "../../MusicAlbumsDB.xml";
        private List<MusicAlbumModel> musicAlbumsList = new List<MusicAlbumModel>();
        private List<MusicAlbumModel> musicAlbumsListUpdated = new List<MusicAlbumModel>();;
        private int? _selectedAlbumIndex;

        #endregion

        #region Properties

        public string Artist
        {
            get { return albumModel.Artist; }
            set
            {
                albumModel.Artist = value;
                OnPropertyChanged(nameof(Artist));
            }
        }

        public string AlbumTitle
        {
            get { return albumModel.AlbumTitle; }
            set
            {
                albumModel.AlbumTitle = value;
                OnPropertyChanged(nameof(AlbumTitle));
            }
        }

        public Genre AlbumGenre
        {
            get { return albumModel.AlbumGenre; }
            set
            {
                albumModel.AlbumGenre = value;
                OnPropertyChanged(nameof(AlbumGenre));
            }
        }

        public IEnumerable<Genre> GenreValues
        {
            get { return Enum.GetValues(typeof(Genre)).Cast<Genre>(); }
        }

        public DateTime ReleaseDate
        {
            get { return albumModel.ReleaseDate; }
            set
            {
                albumModel.ReleaseDate = value;
                OnPropertyChanged(nameof(ReleaseDate));
            }
        }

        public DateTime AddedDate
        {
            get { return albumModel.AddedDate; }
            set
            {
                albumModel.AddedDate = value;
                OnPropertyChanged(nameof(ReleaseDate));
            }
        }

        public List<MusicAlbumModel> MusicAlbumsList
        {
            get { return musicAlbumsList; }            
            set
            {
                musicAlbumsList = value;
                OnPropertyChanged(nameof(MusicAlbumsList));
            }
        }

        public List<MusicAlbumModel> MusicAlbumsListUpdated
        {
            get { return musicAlbumsListUpdated; }
            set
            {
                musicAlbumsListUpdated = value;
                OnPropertyChanged(nameof(MusicAlbumsListUpdated));
            }
        }
        
        public int? SelectedAlbumIndex
        {
            get { return _selectedAlbumIndex; }
            set
            {
                _selectedAlbumIndex = value;
                OnPropertyChanged(nameof(SelectedAlbumIndex));
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        #endregion

        #region Commands

        private ICommand _create;
        public ICommand Create
        {
            get
            {
                if (_create == null)
                    _create = new RelayCommand(
                        p =>
                        {                            
                            AddedDate = DateTime.Now;                            
                            XmlService.Create(filePath, albumModel);
                            MessageBox.Show("Your album has been added!", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                            Artist = "";
                            AlbumTitle = "";
                            AlbumGenre = Genre.Blues;                         
                        },
                        p =>
                        {
                            return !string.IsNullOrEmpty(Artist) && !string.IsNullOrEmpty(AlbumTitle) && ReleaseDate < DateTime.Now && ReleaseDate.Year > 1;
                        }
                );
                return _create;
            }
        }

        private ICommand _read;
        public ICommand Read
        {
            get
            {
                if (_read == null)
                    _read = new RelayCommand(
                        p =>
                        {
                            MusicAlbumsList = XmlService.Read(filePath);
                            MusicAlbumsListUpdated = MusicAlbumModel.DeepClone(musicAlbumsList);
                        }
                );
                return _read;
            }
        }

        private ICommand _update;
        public ICommand Update
        {
            get
            {                
                if (_update == null)
                    _update = new RelayCommand(
                        p =>
                        {
                            int selectedAlbumIndex = (int)_selectedAlbumIndex;

                            string artistToUpdate = musicAlbumsList[selectedAlbumIndex].Artist;
                            string albumTitleToUpdate = musicAlbumsList[selectedAlbumIndex].AlbumTitle;
                            string newArtist = musicAlbumsListUpdated[selectedAlbumIndex].Artist;
                            string newAlbumTitle = musicAlbumsListUpdated[selectedAlbumIndex].AlbumTitle;
                            Genre newAlbumGenre = musicAlbumsListUpdated[selectedAlbumIndex].AlbumGenre;
                            DateTime newReleaseDate = musicAlbumsListUpdated[selectedAlbumIndex].ReleaseDate;                            

                            if (newReleaseDate > DateTime.Now)
                            {
                                MessageBox.Show("New release date cannot be in future", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }                                

                            XmlService.Update(filePath, artistToUpdate, albumTitleToUpdate, newArtist, newAlbumTitle, newAlbumGenre, newReleaseDate);
                            MessageBox.Show("Your album database has been updated!", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                            MusicAlbumsList = MusicAlbumModel.DeepClone(musicAlbumsListUpdated);
                        },
                        p =>
                        {
                            int selectedAlbumIndex;

                            if (!_selectedAlbumIndex.HasValue)
                                return false;
                            else
                                selectedAlbumIndex = (int)_selectedAlbumIndex;

                            return _selectedAlbumIndex != null && (musicAlbumsList[selectedAlbumIndex].Artist != musicAlbumsListUpdated[selectedAlbumIndex].Artist 
                                || musicAlbumsList[selectedAlbumIndex].AlbumTitle != musicAlbumsListUpdated[selectedAlbumIndex].AlbumTitle
                                || musicAlbumsList[selectedAlbumIndex].AlbumGenre != musicAlbumsListUpdated[selectedAlbumIndex].AlbumGenre
                                || musicAlbumsList[selectedAlbumIndex].ReleaseDate != musicAlbumsListUpdated[selectedAlbumIndex].ReleaseDate);
                        }
                );
                return _update;
            }
        }

        private ICommand _delete;
        public ICommand Delete
        {
            get
            {
                if (_delete == null)
                    _delete = new RelayCommand(
                        p =>
                        {
                            if (MessageBox.Show("Are you sure?", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                                return; 
                                
                            int selectedAlbumIndex = (int)_selectedAlbumIndex;

                            string artistToDelete = musicAlbumsList[selectedAlbumIndex].Artist;
                            string albumTitleToDelete = musicAlbumsList[selectedAlbumIndex].AlbumTitle;

                            XmlService.Delete(filePath, artistToDelete, albumTitleToDelete);
                            MessageBox.Show("Your album has been deleted!", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                            MusicAlbumsList = XmlService.Read(filePath);
                            MusicAlbumsListUpdated = MusicAlbumModel.DeepClone(musicAlbumsList);                            
                        },
                        p =>
                        {
                            return _selectedAlbumIndex.HasValue;
                        }
                );
                return _delete;
            }
        }

        #endregion
    }
}
