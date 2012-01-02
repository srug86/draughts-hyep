using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Draughts.Presentation;
using MySql.Data.MySqlClient;

namespace Draughts.Domain
{
    public class GameActions
    {
        InitWin init;
        private Player pl;
        public Player Pl
        {
            get { return pl; }
            set { pl = value; }
        }
        public GameActions(InitWin init, Player pl)
        {
            this.pl = pl;
            this.init = init;
        }
        public void insertPlayer(Player p)
        {
            DBAccess db = new DBAccess("208.11.220.249", "playershyep", "pirri", "123456");
            db.conectar();
            db.insert("INSERT INTO Players(name, pwd, avatar, wins, draws, loses) VALUES('" + p.Name + "','" + p.Pwd + "','" + p.Avatar + "'," + p.Wins + "," + p.Draws + "," + p.Loses + ")");
            db.desconectar();
        }
    }
}
