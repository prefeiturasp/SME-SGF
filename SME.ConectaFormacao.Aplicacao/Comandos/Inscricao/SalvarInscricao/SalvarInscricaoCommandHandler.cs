﻿using AutoMapper;
using MediatR;
using SME.ConectaFormacao.Aplicacao.Dtos.Proposta;
using SME.ConectaFormacao.Dominio.Constantes;
using SME.ConectaFormacao.Dominio.Entidades;
using SME.ConectaFormacao.Dominio.Enumerados;
using SME.ConectaFormacao.Dominio.Excecoes;
using SME.ConectaFormacao.Dominio.Extensoes;
using SME.ConectaFormacao.Infra.Dados;
using SME.ConectaFormacao.Infra.Dados.Repositorios.Interfaces;
using SME.ConectaFormacao.Infra.Servicos.Eol;

namespace SME.ConectaFormacao.Aplicacao
{
    public class SalvarInscricaoCommandHandler : IRequestHandler<SalvarInscricaoCommand, RetornoDTO>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IRepositorioInscricao _repositorioInscricao;
        private readonly ITransacao _transacao;

        public SalvarInscricaoCommandHandler(IMapper mapper, IMediator mediator, IRepositorioInscricao repositorioInscricao, ITransacao transacao)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _repositorioInscricao = repositorioInscricao ?? throw new ArgumentNullException(nameof(repositorioInscricao));
            _transacao = transacao ?? throw new ArgumentNullException(nameof(transacao));
        }

        public async Task<RetornoDTO> Handle(SalvarInscricaoCommand request, CancellationToken cancellationToken)
        {
            var usuarioLogado = await _mediator.Send(ObterUsuarioLogadoQuery.Instancia(), cancellationToken) ??
                throw new NegocioException(MensagemNegocio.USUARIO_NAO_ENCONTRADO);


            if (usuarioLogado.Tipo.EhInterno() && request.InscricaoDTO.CargoCodigo.NaoEstaPreenchido())
                throw new NegocioException(MensagemNegocio.INFORME_O_CARGO);

            var inscricao = _mapper.Map<Inscricao>(request.InscricaoDTO);
            inscricao.UsuarioId = usuarioLogado.Id;
            inscricao.Situacao = SituacaoInscricao.AguardandoAnalise;
            inscricao.Origem = OrigemInscricao.Manual;

            await MapearCargoFuncao(inscricao, cancellationToken);

            var propostaTurma = await _mediator.Send(new ObterPropostaTurmaPorIdQuery(inscricao.PropostaTurmaId), cancellationToken) ??
                                throw new NegocioException(MensagemNegocio.TURMA_NAO_ENCONTRADA);

            await ValidarExisteInscricaoNaProposta(propostaTurma.PropostaId, inscricao.UsuarioId);

            if (usuarioLogado.Tipo == TipoUsuario.Interno)
            {
                await ValidarCargoFuncao(propostaTurma.PropostaId, inscricao.CargoId, inscricao.FuncaoId, cancellationToken);

                await ValidarDreUsuarioInterno(usuarioLogado.Login, inscricao, cancellationToken);
            }
            else
                await ValidarDreUsuarioExterno(inscricao.PropostaTurmaId, usuarioLogado.CodigoEolUnidade, cancellationToken);

            var proposta = await _mediator.Send(new ObterPropostaPorIdQuery(propostaTurma.PropostaId), cancellationToken) ??
                throw new NegocioException(MensagemNegocio.PROPOSTA_NAO_ENCONTRADA);

            return await PersistirInscricao(proposta.FormacaoHomologada == FormacaoHomologada.Sim, inscricao, proposta.IntegrarNoSGA);
        }

        private async Task MapearCargoFuncao(Inscricao inscricao, CancellationToken cancellationToken)
        {
            var codigosFuncoesEol = inscricao.FuncaoCodigo.EstaPreenchido() ? new List<long> { long.Parse(inscricao.FuncaoCodigo) } : Enumerable.Empty<long>();
            var codigosCargosEol = inscricao.CargoCodigo.EstaPreenchido() ? new List<long> { long.Parse(inscricao.CargoCodigo) } : Enumerable.Empty<long>();
            if (codigosFuncoesEol.PossuiElementos() || codigosCargosEol.PossuiElementos())
            {
                var cargosFuncoes = await _mediator.Send(new ObterCargoFuncaoPorCodigoEolQuery(codigosCargosEol, codigosFuncoesEol), cancellationToken);

                inscricao.CargoId = cargosFuncoes.FirstOrDefault(f => f.Tipo == CargoFuncaoTipo.Cargo)?.Id;
                inscricao.FuncaoId = cargosFuncoes.FirstOrDefault(f => f.Tipo == CargoFuncaoTipo.Funcao)?.Id;
            }
        }

        private async Task ValidarEmail(string login, string emailUsuario, string novoEmail, CancellationToken cancellationToken)
        {
            if (emailUsuario != novoEmail)
            {
                var emailValidar = novoEmail.ToLower().Trim();

                if (!emailValidar.EmailEhValido())
                    throw new NegocioException(string.Format(MensagemNegocio.EMAIL_INVALIDO, emailValidar));

                await _mediator.Send(new AlterarEmailServicoAcessosCommand(login, emailValidar), cancellationToken);
            }
        }

        private async Task ValidarExisteInscricaoNaProposta(long propostaId, long usuarioId)
        {
            var possuiInscricaoNaProposta = await _repositorioInscricao.UsuarioEstaInscritoNaProposta(propostaId, usuarioId);
            if (possuiInscricaoNaProposta)
                throw new NegocioException(MensagemNegocio.USUARIO_JA_INSCRITO_NA_PROPOSTA);
        }

        private async Task ValidarCargoFuncao(long propostaId, long? cargoId, long? funcaoId, CancellationToken cancellationToken)
        {
            var temErroCargo = false;
            var temErroFuncao = false;
            var cargosProposta = await _mediator.Send(new ObterPropostaPublicosAlvosPorIdQuery(propostaId), cancellationToken);
            var funcaoAtividadeProposta = await _mediator.Send(new ObterPropostaFuncoesEspecificasPorIdQuery(propostaId), cancellationToken);

            if (cargosProposta.PossuiElementos())
            {
                var cargoFuncaoOutros = await _mediator.Send(ObterCargoFuncaoOutrosQuery.Instancia(), cancellationToken);
                var cargoEhOutros = cargosProposta.Any(t => t.CargoFuncaoId == cargoFuncaoOutros.Id);

                if (cargoId.HasValue && !cargoEhOutros && !cargosProposta.Any(a => a.CargoFuncaoId == cargoId))
                    temErroCargo = true;

            }

            if (funcaoAtividadeProposta.PossuiElementos())
            {
                if (funcaoId.HasValue && !funcaoAtividadeProposta.Any(a => a.CargoFuncaoId == funcaoId))
                    temErroFuncao = true;
            }

            if (temErroCargo && temErroFuncao)
                throw new NegocioException(MensagemNegocio.USUARIO_NAO_POSSUI_CARGO_PUBLI_ALVO_FORMACAO);

            if (!funcaoAtividadeProposta.PossuiElementos() && temErroCargo)
                throw new NegocioException(MensagemNegocio.USUARIO_NAO_POSSUI_CARGO_PUBLI_ALVO_FORMACAO);
        }

        private async Task ValidarDreUsuarioInterno(string registroFuncional, Inscricao inscricao, CancellationToken cancellationToken)
        {
            var dres = await _mediator.Send(new ObterPropostaTurmaDresPorPropostaTurmaIdQuery(inscricao.PropostaTurmaId), cancellationToken);
            dres = dres.Where(t => !t.Dre.Todos);
            if (dres.PossuiElementos())
            {
                var dreUeAtribuicoes = await _mediator.Send(new ObterDreUeAtribuicaoPorRegistroFuncionalCodigoCargoQuery(registroFuncional, inscricao.CargoCodigo), cancellationToken);
                if (dreUeAtribuicoes.PossuiElementos())
                {
                    var dreUeAtribuicao = dreUeAtribuicoes.FirstOrDefault(f => dres.Any(d => d.DreCodigo == f.DreCodigo));
                    if (dreUeAtribuicao.EhNulo())
                        dreUeAtribuicao = dreUeAtribuicoes.FirstOrDefault();

                    inscricao.CargoDreCodigo = dreUeAtribuicao.DreCodigo;
                    inscricao.CargoUeCodigo = dreUeAtribuicao.UeCodigo;
                }

                if ((inscricao.CargoDreCodigo.EstaPreenchido() && !dres.Any(a => a.Dre.Codigo == inscricao.CargoDreCodigo)) ||
                    (inscricao.FuncaoDreCodigo.EstaPreenchido() && !dres.Any(a => a.Dre.Codigo == inscricao.FuncaoDreCodigo)))
                    throw new NegocioException(MensagemNegocio.USUARIO_SEM_LOTACAO_NA_DRE_DA_TURMA);
            }
        }

        private async Task ValidarDreUsuarioExterno(long propostaTurmaId, string codigoEolUnidade, CancellationToken cancellationToken)
        {
            var dres = await _mediator.Send(new ObterPropostaTurmaDresPorPropostaTurmaIdQuery(propostaTurmaId), cancellationToken);
            dres = dres.Where(t => !t.Dre.Todos);
            if (dres.PossuiElementos())
            {
                var unidade = await _mediator.Send(new ObterUnidadePorCodigoEOLQuery(codigoEolUnidade), cancellationToken);
                var codigosUndAdmReferencia = ObterCodigoUnidadeAdmReferenciaEscola(unidade) 
                                              ?? ObterCodigosUnidadeAdmEReferencia(unidade);

                if (!dres.Any(t => codigosUndAdmReferencia.Contains(t.Dre.Codigo)))
                    throw new NegocioException(MensagemNegocio.USUARIO_SEM_LOTACAO_NA_DRE_DA_TURMA);
            }
        }

        private string[]? ObterCodigoUnidadeAdmReferenciaEscola(UnidadeEol unidade)
            => unidade.Tipo == Infra.Servicos.Eol.UnidadeEolTipo.Escola
               ? new string[] { unidade.CodigoReferencia } 
               : null;

        private string[] ObterCodigosUnidadeAdmEReferencia(UnidadeEol unidade)
            => new string[] { unidade.Codigo, unidade.CodigoReferencia };

        private async Task<RetornoDTO> PersistirInscricao(bool formacaoHomologada, Inscricao inscricao, bool integrarNoSGA)
        {
            var transacao = _transacao.Iniciar();
            try
            {
                await _repositorioInscricao.Inserir(inscricao);

                if (!formacaoHomologada)
                {
                    bool confirmada = await _repositorioInscricao.ConfirmarInscricaoVaga(inscricao);
                    if (!confirmada)
                        throw new NegocioException(MensagemNegocio.INSCRICAO_NAO_CONFIRMADA_POR_FALTA_DE_VAGA);

                    inscricao.Situacao = SituacaoInscricao.Confirmada;
                    await _repositorioInscricao.Atualizar(inscricao);
                }

                transacao.Commit();

                var mensagem = MensagemNegocio.INSCRICAO_CONFIRMADA;
                if (!formacaoHomologada && integrarNoSGA)
                    mensagem = MensagemNegocio.INSCRICAO_CONFIRMADA_NA_DATA_INICIO_DA_SUA_TURMA;
                else if (formacaoHomologada)
                    mensagem = MensagemNegocio.INSCRICAO_EM_ANALISE;

                return RetornoDTO.RetornarSucesso(mensagem, inscricao.Id);
            }
            catch
            {
                transacao.Rollback();
                throw;
            }
            finally
            {
                transacao.Dispose();
            }
        }
    }
}
