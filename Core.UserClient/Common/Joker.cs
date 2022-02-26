using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core.UserClient.Common
{
    public partial class Joker : IJoker
    {
        public Joker()
        {
            string dadJokesPath = Path.Combine(Directory.GetCurrentDirectory(), "Common" , "DadJokes.json");
            DadJokes = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(dadJokesPath));

            string yoMommaJokesPath = Path.Combine(Directory.GetCurrentDirectory(), "Common", "YoMommaJokes.json");
            YoMommaJokes = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(yoMommaJokesPath));

            string chuckNorrisJokesPath = Path.Combine(Directory.GetCurrentDirectory(), "Common", "ChuckNorrisJokes.json");
            ChuckNorrisJokes = JsonConvert.DeserializeObject<List<ChuckNorrisJoke>>(File.ReadAllText(chuckNorrisJokesPath))
                .GroupBy(x => x.categories != null && x.categories.Any()).ToDictionary(x=> x.Key, y => y.ToList());
        }

        public List<string> DadJokes { get; }
        public List<string> YoMommaJokes { get; }
        public Dictionary<bool, List<ChuckNorrisJoke>> ChuckNorrisJokes { get; }

        public string GetRandomDadJoke()
        {
            return DadJokes[new Random().Next(0, DadJokes.Count)];
        }

        public string GetRandomYoMommaJoke()
        {
            return YoMommaJokes[new Random().Next(0, YoMommaJokes.Count)];
        }

        public string GetRandomChuckNorrisJoke(bool unSafe = false)
        {
            return ChuckNorrisJokes[unSafe][new Random().Next(0, ChuckNorrisJokes[unSafe].Count)].joke;
        }
    }
}
