using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PBSqlite.Models;

namespace PBSqlite.Services
{
    public interface ITableDataService
    {
        void CreateTable(string tableName);
        void CreateUser(User newUser);
        Table GetTable();
        List<Table> GetAllTables();
        List<User> GetAllUsers();
        Player GetPlayer(int playerId);
        IEnumerable<Player> GetAllPlayer();
        void AddOrUpdatePlayer(Player newPlayer);
        void DeletePlayer(int id);
        IEnumerable<Player> GetByTable(string tblName);
    }

    public class TableDataService : ITableDataService
    {
        private readonly ApplicationDbContext _db;

        public TableDataService(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public void CreateUser(User newUser)
        {
            _db.Add(new User
            {
                UserName = newUser.UserName,
                Password = newUser.Password
            });
            _db.SaveChanges();
        }

        public void CreateTable(string tName)
        {
            _db.Add(new Table { TableName = tName ?? "NONAME" });
            _db.SaveChanges();
        }

        public List<Table> GetAllTables()
        {
            return _db.Table.ToList();
        }

        public List<User> GetAllUsers()
        {
            return _db.Users.ToList();
        }

        public Player GetPlayer(int id)
        {
            var tName = _db.Table.OrderBy(t => t.Id).LastOrDefault();
            var plist = ListFromString(tName?.PlayersData);
            var returnPlayer = new Player();

            foreach (var pl in plist.Where(pl => pl.Id == id))
            {
                returnPlayer.Id = pl.Id;
                returnPlayer.Name = pl.Name;
                returnPlayer.BuyIns = pl.BuyIns;
                returnPlayer.Close = pl.Close;
                returnPlayer.Total = pl.Total;
            }

            return returnPlayer;
        }

        public IEnumerable<Player> GetByTable(string tblName)
        {
            if (!string.IsNullOrEmpty(tblName))
            {
                var tName = _db.Table.FirstOrDefault(t => t.TableName == tblName);
                var plist = ListFromString(tName?.PlayersData);

                return plist;
            }

            return null;
        }

        public Table GetTable()
        {
            return _db.Table.OrderBy(t => t.Id).LastOrDefault();
        }

        public IEnumerable<Player> GetAllPlayer()
        {
            var tName = _db.Table.OrderBy(t => t.Id).LastOrDefault();
            var plist = ListFromString(tName?.PlayersData);

            return plist;
        }

        public void AddOrUpdatePlayer(Player newPlayer)
        {
            var tName = _db.Table.OrderBy(t => t.Id).LastOrDefault();
            var plist = ListFromString(tName?.PlayersData);
            plist ??= new List<Player>();

            var update = false;

            foreach (var pl in plist.Where(pl => pl.Id == newPlayer.Id))
            {
                pl.BuyIns = newPlayer.BuyIns;
                pl.Close = newPlayer.Close;
                pl.Total = (newPlayer.BuyIns + 1) * -5 + newPlayer.Close * 0.5;
                update = true;
            }

            if (!update)
                plist.Add(newPlayer);

            if (tName != null)
            {
                var tupdate = true;
                //Update Table Data as well.
                foreach (var pl in plist)
                {
                    if (tupdate)
                    {
                        tName.TotalBuyIns = 0;
                        tName.TotalLoss = 0;
                        tName.TotalGain = 0;
                        tupdate = false;
                    }
                    if (pl.BuyIns > 0)
                        tName.TotalBuyIns += pl.BuyIns;
                    if (pl.Total < 0)
                        tName.TotalLoss += pl.Total;
                    if (pl.Total > 0)
                        tName.TotalGain += pl.Total;
                }
                tName.TotalTable = ((tName.TotalBuyIns + plist.Count) * 10);
                tName.PlayersData = StringFromList(plist);
            }

            _db.SaveChanges();
        }

        public void DeletePlayer(int id)
        {
            var tName = _db.Table.OrderBy(t => t.Id).LastOrDefault();
            var plist = ListFromString(tName?.PlayersData);

            var index = plist.FindIndex(i => i.Id == id);
            plist.RemoveAt(index);

            if (tName != null)
                tName.PlayersData = StringFromList(plist);

            _db.SaveChanges();
        }

        private static List<Player> ListFromString(string playersData)
        {
            return playersData != null ? JsonConvert.DeserializeObject<List<Player>>(playersData) : null;
        }

        private static string StringFromList(List<Player> playersData)
        {
            return JsonConvert.SerializeObject(playersData);
        }
    }
}