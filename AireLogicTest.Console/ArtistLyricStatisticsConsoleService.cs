using System;
using System.Linq;
using System.Threading.Tasks;
using AireLogicTest.LyricStatistics;
using AireLogicTest.LyricStatistics.Dtos;

namespace AireLogicTest
{
    class ArtistLyricStatisticsConsoleService
    {
        private readonly ILyricStatisticsHelper _lyricsHelper;
        private readonly IArtistMetadataService _artistMetadataService;
        private readonly ISongLyricService _lyricService;

        public ArtistLyricStatisticsConsoleService(ILyricStatisticsHelper lyricsHelper, IArtistMetadataService artistMetadataService, ISongLyricService lyricService)
        {
            _lyricsHelper = lyricsHelper;
            _artistMetadataService = artistMetadataService;
            _lyricService = lyricService;
        }

        public async Task Execute(string[] args)
        {
            string artistName, artistId;
            
            if (!args.Any())
            {
                // no artist name was provided in the arguments so we will request one with a prompt
                (artistName, artistId) = await PerformArtistSelection();
            }
            else
            {
                // sanitise the input a little, we're expecting some name components only here.
                (artistName, artistId) = await PerformArtistSelection(string.Join(" ",
                        args.Select(a => a.Trim().ToLowerInvariant()))); // let's also normalise the name to lowercase
            }

            // now we have the artistId we can go get a collection of tracks for that artist from the metadata service
            var tracks = await _artistMetadataService.GetTrackNamesForArtist(artistId);
            
            // with the tracks we need to collect the lyrics from the lyric service

            var lyrics = await Task.WhenAll(tracks.Select(t => _lyricService.GetLyricForTrack(artistName, t)));
            
            // then calculate statistics
            var statistics = _lyricsHelper.CalculateStatistics(lyrics.Where(l => l != null).ToList());
            
            PresentResults(statistics);
            
        }

        private string RequestArtistName()
        {
            var name = "";
            while (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine();
                Console.Write("Please provide the name of a musical artist and press enter:");
                name = Console.ReadLine();
            }

            return name.Trim().ToLowerInvariant();
        }

        private async Task<(string artistName, string artistId)> PerformArtistSelection(string artistName = "")
        {
            while (true)
            {
                if (string.IsNullOrWhiteSpace(artistName))
                {
                    artistName = RequestArtistName();
                }

                // now we'll go and perform a lookup for the artist
                var artists = await _artistMetadataService.FindArtistKey(artistName);

                if (!artists.Any())
                {
                    throw new Exception($"{artistName} produced no results");
                }

                if (artists.Count > 1)
                {
                    // we have more than one match for that artist, let's display a picker to allow the choice
                    Console.WriteLine();
                    var index = 0;
                    foreach (var artist in artists)
                    {
                        Console.WriteLine($"{index}. {artist.Value}");
                        index++;
                    }

                    int selected = -1;
                    while (selected == -1)
                    {
                        Console.WriteLine();
                        Console.Write("Please select an Artist by their number: ");
                        var value = Console.ReadLine()?.Trim();
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            break; // nothing entered, so break the selection loop
                        }
                        
                        if (!int.TryParse(value, out selected))
                        {
                            Console.WriteLine(
                                $"Sorry `{value}` was not a valid selection, please choose a number between 0 and {artists.Count - 1}");
                        }
                    }

                    if (selected != -1)
                    {
                        var selectedArtistId = artists.Keys.ToList()[selected];
                        return (artists[selectedArtistId], selectedArtistId);
                    }
                }
                else
                {
                    var selectedArtistId = artists.Keys.ToList()[0];
                    return (artists[selectedArtistId], selectedArtistId);
                }

                artistName = string.Empty;
            }
        }

        private void PresentResults(LyricStatisticsDto lyricStatisticsDto)
        {
            Console.WriteLine(lyricStatisticsDto);
        }
    }
}