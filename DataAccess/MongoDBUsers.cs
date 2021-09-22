using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace ShopingBotClassLibrary.DataAccess
{
    public class MongoDBUsers
    {
        private const string dataBaseName = "shopingData";
        private const string collectionName = "users";
        private readonly IMongoCollection<Models.User> userCollection;

        public MongoDBUsers()
        {
            var mongoClient = new MongoClient("your connction string ...");
            IMongoDatabase userDatabase = mongoClient.GetDatabase(dataBaseName);
            userCollection = userDatabase.GetCollection<Models.User>(collectionName);
        }

        public bool CreateNewUser(Models.User user)
        {
            var UserNumber = userCollection.Find(x => x.username == user.username).CountDocuments();
            if (UserNumber == 0)
            {
                userCollection.InsertOne(user);
                return true;
            }
            else
            {
                return false;
            }
        }
        public void UpdateUser(Models.User user, string newUserName)
        {
            var userName = userCollection.Find(x => x.username == user.username).FirstOrDefault();
            userCollection.UpdateOne(x => x.username == user.username, newUserName);
        }
        public Models.User FindUserByUsername(string username)
        {
            try
            {
                return userCollection.Find(x => x.username == username).FirstOrDefault();
            }
            catch
            {
                return new Models.User()
                {
                    username = null,
                    password = null,
                    purchasedProdacts = 0
                };
            }
        }
        public List<Models.User> FindAllUsers()
        {
            return userCollection.Find(new BsonDocument()).ToList();
        }
        public string DeleteAllUsers()
        {
            try
            {
                var deletedUsersCount = userCollection.DeleteMany(new BsonDocument()).DeletedCount;
                return $"Deleted all the users, {deletedUsersCount} users were deleted";
            }
            catch
            {
                return "Faild to delete the users";

            }
        }
        public Models.User DeleteUserByUsername(string username)
        {
            try
            {
                var deletedUsername = userCollection.Find(filter: x => x.username == username).ToList();
                userCollection.DeleteMany(filter:x => x.username == username);
                return deletedUsername[0];
            }
            catch
            {
                return null;
            }
        }
        
    }
}