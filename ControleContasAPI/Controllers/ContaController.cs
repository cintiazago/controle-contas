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
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult<List<ContaModel>> contas()
        {
            ReturnAllServices retorno = new ReturnAllServices();
            try
            {
                if (AutenticacaoServico.Autenticar())
                {
                    return new OkObjectResult(new ContaModel().GetContas());
                }
                else
                {
                    return new ForbidResult();
                }
            }
            catch (Exception ex)
            {
                retorno.Result = false;
                retorno.ErrorMessage = "Erro ao criar: " + ex.Message;
                return StatusCode(500, retorno);
            }
        }

        /// <summary>
        /// Busca uma conta especifica.
        /// </summary>
        /// <param name="id">ID da conta pesquisada</param>
        /// <returns>Objeto contendo dados da conta cadastrada.</returns>
        [HttpGet]
        [Route("conta/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        public ActionResult<ContaModel> conta(int id)
        {
            if (AutenticacaoServico.Autenticar())
            {
                return new OkObjectResult( new ContaModel().GetConta(id));
            }
            else
            {
                return new ForbidResult();
            }
        }

        /// <summary>
        /// Cria uma conta matriz ou filial.
        /// </summary>
        /// <returns>JSON com resultado da operação.</returns>
        [HttpPost]
        [Route("criar")]
        [ProducesResponseType(201)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult Criar([FromBody] ContaModel dados)
        {
            ReturnAllServices retorno = new ReturnAllServices();
            try
            {
                if (AutenticacaoServico.Autenticar())
                {
                    dados.Criar();
                    retorno.Result = true;
                    retorno.ErrorMessage = string.Empty;
                    return new OkObjectResult(retorno);
                }
                else
                {
                    return new ForbidResult();
                }
            }
            catch (Exception ex)
            {
                retorno.Result = false;
                retorno.ErrorMessage = "Erro ao criar: " + ex.Message;
                return StatusCode(500, retorno);
            }
        }

        /// <summary>
        /// Altera uma conta cadastrada.
        /// </summary>
        /// <param name="id">ID da conta a ser alterada.</param>
        /// <returns>JSON com resultado da operação.</returns>
        [HttpPut]
        [Route("atualizar/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult Atualizar(int id, [FromBody] ContaModel dados)
        {
            ReturnAllServices retorno = new ReturnAllServices();

            try
            {
                if (AutenticacaoServico.Autenticar())
                {
                    dados.Id = id;
                    dados.Atualizar();
                    retorno.Result = true;
                    retorno.ErrorMessage = string.Empty;
                    return new OkObjectResult(retorno);
                }
                else
                {
                    return new ForbidResult();
                }
            }
            catch (Exception ex)
            {
                retorno.Result = false;
                retorno.ErrorMessage = "Erro ao tentar atualizar um cliente: " + ex.Message;
                return StatusCode(500, retorno);
            }
        }

        /// <summary>
        /// Exclui uma conta cadastrada.
        /// </summary>
        /// <param name="id">ID da conta a ser deletada</param>
        /// <returns>JSON com resultado da operação.</returns>
        [HttpDelete]
        [Route("deletar/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public ActionResult Deletar(int id)
        {
            ReturnAllServices retorno = new ReturnAllServices();

            try
            {
                if (AutenticacaoServico.Autenticar())
                {
                    retorno.Result = true;
                    retorno.ErrorMessage = "Cliente excluido com sucesso.";
                    new ContaModel().Deletar(id);
                    return new OkResult();
                }
                else
                {
                    return new ForbidResult();
                }
            }
            catch (Exception ex)
            {
                retorno.Result = false;
                retorno.ErrorMessage = ex.Message;
                return StatusCode(500, retorno);
            }
        }

        /// <summary>
        /// Busca todo o histórico da movimentação feita na conta pesquisada.
        /// </summary>
        /// <param name="id_conta">ID da conta relacionada ao histórico.</param>
        /// <returns>Objeto contendo lista de toda a movimentação.</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Route("historico-movimentacao/{id_conta}")]
        public ActionResult<List<MovimentacaoModel>> HistoricoMovimetacao(int id_conta){

            try
            {
                if (AutenticacaoServico.Autenticar())
                {
                    return new OkObjectResult(new ContaModel().GetHistoricoMovimentacao(id_conta));
                }
                else
                {
                    return new ForbidResult();
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
