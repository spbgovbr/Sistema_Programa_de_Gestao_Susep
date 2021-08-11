namespace Susep.SISRH.Application.Queries.RawSql
{
    public static class PessoaRawSqls
    {
        public static string ObterPorFiltro
        {
            get
            {
                return @"
					SELECT DISTINCT 
                           p.pessoaId
                          ,p.pesNome nome
                          ,p.unidadeId
                          ,u.undSiglaCompleta unidade
                          ,p.CargaHoraria
                    FROM [dbo].[Pessoa] p
                        INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON u.unidadeId = p.unidadeId   
					    LEFT OUTER JOIN [ProgramaGestao].[PlanoTrabalhoAtividadeCandidato] c ON p.pessoaId = c.pessoaId
					    LEFT OUTER JOIN  [dbo].[CatalogoDominio] d ON d.catalogoDominioId = c.situacaoId 
                    WHERE   (@unidadeId IS NULL OR p.unidadeId = @unidadeId)
                            AND (@pesNome IS NULL OR p.pesNome  LIKE '%' + @pesNome + '%')  
                            AND  (@catalogoDominioId IS NULL OR d.catalogoDominioId = @catalogoDominioId)                    
                    ORDER BY pesNome ASC, unidadeId DESC, CargaHoraria ASC

                    OFFSET @Offset ROWS
                    FETCH NEXT @PageSize ROWS ONLY;

                    SELECT COUNT(*)
                    FROM [dbo].[Pessoa] p
                        INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON u.unidadeId = p.unidadeId   
					    LEFT OUTER JOIN [ProgramaGestao].[PlanoTrabalhoAtividadeCandidato] c ON p.pessoaId = c.pessoaId
					    LEFT OUTER JOIN  [dbo].[CatalogoDominio] d ON d.catalogoDominioId = c.situacaoId 
                    WHERE   (@unidadeId IS NULL OR p.unidadeId = @unidadeId)
                            AND (@pesNome IS NULL OR p.pesNome  LIKE '%' + @pesNome + '%')  
                            AND  (@catalogoDominioId IS NULL OR d.catalogoDominioId = @catalogoDominioId)                     
                ";
            }
        }

        public static string ObterDetalhes
        {
            get
            {
                return @"
                        SELECT p.pessoaId
                                ,p.pesNome nome
                                ,p.unidadeId
                                ,v.undSiglaCompleta unidade
                                ,p.CargaHoraria		                        
                        FROM [dbo].[Pessoa] p
                        INNER JOIN [dbo].VW_UnidadeSiglaCompleta v ON v.unidadeId = p.unidadeId
                        WHERE p.pessoaId =  @pessoaId
                    
                        SELECT 
	                        ptac.planoTrabalhoAtividadeCandidatoId
	                        ,ptac.planoTrabalhoAtividadeId
	                        ,p.pesNome nome
	                        ,ptac.situacaoId
	                        ,cds.descricao situacao
	                        ,pta.planoTrabalhoId
	                        ,pta.modalidadeExecucaoId
	                        ,cdm.descricao as modalidade
	                        ,pt.unidadeId
	                        ,un.undSigla as unidade
                        FROM [dbo].[Pessoa] p 
                            INNER JOIN [ProgramaGestao].PlanoTrabalhoAtividadeCandidato ptac 
                            ON p.pessoaId = ptac.pessoaId
                            INNER JOIN [ProgramaGestao].PlanoTrabalhoAtividade pta 
                            ON pta.planoTrabalhoAtividadeId = ptac.planoTrabalhoAtividadeId
                            INNER JOIN [ProgramaGestao].PlanoTrabalho pt 
                            ON pt.planoTrabalhoId = pta.planoTrabalhoId
                            INNER JOIN [dbo].CatalogoDominio cds 
                            ON cds.catalogoDominioId = ptac.situacaoId
                            INNER JOIN [dbo].CatalogoDominio cdm 
                            ON cdm.catalogoDominioId = pta.modalidadeExecucaoId
                            INNER JOIN [dbo].Unidade un 
                            ON un.unidadeId =pt.unidadeId
                        WHERE p.pessoaId = @pessoaId                 

                        SELECT 
	                         ptai.planoTrabalhoAtividadeItemId
	                        ,ptai.planoTrabalhoAtividadeId
	                        ,ptai.itemCatalogoId
	                        ,ic.titulo as itemCatalogo
                        FROM [dbo].[Pessoa] p 
                            INNER JOIN [ProgramaGestao].PlanoTrabalhoAtividadeCandidato ptac 
                            ON p.pessoaId = ptac.pessoaId
                            INNER JOIN [ProgramaGestao].PlanoTrabalhoAtividade pta 
                            ON pta.planoTrabalhoAtividadeId = ptac.planoTrabalhoAtividadeId
                            INNER JOIN [ProgramaGestao].PlanoTrabalhoAtividadeItem ptai 
                            ON ptai.planoTrabalhoAtividadeId = pta.planoTrabalhoAtividadeId
                            INNER JOIN [ProgramaGestao].ItemCatalogo ic 
                            ON ic.itemCatalogoId = ptai.itemCatalogoId
                         WHERE p.pessoaId = @pessoaId

                        SELECT 
	                         ptai.planoTrabalhoAtividadeCriterioId
	                        ,ptai.planoTrabalhoAtividadeId
	                        ,ptai.criterioId
	                        ,cd.descricao criterio
                        FROM [dbo].[Pessoa] p 
                            INNER JOIN [ProgramaGestao].PlanoTrabalhoAtividadeCandidato ptac 
                            ON p.pessoaId = ptac.pessoaId
                            INNER JOIN [ProgramaGestao].PlanoTrabalhoAtividade pta 
                            ON pta.planoTrabalhoAtividadeId = ptac.planoTrabalhoAtividadeId
                            INNER JOIN [ProgramaGestao].PlanoTrabalhoAtividadeCriterio ptai 
                            ON ptai.planoTrabalhoAtividadeId = pta.planoTrabalhoAtividadeId
                            INNER JOIN [dbo].CatalogoDominio cd 
                            ON cd.catalogoDominioId = ptai.criterioId
                       WHERE p.pessoaId = @pessoaId

                        SELECT
	                           ptach.planoTrabalhoAtividadeCandidatoHistoricoId
                              ,ptach.planoTrabalhoAtividadeCandidatoId
                              ,ptach.situacaoId
                              ,ptach.data
                              ,ptach.descricao
                              ,ISNULL(pe.pesNome, ptach.responsavelOperacao) responsavelOperacao
                        FROM  
                            [ProgramaGestao].PlanoTrabalhoAtividadeCandidato ptac 
                            INNER JOIN [ProgramaGestao].PlanoTrabalhoAtividadeCandidatoHistorico ptach 
                                ON ptac.planoTrabalhoAtividadeCandidatoId = ptach.planoTrabalhoAtividadeCandidatoId                            
	                        LEFT OUTER JOIN [dbo].[Pessoa] pe ON ptach.responsavelOperacao = CAST(pe.pessoaId AS VARCHAR(12))
                        WHERE ptac.pessoaId = @pessoaId
                        ORDER BY ptach.data DESC
                ";
            }
        }


        public static string ObterDashboard
        {
            get
            {
                return @"

                        --Planos não encerrados nas unidades em que a pessoa é chefe:
						SELECT   p.planoTrabalhoId
                                ,u1.undSiglaCompleta unidade  
                                ,p.dataInicio    
                                ,p.dataFim
                                ,p.situacaoId
		                        ,cd2.descricao situacao                            
                        FROM [ProgramaGestao].[PlanoTrabalho] p
	                        INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u1 ON u1.unidadeId = p.unidadeId
	                        INNER JOIN [dbo].[CatalogoDominio] cd2 ON p.situacaoId = cd2.catalogoDominioId
							INNER JOIN (
								SELECT u.undSiglaCompleta
								FROM [dbo].Pessoa pe
                                    LEFT OUTER JOIN [dbo].[PessoaAlocacaoTemporaria] a ON a.pessoaId = pe.pessoaId AND a.dataFim IS NULL
									INNER JOIN [dbo].[TipoFuncao] tf ON tf.tipoFuncaoId = pe.tipoFuncaoId
									INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON u.unidadeId = COALESCE(a.unidadeId, pe.unidadeId)  
								WHERE pe.pessoaId = @pessoaId AND tf.tfnIndicadorChefia = 1
							) chefe ON (u1.undSiglaCompleta like chefe.undSiglaCompleta + '%') 
						WHERE p.situacaoId <= 309

						UNION 

						-- Planos em execução que o servidor foi selecionado
						SELECT   DISTINCT p.planoTrabalhoId
                                ,u1.undSiglaCompleta unidade  
                                ,p.dataInicio    
                                ,p.dataFim
                                ,p.situacaoId
		                        ,cd2.descricao situacao                            
                        FROM [ProgramaGestao].[PlanoTrabalho] p
	                        INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u1 ON u1.unidadeId = p.unidadeId
	                        INNER JOIN [dbo].[CatalogoDominio] cd2 ON p.situacaoId = cd2.catalogoDominioId
							INNER JOIN [ProgramaGestao].[PlanoTrabalhoAtividade] pa ON pa.planoTrabalhoId = p.planoTrabalhoId
							INNER JOIN [ProgramaGestao].[PlanoTrabalhoAtividadeCandidato] pac ON pa.planoTrabalhoAtividadeId = pac.planoTrabalhoAtividadeId
						WHERE p.situacaoId = 309 AND pac.pessoaId = @pessoaId AND pac.situacaoId = 804 

						UNION
						-- Planos em habilitação na(s) unidades do servidor
						SELECT   p.planoTrabalhoId
                                ,u1.undSiglaCompleta unidade  
                                ,p.dataInicio    
                                ,p.dataFim
                                ,p.situacaoId
		                        ,cd2.descricao situacao                            
                        FROM [ProgramaGestao].[PlanoTrabalho] p
	                        INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u1 ON u1.unidadeId = p.unidadeId
	                        INNER JOIN [dbo].[CatalogoDominio] cd2 ON p.situacaoId = cd2.catalogoDominioId
							INNER JOIN (
								SELECT COALESCE(a.unidadeId, pe.unidadeId)  unidadeId
								FROM [dbo].Pessoa pe
                                    LEFT OUTER JOIN [dbo].[PessoaAlocacaoTemporaria] a ON a.pessoaId = pe.pessoaId AND a.dataFim IS NULL
								WHERE pe.pessoaId = @pessoaId
								UNION
								SELECT u.unidadeIdPai unidadeId
								FROM [dbo].Pessoa pe
                                    LEFT OUTER JOIN [dbo].[PessoaAlocacaoTemporaria] a ON a.pessoaId = pe.pessoaId AND a.dataFim IS NULL
									INNER JOIN [dbo].[TipoFuncao] tf ON tf.tipoFuncaoId = pe.tipoFuncaoId
									INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON u.unidadeId = COALESCE(a.unidadeId, pe.unidadeId)  
								WHERE pe.pessoaId = @pessoaId AND tf.tfnIndicadorChefia = 1
							) us ON p.unidadeId = us.unidadeId
						WHERE p.situacaoId = 307



                        SELECT   p.pactoTrabalhoId
                                ,u1.undSiglaCompleta unidade    
                                ,p.pessoaId pessoaId 
                                ,pe.pesNome pessoa
                                ,p.dataInicio    
                                ,p.dataFim        
                                ,p.situacaoId   
		                        ,cd2.descricao situacao                            
                        FROM [ProgramaGestao].[PactoTrabalho] p
	                        INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u1 ON u1.unidadeId = p.unidadeId   
	                        INNER JOIN [dbo].Pessoa pe ON pe.pessoaId = p.pessoaId  
	                        INNER JOIN [dbo].[CatalogoDominio] cd2 ON p.situacaoId = cd2.catalogoDominioId
	                        INNER JOIN (
		                        SELECT 
			                        CASE WHEN pe.tipoFuncaoId IS NULL THEN pe.pessoaId ELSE NULL END pessoaId
			                        ,u.undSiglaCompleta 
		                        FROM [dbo].Pessoa pe
                                    LEFT OUTER JOIN [dbo].[PessoaAlocacaoTemporaria] a ON a.pessoaId = pe.pessoaId AND a.dataFim IS NULL
			                        INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON u.unidadeId = COALESCE(a.unidadeId, pe.unidadeId)  
		                        WHERE pe.pessoaId = @pessoaId
								UNION 
								SELECT pe.pessoaId
			                           ,up.undSiglaCompleta
		                        FROM [dbo].Pessoa pe
                                    LEFT OUTER JOIN [dbo].[PessoaAlocacaoTemporaria] a ON a.pessoaId = pe.pessoaId AND a.dataFim IS NULL
			                        INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON u.unidadeId = COALESCE(a.unidadeId, pe.unidadeId) 
									INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] up ON up.unidadeId = u.unidadeIdPai 
		                        WHERE pe.pessoaId = @pessoaId AND pe.tipoFuncaoId IS NOT NULL
	                        ) chefe ON (u1.undSiglaCompleta = chefe.undSiglaCompleta AND chefe.pessoaId IS NOT NULL) OR 
									   (u1.undSiglaCompleta like chefe.undSiglaCompleta + '%' AND chefe.pessoaId IS NULL) 
						WHERE p.situacaoId <= 405 AND 
                            (chefe.pessoaId IS NULL OR p.pessoaId = @pessoaId)
                        ORDER BY p.dataInicio, p.dataFim



                        SELECT  p.pactoTrabalhoId
                                ,u1.undSiglaCompleta unidade  
		                        ,pe.pesNome solicitante
		                        ,cd2.descricao tipoSolicitacao
                                ,s.dataSolicitacao
                        FROM [ProgramaGestao].[PactoTrabalhoSolicitacao] s
	                        INNER JOIN [ProgramaGestao].[PactoTrabalho] p ON s.pactoTrabalhoId = p.pactoTrabalhoId
	                        INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u1 ON u1.unidadeId = p.unidadeId   
	                        INNER JOIN [dbo].Pessoa pe ON pe.pessoaId = p.pessoaId  
	                        INNER JOIN [dbo].[CatalogoDominio] cd2 ON s.tipoSolicitacaoId = cd2.catalogoDominioId
	                        INNER JOIN (
		                        SELECT 
			                        CASE WHEN pe.tipoFuncaoId IS NULL THEN pe.pessoaId ELSE NULL END pessoaId
			                        ,u.undSiglaCompleta 
		                        FROM [dbo].Pessoa pe
                                    LEFT OUTER JOIN [dbo].[PessoaAlocacaoTemporaria] a ON a.pessoaId = pe.pessoaId AND a.dataFim IS NULL
			                        INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON u.unidadeId = COALESCE(a.unidadeId, pe.unidadeId) 
		                        WHERE pe.pessoaId = @pessoaId
								UNION 
								SELECT pe.pessoaId
			                           ,up.undSiglaCompleta
		                        FROM [dbo].Pessoa pe
                                    LEFT OUTER JOIN [dbo].[PessoaAlocacaoTemporaria] a ON a.pessoaId = pe.pessoaId AND a.dataFim IS NULL
			                        INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON u.unidadeId = COALESCE(a.unidadeId, pe.unidadeId)  
									INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] up ON up.unidadeId = u.unidadeIdPai 
		                        WHERE pe.pessoaId = @pessoaId AND pe.tipoFuncaoId IS NOT NULL
	                        ) chefe ON (u1.undSiglaCompleta = chefe.undSiglaCompleta AND chefe.pessoaId IS NOT NULL) OR 
									   (u1.undSiglaCompleta like chefe.undSiglaCompleta + '%' AND chefe.pessoaId IS NULL) 
                        WHERE s.analisado = 0 AND p.situacaoId = 405
                        ORDER BY dataSolicitacao

                ";
            }
        }

        public static string ObterPorChave
        {
            get
            {
                return @"
					SELECT p.pessoaId
                            ,p.pesNome nome
                            ,COALESCE(a.unidadeId, p.unidadeId) unidadeId
                            ,u.undSiglaCompleta unidade
                            ,u.tipoFuncaoUnidadeId
                            ,p.CargaHoraria
                            ,p.tipoFuncaoId
                            ,t.tfnIndicadorChefia chefe
                    FROM [dbo].[Pessoa] p
	                    LEFT OUTER JOIN [dbo].[TipoFuncao] t ON t.tipoFuncaoId = p.tipoFuncaoId
                        LEFT OUTER JOIN [dbo].[PessoaAlocacaoTemporaria] a ON a.pessoaId = p.pessoaId AND a.dataFim IS NULL
	                    INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON u.unidadeId = COALESCE(a.unidadeId, p.unidadeId) 
                    WHERE p.pessoaId = @pessoaId
                ";
            }
        }


        public static string ObterComPactoTrabalho
        {
            get
            {
                return @"
					SELECT DISTINCT 
                          p.pessoaId
                          ,RTRIM(LTRIM(p.pesNome)) nome
                          ,p.unidadeId
                          ,u.undSiglaCompleta unidade
                          ,u.tipoFuncaoUnidadeId
                          ,p.CargaHoraria
                          ,p.tipoFuncaoId
                          ,t.tfnIndicadorChefia chefe
                    FROM [dbo].[Pessoa] p
					    INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON u.unidadeId = p.unidadeId  
					    LEFT OUTER JOIN [dbo].[TipoFuncao] t ON t.tipoFuncaoId = p.tipoFuncaoId
	                    INNER JOIN [ProgramaGestao].[PactoTrabalho] pe ON pe.pessoaId = p.pessoaId 
                    ORDER BY nome
                ";
            }
        }
    }
}
