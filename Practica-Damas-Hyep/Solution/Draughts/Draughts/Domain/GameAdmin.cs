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
    public sealed class GameAdmin
    {
        static readonly GameAdmin instance = new GameAdmin();
        private Player pl = new Player("", "", "");
        public Player Pl
        {
            get { return pl; }
            set { pl = value; }
        }
        private int playerNumber = 0;

        public int PlayerNumber
        {
            get { return playerNumber; }
            set { playerNumber = value; }
        }
        private DBProxy db = DBProxy.Instance;

        static GameAdmin()
        {   
        }

        GameAdmin()
        {
        }

        public static GameAdmin Instance
        {
            get
            {
                return instance;
            }
        }

        public void insertPlayer(Player p)
        {
            string sentence = "INSERT INTO Players(name, pwd, avatar, wins, draws, loses) VALUES('" + p.Name + "','" + p.Pwd + "','" + p.Avatar + "'," + p.Wins + "," + p.Draws + "," + p.Loses + ")";
            db.connection();
            db.insert(sentence);
            db.disconnection();
        }

        public bool loginPlayer(String n, String ps)
        {
            bool val = false;
            string sentence = "SELECT * FROM Players WHERE name = '" + n + "'";
            db.connection();
            MySqlDataReader read = db.select(sentence);
            if (read.Read())
            {
                pl.Name = read.GetString("name");
                pl.Pwd = read.GetString("pwd");
                pl.Avatar = read.GetString("avatar");
                pl.Wins = read.GetInt32("wins");
                pl.Draws = read.GetInt32("draws");
                pl.Loses = read.GetInt32("loses");
            }
            db.disconnection();
            if (pl.Pwd.Equals(ps)) val = true;
            return val;
        }

        public bool existPlayer(String name)
        {
            bool var = false;
            string sentece = "SELECT name, pwd FROM Players WHERE name = '" + name + "'";
            db.connection();
            MySqlDataReader read = db.select(sentece);
            if ((read.Read()) == false) var = true;
            db.disconnection();
            return var;
        }

        public void updatePlayer()
        {
            string sentence = "UPDATE Players SET wins = '" + pl.Wins + "', draws = '" +
                pl.Draws + "', loses = '" + pl.Loses + "' WHERE name = '" + pl.Name + "'";
            db.conectar();
            db.insert(sentence);
            db.desconectar();
        }

        public ArrayList loadRanking()
        {
            ArrayList players = new ArrayList();
            string sentence = "SELECT * FROM Players ORDER BY wins DESC LIMIT 10 ";
            db.connection();
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
            db.disconnection();
            return players;
        }
    }
}
