using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Rest;
using System.Collections.Generic;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.ApiService.Models.TiposDocumento;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.Domain.Core.Enums;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class TiposDocumentoService : BaseService
    {
        private TipoDocumentoRepository _tiposDocumentoRepository { get; set; } 
        private DocumentoProponenteRepository _documentoProponenteRepository { get; set; }
        private DocumentoRuleMachinePrePropostaRepository _documentoRuleMachinePrePropostaRepository { get; set; }
        private ViewTiposDocumentoProponenteRepository _viewTiposDocumentoProponenteRepository { get; set; }
        private TiposDocumentoValidator _tiposDocumentoValidator { get; set; }
        public DataSourceResponse<ViewTiposDocumento> Listar(FiltroTiposDocumentoDTO dto)
        {
            var result = _viewTiposDocumentoProponenteRepository.Listar(dto);
            return result;
        }

        public void Salvar(FiltroTiposDocumentoDTO dto)
        {
            var ae = new ApiException();
              var doc = ToDomain(dto);
            var validate = _tiposDocumentoValidator.Validate(doc);
            ae.WithFluentValidation(validate);
            ae.ThrowIfHasError();
            _tiposDocumentoRepository.Save(doc);

        }


        public void Alterar(FiltroTiposDocumentoDTO dto)
        {
            var ae = new ApiException();

            var doc = _tiposDocumentoRepository.FindById(dto.Id);
                if (Validate(dto))
                {
                    doc = ToDomain(dto, doc);
                    _tiposDocumentoRepository.Save(doc);
                }
                else
                {
                    ae.AddError(GlobalMessages.DocumentoVisibilidadeObrigatoriedade);
                }
                ae.ThrowIfHasError();
        }

        public void Excluir(FiltroTiposDocumentoDTO dto)
        {

            var ae = new ApiException();

            var doc = _tiposDocumentoRepository.FindById(dto.Id);
                if (ValidateExclusion(doc))
                {
                    _tiposDocumentoRepository.Delete(doc);
                }
                else
                {
                    ae.AddError(GlobalMessages.DocumentoNaoPodeSerDeletado);
                }
                ae.ThrowIfHasError();

        }

        private bool Validate(FiltroTiposDocumentoDTO dto)
        {
            var obrigatorioPortal = _documentoRuleMachinePrePropostaRepository.Queryable()
                                .Where(reg => reg.TipoDocumento.Id == dto.Id)    
                                .Where(reg => reg.ObrigatorioPortal == true)
                                .Any();

            var obrigatorioHouse = _documentoRuleMachinePrePropostaRepository.Queryable()
                    .Where(reg => reg.TipoDocumento.Id == dto.Id)
                    .Where(reg => reg.ObrigatorioHouse == true)
                    .Any();
            if (!dto.VisivelPortal && obrigatorioPortal)
            {
                return false;
            }
            if(!dto.VisivelLoja && obrigatorioHouse)
            {
                return false;
            }
            return true;
        }

        public bool ValidateExclusion(TipoDocumento documento)
        {
            var dpCount = _documentoProponenteRepository.Queryable()
                  .Where(reg => reg.TipoDocumento.Id == documento.Id)
                  .Any();
            var drmCount = _documentoRuleMachinePrePropostaRepository.Queryable()
                                .Where(reg => reg.TipoDocumento.Id == documento.Id)
                                .Where(reg => reg.ObrigatorioHouse == true || reg.ObrigatorioPortal == true)
                                .Any();
            if(dpCount || drmCount)
            {
                return false;
            }

            return true;
        }
        private TipoDocumento ToDomain(FiltroTiposDocumentoDTO documento, TipoDocumento doc = null)
        {
            if (doc.HasValue())
            {
                doc.Id = documento.Id;
                doc.VisivelPortal = documento.VisivelPortal;
                doc.VisivelLoja = documento.VisivelLoja;
                return doc;

            }
            else
            {
                var doc2 = new TipoDocumento();
                doc2.Nome = documento.Nome;
                doc2.VisivelPortal = documento.VisivelPortal;
                doc2.VisivelLoja = documento.VisivelLoja;
                doc2.Situacao = Situacao.Ativo;
                return doc2;
             }


        }
    }
}
