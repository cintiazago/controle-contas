using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ControleContasAPI.Util
{
    public class DAL
    {
        private static string Server = "servermysql55.mysql.database.azure.com";
        private static string Database = "controle_contas";
        private static string User = "cintiazago@servermysql55";
        private static string Password = "";
        private MySqlConnection Connection;

        private string ConnectionString = $"Server={Server};Database={Database};Uid={User};Pwd={Password};Sslmode=Required;charset=utf8";

        public DAL()
        {            
            Connection = new MySqlConnection(ConnectionString);
            Connection.Open();            
        }

        // Executa: UPDATE, INSERT, DELETE
        public void ExecutarComandoSQL(string sql)
        {
            MySqlCommand Command = new MySqlCommand(sql, Connection);
            Command.ExecuteNonQuery();
        }

        // Retorna Dados do banco
        public DataTable RetornaDataTable(string sql)
        {
            MySqlCommand Command = new MySqlCommand(sql, Connection);
            MySqlDataAdapter DataAdapter = new MySqlDataAdapter(Command);
            DataTable Dados = new DataTable();
            DataAdapter.Fill(Dados);
            return Dados;
        }
    }
}
