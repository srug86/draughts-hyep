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
        /// <summary>
        /// Instancia de la propia de clase.
        /// </summary>
        static readonly GameAdmin instance = new GameAdmin();
        /// <summary>
        /// Jugador que accede a esas base de datos.
        /// </summary>
        private Player pl = new Player("", "", "");
        /// <summary>
        /// Devuelve o modifica el jugador.
        /// </summary>
        /// <value>
        /// Player.
        /// </value>
        public Player Pl
        {
            get { return pl; }
            set { pl = value; }
        }
        /// <summary>
        /// Saber si el jugador es 1 o 2.
        /// </summary>
        private int playerNumber = 0;

        /// <summary>
        /// Devuelve o modifica el número del jugador.
        /// </summary>
        /// <value>
        /// Número jugador.
        /// </value>
        public int PlayerNumber
        {
            get { return playerNumber; }
            set { playerNumber = value; }
        }
        /// <summary>
        /// Instancia de la clase DBProxy.
        /// </summary>
        private DBProxy db = DBProxy.Instance;

        /// <summary>
        /// Inicializa la clase <see cref="GameAdmin"/>.
        /// </summary>
        static GameAdmin()
        {   
        }

        /// <summary>
        /// Impide la creación de una instancia por defecto de la clase <see cref="GameAdmin"/>.
        /// </summary>
        GameAdmin()
        {
        }

        /// <summary>
        /// Devuelve la instancia de GameAdmin.
        /// </summary>
        public static GameAdmin Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Insertar un jugador en la base de datos.
        /// </summary>
        /// <param name="p">Jugador.</param>
        public void insertPlayer(Player p)
        {
            string sentence = "INSERT INTO Players(name, pwd, avatar, wins, draws, loses) VALUES('" + p.Name + "','" + p.Pwd + "','" + p.Avatar + "'," + p.Wins + "," + p.Draws + "," + p.Loses + ")";
            db.connection();
            db.insert(sentence);
            db.disconnection();
        }

        /// <summary>
        /// Login del jugador en la aplicación.
        /// </summary>
        /// <param name="n">Nombre.</param>
        /// <param name="ps">Contraseña.</param>
        /// <returns>Verdadero o falso.</returns>
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

        /// <summary>
        /// Saber si existe un jugador en la base de datos.
        /// </summary>
        /// <param name="name">Nombre.</param>
        /// <returns>Verdadero o falso.</returns>
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

        /// <summary>
        /// Actualiza las victorias,empates y derrotas de un jugador.
        /// </summary>
        public void updatePlayer()
        {
            string sentence = "UPDATE Players SET wins = '" + pl.Wins + "', draws = '" +
                pl.Draws + "', loses = '" + pl.Loses + "' WHERE name = '" + pl.Name + "'";
            db.connection();
            db.insert(sentence);
            db.disconnection();
        }

        /// <summary>
        /// Devuelve el ranking actual de la base de datos.
        /// </summary>
        /// <returns>Lista Players</returns>
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
