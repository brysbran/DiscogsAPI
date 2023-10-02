using System;
using Newtonsoft.Json.Linq;

namespace DiscogsAPIConsole
{
    /// <summary>
    /// Console application which integrates the Discogs API to do a few functions
    /// After coming up with some requirements and a design using a UML diagram I began to implement.
    /// This file will contain the calls to various methods with implement the API as well as allow the user to interact with the console.
    /// </summary>
    public class Program
    {
        static DiscogsService discogsService = new DiscogsService("aXsNFZYDhQneBFDJOQwHOzSGChVHkNnpCFxtORRg");
        static async Task Main(string[] args)
        {
            bool shouldContinue = true;

            while (shouldContinue)
            {
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. Search for Artist");
                Console.WriteLine("2. Search for Release");
                Console.WriteLine("3. Get Label Information");
                Console.WriteLine("4. Get User Collection Items");
                Console.WriteLine("5. Quit");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Enter artist name:");
                        var artistName = Console.ReadLine();
                        var artist = await discogsService.GetArtistByName(artistName);
                        Console.WriteLine($"Artist Name: {artist.Name}");
                        Console.WriteLine($"Arist Profile: {artist.Profile}");
                        Console.WriteLine($"Artist URLS: {artist.Urls}");
                        break;

                    case "2":
                        Console.WriteLine("Enter release query (album name):");
                        var releaseQuery = Console.ReadLine();
                        var release = await discogsService.SearchRelease(releaseQuery);
                        Console.WriteLine($"First Release Found Title: {release.Results[0].Title}");
                        Console.WriteLine($"First Release Found Year of Release: {release.Results[0].Year}");
                        Console.WriteLine($"First Release Found ID: {release.Results[0].Id}");
                        break;

                    case "3":
                        Console.WriteLine("Enter label ID:");
                        var labelId = int.Parse(Console.ReadLine());
                        var label = await discogsService.GetLabel(labelId);
                        Console.WriteLine($"Label Name: {label.Name}");
                        break;

                    case "4":
                        Console.WriteLine("Enter username:");
                        var username = Console.ReadLine();
                        Console.WriteLine("Enter folder ID (default is 0):");
                        var folderId = int.Parse(Console.ReadLine() ?? "0");
                        var collection = await discogsService.GetUserCollectionItems(username, folderId);

                        if (collection.Count > 0)
                        {
                            Console.WriteLine("Collection Items:");
                            foreach (var item in collection)
                            {
                                Console.WriteLine($"- {item.Basic_information.Title} ({item.Basic_information.Year})");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No items found in collection.");
                        }
                        break;

                    case "5":
                        shouldContinue = false;
                        Console.WriteLine("Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
    }
}