using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Draughts.Presentation;
using MySql.Data.MySqlClient;
using Draughts.Communications;

namespace Draughts.Domain
{
    public class GameAdmin
    {
        InitWin init;
        DBProxy db;
        private Player pl;
        public Player Pl
        {
            get { return pl; }
            set { pl = value; }
        }
        public GameAdmin(InitWin init)
        {
            this.init = init;
            this.db = DBProxy.Instance;
        }
        public void insertPlayer(Player p)
        {
            string sentence = "INSERT INTO Players(name, pwd, avatar, wins, draws, loses) VALUES('" + p.Name + "','" + p.Pwd + "','" + p.Avatar + "'," + p.Wins + "," + p.Draws + "," + p.Loses + ")";
            db.conectar();
            db.insert(sentence);
            db.desconectar();
        }
        public bool loginPlayer(String n, String ps)
        {
            bool val = false;
            string pw = "";
            string sentence = "SELECT name, pwd FROM Players WHERE name = '" + n + "'";
            db.conectar();
            MySqlDataReader read = db.select(sentence);
            if (read.Read())
            {
                pw = read.GetString("pwd");
            }
            db.desconectar();
            if (pw.Equals(ps)) val = true;
            return val;
        }
    }
}
