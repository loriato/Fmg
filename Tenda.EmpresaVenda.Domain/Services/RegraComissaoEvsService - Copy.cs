using System;
using Europa.Extensions;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class RegraComissaoEvsService : BaseService
    {
        public ArquivoService _arquivoService { get; set; }
        public ArquivoRepository _arquivoRepository { get; set; }
        public RegraComissaoEvsRepository _regraComissaoEvsRepository { get; set; }
        private AceiteRegraComissaoEvsRepository _aceiteRegraComissaoEvsRepository { get; set; }

        public RegraComissaoEvsService()
        {
        }

        public RegraComissaoEvs SalvarArquivo(byte[] file, RegraComissaoEvs regraComissaoEvs,
            Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda)
        {
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName = $"RegraComissao_{regraComissaoEvs.RegraComissao.Regional}_{regraComissaoEvs.RegraComissao.Id}_{empresaVenda.NomeFantasia}_{date}.pdf";
            fileName = fileName.Trim();

            var arquivo = _arquivoService.CreateFile(file, fileName, fileName, "application/pdf", file.Length);

            Arquivo prevArquivo = regraComissaoEvs.Arquivo;

            if (regraComissaoEvs.IdArquivoDoubleCheck == 0 && regraComissaoEvs.RegraComissao.Situacao == SituacaoRegraComissao.Ativo)
            {
                regraComissaoEvs.ContentTypeDoubleCheck = arquivo.ContentType;
                regraComissaoEvs.HashDoubleCheck = arquivo.Hash;
                regraComissaoEvs.IdArquivoDoubleCheck = arquivo.Id;
                regraComissaoEvs.NomeDoubleCheck = arquivo.Nome;
            }

            regraComissaoEvs.Arquivo = arquivo;

            _regraComissaoEvsRepository.Save(regraComissaoEvs);

            if (prevArquivo.HasValue() && regraComissaoEvs.IdArquivoDoubleCheck != prevArquivo.Id)
            {
                _arquivoRepository.Delete(prevArquivo);
            }

            return regraComissaoEvs;

        }
        //JOB - 7
        public RegraComissaoEvs SalvarArquivoJob(byte[] file, RegraComissao regraComissao,
            Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda, RegraComissaoEvs regraComissaoEvs)
        {
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName = $"RegraComissao_{regraComissao.Regional}_{regraComissao.Id}_{empresaVenda.NomeFantasia}_{date}.pdf";
            fileName = fileName.Trim();

            var arquivo = _arquivoService.CreateFile(file, fileName, fileName, "application/pdf", file.Length);

            regraComissaoEvs.ContentTypeDoubleCheck = arquivo.ContentType;
            regraComissaoEvs.HashDoubleCheck = arquivo.Hash;
            regraComissaoEvs.IdArquivoDoubleCheck = arquivo.Id;
            regraComissaoEvs.NomeDoubleCheck = arquivo.Nome;

            regraComissaoEvs.Arquivo = arquivo;

            _regraComissaoEvsRepository.Save(regraComissaoEvs);
            
            return regraComissaoEvs;
        }
        //7
        public RegraComissaoEvs SalvarArquivo(byte[] file, RegraComissao regraComissao,
            Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda,RegraComissaoEvs regraComissaoEvs)
        {
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName = $"RegraComissao_{regraComissao.Regional}_{regraComissao.Id}_{empresaVenda.NomeFantasia}_{date}.pdf";
            fileName = fileName.Trim();

            var arquivo = _arquivoService.CreateFile(file, fileName, fileName, "application/pdf", file.Length);

            //var regraComissaoEvs = _regraComissaoEvsRepository.Buscar(empresaVenda.Id, regraComissao.Id);

            Arquivo prevArquivo = null;
            if (regraComissaoEvs.IsEmpty())
            {
                regraComissaoEvs = new RegraComissaoEvs
                {
                    RegraComissao = regraComissao,
                    EmpresaVenda = empresaVenda,
                    Descricao = regraComissao.Descricao,
                    Regional = regraComissao.Regional,
                    Situacao = regraComissao.Situacao,
                    Tipo = regraComissao.Tipo
                };
            }
            else
            {
                prevArquivo = regraComissaoEvs.Arquivo;
            }
            
            if (regraComissaoEvs.IdArquivoDoubleCheck == 0 && regraComissao.Situacao == SituacaoRegraComissao.Ativo)
            {
                regraComissaoEvs.ContentTypeDoubleCheck = arquivo.ContentType;
                regraComissaoEvs.HashDoubleCheck = arquivo.Hash;
                regraComissaoEvs.IdArquivoDoubleCheck = arquivo.Id;
                regraComissaoEvs.NomeDoubleCheck = arquivo.Nome;
            }

            regraComissaoEvs.Arquivo = arquivo;

            _regraComissaoEvsRepository.Save(regraComissaoEvs);

            if (prevArquivo.HasValue() && regraComissaoEvs.IdArquivoDoubleCheck != prevArquivo.Id)
            {
                _arquivoRepository.Delete(prevArquivo);
            }

            return regraComissaoEvs;
        }

        public RegraComissaoEvs RegraComissaoEvsVigente(long idEmpresaVenda)
        {
            return _regraComissaoEvsRepository.BuscarRegraEvsVigente(idEmpresaVenda);
        }

        public bool PossuiAceiteParaRegraComissaoEvsVigente(long idEmpresaVenda)
        {
            var regraAtivaEvs = RegraComissaoEvsVigente(idEmpresaVenda);

            if (regraAtivaEvs == null)
            {
                return true;
            }

            return _aceiteRegraComissaoEvsRepository.BuscarAceiteParaRegraEvsAndEmpresaVenda(regraAtivaEvs.Id, idEmpresaVenda);
        }

    }
}