﻿using FluentValidation;
using MediatR;
using SME.ConectaFormacao.Aplicacao.Dtos.Proposta;
using SME.ConectaFormacao.Dominio.Enumerados;
using SME.ConectaFormacao.Dominio.Extensoes;

namespace SME.ConectaFormacao.Aplicacao
{
    public class InserirPropostaCommand : IRequest<RetornoDTO>
    {
        public InserirPropostaCommand(long areaPromotoraId, PropostaDTO propostaDTO)
        {
            PropostaDTO = propostaDTO;
            AreaPromotoraId = areaPromotoraId;
        }

        public long AreaPromotoraId { get; set; }

        public PropostaDTO PropostaDTO { get; }
    }

    public class InserirPropostaCommandValidator : AbstractValidator<InserirPropostaCommand>
    {
        public InserirPropostaCommandValidator()
        {
            RuleFor(f => f.AreaPromotoraId)
                .GreaterThan(0)
                .WithMessage("É necessário informar o Id da área promotora para inserir a proposta");

            RuleFor(f => f.PropostaDTO.TipoFormacao)
                .NotNull()
                .WithMessage("É necessário informar o tipo de formação para inserir a proposta");

            RuleFor(f => f.PropostaDTO.Formato)
                .NotNull()
                .WithMessage("É necessário informar o formato para inserir a proposta");

            When(f => f.PropostaDTO.TipoFormacao == TipoFormacao.Curso, () =>
            {
                RuleFor(x => x.PropostaDTO.Formato).NotEqual(Formato.Hibrido).WithMessage("É permitido o formato Híbrido somente para o tipo de formação evento");
            });

            RuleFor(f => f.PropostaDTO.TiposInscricao)
                .NotNull()
                .WithMessage("É necessário informar o tipo de inscrição para inserir a proposta");

            RuleFor(f => f.PropostaDTO.Dres)
                .NotEmpty()
                .WithMessage("É necessário informar a dre para alterar a proposta");

            RuleFor(f => f.PropostaDTO.CriteriosValidacaoInscricao)
                .NotEmpty()
                .WithMessage("É necessário informar os critérios de validação das inscrições para inserir a proposta");

            RuleFor(f => f.PropostaDTO.QuantidadeTurmas)
                .NotEmpty()
                .WithMessage("É necessário informar a quantidade de turmas para inserir a proposta");

            RuleFor(f => f.PropostaDTO.QuantidadeVagasTurma)
                .NotEmpty()
                .WithMessage("É necessário informar a quantidade de vagas por turma para inserir a proposta");

            RuleFor(f => f.PropostaDTO.Turmas)
                .NotEmpty()
                .WithMessage("É necessário informar a turma para alterar a proposta");

            RuleFor(f => f.PropostaDTO.Justificativa)
                .NotEmpty()
                .WithMessage("É necessário informar a justificativa para inserir a proposta");

            RuleFor(f => f.PropostaDTO.Objetivos)
                .NotEmpty()
                .WithMessage("É necessário informar os objetivos para inserir a proposta");

            RuleFor(f => f.PropostaDTO.ConteudoProgramatico)
                .NotEmpty()
                .WithMessage("É necessário informar o conteúdo programático para inserir a proposta");

            RuleFor(f => f.PropostaDTO.ProcedimentoMetadologico)
                .NotEmpty()
                .WithMessage("É necessário informar os procedimentos metadológicos para inserir a proposta");

            RuleFor(f => f.PropostaDTO.Referencia)
                .NotEmpty()
                .WithMessage("É necessário informar a referência para inserir a proposta");

            RuleFor(f => f.PropostaDTO.PalavrasChaves)
                .NotNull()
                .WithMessage("É necessário informar as palavras-chaves para inserir a proposta");

            RuleFor(f => f.PropostaDTO.LinkParaInscricoesExterna)
                .NotNull()
                .When(y => y.PropostaDTO.TiposInscricao.NaoEhNulo() && y.PropostaDTO.TiposInscricao.Any(tipo => tipo.TipoInscricao == TipoInscricao.Externa))
                .WithMessage("É necessário informar o link para inscrições  para inserir a proposta");
        }
    }
}
