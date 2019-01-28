using ControleContasWeb.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ControleContasWeb.Models
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

        public List<ContaModel> ListarContas()
        {
            List<ContaModel> retorno = new List<ContaModel>();
            string json = WebAPI.RequestGET("conta/contas", string.Empty);

            return JsonConvert.DeserializeObject<List<ContaModel>>(json);

        }

        public ContaModel Carregar(int? id)
        {
            ContaModel retorno = new ContaModel();
            string json = WebAPI.RequestGET("conta/conta", id.ToString());

            return JsonConvert.DeserializeObject<ContaModel>(json);
        }

        public void Inserir()
        {
            string jsonData = JsonConvert.SerializeObject(this);

            if (Id == 0)
            {
                string json = WebAPI.RequestPOST("conta/criar", jsonData);
            }
            else
            {
                string json = WebAPI.RequestPUT("conta/atualizar/"+ Id, jsonData);
            }
        }

        public void Excluir(int id)
        {
            ContaModel retorno = new ContaModel();
            string json = WebAPI.RequestDELETE("conta/deletar", id.ToString());
        }
    }
}
