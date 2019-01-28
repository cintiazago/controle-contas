using ControleContasAPI.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ControleContasAPI.Models
{
    public class ContaModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Data_criacao { get; set; }
        public string Tipo_conta { get; set; }        
        public int Id_conta_matriz { get; set; }
        public double Saldo { get; set; }
        public int Id_pessoa { get; set; }
        public string Situacao { get; set; }

        public void Criar()
        {
            DAL objDAL = new DAL();

            string sql = "INSERT INTO conta (nome, data_criacao, tipo_conta, id_conta_matriz, saldo, id_pessoa, situacao)" +                                    
                         $"values('{Nome}', '{DateTime.Parse(Data_criacao).ToString("yyyy/MM/dd")}', '{Tipo_conta}', '{Id_conta_matriz}', " +
                         $"0, '{Id_pessoa}', 'A')";

            objDAL.ExecutarComandoSQL(sql);
        }

        public void Atualizar()
        {
            DAL objDAL = new DAL();

            string sql = "UPDATE conta SET " +
                         $"nome='{Nome}'" +
                         $"WHERE id={Id}";

            objDAL.ExecutarComandoSQL(sql);
        }

        public void Deletar(int id)
        {
            DAL objDAL = new DAL();

            string sql = $"delete from conta where id = {id}";

            objDAL.ExecutarComandoSQL(sql);
        }

        public List<ContaModel> GetContas()
        {
            List<ContaModel> lista = new List<ContaModel>();
            ContaModel item;

            DAL objDAL = new DAL();

            string sql = "select id, nome, data_criacao, tipo_conta, "+
                         "coalesce(id_conta_matriz, 0) as id_conta_matriz, saldo, "+
                         "id_pessoa, situacao from conta order by nome asc";

            DataTable dados = objDAL.RetornaDataTable(sql);

            for (int i = 0; i < dados.Rows.Count; i++)
            {
                item = new ContaModel()
                {
                    Id = int.Parse(dados.Rows[i]["id"].ToString()),
                    Nome = dados.Rows[i]["nome"].ToString(),
                    Data_criacao = DateTime.Parse(dados.Rows[i]["data_criacao"].ToString()).ToString("dd/MM/yyyy"),
                    Tipo_conta = dados.Rows[i]["tipo_conta"].ToString(),
                    Id_conta_matriz = int.Parse(dados.Rows[i]["id_conta_matriz"].ToString()),
                    Saldo = Double.Parse(dados.Rows[i]["saldo"].ToString()),
                    Id_pessoa = int.Parse(dados.Rows[i]["id_pessoa"].ToString()),
                    Situacao = dados.Rows[i]["situacao"].ToString()
                };
                lista.Add(item);
            }
            return lista;
        }

        public ContaModel GetConta(int id)
        {
            ContaModel item;
            DAL objDAL = new DAL();

            string sql = "select id, nome, data_criacao, tipo_conta, "+
                         "coalesce(id_conta_matriz, 0) as id_conta_matriz, saldo, "+
                         $"id_pessoa, situacao from conta where id = {id}";

            DataTable dados = objDAL.RetornaDataTable(sql);

            if (dados.Rows.Count == 0)
            {
                item = null;
            }
            else
            {
                item = new ContaModel()
                {
                    Id = int.Parse(dados.Rows[0]["id"].ToString()),
                    Nome = dados.Rows[0]["nome"].ToString(),
                    Data_criacao = DateTime.Parse(dados.Rows[0]["data_criacao"].ToString()).ToString("dd/MM/yyyy"),
                    Tipo_conta = dados.Rows[0]["tipo_conta"].ToString(),
                    Id_conta_matriz = int.Parse(dados.Rows[0]["id_conta_matriz"].ToString()),
                    Saldo = Double.Parse(dados.Rows[0]["saldo"].ToString()),
                    Id_pessoa = int.Parse(dados.Rows[0]["id_pessoa"].ToString()),
                    Situacao = dados.Rows[0]["situacao"].ToString(),
                };
            }
            return item;
        }

        public static void AtualizarSaldoConta(int id_conta, string tipo_operacao, double valor)
        {
            DAL objDAL = new DAL();
            string operacao = "+";

            if (tipo_operacao == "D") {
                operacao = "-";
            }
            string sql = "UPDATE conta SET " +
                         $"saldo=saldo {operacao} {valor} " +
                         $"WHERE id={id_conta}";

            objDAL.ExecutarComandoSQL(sql);
        }

        public List<MovimentacaoModel> GetHistoricoMovimentacao(int id_conta)
        {
            List<MovimentacaoModel> lista = new List<MovimentacaoModel>();
            MovimentacaoModel item;

            DAL objDAL = new DAL();

            string sql = "select id, tipo, valor, coalesce(id_conta_debito, 0) as id_conta_debito, " +
                         "coalesce(id_conta_credito, 0) as id_conta_credito, data_estorno, data_movimentacao " +
                         $"from movimentacao where id_conta_debito = {id_conta} or id_conta_credito = {id_conta}";

            DataTable dados = objDAL.RetornaDataTable(sql);

            for (int i = 0; i < dados.Rows.Count; i++)
            {
                string str_data_estorno;
                try
                {
                    str_data_estorno = DateTime.Parse(dados.Rows[0]["data_estorno"].ToString()).ToString("dd/MM/yyyy");
                }
                catch
                {
                    str_data_estorno = "";
                }
                item = new MovimentacaoModel()
                {
                    Id = int.Parse(dados.Rows[0]["id"].ToString()),
                    Tipo = dados.Rows[0]["tipo"].ToString(),
                    Valor = Double.Parse(dados.Rows[0]["valor"].ToString()),
                    Id_conta_debito = int.Parse(dados.Rows[0]["id_conta_debito"].ToString()),
                    Id_conta_credito = int.Parse(dados.Rows[0]["id_conta_credito"].ToString()),
                    Data_estorno = str_data_estorno,
                    Data_movimentacao = DateTime.Parse(dados.Rows[0]["Data_movimentacao"].ToString()).ToString("dd/MM/yyyy")
                };
                lista.Add(item);
            }
            return lista;
        }
    }
}
