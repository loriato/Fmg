using Europa.Extensions;
using System;
using System.Text;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Commons;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class TokenAceiteService : BaseService
    {
        public TokenAceiteRepository _tokenAceiteRepository { get; set; }

        public TokenAceite CriarTokenPara(Usuario usuario, ContratoCorretagem contratoCorretagem)
        {

            var date = DateTime.Now;
            var tokensAntigos = _tokenAceiteRepository.TokenAtivoDe(usuario, contratoCorretagem);
            var tempoReenvio = ProjectProperties.TempoReenvioNovoToken.IsEmpty() ? 5 : ProjectProperties.TempoReenvioNovoToken;

            foreach (var token in tokensAntigos)
            {
                TimeSpan minutos = date.Subtract(token.CriadoEm);
                if (minutos.Minutes <= tempoReenvio)
                {
                    return token;
                }
                else
                {
                    token.Ativo = false;
                    _tokenAceiteRepository.Save(token);
                }

            }

            TokenAceite tokenAceite = new TokenAceite();
            tokenAceite.Usuario = usuario;
            tokenAceite.Token = TokenGenerator.NewToken();
            tokenAceite.Ativo = true;
            tokenAceite.ContratoCorretagem = contratoCorretagem;
            _tokenAceiteRepository.Save(tokenAceite);

            return tokenAceite;
        }

        public TokenAceite CriarTokenPara(Usuario usuario, RegraComissao regraComissao)
        {
            TokenAceite tokenAceite = new TokenAceite();
            tokenAceite.Usuario = usuario;
            tokenAceite.Token = TokenGenerator.NewToken();
            tokenAceite.Ativo = true;
            tokenAceite.RegraComissao = regraComissao;
            _tokenAceiteRepository.Save(tokenAceite);
            return tokenAceite;
        }

        public TokenAceite CriarTokenPara(Usuario usuario, RegraComissaoEvs regraComissaoEvs)
        {

            var date = DateTime.Now;
            var tokensAntigos = _tokenAceiteRepository.TokenAtivoDe(usuario, regraComissaoEvs);
            var tempoReenvio = ProjectProperties.TempoReenvioNovoToken.IsEmpty() ? 5 : ProjectProperties.TempoReenvioNovoToken;
            foreach (var token in tokensAntigos)
            {
                TimeSpan minutos = date.Subtract(token.CriadoEm);
                if (minutos.Minutes <= tempoReenvio)
                {
                    return token;
                }
                else
                {
                    token.Ativo = false;
                    _tokenAceiteRepository.Save(token);
                }

            }

            TokenAceite tokenAceite = new TokenAceite();
            tokenAceite.Usuario = usuario;
            tokenAceite.Token = TokenGenerator.NewToken();
            tokenAceite.Ativo = true;
            tokenAceite.RegraComissao = regraComissaoEvs.RegraComissao;
            _tokenAceiteRepository.Save(tokenAceite);
            return tokenAceite;
        }

        public int InvalidarTokens(int quantidadeHorasCorte)
        {
            var dataParametro = DateTime.Now.AddHours(-quantidadeHorasCorte);
            var queryString = new StringBuilder();
            queryString.Append(" DELETE TokenAceite toac ");
            queryString.Append(" WHERE toac.CriadoEm < :dataCorte ");
            queryString.Append(" AND toac.Ativo = :ativo");

            var query = _session.CreateQuery(queryString.ToString());
            query.SetParameter("dataCorte", dataParametro);
            query.SetParameter("ativo", true);

            return query.ExecuteUpdate();
        }
    }
}
