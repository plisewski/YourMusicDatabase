using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace YourMusicDatabase.Model
{
    public enum Genre { Blues, Country, Electronic, Folk, HipHop, Jazz, Pop, Rock, Soul, Other }

    [SerializableAttribute]
    public class MusicAlbumModel
    {
        #region Properties

        public string Artist { get; set; }
        public string AlbumTitle { get; set; }
        public Genre AlbumGenre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime AddedDate { get; set; }

        #endregion

        #region Methods
        public static Genre ParseGenre(string descriptionGenre)
        {
            switch (descriptionGenre)
            {
                case "Blues":
                    return Genre.Blues;
                case "Country":
                    return Genre.Country;
                case "Electronic":
                    return Genre.Electronic;
                case "Folk":
                    return Genre.Folk;
                case "HipHop":
                    return Genre.HipHop;
                case "Jazz":
                    return Genre.Jazz;
                case "Pop":
                    return Genre.Pop;
                case "Rock":
                    return Genre.Rock;
                case "Soul":
                    return Genre.Soul;
                case "Other":
                    return Genre.Other;
                default:
                    throw new Exception("Unrecognized Genre Type");
            }
        }

        // https://stackoverflow.com/questions/129389/how-do-you-do-a-deep-copy-of-an-object-in-net-c-specifically/129395#129395
        public static T DeepClone<T>(T obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }

        #endregion
    }
}
