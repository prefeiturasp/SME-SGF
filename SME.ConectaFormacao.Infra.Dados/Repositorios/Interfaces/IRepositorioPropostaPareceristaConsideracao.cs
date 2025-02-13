using SME.ConectaFormacao.Dominio.Entidades;
using SME.ConectaFormacao.Dominio.Enumerados;
using SME.ConectaFormacao.Dominio.Repositorios;

namespace SME.ConectaFormacao.Infra.Dados.Repositorios.Interfaces
{
    public interface IRepositorioPropostaPareceristaConsideracao : IRepositorioBaseAuditavel<PropostaPareceristaConsideracao>
    {
        Task<IEnumerable<PropostaPareceristaConsideracao>> ObterPorPropostaIdECampo(long propostaId, CampoConsideracao campoConsideracao);
        Task<bool> ExistemConsideracoesPorParaceristaId(long pareceristaId);
    }
}