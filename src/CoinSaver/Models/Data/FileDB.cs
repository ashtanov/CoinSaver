using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CoinSaver.Models
{
    public class FileDB : IDataLayer
    {
        const string dbfilename = "db.fl";
        public Dictionary<string, List<Purchase>> _db;
        public FileDB()
        {
            _db = new Dictionary<string, List<Purchase>>();
            try
            {
                LoadDB();
            }
            catch
            {
                // ignored
            }
        }

        IEnumerable<Purchase> IDataLayer.GetSpendings(string name)
        {
            List<Purchase> pr;
            if (_db.TryGetValue(name, out pr))
                return pr;
            else
            {
                _db.Add(name, new List<Purchase>());
                return _db[name];
            }

        }

        async Task IDataLayer.SaveSpendingAsync(string name, Purchase spending)
        {
            List<Purchase> pr;
            if (_db.TryGetValue(name, out pr))
                pr.Add(spending);
            else
                _db.Add(name, new List<Purchase> { spending });
            await SaveDB();
        }

        private async Task SaveDB()
        {
            var sb = new System.Text.StringBuilder();
            foreach (var kvp in _db)
            {
                sb.AppendLine($"@@@{kvp.Key}@@@");
                foreach (var p in kvp.Value)
                    sb.AppendLine(p.ToString());
            }
            using (var fw = new System.IO.FileStream(dbfilename, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))
            using (var sw = new System.IO.StreamWriter(fw))
            {
                await sw.WriteAsync(sb.ToString());
            }
        }

        public void LoadDB()
        {
            using (var fw = new System.IO.FileStream(dbfilename, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read))
            using (var sw = new System.IO.StreamReader(fw))
            {
                string currentName = null;
                List<Purchase> lp = new List<Purchase>();
                while (!sw.EndOfStream)
                {
                    string currline = sw.ReadLine();
                    if (currline.Contains("@@@"))
                    {
                        if (currentName != null)
                            _db.Add(currentName, lp);
                        lp = new List<Purchase>();
                        currentName = currline.Substring(3, currline.Length - 6);
                    }
                    else
                        lp.Add(Purchase.Parse(currline));
                }
                if (currentName != null)
                    _db.Add(currentName, lp);
            }
        }
    }
}
