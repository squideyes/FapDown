using FapDown;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder().
    AddUserSecrets<Program>()
    .Build();

var galleryId = int.Parse(config["Gallery:Id"]);
var name = config["Gallery:Name"];

var fetcher = new Fetcher(galleryId, name);

await fetcher.FetchAsync();

Console.ReadKey(true);