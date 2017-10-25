namespace ExcaliburAuth
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var auth = new ExcaliburAuthHandler();
            auth.HandleAuth("siamant", "A8tOyUPOx21yr5xZfwTf+g==", "");
        }
    }
}