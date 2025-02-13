﻿using MediatR;
using SME.ConectaFormacao.Aplicacao.Dtos.Proposta;
using SME.ConectaFormacao.Aplicacao.Interfaces.Proposta;
using SME.ConectaFormacao.Dominio.Constantes;
using SME.ConectaFormacao.Dominio.Enumerados;
using SME.ConectaFormacao.Dominio.Excecoes;
using SME.ConectaFormacao.Dominio.Extensoes;
using SME.ConectaFormacao.Infra;

namespace SME.ConectaFormacao.Aplicacao.CasosDeUso.Proposta
{
    public class CasoDeUsoRecusarProposta : CasoDeUsoAbstrato, ICasoDeUsoRecusarProposta
    {
        public CasoDeUsoRecusarProposta(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(long propostaId, PropostaJustificativaDTO propostaJustificativa)
        {
            var proposta = await mediator.Send(new ObterPropostaPorIdQuery(propostaId));
            if (proposta.EhNulo() || proposta.Excluido)
                throw new NegocioException(MensagemNegocio.PROPOSTA_NAO_ENCONTRADA);

            var situacoes = new[] { SituacaoProposta.AguardandoAnaliseParecerPelaDF, SituacaoProposta.AguardandoValidacaoFinalPelaDF };
            if (!situacoes.Contains(proposta.Situacao))
                throw new NegocioException(MensagemNegocio.PROPOSTA_NAO_ESTA_COMO_AGUARDANDO_PARECER_DF);

            if (propostaJustificativa.Justificativa.NaoEstaPreenchido())
                throw new NegocioException(MensagemNegocio.JUSTIFICATIVA_NAO_INFORMADA);

            var situacao = SituacaoProposta.Recusada;

            await mediator.Send(new EnviarPropostaCommand(propostaId, situacao));
            await mediator.Send(new SalvarPropostaMovimentacaoCommand(propostaId, situacao, propostaJustificativa.Justificativa));

            var perfilLogado = await mediator.Send(ObterGrupoUsuarioLogadoQuery.Instancia());
            var ehAdminDF = perfilLogado.EhPerfilAdminDF();

            if (ehAdminDF)
                await mediator.Send(new PublicarNaFilaRabbitCommand(RotasRabbit.NotificarAreaPromotoraSobreValidacaoFinalPelaDF, proposta.Id));

            return true;
        }
    }
}
