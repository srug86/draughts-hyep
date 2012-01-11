using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Draughts.Communications
{
    class DBProxy
    {
        const string server = "208.11.220.249";
        const string basedatos = "playershyep";
        const string user = "pirri";
        const string password = "123456";
        MySqlConnection connect;
        MySqlDataReader read;
        

        public DBProxy()
        {

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
