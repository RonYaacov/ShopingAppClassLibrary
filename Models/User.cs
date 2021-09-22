using MongoDB.Bson;
using System.Collections.Generic;

namespace ShopingBotClassLibrary.Models
{
    public class User
    {
        public ObjectId _id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int purchasedProdacts { get; set; }    
    }
}