namespace Susep.SISRH.Application.Queries.RawSql
{
    public static class PactoTrabalhoRawSqls
    {

        public static string ObterPorChave
        {
            get
            {
                return @"
					SELECT   p.pactoTrabalhoId
                            ,p.planoTrabalhoId
                            ,p.unidadeId
                            ,u.undSiglaCompleta unidade     
                            ,p.pessoaId
                            ,pe.pesNome pessoa
                            ,p.dataInicio    
                            ,p.dataFim       
                            ,p.formaExecucaoId
		                    ,cd1.descricao formaExecucao
                            ,p.situacaoId
		                    ,cd2.descricao situacao                            
                            ,cargaHorariaDiaria
                            ,percentualExecucao
                            ,relacaoPrevistoRealizado
                            ,tempoTotalDisponivel
                            ,pl.tempoComparecimento                    
							,h.responsavelOperacao responsavelEnvioAceite
                    FROM [ProgramaGestao].[PactoTrabalho] p
	                    INNER JOIN [ProgramaGestao].[PlanoTrabalho] pl ON p.planoTrabalhoId = pl.planoTrabalhoId
	                    INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON u.unidadeId = p.unidadeId   
	                    INNER JOIN [dbo].Pessoa pe ON pe.pessoaId = p.pessoaId 
	                    INNER JOIN [dbo].[CatalogoDominio] cd1 ON p.formaExecucaoId = cd1.catalogoDominioId 
	                    INNER JOIN [dbo].[CatalogoDominio] cd2 ON p.situacaoId = cd2.catalogoDominioId 
						LEFT OUTER JOIN [ProgramaGestao].[PactoTrabalhoHistorico] h ON p.pactoTrabalhoId = h.pactoTrabalhoId AND h.[situacaoId] = 402
                    WHERE p.pactoTrabalhoId = @pactoTrabalhoId
                    ORDER BY h.DataOperacao DESC
                ";
            }
        }


        public static string ObterAtual
        {
            get
            {
                return @"
					SELECT TOP 1
                            p.pactoTrabalhoId
                            ,p.planoTrabalhoId
                            ,p.unidadeId
                            ,u.undSiglaCompleta unidade     
                            ,p.pessoaId
                            ,pe.pesNome pessoa
                            ,p.dataInicio    
                            ,p.dataFim       
                            ,p.formaExecucaoId
		                    ,cd1.descricao formaExecucao
                            ,p.situacaoId
		                    ,cd2.descricao situacao                            
                            ,cargaHorariaDiaria
                            ,percentualExecucao
                            ,relacaoPrevistoRealizado
                            ,tempoTotalDisponivel
                            ,pl.tempoComparecimento
                    FROM [ProgramaGestao].[PactoTrabalho] p
                        INNER JOIN [ProgramaGestao].[PactoTrabalhoHistorico] h ON h.pactoTrabalhoId = p.pactoTrabalhoId
	                    INNER JOIN [ProgramaGestao].[PlanoTrabalho] pl ON p.planoTrabalhoId = pl.planoTrabalhoId
	                    INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON u.unidadeId = p.unidadeId   
	                    INNER JOIN [dbo].Pessoa pe ON pe.pessoaId = p.pessoaId 
	                    INNER JOIN [dbo].[CatalogoDominio] cd1 ON p.formaExecucaoId = cd1.catalogoDominioId 
	                    INNER JOIN [dbo].[CatalogoDominio] cd2 ON p.situacaoId = cd2.catalogoDominioId 
                    WHERE p.situacaoId in (@situacaoRascunho, @situacaoEnviadoAceite, @situacaoAceito, @situacaoEmExecucao)
                        AND p.pessoaId = @pessoaId
                    ORDER BY p.dataInicio 
                ";
            }
        }

        public static string ObterPactosTrabalhoPorPlano
        {
            get
            {
                return @"
					SELECT  p.pactoTrabalhoId
                            ,p.planoTrabalhoId
                            ,p.unidadeId
                            ,u.undSiglaCompleta unidade     
                            ,p.pessoaId
                            ,pe.pesNome pessoa
                            ,p.dataInicio    
                            ,p.dataFim       
                            ,p.formaExecucaoId
		                    ,cd1.descricao formaExecucao
                            ,p.situacaoId
		                    ,cd2.descricao situacao
                    FROM [ProgramaGestao].[PactoTrabalho] p
	                    INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON u.unidadeId = p.unidadeId   
	                    INNER JOIN [dbo].Pessoa pe ON pe.pessoaId = p.pessoaId 
	                    INNER JOIN [dbo].[CatalogoDominio] cd1 ON p.formaExecucaoId = cd1.catalogoDominioId 
	                    INNER JOIN [dbo].[CatalogoDominio] cd2 ON p.situacaoId = cd2.catalogoDominioId 
                    WHERE p.planoTrabalhoId = @planoTrabalhoId
                    ORDER BY p.dataInicio DESC, p.dataFim DESC, pe.pesNome
                ";
            }
        }

