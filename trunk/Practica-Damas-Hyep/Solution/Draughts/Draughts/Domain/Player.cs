using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Draughts.Domain
{
    /// <summary>
    /// Clase jugador, la cual tiene los atributos: name (string), pwd (string), avatar (string), wins (int), draws (int) y loses (int).
    /// Además tiene sus correspondientes métodos de get y set, como los constructores.
    /// </summary>
    public class Player
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string pwd;
        public string Pwd
        {
            get { return pwd; }
            set { pwd = value; }
        }
        private string avatar;
        public string Avatar
        {
            get { return avatar; }
            set { avatar = value; }
        }
        private int wins;
        public int Wins
        {
            get { return wins; }
            set { wins = value; }
        }
        private int draws;
        public int Draws
        {
            get { return draws; }
            set { draws = value; }
        }
        private int loses;
        public int Loses
        {
            get { return loses; }
            set { loses = value; }
        }

        public Player(string name, string pwd, string avatar)
        {
            this.name = name;
            this.pwd = pwd;
            this.avatar = avatar;
            this.wins = 0;
            this.draws = 0;
            this.loses = 0;
        }
        
    }
}
