﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.ConectaFormacao.Aplicacao.Dtos;
using SME.ConectaFormacao.Aplicacao.Dtos.Proposta;
using SME.ConectaFormacao.Aplicacao.DTOS;
using SME.ConectaFormacao.Aplicacao.Interfaces.Formacao;
using SME.ConectaFormacao.Aplicacao.Interfaces.Proposta;
using SME.ConectaFormacao.Dominio.Enumerados;
using SME.ConectaFormacao.Webapi.Controllers.Filtros;

namespace SME.ConectaFormacao.Webapi.Controllers
{
    [Authorize("Bearer")]
    public class PropostaController : BaseController
    {
        [HttpGet("informacoes-cadastrante")]
        [ProducesResponseType(typeof(PropostaInformacoesCadastranteDTO), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_I, Permissao.Proposta_A, Permissao.Proposta_E, Policy = "Bearer")]
        public async Task<IActionResult> ObterInformacoesCadastrante(
            [FromServices] ICasoDeUsoObterInformacoesCadastrante casoDeUsoObterInformacoesCadastrante,
            [FromQuery] long? propostaId)
        {
            return Ok(await casoDeUsoObterInformacoesCadastrante.Executar(propostaId));
        }

        [HttpGet("roteiro")]
        [ProducesResponseType(typeof(RoteiroPropostaFormativaDTO), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_I, Permissao.Proposta_A, Permissao.Proposta_E, Policy = "Bearer")]
        public async Task<IActionResult> ObterRoteiroPropostaFormativa(
            [FromServices] ICasoDeUsoObterRoteiroPropostaFormativa casoDeUsoObterRoteiroPropostaFormativa)
        {
            return Ok(await casoDeUsoObterRoteiroPropostaFormativa.Executar());
        }