        public static string ObterHistorico
        {
            get
            {
                return @"
					SELECT  pactoTrabalhoId
                            ,p.pactoTrabalhoHistoricoId
                            ,situacaoId
							,cd.descricao situacao 
                            ,observacoes
                            ,ISNULL(pe.pesNome, responsavelOperacao) responsavelOperacao
                            ,dataOperacao
                    FROM [ProgramaGestao].[PactoTrabalhoHistorico] p  
	                    INNER JOIN [dbo].[CatalogoDominio] cd ON p.situacaoId = cd.catalogoDominioId 
	                    LEFT OUTER JOIN [dbo].[Pessoa] pe ON p.responsavelOperacao = CAST(pe.pessoaId AS VARCHAR(12))
                    WHERE p.pactoTrabalhoId = @pactoTrabalhoId
                    ORDER BY dataOperacao DESC
                ";
            }
        }

        public static string ObterPorFiltro
        {
            get
            {
                return @"

                    SELECT DISTINCT
                            p.pactoTrabalhoId
                            ,p.planoTrabalhoId
                            ,p.unidadeId
                            ,u.undSiglaCompleta unidade   
                            ,p.pessoaId
                            ,pe.pesNome pessoa
                            ,p.dataInicio    
                            ,p.dataFim   
                            ,p.formaExecucaoId
		                    ,cd1.descricao formaExecucao
                            ,p.situacaoId
		                    ,cd2.descricao situacao
                            ,p.percentualExecucao
                    FROM [ProgramaGestao].[PactoTrabalho] p
	                    INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON u.unidadeId = p.unidadeId     
	                    INNER JOIN [dbo].Pessoa pe ON pe.pessoaId = p.pessoaId   
	                    INNER JOIN [dbo].[CatalogoDominio] cd1 ON p.formaExecucaoId = cd1.catalogoDominioId 
	                    INNER JOIN [dbo].[CatalogoDominio] cd2 ON p.situacaoId = cd2.catalogoDominioId 
                        #CONTROLE#
                    WHERE   (@pessoaId IS NULL OR p.pessoaId = @pessoaId)
                            AND (@situacaoId IS NULL OR p.situacaoId = @situacaoId)
                            AND (@formaExecucaoId IS NULL OR p.formaExecucaoId = @formaExecucaoId)
                            AND (@dataInicio IS NULL OR p.dataFim >= @dataInicio)
                            AND (@dataFim IS NULL OR p.dataInicio <= @dataFim)

                    ORDER BY dataInicio DESC, dataFim DESC, u.undSiglaCompleta

                    OFFSET @Offset ROWS
                    FETCH NEXT @PageSize ROWS ONLY;

                    SELECT COUNT(*) 
                    FROM (
                        SELECT DISTINCT p.pactoTrabalhoId
                        FROM [ProgramaGestao].[PactoTrabalho] p
	                        INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON u.unidadeId = p.unidadeId     
	                        INNER JOIN [dbo].Pessoa pe ON pe.pessoaId = p.pessoaId   
	                        INNER JOIN [dbo].[CatalogoDominio] cd1 ON p.formaExecucaoId = cd1.catalogoDominioId 
	                        INNER JOIN [dbo].[CatalogoDominio] cd2 ON p.situacaoId = cd2.catalogoDominioId 
                            #CONTROLE#
                        WHERE   (@pessoaId IS NULL OR p.pessoaId = @pessoaId)
                                AND (@situacaoId IS NULL OR p.situacaoId = @situacaoId)
                                AND (@dataInicio IS NULL OR p.dataFim >= @dataInicio)
                                AND (@dataFim IS NULL OR p.dataInicio <= @dataFim)
                    ) contador
                ";
            }
        }


