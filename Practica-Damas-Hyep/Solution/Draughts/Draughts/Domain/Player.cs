using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Draughts.Domain
{
    /// <summary>
    /// Clase Jugador de una partida de damas.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Nombre del jugador.
        /// </summary>
        private string name;
        /// <summary>
        /// Devuelve o modifica el nombre.
        /// </summary>
        /// <value>
        /// Nombre.
        /// </value>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        /// Contraseña del jugador.
        /// </summary>
        private string pwd;
        /// <summary>
        /// Devuelve o modifica la contraseña.
        /// </summary>
        /// <value>
        /// Contraseña.
        /// </value>
        public string Pwd
        {
            get { return pwd; }
            set { pwd = value; }
        }
        /// <summary>
        /// Avatar del jugador.
        /// </summary>
        private string avatar;
        /// <summary>
        /// Devuelve o modifica el avatar.
        /// </summary>
        /// <value>
        /// Avatar.
        /// </value>
        public string Avatar
        {
            get { return avatar; }
            set { avatar = value; }
        }
        /// <summary>
        /// Victorias del jugador
        /// </summary>
        private int wins;
        /// <summary>
        /// Devuelve o modifica las victorias.
        /// </summary>
        /// <value>
        /// Victorias.
        /// </value>
        public int Wins
        {
            get { return wins; }
            set { wins = value; }
        }
        /// <summary>
        /// Empates del jugador.
        /// </summary>
        private int draws;
        /// <summary>
        /// Devuelve o modifica los empates del jugador.
        /// </summary>
        /// <value>
        /// Empates.
        /// </value>
        public int Draws
        {
            get { return draws; }
            set { draws = value; }
        }
        /// <summary>
        /// Derrotas del jugador.
        /// </summary>
        private int loses;
        /// <summary>
        /// Devuelve o modifica las derrotas.
        /// </summary>
        /// <value>
        /// Derrotas.
        /// </value>
        public int Loses
        {
            get { return loses; }
            set { loses = value; }
        }

        /// <summary>
        /// Constructor de la clase <see cref="Player"/>.
        /// </summary>
        /// <param name="name">Nombre.</param>
        /// <param name="pwd">Contraseña.</param>
        /// <param name="avatar">Avatar.</param>
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
