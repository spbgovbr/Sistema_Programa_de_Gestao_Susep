/**************************************************************************************/

/* VEWS API SUSEP - VERSÕES ANTERIORES A v7 */

/**************************************************************************************/

/* VW_ATIVIDADE */

CREATE OR ALTER VIEW VW_ATIVIDADE
AS 
SELECT        ROW_NUMBER() OVER (ORDER BY ativ.titulo, ativ.complexidade) AS id_atividade, 
	CASE WHEN ativ.complexidade IS NOT NULL 
		THEN ativ.titulo + ' - ' + ativ.complexidade 
		ELSE ativ.titulo 
	END AS nome_atividade
FROM            [ProgramaGestao].[ItemCatalogo] AS ativ
GROUP BY ativ.titulo, ativ.complexidade
GO

/**************************************************************************************/
							   
/* VW_PACTO_HORAS_HOMOLOGADAS */

CREATE OR ALTER VIEW VW_PACTO_HORAS_HOMOLOGADAS
AS 
SELECT PactoTrabalhoId AS IdPacto, sum(tempoHomologado) as HorasHomologadas
FROM [ProgramaGestao].[PactoTrabalhoAtividade]
WHERE NOTA IS NOT NULL
GROUP BY PactoTrabalhoId
GO

/**************************************************************************************/

/* VW_PACTO */

CREATE OR ALTER VIEW VW_PACTO
AS 
select [pactoTrabalhoId] as id_pacto,
    CASE WHEN pa.situacaoId IN (404) THEN 'cancelado' ELSE NULL END AS situacao, 
	pe.pesMatriculaSiape as matricula_siape,  pe.pesCPF as cpf,
	pe.pesnome as nome_participante,
	s.undCodigoSIORG as cod_unidade_exercicio, 
	s.undDescricao as nome_unidade_exercicio, 
	s.undSiglaCompleta as sigla_unidade_exercicio,
	cd1.descricao as desc_situacao_pacto,
	pa.formaExecucaoId - 100 as modalidade_execucao,
	pe.cargaHoraria * 5 as carga_horaria_semanal,
	pa.dataInicio as data_inicio, pa.dataFim as data_fim,
	pa.tempoTotalDisponivel as carga_horaria_total,
	NULL as data_interrupcao, 
	'true' as entregue_no_prazo,
	vw.HorasHomologadas as horas_homologadas
from [ProgramaGestao].[PactoTrabalho] as pa 
	inner join dbo.Pessoa as pe on pe.pessoaId = pa.pessoaId
	inner join VW_UnidadeSiglaCompleta as s on pa.unidadeId = s.unidadeId
	inner join [dbo].[CatalogoDominio] AS cd1 on pa.situacaoId = cd1.catalogoDominioId
	inner join VW_PACTO_HORAS_HOMOLOGADAS AS vw on vw.IdPacto = pa.pactoTrabalhoId
WHERE pa.situacaoId in (403, 405, 406 , 407)
GO	

/**************************************************************************************/

/* VW_PRODUTO */

CREATE OR ALTER VIEW VW_PRODUTO
AS 
SELECT ROW_NUMBER() OVER (ORDER BY pta.pactoTrabalhoAtividadeId ASC) AS id
    ,pta.pactoTrabalhoAtividadeId id_produto
    ,pta.pactoTrabalhoId AS id_pacto
    ,ic.itemCatalogoId AS id_atividade
    ,NULL nome_grupo_atividade
    ,ic.titulo nome_atividade
    ,ic.complexidade faixa_complexidade
    ,ic.definicaoComplexidade parametros_complexidade
    ,CASE WHEN pt.formaExecucaoId = 101 --OR (pt.formaExecucaoId = 102  AND pta.modalidadeExecucaoId = 101) 
        THEN pta.tempoPrevistoPorItem ELSE 0 END tempo_presencial_estimado
    ,CASE WHEN pt.formaExecucaoId = 101 --OR (pt.formaExecucaoId = 102 AND pta.modalidadeExecucaoId = 101)
        THEN pta.tempoRealizado ELSE 0 END tempo_presencial_executado
    ,CASE WHEN pt.formaExecucaoId = 101 --OR (pt.formaExecucaoId = 102 AND pta.modalidadeExecucaoId = 101)
        THEN pta.tempoHomologado ELSE 0 END tempo_presencial_homologado
    ,CASE WHEN pt.formaExecucaoId = 103 --OR (pt.formaExecucaoId = 102 AND pta.modalidadeExecucaoId = 103)
        THEN pta.tempoPrevistoPorItem ELSE 0 END tempo_teletrabalho_estimado
    ,CASE WHEN pt.formaExecucaoId = 103 --OR (pt.formaExecucaoId = 102 AND pta.modalidadeExecucaoId = 103)
        THEN pta.tempoRealizado ELSE 0 END tempo_teletrabalho_executado
    ,CASE WHEN pt.formaExecucaoId = 103 --OR (pt.formaExecucaoId = 102 AND pta.modalidadeExecucaoId = 103)
        THEN pta.tempoHomologado ELSE 0 END tempo_teletrabalho_homologado
    ,ic.entregasEsperadas entrega_esperada
    ,1 qtde_entregas
    ,NULL qtde_entregas_efetivas
    ,pta.nota avaliacao
    ,NULL data_avaliacao
    ,pta.justificativa justificativa
FROM ProgramaGestao.PactoTrabalhoAtividade pta
    INNER JOIN ProgramaGestao.PactoTrabalho pt ON pta.pactoTrabalhoId = pt.pactoTrabalhoId
    INNER JOIN ProgramaGestao.ItemCatalogo ic ON pta.itemCatalogoId = ic.itemCatalogoId
GO

/**************************************************************************************/

/* VW_SITUACAO_PACTO */

CREATE OR ALTER VIEW VW_SITUACAO_PACTO
AS
SELECT catalogodominioid as id, DESCRICAO 
FROM CatalogoDominio
WHERE classificacao = 'SituacaoPactoTrabalho'
GO

/**************************************************************************************/

/* VW_TIPO_ATIVIDADE */

CREATE OR ALTER VIEW VW_TIPO_ATIVIDADE
AS
SELECT        ROW_NUMBER() OVER (ORDER BY ativ.itemCatalogoId ASC) AS id_atividade, 
	ativ.titulo , ativ.complexidade,
	ativ.definicaoComplexidade
FROM            [ProgramaGestao].[ItemCatalogo] AS ativ
GO

/**************************************************************************************/

/* VW_UNIDADE */

CREATE OR ALTER VIEW VW_UNIDADE
AS
SELECT        unidadeId AS id_unidade, undDescricao as nome, undSiglaCompleta as sigla, undCodigoSIORG as codigo
FROM            dbo.VW_UnidadeSiglaCompleta
WHERE        (situacaoUnidadeId = 1)
GO

/**************************************************************************************/
