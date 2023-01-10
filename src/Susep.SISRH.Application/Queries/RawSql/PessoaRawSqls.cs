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
                          ,UPPER(RTRIM(LTRIM(p.pesNome))) nome
                          ,p.unidadeId
                          ,u.undSiglaCompleta unidade
                          ,sp.situacaoPessoaId
                          ,sp.spsDescricao situacaoPessoa
                          ,tv.tipoVinculoId
                          ,tv.tvnDescricao tipoVinculo
                          ,p.CargaHoraria
                    FROM [dbo].[Pessoa] p
                        LEFT OUTER JOIN [dbo].[PessoaAlocacaoTemporaria] pat ON p.pessoaId = pat.pessoaId AND (dataFim IS NULL OR dataFim > GETDATE())
                        INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON u.unidadeId = ISNULL(pat.unidadeId, p.unidadeId)   
					    INNER JOIN [dbo].[situacaoPessoa] sp ON sp.situacaoPessoaId = p.situacaoPessoaId
					    INNER JOIN [dbo].[TipoVinculo] tv ON tv.tipoVinculoId = p.tipoVinculoId
                    WHERE   (@unidadeId IS NULL OR ISNULL(pat.unidadeId, p.unidadeId)  = @unidadeId)
                            AND (@pesNome IS NULL OR p.pesNome  LIKE '%' + @pesNome + '%')    
                            AND p.situacaoPessoaId = 1 AND p.tipoVinculoId not in (5, 6) 
                    ORDER BY nome ASC, unidadeId DESC, CargaHoraria ASC

                    OFFSET @Offset ROWS
                    FETCH NEXT @PageSize ROWS ONLY;

                    SELECT COUNT(*)
                    FROM [dbo].[Pessoa] p
                        LEFT OUTER JOIN [dbo].[PessoaAlocacaoTemporaria] pat ON p.pessoaId = pat.pessoaId AND (dataFim IS NULL OR dataFim > GETDATE())
                        INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON u.unidadeId = ISNULL(pat.unidadeId, p.unidadeId)   
					    INNER JOIN [dbo].[situacaoPessoa] sp ON sp.situacaoPessoaId = p.situacaoPessoaId
					    INNER JOIN [dbo].[TipoVinculo] tv ON tv.tipoVinculoId = p.tipoVinculoId
                    WHERE   (@unidadeId IS NULL OR ISNULL(pat.unidadeId, p.unidadeId)  = @unidadeId)
                            AND (@pesNome IS NULL OR p.pesNome  LIKE '%' + @pesNome + '%')                      
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


        public static string ObterDashboardPlanos
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
	                        INNER JOIN [dbo].[Unidade] u2 ON u1.unidadeId = u2.unidadeId
	                        INNER JOIN [dbo].[CatalogoDominio] cd2 ON p.situacaoId = cd2.catalogoDominioId							
						WHERE p.situacaoId <= 309 AND p.unidadeId IN (
                                SELECT u.unidadeId
                                FROM [VW_UnidadeSiglaCompleta] u
                                    INNER JOIN (
                                        SELECT undSiglaCompleta
                                        FROM Unidade uc 
                                            INNER JOIN [VW_UnidadeSiglaCompleta] vud ON uc.unidadeId = vud.unidadeId
                                        WHERE (uc.pessoaIdChefe = @pessoaId OR uc.pessoaIdChefeSubstituto = @pessoaId)
                                    ) uchefia ON u.undSiglaCompleta like uchefia.undSiglaCompleta + '%')

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

                ";
            }
        }

        public static string ObterDashboardPactos
        {
            get
            {
                return @"
                        SELECT   pt.pactoTrabalhoId
                                ,pt.planoTrabalhoId
                                ,u1.undSiglaCompleta unidade    
                                ,pt.pessoaId pessoaId 
                                ,pe.pesNome pessoa
                                ,pt.dataInicio    
                                ,pt.dataFim        
                                ,pt.situacaoId   
		                        ,cd2.descricao situacao                            
                        FROM [ProgramaGestao].[PactoTrabalho] pt
	                        INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u1 ON u1.unidadeId = pt.unidadeId   
	                        INNER JOIN [dbo].Pessoa pe ON pe.pessoaId = pt.pessoaId  
	                        INNER JOIN [dbo].[CatalogoDominio] cd2 ON pt.situacaoId = cd2.catalogoDominioId
	                        INNER JOIN (
                                --Unidade da pessoa
                                SELECT p.unidadeId, p.pessoaId 
                                FROM Pessoa p
                                WHERE p.pessoaId = @pessoaId

                                --Unidade da pessoa caso em vinculação técnica
                                UNION

                                SELECT pat.unidadeId, pat.pessoaId 
                                FROM PessoaAlocacaoTemporaria pat
                                WHERE pat.pessoaId = @pessoaId

                                UNION

                                --Unidade de cima se a pessoa for chefe de unidade
                                SELECT DISTINCT u.unidadeIdPai, @pessoaId
                                FROM Unidade u
                                WHERE (u.pessoaIdChefe = @pessoaId OR u.pessoaIdChefeSubstituto = @pessoaId) 

                                UNION

                                --Unidades abaixo se a pessoa for chefe
                                SELECT u.unidadeId, null
                                FROM [VW_UnidadeSiglaCompleta] u
                                    --INNER JOIN Pessoa p ON u.unidadeId = p.unidadeId 
                                    INNER JOIN (
                                        SELECT undSiglaCompleta
                                        FROM Unidade uc 
                                            INNER JOIN [VW_UnidadeSiglaCompleta] vud ON uc.unidadeId = vud.unidadeId
                                        WHERE (uc.pessoaIdChefe = @pessoaId OR uc.pessoaIdChefeSubstituto = @pessoaId)
                                    ) uchefia ON u.undSiglaCompleta like uchefia.undSiglaCompleta + '%'
                                
                                UNION

                                -- Se a pessoa for chefe, vinculações técnicas nas unidades abaixo 
                                SELECT pat.unidadeId, pat.pessoaId
                                FROM PessoaAlocacaoTemporaria pat
                                    INNER JOIN [VW_UnidadeSiglaCompleta] upat ON pat.unidadeId = upat.unidadeId
                                    INNER JOIN (
                                        SELECT undSiglaCompleta
                                        FROM Unidade uc 
                                            INNER JOIN [VW_UnidadeSiglaCompleta] vud ON uc.unidadeId = vud.unidadeId
                                        WHERE (uc.pessoaIdChefe = @pessoaId OR uc.pessoaIdChefeSubstituto = @pessoaId)
                                    ) uchefia ON upat.undSiglaCompleta like uchefia.undSiglaCompleta + '%'
                            ) unidadesPessoa ON unidadesPessoa.unidadeId = pt.unidadeId AND ISNULL(unidadesPessoa.pessoaId, pt.pessoaId) = pt.pessoaId
						WHERE pt.situacaoId <= 405 
                        ORDER BY pt.dataInicio, pt.dataFim

                ";
            }
        }

        public static string ObterDashboardPendencias
        {
            get
            {
                return @"
                        SELECT  pt.pactoTrabalhoId
                                ,u1.undSiglaCompleta unidade  
		                        ,pe.pesNome solicitante
                                ,1 tipoSolicitacaoId
		                        ,cd2.descricao tipoSolicitacao
                                ,s.dataSolicitacao
                        FROM [ProgramaGestao].[PactoTrabalhoSolicitacao] s
	                        INNER JOIN [ProgramaGestao].[PactoTrabalho] pt ON s.pactoTrabalhoId = pt.pactoTrabalhoId
	                        INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u1 ON u1.unidadeId = pt.unidadeId   
	                        INNER JOIN [dbo].Pessoa pe ON pe.pessoaId = pt.pessoaId  
	                        INNER JOIN [dbo].[CatalogoDominio] cd2 ON s.tipoSolicitacaoId = cd2.catalogoDominioId
	                        INNER JOIN (
                                --Unidade da pessoa
                                SELECT p.unidadeId, p.pessoaId 
                                FROM Pessoa p
                                WHERE p.pessoaId = @pessoaId

                                --Unidade da pessoa caso em vinculação técnica
                                UNION

                                SELECT pat.unidadeId, pat.pessoaId 
                                FROM PessoaAlocacaoTemporaria pat
                                WHERE pat.pessoaId = @pessoaId

                                UNION

                                --Unidade de cima se a pessoa for chefe de unidade
                                SELECT DISTINCT u.unidadeIdPai, @pessoaId
                                FROM Unidade u
                                WHERE (u.pessoaIdChefe = @pessoaId OR u.pessoaIdChefeSubstituto = @pessoaId) 

                                UNION

                                --Unidades abaixo se a pessoa for chefe
                                SELECT p.unidadeId, p.pessoaId
                                FROM [VW_UnidadeSiglaCompleta] u
                                    INNER JOIN Pessoa p ON u.unidadeId = p.unidadeId 
                                    INNER JOIN (
                                        SELECT undSiglaCompleta
                                        FROM Unidade uc 
                                            INNER JOIN [VW_UnidadeSiglaCompleta] vud ON uc.unidadeId = vud.unidadeId
                                        WHERE (uc.pessoaIdChefe = @pessoaId OR uc.pessoaIdChefeSubstituto = @pessoaId)
                                    ) uchefia ON u.undSiglaCompleta like uchefia.undSiglaCompleta + '%'
                                
                                UNION

                                -- Se a pessoa for chefe, vinculações técnicas nas unidades abaixo 
                                SELECT pat.unidadeId, pat.pessoaId
                                FROM PessoaAlocacaoTemporaria pat
                                    INNER JOIN [VW_UnidadeSiglaCompleta] upat ON pat.unidadeId = upat.unidadeId
                                    INNER JOIN (
                                        SELECT undSiglaCompleta
                                        FROM Unidade uc 
                                            INNER JOIN [VW_UnidadeSiglaCompleta] vud ON uc.unidadeId = vud.unidadeId
                                        WHERE (uc.pessoaIdChefe = @pessoaId OR uc.pessoaIdChefeSubstituto = @pessoaId)
                                    ) uchefia ON upat.undSiglaCompleta like uchefia.undSiglaCompleta + '%'
                            ) unidadesPessoa ON unidadesPessoa.pessoaId = pt.pessoaId AND unidadesPessoa.unidadeId = pt.unidadeId
                        WHERE s.analisado = 0 AND 
                              pt.situacaoId = 405
                        

                        UNION 

                        SELECT 
	                        pt.pactoTrabalhoId
                            ,u1.undSiglaCompleta unidade  
	                        ,pe.pesNome solicitante
                            ,2 tipoSolicitacaoId
	                        ,'Avaliação de atividade' tipoSolicitacao
	                        ,pth.dataSolicitacao
                        FROM ProgramaGestao.PactoTrabalho pt 
                             INNER JOIN (
		                        SELECT 
	                                pt.pactoTrabalhoId
	                                ,MIN(pt.dataFim) dataSolicitacao
                                FROM ProgramaGestao.PactoTrabalho pt 
                                    INNER JOIN ProgramaGestao.PactoTrabalhoAtividade pta ON pt.pactoTrabalhoId = pta.pactoTrabalhoId
	                                INNER JOIN (                               

                                        --Unidades abaixo se a pessoa for chefe
                                        SELECT p.unidadeId, p.pessoaId
                                        FROM [VW_UnidadeSiglaCompleta] u
                                            INNER JOIN Pessoa p ON u.unidadeId = p.unidadeId 
                                            INNER JOIN (
                                                SELECT undSiglaCompleta
                                                FROM Unidade uc 
                                                    INNER JOIN [VW_UnidadeSiglaCompleta] vud ON uc.unidadeId = vud.unidadeId
                                                WHERE (uc.pessoaIdChefe = @pessoaId OR uc.pessoaIdChefeSubstituto = @pessoaId)
                                            ) uchefia ON u.undSiglaCompleta like uchefia.undSiglaCompleta + '%'
                                
                                        UNION

                                        -- Se a pessoa for chefe, vinculações técnicas nas unidades abaixo 
                                        SELECT pat.unidadeId, pat.pessoaId
                                        FROM PessoaAlocacaoTemporaria pat
                                            INNER JOIN [VW_UnidadeSiglaCompleta] upat ON pat.unidadeId = upat.unidadeId
                                            INNER JOIN (
                                                SELECT undSiglaCompleta
                                                FROM Unidade uc 
                                                    INNER JOIN [VW_UnidadeSiglaCompleta] vud ON uc.unidadeId = vud.unidadeId
                                                WHERE (uc.pessoaIdChefe = @pessoaId OR uc.pessoaIdChefeSubstituto = @pessoaId)
                                            ) uchefia ON upat.undSiglaCompleta like uchefia.undSiglaCompleta + '%'
                                    ) unidadesPessoa ON unidadesPessoa.pessoaId = pt.pessoaId AND unidadesPessoa.unidadeId = pt.unidadeId
                                WHERE nota IS NULL
	                                AND pta.dataFim IS NOT NULL
	                                AND pta.situacaoId = 503
                                GROUP BY pt.pactoTrabalhoId
	                         ) pth ON pt.pactoTrabalhoId = pth.pactoTrabalhoId 
	                         INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u1 ON u1.unidadeId = pt.unidadeId   
	                         INNER JOIN [dbo].Pessoa pe ON pe.pessoaId = pt.pessoaId 


                        UNION

                        SELECT 
	                        pt.pactoTrabalhoId
                            ,u1.undSiglaCompleta unidade  
	                        ,pe.pesNome solicitante
                            ,3 tipoSolicitacaoId
	                        ,'Aceite de plano' tipoSolicitacao
	                        ,pth.dataSolicitacao
                        FROM ProgramaGestao.PactoTrabalho pt 
                             INNER JOIN (
		                        SELECT 
	                                pth.pactoTrabalhoId
	                                ,MAX(pth.DataOperacao) dataSolicitacao
                                FROM ProgramaGestao.PactoTrabalho pt 
                                    INNER JOIN ProgramaGestao.PactoTrabalhoHistorico pth ON pt.pactoTrabalhoId = pth.pactoTrabalhoId AND pt.situacaoId = pth.situacaoId
	                                INNER JOIN (                                

                                        --Unidades abaixo se a pessoa for chefe
                                        SELECT p.unidadeId, p.pessoaId
                                        FROM [VW_UnidadeSiglaCompleta] u
                                            INNER JOIN Pessoa p ON u.unidadeId = p.unidadeId 
                                            INNER JOIN (
                                                SELECT undSiglaCompleta
                                                FROM Unidade uc 
                                                    INNER JOIN [VW_UnidadeSiglaCompleta] vud ON uc.unidadeId = vud.unidadeId
                                                WHERE (uc.pessoaIdChefe = @pessoaId OR uc.pessoaIdChefeSubstituto = @pessoaId)
                                            ) uchefia ON u.undSiglaCompleta like uchefia.undSiglaCompleta + '%'
                                
                                        UNION

                                        -- Se a pessoa for chefe, vinculações técnicas nas unidades abaixo 
                                        SELECT pat.unidadeId, pat.pessoaId
                                        FROM PessoaAlocacaoTemporaria pat
                                            INNER JOIN [VW_UnidadeSiglaCompleta] upat ON pat.unidadeId = upat.unidadeId
                                            INNER JOIN (
                                                SELECT undSiglaCompleta
                                                FROM Unidade uc 
                                                    INNER JOIN [VW_UnidadeSiglaCompleta] vud ON uc.unidadeId = vud.unidadeId
                                                WHERE (uc.pessoaIdChefe = @pessoaId OR uc.pessoaIdChefeSubstituto = @pessoaId)
                                            ) uchefia ON upat.undSiglaCompleta like uchefia.undSiglaCompleta + '%'
                                    ) unidadesPessoa ON unidadesPessoa.pessoaId = pt.pessoaId AND unidadesPessoa.unidadeId = pt.unidadeId
                                WHERE pt.situacaoId = 402
                                GROUP BY pth.pactoTrabalhoId
	                         ) pth ON pt.pactoTrabalhoId = pth.pactoTrabalhoId 
	                         INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u1 ON u1.unidadeId = pt.unidadeId   
	                         INNER JOIN [dbo].Pessoa pe ON pe.pessoaId = pt.pessoaId      
                        WHERE pt.situacaoId = 402

                        UNION

                        SELECT DISTINCT 
                            pt.planoTrabalhoId
                            ,u1.undSiglaCompleta unidade  
	                        ,pe.pesNome solicitante
                            ,4 tipoSolicitacaoId
	                        ,'Sem plano em execução' tipoSolicitacao
	                        ,getdate()
                        FROM	ProgramaGestao.PlanoTrabalho pt 
	                        INNER JOIN ProgramaGestao.PlanoTrabalhoAtividade pta ON pt.planoTrabalhoId = pta.planoTrabalhoId
	                        INNER JOIN ProgramaGestao.PlanoTrabalhoAtividadeCandidato ptac ON ptac.planoTrabalhoAtividadeId = pta.planoTrabalhoAtividadeId
	                        INNER JOIN Pessoa pe ON ptac.pessoaId = pe.pessoaId
                            INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u1 ON u1.unidadeId = pt.unidadeId   
	                        INNER JOIN (                               

                                --Unidades abaixo se a pessoa for chefe
                                SELECT p.unidadeId, p.pessoaId
                                FROM [VW_UnidadeSiglaCompleta] u
                                    INNER JOIN Pessoa p ON u.unidadeId = p.unidadeId 
                                    INNER JOIN (
                                        SELECT undSiglaCompleta
                                        FROM Unidade uc 
                                            INNER JOIN [VW_UnidadeSiglaCompleta] vud ON uc.unidadeId = vud.unidadeId
                                        WHERE (uc.pessoaIdChefe = @pessoaId OR uc.pessoaIdChefeSubstituto = @pessoaId)
                                    ) uchefia ON u.undSiglaCompleta like uchefia.undSiglaCompleta + '%'
                                
                                UNION

                                -- Se a pessoa for chefe, vinculações técnicas nas unidades abaixo 
                                SELECT pat.unidadeId, pat.pessoaId
                                FROM PessoaAlocacaoTemporaria pat
                                    INNER JOIN [VW_UnidadeSiglaCompleta] upat ON pat.unidadeId = upat.unidadeId
                                    INNER JOIN (
                                        SELECT undSiglaCompleta
                                        FROM Unidade uc 
                                            INNER JOIN [VW_UnidadeSiglaCompleta] vud ON uc.unidadeId = vud.unidadeId
                                        WHERE (uc.pessoaIdChefe = @pessoaId OR uc.pessoaIdChefeSubstituto = @pessoaId)
                                    ) uchefia ON upat.undSiglaCompleta like uchefia.undSiglaCompleta + '%'
                            ) unidadesPessoa ON unidadesPessoa.pessoaId = ptac.pessoaId AND unidadesPessoa.unidadeId = pt.unidadeId
	                        LEFT OUTER JOIN ProgramaGestao.PactoTrabalho pct ON pct.pessoaId = ptac.pessoaId AND getdate() BETWEEN pct.dataInicio AND pct.dataFim AND pct.situacaoId = 405
                        WHERE getdate() BETWEEN pt.dataInicio AND pt.dataFim  
	                        AND pt.situacaoId = 309	
                            AND pct.pactoTrabalhoId IS NULL

                        ORDER BY tipoSolicitacao, dataSolicitacao
                ";
            }
        }

        public static string ObterPorChave
        {
            get
            {
                return @"
					SELECT p.pessoaId
                            ,LTRIM(RTRIM(p.pesNome)) nome
                            ,COALESCE(a.unidadeId, p.unidadeId) unidadeId
                            ,p.unidadeId unidadeIdOriginal
                            ,u.undSiglaCompleta unidade
                            ,u.undNivel nivelUnidade
                            ,u.tipoFuncaoUnidadeId
                            ,p.CargaHoraria
                            ,p.tipoFuncaoId
                            ,IIF(u1.unidadeId IS NOT NULL, 1, 0) chefe
                    FROM [dbo].[Pessoa] p
                        LEFT OUTER JOIN [dbo].[PessoaAlocacaoTemporaria] a ON a.pessoaId = p.pessoaId AND a.dataFim IS NULL
                        INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON u.unidadeId = COALESCE(a.unidadeId, p.unidadeId) 
                        LEFT OUTER JOIN [dbo].[Unidade] u1 ON u1.unidadeId = COALESCE(a.unidadeId, p.unidadeId) AND (u1.pessoaIdChefe = @pessoaId OR u1.pessoaIdChefeSubstituto = @pessoaId)
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
                          ,ISNULL(pat.unidadeId, p.unidadeId) unidadeId
                          ,ISNULL(u2.undSiglaCompleta, u1.undSiglaCompleta) unidade
                          ,ISNULL(u2.tipoFuncaoUnidadeId, u1.tipoFuncaoUnidadeId)
                          ,p.CargaHoraria
                          ,p.tipoFuncaoId
                          ,t.tfnIndicadorChefia chefe
                    FROM [dbo].[Pessoa] p
					    INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u1 ON u1.unidadeId = p.unidadeId  
					    LEFT OUTER JOIN [dbo].[PessoaAlocacaoTemporaria] pat ON pat.pessoaId = p.pessoaId AND pat.dataFim IS NULL
					    LEFT OUTER JOIN [dbo].[VW_UnidadeSiglaCompleta] u2 ON u2.unidadeId = pat.unidadeId  
					    LEFT OUTER JOIN [dbo].[TipoFuncao] t ON t.tipoFuncaoId = p.tipoFuncaoId
	                    INNER JOIN [ProgramaGestao].[PactoTrabalho] pe ON pe.pessoaId = p.pessoaId 
                    ORDER BY nome
                ";
            }
        }


        public static string ObterAgendamentos
        {
            get
            {
                return @"
					SELECT DISTINCT a.agendamentoPresencialId, a.pessoaId, pe.pesNome as pessoa, a.dataAgendada
                    FROM programaGestao.AgendamentoPresencial a
                        INNER JOIN Pessoa pe ON pe.pessoaId = a.pessoaId
                        #FILTROPESSOALOGADA#
                    WHERE dataAgendada BETWEEN @dataInicio AND @dataFim
	                    AND (@pessoaId IS NULL OR a.pessoaId = @pessoaId)
                    ORDER BY dataAgendada, pe.pesNome
                ";
            }
        }

        public static string FiltroPessoaLogada
        {
            get
            {
                return @"
					INNER JOIN(    
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

                            -- Se a pessoa for chefe, vinculações técnicas nas unidades abaixo
                            SELECT pat.unidadeId, pat.pessoaId
                            FROM PessoaAlocacaoTemporaria pat
                                INNER JOIN[VW_UnidadeSiglaCompleta] upat ON pat.unidadeId = upat.unidadeId
                               INNER JOIN(
                                   SELECT undSiglaCompleta

                                   FROM Unidade uc
                                       INNER JOIN[VW_UnidadeSiglaCompleta] vud ON uc.unidadeId = vud.unidadeId

                                   WHERE (uc.pessoaIdChefe = @pessoaLogadaId OR uc.pessoaIdChefeSubstituto = @pessoaLogadaId)
                                ) uchefia ON upat.undSiglaCompleta like uchefia.undSiglaCompleta + '%'
                        ) unidadesPessoa ON unidadesPessoa.pessoaId = a.pessoaId OR a.pessoaId = @pessoaLogadaId
                ";
            }
        }

        public static string ObterPactosTrabalhoEmExecucao
        {
            get
            {
                return @"
                        SELECT   pt.pactoTrabalhoId
                                ,pt.planoTrabalhoId
                                ,u1.undSiglaCompleta unidade
                                ,pt.pessoaId pessoaId
                                ,pe.pesNome pessoa
                                ,pt.dataInicio    
                                ,pt.dataFim        
                                ,pt.situacaoId   
		                        ,cd2.descricao situacao
                        FROM [ProgramaGestao].[PactoTrabalho] pt
                            INNER JOIN[dbo].[VW_UnidadeSiglaCompleta] u1 ON u1.unidadeId = pt.unidadeId
                            INNER JOIN[dbo].Pessoa pe ON pe.pessoaId = pt.pessoaId
                            INNER JOIN[dbo].[CatalogoDominio] cd2 ON pt.situacaoId = cd2.catalogoDominioId
                        WHERE pt.pessoaId = @pessoaId AND pt.situacaoId = 405
                ";
            }
        }

        




    }
}
