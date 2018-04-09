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

        private readonly MusicAlbumModel _albumModel = new MusicAlbumModel();        
        private const string FilePath = "../../MusicAlbumsDB.xml";
        private List<MusicAlbumModel> _musicAlbumsList = new List<MusicAlbumModel>();
        private List<MusicAlbumModel> _musicAlbumsListUpdated = new List<MusicAlbumModel>();
        private int? _selectedAlbumIndex;

        #endregion

        #region Properties

        public string Artist
        {
            get => _albumModel.Artist;
            set
            {
                _albumModel.Artist = value;
                OnPropertyChanged(nameof(Artist));
            }
        }

        public string AlbumTitle
        {
            get => _albumModel.AlbumTitle;
            set
            {
                _albumModel.AlbumTitle = value;
                OnPropertyChanged(nameof(AlbumTitle));
            }
        }

        public Genre AlbumGenre
        {
            get => _albumModel.AlbumGenre;
            set
            {
                _albumModel.AlbumGenre = value;
                OnPropertyChanged(nameof(AlbumGenre));
            }
        }

        public IEnumerable<Genre> GenreValues => Enum.GetValues(typeof(Genre)).Cast<Genre>();

        public DateTime ReleaseDate
        {
            get => _albumModel.ReleaseDate;
            set
            {
                _albumModel.ReleaseDate = value;
                OnPropertyChanged(nameof(ReleaseDate));
            }
        }

        public DateTime AddedDate
        {
            get => _albumModel.AddedDate;
            set
            {
                _albumModel.AddedDate = value;
                OnPropertyChanged(nameof(ReleaseDate));
            }
        }

        public List<MusicAlbumModel> MusicAlbumsList
        {
            get => _musicAlbumsList;
            set
            {
                _musicAlbumsList = value;
                OnPropertyChanged(nameof(MusicAlbumsList));
            }
        }

        public List<MusicAlbumModel> MusicAlbumsListUpdated
        {
            get => _musicAlbumsListUpdated;
            set
            {
                _musicAlbumsListUpdated = value;
                OnPropertyChanged(nameof(MusicAlbumsListUpdated));
            }
        }
        
        public int? SelectedAlbumIndex
        {
            get => _selectedAlbumIndex;
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
                            XmlService.Create(FilePath, _albumModel);
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
                            MusicAlbumsList = XmlService.Read(FilePath);
                            MusicAlbumsListUpdated = MusicAlbumModel.DeepClone(_musicAlbumsList);
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
                            if (_selectedAlbumIndex != null)
                            {
                                int selectedAlbumIndex = (int)_selectedAlbumIndex;

                                string artistToUpdate = _musicAlbumsList[selectedAlbumIndex].Artist;
                                string albumTitleToUpdate = _musicAlbumsList[selectedAlbumIndex].AlbumTitle;
                                string newArtist = _musicAlbumsListUpdated[selectedAlbumIndex].Artist;
                                string newAlbumTitle = _musicAlbumsListUpdated[selectedAlbumIndex].AlbumTitle;
                                Genre newAlbumGenre = _musicAlbumsListUpdated[selectedAlbumIndex].AlbumGenre;
                                DateTime newReleaseDate = _musicAlbumsListUpdated[selectedAlbumIndex].ReleaseDate;                            

                                if (newReleaseDate > DateTime.Now)
                                {
                                    MessageBox.Show("New release date cannot be in future", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }                                

                                XmlService.Update(FilePath, artistToUpdate, albumTitleToUpdate, newArtist, newAlbumTitle, newAlbumGenre, newReleaseDate);
                            }

                            MessageBox.Show("Your album database has been updated!", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                            MusicAlbumsList = MusicAlbumModel.DeepClone(_musicAlbumsListUpdated);
                        },
                        p =>
                        {
                            int selectedAlbumIndex;

                            if (!_selectedAlbumIndex.HasValue)
                                return false;
                            else
                                selectedAlbumIndex = (int)_selectedAlbumIndex;

                            return _selectedAlbumIndex != null && (_musicAlbumsList[selectedAlbumIndex].Artist != _musicAlbumsListUpdated[selectedAlbumIndex].Artist 
                                || _musicAlbumsList[selectedAlbumIndex].AlbumTitle != _musicAlbumsListUpdated[selectedAlbumIndex].AlbumTitle
                                || _musicAlbumsList[selectedAlbumIndex].AlbumGenre != _musicAlbumsListUpdated[selectedAlbumIndex].AlbumGenre
                                || _musicAlbumsList[selectedAlbumIndex].ReleaseDate != _musicAlbumsListUpdated[selectedAlbumIndex].ReleaseDate);
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

                            if (_selectedAlbumIndex != null)
                            {
                                int selectedAlbumIndex = (int)_selectedAlbumIndex;

                                string artistToDelete = _musicAlbumsList[selectedAlbumIndex].Artist;
                                string albumTitleToDelete = _musicAlbumsList[selectedAlbumIndex].AlbumTitle;

                                XmlService.Delete(FilePath, artistToDelete, albumTitleToDelete);
                            }

                            MessageBox.Show("Your album has been deleted!", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                            MusicAlbumsList = XmlService.Read(FilePath);
                            MusicAlbumsListUpdated = MusicAlbumModel.DeepClone(_musicAlbumsList);                            
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
