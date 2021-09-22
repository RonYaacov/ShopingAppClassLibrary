using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace ShopingBotClassLibrary.DataAccess
{
    public class LiteDBUsers
    {
        private string databaseName { get; set; } = "shopingData";
        private string collectionName { get; set; } = "users";



        public bool CreateNewUser(Models.User user)
        {
            using (var db = new LiteDatabase(@"C:\LiteDB\database.db"))
            {
                var collection = db.GetCollection<Models.User>(collectionName);
                int usersCount = collection.Find(x => x.username == user.username).Count();
                if (usersCount == 0)
                {
                    collection.Insert(user);
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
        public Models.User FindUserByUsername(string username)
        {
            using (var db = new LiteDatabase(@"C:\LiteDB\database.db"))
            {
                try
                {
                    var collection = db.GetCollection<Models.User>(collectionName);
                    return collection.Find(x => x.username == username).First();
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
        }

        public List<Models.User> FindAllUsers()
        {
            using (var db = new LiteDatabase(@"C:\LiteDB\database.db"))
            {
                var collection = db.GetCollection<Models.User>(collectionName);
                return collection.FindAll().ToList();
            }
        }
        public string DeleteAllUsers()
        {
            try
            {
                using (var db = new LiteDatabase(@"C:\LiteDB\database.db"))
                {
                    var collection = db.GetCollection<Models.User>(collectionName);
                    var deletedUsersCount = collection.DeleteAll();
                    return $"Deleted all the users, {deletedUsersCount} users were deleted";
                }
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
                using (var db = new LiteDatabase(@"C:\LiteDB\database.db"))
                {
                    var collection = db.GetCollection<Models.User>(collectionName);
                    var deletedUsername = collection.Find(x => x.username == username).ToList();
                    collection.DeleteMany(x => x.username == username);
                    return deletedUsername.First();                    
                }
            }
            catch
            {
                return null;
            }
        }
        public Models.User UpdateUser(Models.User user)
        {
            using(var db = new LiteDatabase(@"C:\LiteDB\database.db"))
            {
                try
                {
                    var collection = db.GetCollection<Models.User>(collectionName);
                    var userToUpdate = collection.Find(x => x.username == user.username).First();
                    userToUpdate.password = user.password;
                    userToUpdate.purchasedProdacts = user.purchasedProdacts;
                    collection.Update(userToUpdate);
                    return userToUpdate;
                }
                catch

                {
                    return null;
                }
                
                
            }
        }
    }
}