        [HttpGet("criterio-validacao-inscricao")]
        [ProducesResponseType(typeof(IEnumerable<CriterioValidacaoInscricaoDTO>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_I, Permissao.Proposta_A, Permissao.Proposta_E, Policy = "Bearer")]
        public async Task<IActionResult> ObterCriterioValidacaoInscricao(
            [FromServices] ICasoDeUsoObterCriterioValidacaoInscricao casoDeUsoObterCriterioValidacaoInscricao,
            [FromQuery] bool exibirOpcaoOutros = false)
        {
            return Ok(await casoDeUsoObterCriterioValidacaoInscricao.Executar(exibirOpcaoOutros));
        }

        [HttpGet("tipo-formacao")]
        [ProducesResponseType(typeof(IEnumerable<RetornoListagemDTO>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_C, Permissao.Proposta_I, Permissao.Proposta_A, Permissao.Proposta_E, Policy = "Bearer")]
        public async Task<IActionResult> ObterTipoFormacao(
            [FromServices] ICasoDeUsoObterTipoFormacao casoDeUsoObterTipoFormacao)
        {
            return Ok(await casoDeUsoObterTipoFormacao.Executar());
        }

        [HttpGet("tipo-inscricao")]
        [ProducesResponseType(typeof(IEnumerable<RetornoListagemDTO>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_C, Permissao.Proposta_I, Permissao.Proposta_A, Permissao.Proposta_E, Policy = "Bearer")]
        public async Task<IActionResult> ObterTipoInscricao(
            [FromServices] ICasoDeUsoObterTipoInscricao casoDeUsoObterTipoInscricao)
        {
            return Ok(await casoDeUsoObterTipoInscricao.Executar());
        }

        [HttpGet("formatos/tipo-formacao/{tipoFormacao}")]
        [ProducesResponseType(typeof(IEnumerable<RetornoListagemDTO>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_C, Permissao.Proposta_I, Permissao.Proposta_A, Permissao.Proposta_E, Policy = "Bearer")]
        public async Task<IActionResult> Obterformatos(
            [FromServices] ICasoDeUsoObterFormatos casoDeUsoObterFormatos,
            [FromRoute] TipoFormacao tipoFormacao)
        {
            return Ok(await casoDeUsoObterFormatos.Executar(tipoFormacao));
        }

        [HttpGet("situacao")]
        [ProducesResponseType(typeof(IEnumerable<RetornoListagemDTO>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_C, Permissao.Proposta_I, Permissao.Proposta_A, Permissao.Proposta_E, Policy = "Bearer")]
        public async Task<IActionResult> ObterSituacoes(
            [FromServices] ICasoDeUsoObterSituacoesProposta casoDeUsoObterSituacoesProposta)
        {
            return Ok(await casoDeUsoObterSituacoesProposta.Executar());
        }

        [HttpGet("tipo-encontro")]
        [ProducesResponseType(typeof(IEnumerable<RetornoListagemDTO>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_C, Permissao.Proposta_I, Permissao.Proposta_A, Permissao.Proposta_E, Policy = "Bearer")]
        public async Task<IActionResult> ObterTipoEncontro(
            [FromServices] ICasoDeUsoObterTipoEncontro casoDeUsoObterTipoEncontro)
        {
            return Ok(await casoDeUsoObterTipoEncontro.Executar());
        }

        [HttpGet("formacao-homologada")]
        [ProducesResponseType(typeof(IEnumerable<RetornoListagemDTO>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_C, Permissao.Proposta_I, Permissao.Proposta_A, Permissao.Proposta_E, Policy = "Bearer")]
        public async Task<IActionResult> ObterFormacaoHomologada(
            [FromServices] ICasoDeUsoObterFormacaoHomologada casoDeUsoObterFormacaoHomologada)
        {
            return Ok(await casoDeUsoObterFormacaoHomologada.Executar());
        }

        [HttpGet("{id}/turma")]
        [ProducesResponseType(typeof(IEnumerable<RetornoListagemDTO>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_C, Permissao.Proposta_I, Permissao.Proposta_A, Permissao.Proposta_E, Policy = "Bearer")]
        public async Task<IActionResult> ObterTurmas(
            [FromServices] ICasoDeUsoObterTurmasProposta casoDeUsoObterTurmasProposta,
            [FromRoute] long id)
        {
            return Ok(await casoDeUsoObterTurmasProposta.Executar(id));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PropostaCompletoDTO), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_C, Permissao.Proposta_I, Permissao.Proposta_A, Permissao.Proposta_E, Policy = "Bearer")]
        public async Task<IActionResult> ObterPropostaPorId(
            [FromServices] ICasoDeUsoObterPropostaPorId casoDeUsoObterPropostaPorId,
            [FromRoute] long id)
        {
            return Ok(await casoDeUsoObterPropostaPorId.Executar(id));
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginacaoResultadoDTO<PropostaPaginadaDTO>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_C, Permissao.Proposta_I, Permissao.Proposta_A, Permissao.Proposta_E, Policy = "Bearer")]
        public async Task<IActionResult> ObterPropostaPaginada(
            [FromServices] ICasoDeUsoObterPropostaPaginacao casoDeUsoObterPropostaPaginacao,
            [FromQuery] PropostaFiltrosDTO propostaFiltrosDTO)
        {
            return Ok(await casoDeUsoObterPropostaPaginacao.Executar(propostaFiltrosDTO));
        }

        [HttpPost]
        [ProducesResponseType(typeof(RetornoDTO), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_I, Policy = "Bearer")]
        public async Task<IActionResult> InserirProposta(
            [FromServices] ICasoDeUsoInserirProposta casoDeUsoInserirProposta,
            [FromBody] PropostaDTO propostaDTO)
        {
            return Ok(await casoDeUsoInserirProposta.Executar(propostaDTO));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RetornoDTO), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_A, Policy = "Bearer")]
        public async Task<IActionResult> AlterarProposta(
           [FromServices] ICasoDeUsoAlterarProposta casoDeUsoAlterarProposta,
           [FromRoute] long id,
           [FromBody] PropostaDTO propostaDTO)
        {
            return Ok(await casoDeUsoAlterarProposta.Executar(id, propostaDTO));
        }

        [HttpPut("devolver-proposta/{id}")]
        [ProducesResponseType(typeof(RetornoDTO), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_A, Policy = "Bearer")]
        public async Task<IActionResult> DevolverProposta(
            [FromServices] ICasoDeUsoDevolverProposta casoDeUso,
            [FromRoute] long id,
            [FromBody] DevolverPropostaDTO devolverPropostaDto)
        {
            return Ok(await casoDeUso.Executar(id, devolverPropostaDto));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_E, Policy = "Bearer")]
        public async Task<IActionResult> RemoverProposta(
          [FromServices] ICasoDeUsoRemoverProposta casoDeUsoRemoverProposta,
          [FromRoute] long id)
        {
            return Ok(await casoDeUsoRemoverProposta.Executar(id));
        }

        [HttpGet("{propostaId}/encontro")]
        [ProducesResponseType(typeof(PaginacaoResultadoDTO<PropostaEncontroDTO>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        public async Task<IActionResult> ObterPropostaEncontrosPaginado(
            [FromServices] ICasoDeUsoObterPropostaEncontroPaginacao casoDeUsoObterPropostaEncontroPaginacao,
            [FromRoute] long propostaId)
        {
            return Ok(await casoDeUsoObterPropostaEncontroPaginacao.Executar(propostaId));
        }

        [HttpPost("{propostaId}/encontro")]
        [ProducesResponseType(typeof(long), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_C, Permissao.Proposta_I, Permissao.Proposta_A, Permissao.Proposta_E, Policy = "Bearer")]
        public async Task<IActionResult> SalvarPropostaEncontro(
            [FromServices] ICasoDeUsoSalvarPropostaEncontro casoDeUsoSalvarPropostaEncontro,
            [FromRoute] long propostaId,
            [FromBody] PropostaEncontroDTO propostaEncontroDTO)
        {
            return Ok(await casoDeUsoSalvarPropostaEncontro.Executar(propostaId, propostaEncontroDTO));
        }

        [HttpDelete("encontro/{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_C, Permissao.Proposta_I, Permissao.Proposta_A, Permissao.Proposta_E, Policy = "Bearer")]
        public async Task<IActionResult> RemoverPropostaEncontro(
            [FromServices] ICasoDeUsoRemoverPropostaEncontro casoDeUsoRemoverPropostaEncontro,
            [FromRoute] long id)
        {
            return Ok(await casoDeUsoRemoverPropostaEncontro.Executar(id));
        }

        [HttpGet("comunicado-acao-formativa/{propostaId}")]
        [ProducesResponseType(typeof(ComunicadoAcaoFormativaDTO), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_C, Permissao.Proposta_I, Permissao.Proposta_A, Permissao.Proposta_E, Policy = "Bearer")]
        public async Task<IActionResult> ObterComunicadoAcaoFormativaPorParametro([FromServices] ICasoDeUsoObterComunicadoAcaoFormativa casoDeUsoObterComunicadoAcaoFormativa, [FromRoute] long propostaId)
        {
            return Ok(await casoDeUsoObterComunicadoAcaoFormativa.Executar(propostaId));
        }

        [HttpGet("nome-profissional/{registroFuncional}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_C, Permissao.Proposta_I, Permissao.Proposta_A, Permissao.Proposta_E, Policy = "Bearer")]
        public async Task<IActionResult> ObterNomeProfissionalTutorRegente([FromRoute] string registroFuncional, [FromServices] ICasoDeUsoObterNomeRegenteTutor useCase)
        {
            return Ok(await useCase.Executar(registroFuncional));
        }

        [HttpPost("{propostaId}/regente")]
        [ProducesResponseType(typeof(long), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_C, Permissao.Proposta_I, Permissao.Proposta_A, Permissao.Proposta_E, Policy = "Bearer")]
        public async Task<IActionResult> SalvarPropostaProfissionalRegente([FromServices] ICasoDeUsoSalvarPropostaRegente casoDeUsoSalvarPropostaRegente,
            [FromRoute] long propostaId,
            [FromBody] PropostaRegenteDTO propostaRegenteDto)
        {
            return Ok(await casoDeUsoSalvarPropostaRegente.Executar(propostaId, propostaRegenteDto));
        }

        [HttpGet("{propostaId}/regente")]
        [ProducesResponseType(typeof(PaginacaoResultadoDTO<PropostaRegenteDTO>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_C, Permissao.Proposta_I, Permissao.Proposta_A, Permissao.Proposta_E, Policy = "Bearer")]
        public async Task<IActionResult> ObterPropostaRegentePaginado([FromRoute] long propostaId,
            [FromServices] ICasoDeUsoObterPropostaRegentePaginacao useCase)
        {
            return Ok(await useCase.Executar(propostaId));
        }

        [HttpGet("regente/{regenteId}")]
        [ProducesResponseType(typeof(PropostaRegenteDTO), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_C, Permissao.Proposta_I, Permissao.Proposta_A, Permissao.Proposta_E, Policy = "Bearer")]
        public async Task<IActionResult> ObterPropostaRegentePorId([FromRoute] long regenteId,
            [FromServices] ICasoDeUsoObterPropostaRegentePorId useCase)
        {
            return Ok(await useCase.Executar(regenteId));
        }

        [HttpDelete("regente/{regenteId}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_C, Permissao.Proposta_I, Permissao.Proposta_A, Permissao.Proposta_E, Policy = "Bearer")]
        public async Task<IActionResult> ExcluirRegente([FromRoute] long regenteId,
            [FromServices] ICasoDeUsoRemoverPropostaRegente useCase)
        {
            return Ok(await useCase.Executar(regenteId));
        }

        [HttpPost("{propostaId}/tutor")]
        [ProducesResponseType(typeof(long), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_C, Permissao.Proposta_I, Permissao.Proposta_A, Permissao.Proposta_E, Policy = "Bearer")]
        public async Task<IActionResult> SalvarPropostaProfissionalTutor([FromServices] ICasoDeUsoSalvarPropostaTutor casoDeUsoSalvarPropostaTutor,
            [FromRoute] long propostaId,
            [FromBody] PropostaTutorDTO propostaTutorDto)
        {
            return Ok(await casoDeUsoSalvarPropostaTutor.Executar(propostaId, propostaTutorDto));
        }

        [HttpDelete("tutor/{tutorId}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_C, Permissao.Proposta_I, Permissao.Proposta_A, Permissao.Proposta_E, Policy = "Bearer")]
        public async Task<IActionResult> ExcluirTutor([FromRoute] long tutorId,
            [FromServices] ICasoDeUsoRemoverPropostaTutor useCase)
        {
            return Ok(await useCase.Executar(tutorId));
        }

        [HttpGet("{propostaId}/tutor")]
        [ProducesResponseType(typeof(PaginacaoResultadoDTO<PropostaTutorDTO>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_C, Permissao.Proposta_I, Permissao.Proposta_A, Permissao.Proposta_E, Policy = "Bearer")]
        public async Task<IActionResult> ObterPropostaTutorPaginado([FromRoute] long propostaId,
            [FromServices] ICasoDeUsoObterPropostaTutorPaginacao useCase)
        {
            return Ok(await useCase.Executar(propostaId));
        }

        [HttpGet("tutor/{tutorId}")]
        [ProducesResponseType(typeof(PropostaTutorDTO), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_C, Permissao.Proposta_I, Permissao.Proposta_A, Permissao.Proposta_E, Policy = "Bearer")]
        public async Task<IActionResult> ObterPropostaTutorPorId([FromRoute] long tutorId,
            [FromServices] ICasoDeUsoObterPropostaTutorPorId useCase)
        {
            return Ok(await useCase.Executar(tutorId));
        }

        [HttpPatch("{propostaId}/enviar")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_C, Permissao.Proposta_I, Permissao.Proposta_A, Permissao.Proposta_E, Policy = "Bearer")]
        public async Task<IActionResult> EnviarProposta(
            [FromServices] ICasoDeUsoEnviarProposta useCase,
            [FromRoute] long propostaId)
        {
            return Ok(await useCase.Executar(propostaId));
        }

        [HttpGet("dashboard")]
        [ProducesResponseType(typeof(IEnumerable<PropostaDashboardDTO>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_C, Permissao.Proposta_I, Permissao.Proposta_A, Permissao.Proposta_E, Policy = "Bearer")]
        public async Task<IActionResult> ObterPropostasDashboard([FromQuery] PropostaFiltrosDashboardDTO filtro,
        [FromServices] ICasoDeUsoObterPropostasDashboard useCase)
        {
            return Ok(await useCase.Executar(filtro));
        }

        [HttpDelete("parecer/{parecerId}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_E, Policy = "Bearer")]
        public async Task<IActionResult> RemoverParecer([FromRoute] long parecerId, [FromServices] ICasoDeUsoRemoverParecerDaProposta casoDeUso)
        {
            return Ok(await casoDeUso.Executar(parecerId));
        }

        [HttpPost("parecer")]
        [ProducesResponseType(typeof(RetornoDTO), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_I, Policy = "Bearer")]
        public async Task<IActionResult> InserirPropostaParecer([FromServices] ICasoDeUsoSalvarPropostaPareceristaConsideracao casoDeUsoSalvarPropostaPareceristaConsideracao,
            [FromBody] PropostaPareceristaConsideracaoCadastroDTO propostaPareceristaConsideracaoCadastroDto)
        {
            return Ok(await casoDeUsoSalvarPropostaPareceristaConsideracao.Executar(propostaPareceristaConsideracaoCadastroDto));
        }

        [HttpPut("parecer")]
        [ProducesResponseType(typeof(RetornoDTO), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_A, Policy = "Bearer")]
        public async Task<IActionResult> AlterarPropostaParecer([FromServices] ICasoDeUsoSalvarPropostaPareceristaConsideracao casoDeUsoSalvarPropostaPareceristaConsideracao,
            [FromBody] PropostaPareceristaConsideracaoCadastroDTO propostaPareceristaConsideracaoCadastroDto)
        {
            return Ok(await casoDeUsoSalvarPropostaPareceristaConsideracao.Executar(propostaPareceristaConsideracaoCadastroDto));
        }

        [HttpGet("parecer")]
        [ProducesResponseType(typeof(PropostaPareceristaConsideracaoCompletoDTO), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterPropostaPareceresPorPropostaIdECampo(
            [FromServices] ICasoDeUsoObterPropostaParecer casoDeUsoSalvarPropostaParecer,
            [FromQuery] PropostaParecerFiltroDTO propostaParecerFiltroDTO)
        {
            return Ok(await casoDeUsoSalvarPropostaParecer.Executar(propostaParecerFiltroDTO));
        }

        [HttpGet("{propostaId}/parecerista/sugestao")]
        [ProducesResponseType(typeof(IEnumerable<PropostaPareceristaSugestaoDTO>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_I, Permissao.Proposta_A, Policy = "Bearer")]
        public async Task<IActionResult> ObterSugestoesPareceristaAprovada(
            [FromServices] ICasoDeUsoObterSugestaoParecerPareceristas casoDeUso,
            [FromRoute] long propostaId)
        {
            return Ok(await casoDeUso.Executar(propostaId));
        }

        [HttpPost("{propostaId}/parecerista/enviar")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_I, Policy = "Bearer")]
        public async Task<IActionResult> EnviarPropostaParecerista(
            [FromServices] ICasoDeUsoEnviarPropostaParecerista casoDeUso,
            [FromRoute] long propostaId)
        {
            return Ok(await casoDeUso.Executar(propostaId));
        }

        [HttpPost("{propostaId}/parecerista/aprovar")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_I, Policy = "Bearer")]
        public async Task<IActionResult> AprovarPropostaParecerista(
            [FromServices] ICasoDeUsoAprovarPropostaParecerista casoDeUso,
            [FromRoute] long propostaId,
            [FromBody] PropostaJustificativaDTO propostaJustificativaDTO)
        {
            return Ok(await casoDeUso.Executar(propostaId, propostaJustificativaDTO));
        }

        [HttpPost("{propostaId}/parecerista/recusar")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_I, Policy = "Bearer")]
        public async Task<IActionResult> RecusarPropostaParecerista(
            [FromServices] ICasoDeUsoRecusarPropostaParecerista casoDeUso,
            [FromRoute] long propostaId,
            [FromBody] PropostaJustificativaDTO propostaJustificativaDTO)
        {
            return Ok(await casoDeUso.Executar(propostaId, propostaJustificativaDTO));
        }

        [HttpPost("{propostaId}/aprovar")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_I, Policy = "Bearer")]
        public async Task<IActionResult> AprovarProposta(
            [FromServices] ICasoDeUsoAprovarProposta casoDeUso,
            [FromRoute] long propostaId,
            [FromBody] PropostaJustificativaDTO propostaJustificativaDTO)
        {
            return Ok(await casoDeUso.Executar(propostaId, propostaJustificativaDTO));
        }

        [HttpPost("{propostaId}/recusar")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_I, Policy = "Bearer")]
        public async Task<IActionResult> RecusarProposta(
            [FromServices] ICasoDeUsoRecusarProposta casoDeUso,
            [FromRoute] long propostaId,
            [FromBody] PropostaJustificativaDTO propostaJustificativaDTO)
        {
            return Ok(await casoDeUso.Executar(propostaId, propostaJustificativaDTO));
        }

        [HttpGet("{propostaId}/relatorio/lauda-publicacao")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterRelatorioLaudaDePublicacao(
            [FromServices] ICasoDeUsoObterRelatorioPropostaLaudaPublicacao casoDeUso,
            [FromRoute] long propostaId)
        {
            return Ok(await casoDeUso.Executar(propostaId));
        }

        [HttpGet("{propostaId}/relatorio/lauda-completa")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        [Permissao(Permissao.Proposta_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterRelatorioLaudaCompleta(
            [FromServices] ICasoDeUsoObterRelatorioPropostaLaudaCompleta casoDeUso,
            [FromRoute] long propostaId)
        {
            return Ok(await casoDeUso.Executar(propostaId));
        }

        [HttpGet("horas-totais-proposta")]
        [ProducesResponseType(typeof(IEnumerable<RetornoListagemDTO>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 400)]
        [ProducesResponseType(typeof(RetornoBaseDTO), 500)]
        public async Task<IActionResult> ObterHorasTotaisProposta([FromServices] ICasoDeUsoObterHorasTotaisProposta useCase)
        {
            return Ok(await useCase.Executar());
        }
    }
}
