using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Data.SQLite;
namespace WindowTracking
{

    public class EventRecorder
    {
        class Event
        {
            public DateTime time_start;
            public DateTime time_end;
            public string name;
        }
        Event _stack = null;
        SQLiteDatabase _database;
        public EventRecorder(string pPath, string pDatabaseFilename) {
            //check if file exists
            if (System.IO.File.Exists(pPath + pDatabaseFilename)) {
                _database = new SQLiteDatabase(pPath + pDatabaseFilename);
                Console.WriteLine("opened old database");
            }
            else {
                if(!System.IO.Directory.Exists(pPath))
                    System.IO.Directory.CreateDirectory(pPath);
                SQLiteConnection.CreateFile(pPath + pDatabaseFilename);
                _database = new SQLiteDatabase(pPath + pDatabaseFilename);
                _database.ExecuteNonQuery(
                @"CREATE TABLE [events] (
                [id] INTEGER  NOT NULL PRIMARY KEY,
                [name] VARCHAR(64)  NULL,
                [time_start] INTEGER NOT NULL,
                [time_end] INTEGER NOT NULL)");
            }
        }
        public void SwapEvent(string pNameTag) {
            if (_stack != null && pNameTag == _stack.name)
                return; //event is allready running
            if (_stack != null)
                StoreRecord(_stack);
            _stack = new Event { time_start = DateTime.Now, time_end = DateTime.Now, name = pNameTag };
        }
        void StoreRecord(Event pEvent) {
            pEvent.time_end = DateTime.Now;
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("name", pEvent.name);
            data.Add("time_start", pEvent.time_start.ToUnixTime().ToString());
            data.Add("time_end", pEvent.time_end.ToUnixTime().ToString());
            _database.Insert("events", data);
        }
    }
}
