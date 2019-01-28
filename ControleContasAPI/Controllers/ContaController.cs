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
    public class ContaController : ControllerBase
    {
        Autenticacao AutenticacaoServico;

        public ContaController(IHttpContextAccessor context)
        {
            AutenticacaoServico = new Autenticacao(context);
        }

        /// <summary>
        /// Recupera uma lista de todas as contas cadastradas.
        /// </summary>        
        /// <returns>Objeto contendo lista das contas</returns>
        [HttpGet]
        [Route("contas")]
        public List<ContaModel> contas()
        {
            AutenticacaoServico.Autenticar();
            return new ContaModel().GetContas();
        }

        /// <summary>
        /// Busca uma conta especifica.
        /// </summary>
        /// <param name="id">ID da conta pesquisada</param>
        /// <returns>Objeto contendo dados da conta cadastrada.</returns>
        [HttpGet]
        [Route("conta/{id}")]
        public ContaModel conta(int id)
        {
            AutenticacaoServico.Autenticar();
            return new ContaModel().GetConta(id);
        }

        /// <summary>
        /// Cria uma conta matriz ou filial.
        /// </summary>
        /// <returns>JSON com resultado da operação.</returns>
        [HttpPost]
        [Route("criar")]
        public ReturnAllServices Criar([FromBody] ContaModel dados)
        {
            ReturnAllServices retorno = new ReturnAllServices();
            try
            {
                AutenticacaoServico.Autenticar();
                dados.Criar();
                retorno.Result = true;
                retorno.ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                retorno.Result = false;
                retorno.ErrorMessage = "Erro ao criar: " + ex.Message;
            }
            return retorno;
        }

        /// <summary>
        /// Altera uma conta cadastrada.
        /// </summary>
        /// <param name="id">ID da conta a ser alterada.</param>
        /// <returns>JSON com resultado da operação.</returns>
        [HttpPut]
        [Route("atualizar/{id}")]
        public ReturnAllServices Atualizar(int id, [FromBody] ContaModel dados)
        {
            ReturnAllServices retorno = new ReturnAllServices();

            try
            {
                AutenticacaoServico.Autenticar();
                dados.Id = id;
                dados.Atualizar();
                retorno.Result = true;
                retorno.ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                retorno.Result = false;
                retorno.ErrorMessage = "Erro ao tentar atualizar um cliente: " + ex.Message;
            }
            return retorno;
        }

        /// <summary>
        /// Exclui uma conta cadastrada.
        /// </summary>
        /// <param name="id">ID da conta a ser deletada</param>
        /// <returns>JSON com resultado da operação.</returns>
        [HttpDelete]
        [Route("deletar/{id}")]
        public ReturnAllServices Deletar(int id)
        {
            ReturnAllServices retorno = new ReturnAllServices();

            try
            {
                AutenticacaoServico.Autenticar();
                retorno.Result = true;
                retorno.ErrorMessage = "Cliente excluido com sucesso.";
                new ContaModel().Deletar(id);
            }
            catch (Exception ex)
            {
                retorno.Result = false;
                retorno.ErrorMessage = ex.Message;
            }
            return retorno;
        }

        /// <summary>
        /// Busca todo o histórico da movimentação feita na conta pesquisada.
        /// </summary>
        /// <param name="id_conta">ID da conta relacionada ao histórico.</param>
        /// <returns>Objeto contendo lista de toda a movimentação.</returns>
        [HttpGet]
        [Route("historico-movimentacao/{id_conta}")]
        public List<MovimentacaoModel> HistoricoMovimetacao(int id_conta){

            return new ContaModel().GetHistoricoMovimentacao(id_conta);
        }

    }
}
