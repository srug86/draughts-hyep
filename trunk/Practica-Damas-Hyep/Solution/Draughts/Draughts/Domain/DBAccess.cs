using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Draughts.Domain
{
    public class DBAccess
    {
        string server;
        string basedatos;
        string user;
        string password;
        MySqlConnection connect;
        MySqlDataReader read;

        public DBAccess(string sv, string bd, string us, string pw)
        {
            server = sv;
            basedatos = bd;
            user = us;
            password = pw;
        }
        public void conectar()
        {
            connect = new MySqlConnection("Server=" + server + ";Uid=" + user +
                ";database=" + basedatos + ";use procedure bodies=False; Pwd=" +
                password);
            connect.Open();
        }
        public void desconectar()
        {
            if (read != null) read.Close();
            connect.Close();
        }
        public void insert(string SQL)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = SQL;
            cmd.Connection = connect;
            cmd.ExecuteNonQuery();
        }
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