        public static string ControleAcesso
        {
            get
            {
                return @"
                            INNER JOIN( SELECT u1.unidadeId, pe.pessoaId
                                        FROM [dbo].[VW_UnidadeSiglaCompleta] u1
                                        LEFT OUTER JOIN [dbo].[pessoa] pe ON pe.unidadeId = u1.unidadeId
                                        INNER JOIN (SELECT u.undSiglaCompleta
                                                        FROM [dbo].Pessoa pe
                                                            LEFT OUTER JOIN[dbo].[PessoaAlocacaoTemporaria] a ON a.pessoaId = pe.pessoaId AND a.dataFim IS NULL
                                                            INNER JOIN[dbo].[TipoFuncao] tf ON tf.tipoFuncaoId = pe.tipoFuncaoId

                                                            INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON u.unidadeId = COALESCE(a.unidadeId, pe.unidadeId)

                                                        WHERE pe.pessoaId = @pessoaLogadaId AND tf.tfnIndicadorChefia = 1
                                                    ) chefe ON(u1.undSiglaCompleta like chefe.undSiglaCompleta + '%')
                                        
                                        UNION 

                                        SELECT pe.unidadeId, pe.pessoaId FROM[dbo].[pessoa] pe WHERE pe.pessoaId = @pessoaLogadaId
                                       ) controle ON controle.pessoaId = p.pessoaId OR controle.unidadeId = p.unidadeId
                        ";
            }
        }
        public static string ObterAtividades
        {
            get
            {
                return @"
					SELECT  p.pactoTrabalhoAtividadeId
                            ,p.pactoTrabalhoId
                            ,p.itemCatalogoId
                            ,i.titulo + IIF(i.complexidade IS NULL OR i.complexidade = '', '', ' - ' + i.complexidade) as itemCatalogo
                            ,i.complexidade as itemCatalogoComplexidade
							,i.calculoTempoId formaCalculoTempoItemCatalogoId
                            ,p.quantidade
                            ,p.tempoPrevistoPorItem
                            ,p.tempoPrevistoTotal
                            ,p.dataInicio
                            ,p.dataFim
                            ,p.tempoRealizado
                            ,p.tempoHomologado
                            ,p.situacaoId
		                    ,cd.descricao situacao
		                    ,p.descricao
                            ,p.nota
                            ,p.justificativa
                            ,modalidadeExecucaoId
                            ,CASE WHEN p.modalidadeExecucaoId IS NULL THEN 
                                IIF(pt.formaExecucaoId = 101, 0, 1)
                             ELSE IIF(p.modalidadeExecucaoId = 101, 0, 1) END execucaoRemota
                            ,p.consideracoesConclusao as consideracoes
                    FROM [ProgramaGestao].[PactoTrabalhoAtividade] p
                        INNER JOIN [ProgramaGestao].[PactoTrabalho] pt ON p.pactoTrabalhoId = pt.pactoTrabalhoId
	                    INNER JOIN [ProgramaGestao].[ItemCatalogo] i ON p.itemCatalogoId = i.itemCatalogoId 
	                    INNER JOIN [dbo].[CatalogoDominio] cd ON p.situacaoId = cd.catalogoDominioId  
                    WHERE p.pactoTrabalhoId = @pactoTrabalhoId;
                ";
            }
        }
        public static string ObterSolicitacoes
        {
            get
            {
                return @"
					SELECT  p.pactoTrabalhoSolicitacaoId                          
                            ,pactoTrabalhoSolicitacaoId
                            ,pactoTrabalhoId
                            ,tipoSolicitacaoId
                            ,cd.descricao tipoSolicitacao
                            ,dataSolicitacao
                            ,solicitante as solicitanteId
                            ,ISNULL(pe.pesNome, solicitante) solicitante
                            ,dadosSolicitacao
                            ,observacoesSolicitante
                            ,analisado
                            ,dataAnalise
                            ,analista
                            ,aprovado
                            ,observacoesAnalista
                    FROM [ProgramaGestao].[PactoTrabalhoSolicitacao] p
	                    INNER JOIN [dbo].[CatalogoDominio] cd ON p.tipoSolicitacaoId = cd.catalogoDominioId  
	                    LEFT OUTER JOIN [dbo].Pessoa pe ON cast(pe.pessoaId as varchar(15))= solicitante
                    WHERE p.pactoTrabalhoId = @pactoTrabalhoId
                    ORDER BY dataSolicitacao DESC, dataAnalise DESC;
                ";
            }
        }

        public static string ObterAssuntosDoPactoEPlanoTrabalho
        {
            get
            {
                return @"
                    -- Recuperar assuntos do plano de trabalho
                    SELECT NULL AS pactoTrabalhoAtividadeId,
                           ptai.itemCatalogoId,
	                       a.assuntoId,
                           a.valor AS descricao
                    FROM [ProgramaGestao].[PlanoTrabalhoAtividadeItem] ptai 
                    INNER JOIN [ProgramaGestao].[PlanoTrabalhoAtividade] pta ON ptai.planoTrabalhoAtividadeId = pta.planoTrabalhoAtividadeId
                    INNER JOIN [ProgramaGestao].[PlanoTrabalhoAtividadeAssunto] ptaa ON pta.planoTrabalhoAtividadeId = ptaa.planoTrabalhoAtividadeId
                    INNER JOIN [ProgramaGestao].[Assunto] a ON ptaa.assuntoId = a.assuntoId
                    INNER JOIN [ProgramaGestao].[PactoTrabalho] pct ON pct.planoTrabalhoId = pta.planoTrabalhoId
                    WHERE pct.pactoTrabalhoId = @pactoTrabalhoId
                    AND   a.ativo = 1;

                    -- Recuperar assuntos dos pacto de trabalho
                    SELECT pta.pactoTrabalhoAtividadeId,
                           pta.itemCatalogoId,
                           a.assuntoId,
	                       a.valor AS descricao
                    FROM [ProgramaGestao].[PactoTrabalhoAtividadeAssunto] ptaa
                    INNER JOIN [ProgramaGestao].[PactoTrabalhoAtividade] pta ON ptaa.pactoTrabalhoAtividadeId = pta.pactoTrabalhoAtividadeId
                    INNER JOIN [ProgramaGestao].[Assunto] a ON ptaa.assuntoId = a.assuntoId
                    WHERE pta.pactoTrabalhoId = @pactoTrabalhoId
                    AND   a.ativo = 1;
                ";
            }
        }
    }
}
