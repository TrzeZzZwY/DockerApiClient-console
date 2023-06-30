using CRUD_Console_Api_Client.Modles;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Net;

namespace CRUD_Console_Api_Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting app");
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://host.docker.internal:5000/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            while (true)
            {
                switch (Console.ReadLine().ToUpper())
                {
                    case "ADD":
                        client.AddAsync();
                        break;
                    case "GET":
                        client.GetAsync();
                        break;
                    case "DELETE":
                        client.DeleteAsync();
                        break;
                    case "PATCH":
                        client.PatchAsync();
                        break;
                    default:
                        Console.WriteLine("Nie rozpoznano komendy");
                        break;
                }
            }

        }
    }

    public static class CRUD
    {
        public static async void AddAsync(this HttpClient client)
        {
            var input = GetMovieData();
            var json = $$"""{"Title":"{{input.Title}}","Date":null,"Name":"{{input.Director.Name}}","SureName":"{{input.Director.SureName}}"}""";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var resposne = await client.PostAsync("/api/Movie", content);
                var jsonResponeContent = await resposne.Content.ReadAsStringAsync();
                var movie = JsonConvert.DeserializeObject<Movie>(jsonResponeContent);
                DisplayMovieData(movie);
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync($"Error: {e.Message}");
            }
        }
        public static async void GetAsync(this HttpClient client)
        {
            try
            {
                var result = await client.GetAsync("/api/Movie");
                var json = await result.Content.ReadAsStringAsync();
                List<Movie> movies = JsonConvert.DeserializeObject<IEnumerable<Movie>>(json).ToList();
                DisplayMovieData(movies);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }
        public static async void DeleteAsync(this HttpClient client)
        {
            try
            {
                await Console.Out.WriteLineAsync("Podaj Id:");
                int id = int.Parse(Console.ReadLine());
                await client.DeleteAsync($"/api/Movie/{id}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }
        public static async void PatchAsync(this HttpClient client)
        {
            await Console.Out.WriteLineAsync("Podaj Id:");
            int id = int.Parse(Console.ReadLine());
            var input = GetMovieData();
            var json = $$"""{"Title":"{{input.Title}}","Date":null,"Name":"{{input.Director.Name}}","SureName":"{{input.Director.SureName}}"}""";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                await client.PatchAsync($"/api/Movie/{id}", content);
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync($"Error: {e.Message}");
            }
        }
        static Movie GetMovieData()
        {
            Console.WriteLine("\nPodaj tytuł filmu:\n");
            string? title = Console.ReadLine();
            Console.WriteLine("\nPodaj imię reżysera:\n");
            string? DirectionName = Console.ReadLine();
            Console.WriteLine("\nPodaj nazwisko reżysera:\n");
            string? DirectionSurName = Console.ReadLine();
            if (title is null || DirectionName is null || DirectionSurName is null)
            {
                Console.WriteLine("\n Złe dane\n");
                throw new Exception();
            }
            return new Movie() { Director = new Director() { Name = DirectionName, SureName = DirectionSurName }, Title = title };
        }

        static void DisplayMovieData(Movie movie)
        {
            Console.WriteLine($"id:{movie.Id}, title:{movie.Title},directior: {movie.Director.Name} {movie.Director.SureName}");
        }
        static void DisplayMovieData(IEnumerable<Movie> movies)
        {
            foreach (var m in movies)
            {
                DisplayMovieData(m);
                Console.WriteLine();
            }
        }
    }
}