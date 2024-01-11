namespace App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new Startup().ConfigureApplicationAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}