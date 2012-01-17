using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Draughts.Presentation;
using MySql.Data.MySqlClient;
using Draughts.Communications;
using System.Data;
using System.Collections;

namespace Draughts.Domain
{
    /// <summary>
    /// Implementa la gestión de jugadores de la base de datos.
    /// </summary>
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

        public bool existPlayer(String name)
        {
            bool var = false;
            string sentece = "SELECT name, pwd FROM Players WHERE name = '" + name + "'";
            db.conectar();
            MySqlDataReader read = db.select(sentece);
            if ((read.Read()) == false) var = true;
            db.desconectar();
            return var;
        }

        public ArrayList loadRanking()
        {
            ArrayList players = new ArrayList();
            string sentence = "SELECT * FROM Players ORDER BY wins DESC LIMIT 10 ";
            db.conectar();
            MySqlDataReader read = db.select(sentence);
            while (read.Read())
            {
                Player aux = new Player("", "", "");
                aux.Name = read.GetString("name");
                aux.Pwd = read.GetString("pwd");
                aux.Avatar = read.GetString("avatar");
                aux.Wins = read.GetInt32("wins");
                aux.Draws = read.GetInt32("draws");
                aux.Loses = read.GetInt32("loses");
                players.Add(aux);
            }
            db.desconectar();
            return players;
        }
    }
}
