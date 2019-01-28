using ControleContasAPI.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ControleContasAPI.Models
{
    public class MovimentacaoModel
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public double Valor { get; set; }
        public int Id_conta_debito { get; set; }
        public int Id_conta_credito { get; set; }       
        public string Data_estorno { get; set; }
        public string Data_movimentacao { get; set; }
                
        public MovimentacaoModel GetMovimentacao(int id)
        {
            MovimentacaoModel item;
            DAL objDAL = new DAL();

            string sql = "select id, tipo, valor, coalesce(id_conta_debito, 0) as id_conta_debito, "+
                         "coalesce(id_conta_credito, 0) as id_conta_credito, data_estorno, data_movimentacao "+
                         $"from movimentacao where id = {id}";
            DataTable dados = objDAL.RetornaDataTable(sql);

            if (dados.Rows.Count == 0)
            {
                item = null;
            }
            else
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
            }
            return item;
        }

        // Transferencia entre contas
        public void Transferir()
        {
            DAL objDAL = new DAL();            
            
            string sql = "INSERT INTO movimentacao (tipo, valor, id_conta_debito, id_conta_credito, data_estorno, data_movimentacao) " +
                         $"values('T', '{Valor}', '{Id_conta_debito}', '{Id_conta_credito}', null, " +
                         $"'{DateTime.Parse(Data_movimentacao).ToString("yyyy/MM/dd")}')";

            objDAL.ExecutarComandoSQL(sql);
            ContaModel.AtualizarSaldoConta(Id_conta_credito, "C", Valor);
            ContaModel.AtualizarSaldoConta(Id_conta_debito, "D", Valor);
        }

        // Movimentacao simples de Credito ou Debito        
        public void RealizarMovimentacao(int id_conta)
        {
            DAL objDAL = new DAL();
            string operacao;

            // A = Aporte, T = transferencia, D = Debito
            operacao = Tipo == "A" ? "C" : "D";

            string sql = "INSERT INTO movimentacao (tipo, valor, id_conta_debito, "+
                         "id_conta_credito, data_estorno, data_movimentacao) " +
                         $"values('{Tipo}', {Valor}, null, {id_conta}, null, " +
                         $"'{DateTime.Parse(Data_movimentacao).ToString("yyyy/MM/dd")}')";

            objDAL.ExecutarComandoSQL(sql);
            ContaModel.AtualizarSaldoConta(id_conta, operacao, Valor);            
        }

        // Estorno de movimentação
        public static void RealizarEstorno(int id_movimentacao)
        {      
            MovimentacaoModel movimentacao = new MovimentacaoModel().GetMovimentacao(id_movimentacao);

            // Transferencia
            if (movimentacao.Tipo == "T")
            {
                // Estorno credito
                ContaModel.AtualizarSaldoConta(movimentacao.Id_conta_credito, "D", movimentacao.Valor);

                // Estorno debito
                ContaModel.AtualizarSaldoConta(movimentacao.Id_conta_debito, "C", movimentacao.Valor);
            }
            else if (movimentacao.Tipo == "A" || movimentacao.Tipo == "C")
            {
                // Aporte ou Credito
                ContaModel.AtualizarSaldoConta(movimentacao.Id_conta_credito, "D", movimentacao.Valor);
            }
            else if (movimentacao.Tipo == "D")
            {
                // Debito
                ContaModel.AtualizarSaldoConta(movimentacao.Id_conta_debito, "C", movimentacao.Valor);
            }

            DAL objDAL = new DAL();

            string sql = "UPDATE movimentacao SET " +
                         $"data_estorno='{DateTime.Now.ToString("yyyy/MM/dd")}'" +
                         $"WHERE id={movimentacao.Id}";

            objDAL.ExecutarComandoSQL(sql);
        }
    }
}
