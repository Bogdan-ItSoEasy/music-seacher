using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicSearcher
{
    class Program
    {

        static bool SelectArtist(List<string> artists, out uint result)
        {
            var count = artists.Count();
            Console.WriteLine($"Найдено {count} вариантов. Введите номер артиста.\n" +
                        $"Если нужного артиста нет, то введите 0.\n" +
                        string.Join('\n', artists.Select((x, i) => $"{i + 1}: {x}")));
            return uint.TryParse(Console.ReadLine(), out result) && result <= count;
        }

        static async System.Threading.Tasks.Task Main()
        {
            while (true)
            {
                Console.WriteLine("Введите название артиста: (exit - для выхода из программы)");
                var artist = Console.ReadLine();
                if (artist == "exit")
                    break;

                var artists = await DataProvider.GetArtistByTermAsync(artist);
                if (artists is null || artists.Count == 0)
                {
                    Console.WriteLine("Не удалось найди артиста. Попробуйте ещё раз.");
                    continue;
                }
                (string id, string artist) selectedArtist;
                if (artists.Count == 1)
                {
                    selectedArtist = artists.First();
                }
                else
                {
                    uint artistIndex;
                    while (!SelectArtist(artists.Select(x => x.artist).ToList(), out artistIndex))
                    {
                        Console.WriteLine("Введен некорректный номер артиста. Попробуйте ещё раз.");
                    }
                    if (artistIndex == 0)
                    {
                        continue;
                    }

                    selectedArtist = artists[(int)artistIndex - 1];
                }
                var albums = await DataProvider.GetAlbumsByArtistId(selectedArtist.id);
                if (albums is null || albums.Count == 0)
                { 
                    Console.WriteLine($"{selectedArtist.artist}. Альбомы не найдены.");
                }
                else
                {
                    Console.WriteLine($"{selectedArtist.artist}. Найдены следующие альбомы:\n" +
                        string.Join('\n', albums.Select((x, i) => $"{i + 1}) {x}")));
                    DataProvider.SaveResult(selectedArtist, albums);
                }

                
            }
        }
    }
}
