using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class EmpreendimentoService : BaseService
    {
        private EmpreendimentoRepository _empreendimentoRepository { get; set; }
        private BreveLancamentoRepository _breveLancamentoRepository { get; set; }
        private EnderecoEmpreendimentoRepository _enderecoEmpreendimentoRepository { get; set; }
        private ViewEmpreendimentoExportacaoRepository _viewEmpreendimentoExportacaoRepository { get; set; }

        public Empreendimento Salvar(Empreendimento empreendimento, BusinessRuleException bre)
        {
            // Realiza as validações de Empreendimento
            var emprResult = new EmpreendimentoValidator(_empreendimentoRepository).Validate(empreendimento);

            // Verifica se retornou algum erro
            bre.WithFluentValidation(emprResult);

            if (emprResult.IsValid)
            {
                _empreendimentoRepository.Save(empreendimento);
            }

            return empreendimento;
        }

        public void ExcluirPorId(long idEmpreendimento)
        {
            var exc = new BusinessRuleException();
            var reg = _empreendimentoRepository.FindById(idEmpreendimento);
            var endereco = _enderecoEmpreendimentoRepository.FindByEmpreendimento(idEmpreendimento);

            try
            {
                _enderecoEmpreendimentoRepository.Delete(endereco);
                _empreendimentoRepository.Delete(reg);
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    exc.AddError(GlobalMessages.RemovidoSemSucesso).WithParam(reg.ChaveCandidata()).Complete();
                }
            }

            exc.ThrowIfHasError();
        }

        public DataSourceResponse<Empreendimento> ListarEmpreendimentos(DataSourceRequest request)
        {
            var query = _empreendimentoRepository.Queryable();
            if (request.filter != null && request.filter.Count > 0)
            {
                foreach (var filtro in request.filter)
                {
                    if (filtro.column.ToLower().Equals("nome"))
                    {
                        query = query.Where(x => x.Nome.ToLower().Contains(filtro.value.ToLower()));
                    }

                    if (filtro.column.ToLower().Equals("semassociacaobrevelancamento"))
                    {
                        var associados = _breveLancamentoRepository.Queryable()
                            .Where(x => x.Empreendimento != null)
                            .Select(x => x.Empreendimento.Id);
                        query = query.Where(x => !associados.Contains(x.Id));
                    }

                    if (filtro.column.ToLower().Equals("regional"))
                    {
                        var empreendimentosRegional = _enderecoEmpreendimentoRepository.EnderecosDaRegional(filtro.value).Select(x => x.Empreendimento.Id);
                        query = query.Where(x => empreendimentosRegional.Contains(x.Id));
                    }
                }
            }

            var result = query.Select(x => new Empreendimento()
            {
                Nome = x.Nome,
                Id = x.Id,
                CriadoEm = x.CriadoEm
            }).ToDataRequest(request);
            return result;
        }

        public byte[] Exportar(DataSourceRequest request, FiltroEmpreendimentoDTO dto)
        {
            var list = ListarView(request, dto);
            return GravarRegistrosExcel(list.records.ToList());
        }

        private byte[] GravarRegistrosExcel(List<ViewEmpreendimentoExportacao> empreendimentos)
        {
            var excel = ExcelUtil.NewInstance(20)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeader());

            foreach (var model in empreendimentos)
            {
                excel.CreateCellValue(model.Regional)
                    .CreateCellValue(model.CodigoEmpresa)
                    .CreateCellValue(model.Divisao)
                    .CreateCellValue(model.Nome)
                    .CreateCellValue(model.CNPJ)
                    .CreateCellValue(model.NomeEmpresa)
                    .CreateCellValue(model.RegistroIncorporacao)
                    .CreateCellValue(model.Cep)
                    .CreateCellValue(model.Logradouro)
                    .CreateCellValue(model.Numero)
                    .CreateCellValue(model.Complemento)
                    .CreateCellValue(model.Bairro)
                    .CreateCellValue(model.Cidade)
                    .CreateCellValue(model.Estado)
                    .CreateCellValue(model.Mancha)
                    .CreateDateTimeCell(model.DataLancamento)
                    .CreateDateTimeCell(model.PrevisaoEntrega)
                    .CreateDateTimeCell(model.DataEntrega);
            }

            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Regional,
                GlobalMessages.CodigoEmpresa,
                GlobalMessages.Divisao,
                GlobalMessages.NomeEmpreendimento,
                GlobalMessages.Cnpj,
                GlobalMessages.NomeEmpresa,
                GlobalMessages.RegistroIncorporacao,
                GlobalMessages.CEP,
                GlobalMessages.Rua,
                GlobalMessages.Numero,
                GlobalMessages.Complemento,
                GlobalMessages.Bairro,
                GlobalMessages.Cidade,
                GlobalMessages.Estado,
                GlobalMessages.Mancha,
                GlobalMessages.DataLancamento,
                GlobalMessages.PrevisaoEntrega,
                GlobalMessages.DataEntrega
            };

            return header.ToArray();
        }

        public DataSourceResponse<ViewEmpreendimentoExportacao> ListarView(DataSourceRequest request, FiltroEmpreendimentoDTO dto)
        {
            return _viewEmpreendimentoExportacaoRepository
                .ListarView(dto).ToDataRequest(request);
        }

        public int AlterarTipoModalidade(long[] ids, TipoModalidadeComissao novoTipo)
        {
            var num = 0;
            foreach (var id in ids)
            {

                var empreendimento = _empreendimentoRepository.FindById(id);
                if (empreendimento.ModalidadeComissao != novoTipo)
                {
                    num++;
                    empreendimento.ModalidadeComissao = novoTipo;
                    _empreendimentoRepository.Save(empreendimento);
                }
            }
            return num;
        }

        public int AlterarTipoModalidadeProgramaFidelidade(long[] ids, TipoModalidadeProgramaFidelidade novoTipo)
        {
            var num = 0;
            foreach (var id in ids)
            {

                var empreendimento = _empreendimentoRepository.FindById(id);
                if (empreendimento.ModalidadeProgramaFidelidade != novoTipo)
                {
                    num++;
                    empreendimento.ModalidadeProgramaFidelidade = novoTipo;
                    _empreendimentoRepository.Save(empreendimento);
                }
            }
            return num;
        }
    }
}