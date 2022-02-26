namespace Core.Access.Utility.Authentication
{
    public class BasicCredentials
    {
        public string Identifier { get; set; }

        public string Secret { get; set; }

        public bool IsIdentifierMatchingWith(string val) => Identifier.Equals(val);
    }
}
