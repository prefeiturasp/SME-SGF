﻿using SME.ConectaFormacao.Dominio.Entidades;
using SME.ConectaFormacao.Dominio.Enumerados;
using SME.ConectaFormacao.Dominio.Repositorios;

namespace SME.ConectaFormacao.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioUsuario : IRepositorioBaseAuditavel<Usuario>
    {
        Task<Usuario> ObterPorLogin(string login);
        public Task AtivarCadastroUsuario(long usuarioId);
        Task<Usuario> ObterPorCpf(string cpf);
        Task<bool> AtualizarEmailEducacional(string login, string email);
        Task<bool> AtualizarTipoEmail(string login, int tipo);
        Task<(int tipo, string? email)> ObterEmailEducacionalPorLogin(string login);
        Task<IEnumerable<Usuario>> ObterUsuarioInternoPorId(long[] ids);
        Task<bool> UsuarioPossuiPropostaCadastrada(string login);
        Task<int> ObterTotalUsuarioRedeParceria(long[] areaPromotoraIds, string? nome, string? cpf, SituacaoUsuario? situacao);
        Task<IEnumerable<Usuario>> ObterUsuarioRedeParceria(long[] areaPromotoraIds, string? nome, string? cpf, SituacaoUsuario? situacao, int numeroPagina, int numeroRegistros);
    }
}
