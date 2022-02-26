using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Core.ResourceApi.Common
{
    public class Joker : IJoker
    {
        public Joker()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Common", "KnockKnockJokes.json");
            Jokes = JsonConvert.DeserializeObject<List<Joke>>(File.ReadAllText(path));
        }

        public List<Joke> Jokes { get; private set; }

        public string GetRandomJoke()
        {
            return Jokes[new Random().Next(0, Jokes.Count)].Frame();
        }
    }
}
