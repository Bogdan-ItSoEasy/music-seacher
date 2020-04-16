using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;


namespace MusicSearcher
{
    internal static class CacheProvider
    {
        
        private static readonly IMemoryCache Cache = new MemoryCache(new MemoryCacheOptions());

        internal static List<(string id, string artist)> GetArtistByTermAsync(string artist)
        {
            return Cache.TryGetValue(artist.ToLower(), out var result) && result is string id? new List<(string id, string artist)>(){(id, artist)} : null;
        }

        internal static List<string> GetAlbumsByArtistId(string id)
        {
            return Cache.TryGetValue(id.ToLower(), out var result) ? result as List<string> : null;
        }

        internal static void SaveResult((string id, string artist) selectedArtist, List<string> albums)
        {
            Cache.Set(selectedArtist.artist.ToLower(), selectedArtist.id, DateTimeOffset.Now.AddHours(1));
            Cache.Set(selectedArtist.id.ToLower(), albums, DateTimeOffset.Now.AddHours(1));
        }
    }
}
