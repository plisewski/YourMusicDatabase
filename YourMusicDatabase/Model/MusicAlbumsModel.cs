using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourMusicDatabase.Model
{
    public enum Genre { Blues, Country, Electronic, Folk, HipHop, Jazz, Pop, Rock, Soul, Other }

    public class MusicAlbumsModel
    {
        public string Artist { get; set; }
        public string AlbumTitle { get; set; }
        public Genre AlbumGenre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
