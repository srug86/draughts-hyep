using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Draughts.Domain
{
    class Player
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
        private int avatar;
        public int Avatar
        {
            get { return avatar; }
            set { avatar = value; }
        }
        private int ganadas;
        public int Ganadas
        {
            get { return ganadas; }
            set { ganadas = value; }
        }
        private int empatadas;
        public int Empatadas
        {
            get { return empatadas; }
            set { empatadas = value; }
        }
        private int perdidas;
        public int Perdidas
        {
            get { return perdidas; }
            set { perdidas = value; }
        }

        public Player(string name, string pwd, int avatar)
        {
            this.name = name;
            this.pwd = pwd;
            this.avatar = avatar;
            this.ganadas = 0;
            this.empatadas = 0;
            this.perdidas = 0;
        }
        
    }
}
