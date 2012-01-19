using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Draughts.Communications
{
    /// <summary>
    /// Clase que accede a la base de datos de forma genérica para cualquier tipo de consulta SQL.
    /// Esta clase utiliza el patrón Singleton y el Proxy.
    /// </summary>
    public sealed class DBProxy
    {
        /// <summary>
        /// Instancia Singleton de la clase DBProxy.
        /// </summary>
        static readonly DBProxy instance = new DBProxy();
        /// <summary>
        /// Constante con el valor de la dirreción ip del servidor de la base de datos.
        /// </summary>
        const string server = "208.11.220.249";
        /// <summary>
        /// Constante con el nombre de la base de datos.
        /// </summary>
        const string database = "playershyep";
        /// <summary>
        /// Constante con el nombre del usuario para acceder a la base de datos.
        /// </summary>
        const string user = "pirri";
        /// <summary>
        /// Constante con la password para acceder a la base de datos.
        /// </summary>
        const string password = "123456";
        /// <summary>
        /// Tipo de variable para conectarse a través de MySql.
        /// </summary>
        MySqlConnection connect;
        /// <summary>
        /// Estructura de datos que almacena los resultados de una consulta SQL.
        /// </summary>
        MySqlDataReader read;

        /// <summary>
        /// Inicializa la clase <see cref="DBProxy"/>.
        /// </summary>
        static DBProxy()
        {
        }

        /// <summary>
        /// Impide la creación de una instancia por defecto de la clase <see cref="DBProxy"/>.
        /// </summary>
        DBProxy()
        {
        }

        /// <summary>
        /// Devuelve la instancia DBProxy.
        /// </summary>
        public static DBProxy Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Conexión a la base de datos.
        /// </summary>
        public void connection()
        {
            connect = new MySqlConnection("Server=" + server + ";Uid=" + user +
                ";database=" + database + ";use procedure bodies=False; Pwd=" +
                password);
            connect.Open();
        }
        /// <summary>
        /// Desconexión de la base de datos.
        /// </summary>
        public void disconnection()
        {
            if (read != null) read.Close();
            connect.Close();
        }
        /// <summary>
        /// Ejecuta una sentencia INSERT SQL.
        /// </summary>
        /// <param name="SQL">Sentencia SQL.</param>
        public void insert(string SQL)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = SQL;
            cmd.Connection = connect;
            cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// Ejecuta una sentencia SELECT SQL.
        /// </summary>
        /// <param name="SQL">Sentencia SQL.</param>
        /// <returns></returns>
        public MySqlDataReader select(string SQL)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = SQL;
            cmd.Connection = connect;
            read = cmd.ExecuteReader();
            return read;
        }
    }
}
