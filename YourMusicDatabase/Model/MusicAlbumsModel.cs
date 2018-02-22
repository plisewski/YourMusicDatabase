using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourMusicDatabase.Model
{
    public class MusicAlbumsModel
    {
        public int AlbumID { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public Genre AlbumGenre { get; set; }

    }
    public enum Genre
    {
        Other,
        Blues,
	    Country,            
	    Electronic,
	    Folk,
	    HipHop,
	    Jazz,	    
	    Pop,
	    Soul,
	    Rock
    }
}
