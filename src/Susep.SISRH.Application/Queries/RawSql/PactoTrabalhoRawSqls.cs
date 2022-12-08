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
                            ,pl.dataInicio planoTrabalhoDataInicio
                            ,pl.dataFim planoTrabalhoDataFim
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
                            ,p.tipoFrequenciaTeletrabalhoParcialId
		                    ,cd3.descricao tipoFrequenciaTeletrabalhoParcial
		                    ,p.quantidadeDiasFrequenciaTeletrabalhoParcial
                    FROM [ProgramaGestao].[PactoTrabalho] p
	                    INNER JOIN [ProgramaGestao].[PlanoTrabalho] pl ON p.planoTrabalhoId = pl.planoTrabalhoId
	                    INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON u.unidadeId = p.unidadeId   
	                    INNER JOIN [dbo].Pessoa pe ON pe.pessoaId = p.pessoaId 
	                    INNER JOIN [dbo].[CatalogoDominio] cd1 ON p.formaExecucaoId = cd1.catalogoDominioId 
	                    INNER JOIN [dbo].[CatalogoDominio] cd2 ON p.situacaoId = cd2.catalogoDominioId 
						LEFT OUTER JOIN [ProgramaGestao].[PactoTrabalhoHistorico] h ON p.pactoTrabalhoId = h.pactoTrabalhoId AND h.[situacaoId] = 402
						LEFT OUTER JOIN [dbo].[CatalogoDominio] cd3 ON p.tipoFrequenciaTeletrabalhoParcialId = cd3.catalogoDominioId 
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
					SELECT  p.pactoTrabalhoId
                            ,p.planoTrabalhoId
                            ,p.unidadeId
                            ,u.undSiglaCompleta unidade     
                            ,p.pessoaId
                            ,pe.pesNome pessoa
                            ,p.dataInicio    
                            ,p.dataFim       
                            ,pl.dataInicio planoTrabalhoDataInicio   
                            ,pl.dataFim planoTrabalhoDataFim     
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
                    ORDER BY p.dataInicio DESC
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
                            pt.pactoTrabalhoId
                            ,pt.planoTrabalhoId
                            ,pt.unidadeId
                            ,u.undSiglaCompleta unidade   
                            ,pt.pessoaId
                            ,pe.pesNome pessoa
                            ,pt.dataInicio    
                            ,pt.dataFim   
                            ,pt.formaExecucaoId
		                    ,cd1.descricao formaExecucao
                            ,pt.situacaoId
		                    ,cd2.descricao situacao
                            ,pt.percentualExecucao
                    FROM [ProgramaGestao].[PactoTrabalho] pt
	                    INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON u.unidadeId = pt.unidadeId     
	                    INNER JOIN [dbo].Pessoa pe ON pe.pessoaId = pt.pessoaId   
	                    INNER JOIN [dbo].[CatalogoDominio] cd1 ON pt.formaExecucaoId = cd1.catalogoDominioId 
	                    INNER JOIN [dbo].[CatalogoDominio] cd2 ON pt.situacaoId = cd2.catalogoDominioId 
                        #CONTROLE#
                    WHERE   (@pessoaId IS NULL OR pt.pessoaId = @pessoaId)
                            AND (@situacaoId IS NULL OR pt.situacaoId = @situacaoId)
                            AND (@formaExecucaoId IS NULL OR pt.formaExecucaoId = @formaExecucaoId)
                            AND (@dataInicio IS NULL OR pt.dataFim >= @dataInicio)
                            AND (@dataFim IS NULL OR pt.dataInicio <= @dataFim)
                            AND (@unidadeId IS NULL OR pt.unidadeId = @unidadeId)

                    ORDER BY dataInicio DESC, dataFim DESC, u.undSiglaCompleta

                    OFFSET @Offset ROWS
                    FETCH NEXT @PageSize ROWS ONLY;

                    SELECT COUNT(*) 
                    FROM (
                        SELECT DISTINCT pt.pactoTrabalhoId
                        FROM [ProgramaGestao].[PactoTrabalho] pt
	                        INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON u.unidadeId = pt.unidadeId     
	                        INNER JOIN [dbo].Pessoa pe ON pe.pessoaId = pt.pessoaId   
	                        INNER JOIN [dbo].[CatalogoDominio] cd1 ON pt.formaExecucaoId = cd1.catalogoDominioId 
	                        INNER JOIN [dbo].[CatalogoDominio] cd2 ON pt.situacaoId = cd2.catalogoDominioId 
                            #CONTROLE#
                        WHERE   (@pessoaId IS NULL OR pt.pessoaId = @pessoaId)
                                AND (@situacaoId IS NULL OR pt.situacaoId = @situacaoId)
                                AND (@dataInicio IS NULL OR pt.dataFim >= @dataInicio)
                                AND (@dataFim IS NULL OR pt.dataInicio <= @dataFim)
                    ) contador
                ";
            }
        }


        public static string ControleAcesso
        {
            get
            {
                return @"
                            INNER JOIN (
                                --Unidade da pessoa
                                SELECT p.unidadeId, p.pessoaId 
                                FROM Pessoa p
                                WHERE p.pessoaId = @pessoaLogadaId

                                --Unidade da pessoa caso em vinculação técnica
                                UNION

                                SELECT pat.unidadeId, pat.pessoaId 
                                FROM PessoaAlocacaoTemporaria pat
                                WHERE pat.pessoaId = @pessoaLogadaId

                                UNION

                                --Unidade de cima se a pessoa for chefe de unidade
                                SELECT DISTINCT u.unidadeIdPai, @pessoaLogadaId
                                FROM Unidade u
                                WHERE (u.pessoaIdChefe = @pessoaLogadaId OR u.pessoaIdChefeSubstituto = @pessoaLogadaId) 

                                UNION

                                --Unidades abaixo se a pessoa for chefe
                                SELECT p.unidadeId, p.pessoaId
                                FROM [VW_UnidadeSiglaCompleta] u
                                    INNER JOIN Pessoa p ON u.unidadeId = p.unidadeId 
                                    INNER JOIN (
                                        SELECT undSiglaCompleta
                                        FROM Unidade uc 
                                            INNER JOIN [VW_UnidadeSiglaCompleta] vud ON uc.unidadeId = vud.unidadeId
                                        WHERE (uc.pessoaIdChefe = @pessoaLogadaId OR uc.pessoaIdChefeSubstituto = @pessoaLogadaId)
                                    ) uchefia ON u.undSiglaCompleta like uchefia.undSiglaCompleta + '%'
                                
                                UNION

                                --Se a pessoa for chefe, traz as pessoas que são chefes das unidades abaixo
                                SELECT u.unidadeId, uusub.pessoaIdChefe
                                FROM Unidade u 
                                    INNER JOIN [VW_UnidadeSiglaCompleta] usub ON u.unidadeId = usub.unidadeIdPai
                                    INNER JOIN Unidade uusub ON uusub.unidadeId = usub.unidadeId
                                WHERE (u.pessoaIdChefe = @pessoaLogadaId OR u.pessoaIdChefeSubstituto = @pessoaLogadaId)                                    
                                
                                UNION
                                
                                --Se a pessoa for chefe, traz as pessoas que são substitutos das unidades abaixo
                                SELECT u.unidadeId, uusub.pessoaIdChefeSubstituto
                                FROM Unidade u 
                                    INNER JOIN [VW_UnidadeSiglaCompleta] usub ON u.unidadeId = usub.unidadeIdPai
                                    INNER JOIN Unidade uusub ON uusub.unidadeId = usub.unidadeId
                                WHERE (u.pessoaIdChefe = @pessoaLogadaId OR u.pessoaIdChefeSubstituto = @pessoaLogadaId)                                    
                                
                                UNION

                                -- Se a pessoa for chefe, vinculações técnicas nas unidades abaixo 
                                SELECT pat.unidadeId, pat.pessoaId
                                FROM PessoaAlocacaoTemporaria pat
                                    INNER JOIN [VW_UnidadeSiglaCompleta] upat ON pat.unidadeId = upat.unidadeId
                                    INNER JOIN (
                                        SELECT undSiglaCompleta
                                        FROM Unidade uc 
                                            INNER JOIN [VW_UnidadeSiglaCompleta] vud ON uc.unidadeId = vud.unidadeId
                                        WHERE (uc.pessoaIdChefe = @pessoaLogadaId OR uc.pessoaIdChefeSubstituto = @pessoaLogadaId)
                                    ) uchefia ON upat.undSiglaCompleta like uchefia.undSiglaCompleta + '%'
                            ) unidadesPessoa ON (pt.pessoaId = @pessoaLogadaId) OR (unidadesPessoa.pessoaId = pt.pessoaId AND unidadesPessoa.unidadeId = pt.unidadeId)
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
        public static string ObterInformacoes
        {
            get
            {
                return @"
					SELECT  pactoTrabalhoInformacaoId, 
		                    pactoTrabalhoId,
		                    informacao,
                            ISNULL(pe.pesNome, responsavelRegistro) responsavelRegistro,
		                    dataRegistro
                    FROM ProgramaGestao.PactoTrabalhoInformacao p
	                    LEFT OUTER JOIN Pessoa pe ON p.responsavelRegistro = CAST(pe.pessoaId AS VARCHAR(12))
	                    WHERE pactoTrabalhoId = @pactoTrabalhoId
                    ORDER BY dataRegistro DESC
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
        public static string ObterDeclaracoes
        {
            get
            {
                return @"
					SELECT cd.catalogoDominioId id, cd.descricao
                    FROM CatalogoDominio cd
                    WHERE cd.classificacao = 'DeclaracaoPactoTrabalho' 
                        AND (SELECT COUNT(1) FROM ProgramaGestao.PactoTrabalhoDeclaracao WHERE declaracaoId = cd.catalogoDominioId AND pactoTrabalhoId = @pactoTrabalhoId AND aceita = 1) = 0

                ";
            }
        }
    }
}
