﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControleContasAPI.Util
{
    public class Autenticacao
    {
        public static string TOKEN = "AppControleConta2019@#%";
        public static string FALHA_AUTENTICACAO = "Falha na autenticação... O token informado é inválido!";
        IHttpContextAccessor contextAccessor;

        public Autenticacao(IHttpContextAccessor context)
        {
            contextAccessor = context;
        }

        public void Autenticar()
        {
            try
            {
                string TokenRecebido = contextAccessor.HttpContext.Request.Headers["Token"].ToString();

                if (String.Equals(TOKEN, TokenRecebido) == false)
                {
                    throw new Exception(FALHA_AUTENTICACAO);
                }
            }
            catch
            {
                throw new Exception(FALHA_AUTENTICACAO);
            }
        }
    }
}
