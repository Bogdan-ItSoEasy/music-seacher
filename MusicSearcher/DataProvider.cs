using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicSearcher
{
    internal class DataProvider
    {
        internal static async Task<List<(string id, string artist)>> GetArtistByTermAsync(string artist)
        {
            try
            {
                return await ItunesApiProvider.GetArtistByTermAsync(artist);
            }
            catch (Exception)
            {
                Console.WriteLine("Проблемы с соединением. Попытка получить данные из кэша");
                return CacheProvider.GetArtistByTermAsync(artist);
            }
        }

        internal static async Task<List<string>> GetAlbumsByArtistId(string id)
        {
            try
            {
                return await ItunesApiProvider.GetAlbumsByArtistId(id);
            }
            catch (Exception)
            {
                Console.WriteLine("Проблемы с соединением. Попытка получить данные из кэша");
                return CacheProvider.GetAlbumsByArtistId(id); 
            }
        }

        internal static void SaveResult((string id, string artist) selectedArtist, List<string> albums)
        {
            CacheProvider.SaveResult(selectedArtist, albums);
        }
    }
}
