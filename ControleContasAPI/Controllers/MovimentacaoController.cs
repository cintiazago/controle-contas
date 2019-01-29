using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControleContasAPI.Models;
using ControleContasAPI.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControleContasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimentacaoController : ControllerBase
    {
        Autenticacao AutenticacaoServico;

        public MovimentacaoController(IHttpContextAccessor context)
        {
            AutenticacaoServico = new Autenticacao(context);
        }

        /// <summary>
        /// Efetua a transferencia de uma conta para outra.
        /// Tranferencias são permitidas apenas de uma Conta Matriz para Filiais, ou
        /// contas filiais para outras filiais, desde que estejam na mesma árvore de contas.       
        /// </summary>       
        /// <returns>JSON com resultado da operação.</returns>
        [HttpPost]
        [Route("transferir")]
        public ActionResult Transferir([FromBody] MovimentacaoModel dados)
        {
            ReturnAllServices retorno = new ReturnAllServices();

            try
            {
                AutenticacaoServico.Autenticar();
                ContaModel contaD = new ContaModel().GetConta(dados.Id_conta_debito);
                ContaModel contaC = new ContaModel().GetConta(dados.Id_conta_credito);

                if ((contaD.Tipo_conta == "F") && (contaC.Tipo_conta == "M"))
                {
                    retorno.Result = false;
                    retorno.ErrorMessage = "Não é permitido efetuar transferencia de Conta Filial para Conta Matriz.";
                    return StatusCode(403, retorno);
                }
                else if ((contaC.Tipo_conta == "M"))
                {
                    retorno.Result = false;
                    retorno.ErrorMessage = "Não é permitido efetuar transferencia para Conta Matriz. Realize um Aporte.";
                    return StatusCode(403, retorno);
                }
                else if (contaD.Saldo < dados.Valor)
                {
                    retorno.Result = false;
                    retorno.ErrorMessage = $"Saldo insuficiente na Conta." + contaD.Id;
                    return StatusCode(403, retorno);
                }
                else
                {
                    dados.Transferir();
                    retorno.Result = true;
                    retorno.ErrorMessage = "Transferencia realizada com sucesso.";
                }
            }
            catch (Exception ex)
            {
                retorno.Result = false;
                retorno.ErrorMessage = "Erro ao tentar efeturar transferencia: " + ex.Message;
                return StatusCode(500, retorno);
            }
            return StatusCode(200, retorno);
        }

        /// <summary>
        /// Realita o aporte para uma determinada Conta Matriz.
        /// </summary>
        /// <param name="id_conta">ID da conta a receber o aporte.</param>
        /// <returns>JSON com resultado da operação.</returns>
        [HttpPost]
        [Route("aporte/{id_conta}")]
        public ActionResult Aporte(int id_conta, [FromBody] MovimentacaoModel dados)
        {
            ReturnAllServices retorno = new ReturnAllServices();

            try
            {
                AutenticacaoServico.Autenticar();
                ContaModel conta_mov = new ContaModel().GetConta(id_conta);

                if (conta_mov.Tipo_conta != "M") {
                    retorno.Result = false;
                    retorno.ErrorMessage = "Aporte não pode ser realizado para conta Filial.";
                    return StatusCode(403, retorno);
                }
                else
                {
                    dados.RealizarMovimentacao(id_conta);
                    retorno.Result = true;
                    retorno.ErrorMessage = "Aporte realizado com sucesso.";
                }
            }
            catch (Exception ex)
            {
                retorno.Result = false;
                retorno.ErrorMessage = "Erro ao tentar realizar aporte: " + ex.Message;
                return StatusCode(500, retorno);
            }
            return StatusCode(200, retorno);
        }

        /// <summary>
        /// Estorna uma movimentação efetuada.        
        /// </summary>
        /// <param name="id">ID da movimentação</param>
        /// <returns>JSON com resultado da operação.</returns>
        [HttpPost]
        [Route("estornar/{id}")]
        public ActionResult Estornar(int id)
        {
            ReturnAllServices retorno = new ReturnAllServices();
            
            try
            {
                AutenticacaoServico.Autenticar();
                MovimentacaoModel.RealizarEstorno(id);
                retorno.Result = true;
                retorno.ErrorMessage = "Estorno realizado com sucesso.";                
            }
            catch (Exception ex)
            {
                retorno.Result = false;
                retorno.ErrorMessage = "Erro ao tentar realizar estorno: " + ex.Message;
                return StatusCode(500, retorno);
            }
            return StatusCode(200, retorno);
        }
    }
}