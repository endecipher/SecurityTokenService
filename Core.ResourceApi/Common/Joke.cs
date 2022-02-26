namespace Core.ResourceApi.Common
{
    public class Joke {
        public string name { get; set; }
        public string answer { get; set; }
        public string Frame()
        {
            return $"Knock-Knock! \n Who's There? \n {name}. \n {name} who?! \n {name} {answer}!";
        }
    }
}
