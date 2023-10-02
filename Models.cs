using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscogsAPIConsole
{
    /// <summary>
    /// This file contains all the object types used for deserialization of JSON when using the API.
    /// Contains small object type classes for use in the DiscogsService class.
    /// </summary>
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Profile { get; set; }
        public List<string> Urls { get; set; }
    }
    public class Pagination
    {
        public int Per_page { get; set; }
        public int Items { get; set; }
        public int Page { get; set; }
        public int Pages { get; set; }
        
    }

    public class ReleaseSearchResult
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        
    }

    public class ReleaseSearchResponse
    {
        public Pagination Pagination { get; set; }
        public List<ReleaseSearchResult> Results { get; set; }
    }
    public class Label
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Profile { get; set; }
        public string Contact_info { get; set; }
        public List<string> Urls { get; set; }
        
    }
    public class MarketplaceListing
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Price { get; set; }
    }

    public class MarketplaceSearchResponse
    {
        public Pagination Pagination { get; set; }
        public List<MarketplaceListing> Listings { get; set; }
    }
    public class BasicInformationForUser
    {
        public string Title { get; set; }
        public int Year { get; set; }
        // Add other properties as per the fields in the basic_information object
    }
    public class CollectionRelease
    {
        public int Id { get; set; }
        public int Instance_id { get; set; }
        public DateTime Date_added { get; set; }
        public BasicInformationForUser Basic_information { get; set; }
    }

    public class UserCollectionResponse
    {
        public Pagination Pagination { get; set; }
        public List<CollectionRelease> Releases { get; set; }
    }
}
