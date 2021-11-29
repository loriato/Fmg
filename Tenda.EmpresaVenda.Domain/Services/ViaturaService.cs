using Europa.Fmg.Domain.Repository;
using System;
using Tenda.Domain.Core.Services;
using Tenda.Domain.Fmg.Models;
using Tenda.Domain.Security.Models;

namespace Europa.Fmg.Domain.Services
{
    public class ViaturaService : BaseService
    {
        private ViaturaUsuarioRepository _viaturaUsuarioRepository { get; set; }
        private ViaturaRepository _viaturaRepository { get; set; }
        public Viatura Salvar(Viatura viatura)
        {
            viatura.Placa = viatura.Placa.ToUpper();
            viatura.Situacao = Tenda.Domain.Fmg.Enums.SituacaoViatura.Ativo;
            _viaturaRepository.Save(viatura);
            return viatura;
        }

        public void Alocar(Usuario usuario, Viatura viatura)
        {
            var viaturaUsuario = new ViaturaUsuario();
            viaturaUsuario.Pedido = DateTime.Now;
            viaturaUsuario.QuilometragemAntigo = viatura.Quilometragem;
            viaturaUsuario.Usuario = usuario;
            viaturaUsuario.Viatura = viatura;
            _viaturaUsuarioRepository.Save(viaturaUsuario);

            viatura.Situacao = Tenda.Domain.Fmg.Enums.SituacaoViatura.EmPercurso;
            _viaturaRepository.Save(viatura);
        }
        public ViaturaUsuario Desalocar(Viatura viatura, long quilometragem)
        {
            var viaturaUsuario = _viaturaUsuarioRepository.BuscarEmAberto(viatura.Id);
            viaturaUsuario.Entrega = DateTime.Now;
            viaturaUsuario.QuilometragemNovo = quilometragem;
            _viaturaUsuarioRepository.Save(viaturaUsuario);

            viatura.Situacao = Tenda.Domain.Fmg.Enums.SituacaoViatura.Ativo;
            viatura.Quilometragem = quilometragem;
            _viaturaRepository.Save(viatura);
            return viaturaUsuario;
        }
    }
}
