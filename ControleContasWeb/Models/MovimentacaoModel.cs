using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControleContasWeb.Models
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
    }
}
