using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace YourMusicDatabase.Model
{
    public static class XmlService
    {
        // Create/Add
        public static void Create(string filePath, MusicAlbumModel musicAlbum)
        {
            try
            {
                var xml = XDocument.Load(filePath);
                var album = new XElement("MusicAlbum",
                        new XElement("Artist", musicAlbum.Artist),
                        new XElement("AlbumTitle", musicAlbum.AlbumTitle),
                        new XElement("AlbumGenre", musicAlbum.AlbumGenre),
                        new XElement("ReleaseDate", musicAlbum.ReleaseDate),
                        new XElement("AddedDate", musicAlbum.AddedDate)
                );
                xml.Root?.Add(album);
                xml.Save(filePath);
            }
            catch (FileLoadException ex)
            {
                throw new Exception("There was a problem while loading the XML file", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("There was an error while adding to the XML file", ex);
            }
        }

        // Read
        public static List<MusicAlbumModel> Read(string filePath)
        {
            try
            {
                var xml = XDocument.Load(filePath);

                return (xml.Root?.Elements("MusicAlbum")
                            .Select(e => new MusicAlbumModel
                            {
                                Artist = e.Element("Artist")?.Value,
                                AlbumTitle = e.Element("AlbumTitle")?.Value,
                                AlbumGenre = MusicAlbumModel.ParseGenre(e.Element("AlbumGenre")?.Value),
                                ReleaseDate = DateTime.Parse(e.Element("ReleaseDate")?.Value),
                                AddedDate = DateTime.Parse(e.Element("AddedDate")?.Value)
                            }) ?? throw new InvalidOperationException()).ToList();
            }
            catch (FileLoadException ex)
            {
                throw new Exception("There was a problem while loading the XML file", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("There was an error while reading from the XML file", ex);
            }
        }

        // Update
        public static void Update(string filePath, string artist, string albumTitle, string newArtist, string newAlbumTitle, Genre newAlbumGenre, DateTime newReleaseDate)
        {
            var xml = XDocument.Load(filePath);            
            var album = xml.Descendants("MusicAlbum").Single(x => x.Element("Artist").Value.Equals(artist) && x.Element("AlbumTitle").Value.Equals(albumTitle));
            album.SetElementValue("Artist", newArtist);
            album.SetElementValue("AlbumTitle", newAlbumTitle);
            album.SetElementValue("AlbumGenre", newAlbumGenre);
            album.SetElementValue("ReleaseDate", newReleaseDate);
            
            //TODO -> consider adding another model field -> LastModified
                                                   
            xml.Save(filePath);
        }

        // Delete
        public static void Delete(string filePath, string artist, string albumTitle)
        {
            var xml = XDocument.Load(filePath);            
            var album = xml.Descendants("MusicAlbum").Single(x => x.Element("Artist").Value.Equals(artist) && x.Element("AlbumTitle").Value.Equals(albumTitle));
            album.Remove();
            xml.Save(filePath);
        }
    }
}
