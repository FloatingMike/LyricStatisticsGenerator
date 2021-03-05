using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AireLogicTest.LyricStatistics;
using AireLogicTest.LyricStatistics.Dtos;

namespace AireLogicTest
{
    public class ArtistLyricStatisticsConsoleService
    {
        private readonly ILyricStatisticsHelper _lyricsHelper;
        private readonly IArtistMetadataService _artistMetadataService;
        private readonly ISongLyricService _lyricService;
        private readonly IResultPresentationService _resultPresentationService;
        private readonly IInputService _inputService;

        public ArtistLyricStatisticsConsoleService(ILyricStatisticsHelper lyricsHelper, IArtistMetadataService artistMetadataService, ISongLyricService lyricService, IResultPresentationService resultPresentationService, IInputService inputService)
        {
            _lyricsHelper = lyricsHelper;
            _artistMetadataService = artistMetadataService;
            _lyricService = lyricService;
            _resultPresentationService = resultPresentationService;
            _inputService = inputService;
        }

        public async Task Execute(params string[] args)
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

            _resultPresentationService.OutputStatus($"We have {tracks.Count} tracks");
            var lyrics = new List<LyricDto>();
            var concurrencySemaphore = new SemaphoreSlim(5);
            
            foreach(var track in tracks)
            {
                _resultPresentationService.OutputStatus($"Requesting Lyrics for {track}");
                var lyric = await _lyricService.GetLyricForTrack(artistName, track);
                if (lyric != null)
                {
                    lyrics.Add(lyric);
                }
            }
            
            // then calculate statistics
            var statistics = _lyricsHelper.CalculateStatistics(lyrics);
            
            _resultPresentationService.PresentResults(statistics);
        }

        private string RequestArtistName()
        {
            var name = "";
            var prompt = new StringBuilder();
            prompt.AppendLine();
            prompt.Append("Please provide the name of a musical artist and press enter:");
            while (string.IsNullOrWhiteSpace(name))
            {
                name = _inputService.RequestInput(prompt.ToString());
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
                    var index = 0;
                    var sb = new StringBuilder();
                    sb.AppendLine();
                    foreach (var artist in artists)
                    {
                        sb.AppendLine($"{index}. {artist.Value}");
                        index++;
                    }

                    sb.AppendLine();
                    sb.Append("Please select an Artist by their number: ");
                    
                    int selected = -1;
                    while (selected == -1)
                    {
                        var value = _inputService.RequestInput(sb.ToString());
                        
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            break; // nothing entered, so break the selection loop
                        }
                        
                        if (!int.TryParse(value, out selected))
                        {
                            _resultPresentationService.OutputStatus(
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
    }
}