using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MusicSearcher
{
    public static class ItunesApiProvider
    {
        internal static async Task<List<(string id, string artist)>> GetArtistByTermAsync(string term)
        {
            var request = new HttpClient();
            var responce = await request
                .GetStringAsync($@"https://itunes.apple.com/search?term={term.Trim().Replace(' ', '+')}&media=music&entity=musicArtist&attribute=artistTerm");

            var json = JObject.Parse(responce)["results"]; 
            return json.Select(x=>(id:x["artistId"].Value<string>(), artist:x["artistName"].Value<string>())).ToList();
        }

        internal static async Task<List<string>> GetAlbumsByArtistId(string id)
        {
            var request = new HttpClient();
            var responce = await request.GetStringAsync($@"https://itunes.apple.com/lookup?id={id}&entity=album");
            
            return JObject.Parse(responce)["results"]
                .Where(x => x["wrapperType"].Value<string>() == "collection")
                .Select(x => x["collectionName"].Value<string>())
                .ToList();
        }
    }
}
