namespace Susep.SISRH.Application.Queries.RawSql
{
    public static class PlanoTrabalhoRawSqls
    {

        public static string ObterPorChave
        {
            get
            {
                return @"
					SELECT  planoTrabalhoId
                            ,p.unidadeId
                            ,u.undSiglaCompleta unidade
                            ,dataInicio
                            ,dataFim
                            ,tempoComparecimento
                            ,tempoFaseHabilitacao
                            ,situacaoId
							,cd.descricao situacao 
                            ,totalServidoresSetor                            
                    FROM [ProgramaGestao].[PlanoTrabalho] p
	                    INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON u.unidadeId = p.unidadeId   
	                    INNER JOIN [dbo].[CatalogoDominio] cd ON p.situacaoId = cd.catalogoDominioId 
                    WHERE p.planoTrabalhoId = @planoTrabalhoId
                ";
            }
        }

        public static string ObterAtividadesPlano
        {
            get
            {
                return @"
					SELECT  p.planoTrabalhoAtividadeId
                            ,p.planoTrabalhoId
                            ,p.modalidadeExecucaoId
							,cd.descricao modalidadeExecucao 
                            ,p.quantidadeColaboradores
                            ,p.descricao
                    FROM [ProgramaGestao].[PlanoTrabalhoAtividade] p
	                    INNER JOIN [dbo].[CatalogoDominio] cd ON p.modalidadeExecucaoId = cd.catalogoDominioId 
                    WHERE p.planoTrabalhoId = @planoTrabalhoId;
                            
                    SELECT  pa.planoTrabalhoAtividadeId
                            ,pa.planoTrabalhoAtividadeItemId
                            ,pa.itemCatalogoId                            
                            ,i.titulo + IIF(i.complexidade IS NULL OR i.complexidade = '', '', ' - ' + i.complexidade) as itemCatalogo
                            ,i.complexidade as itemCatalogoComplexidade
                    FROM [ProgramaGestao].[PlanoTrabalhoAtividadeItem] pa
                        INNER JOIN [ProgramaGestao].[PlanoTrabalhoAtividade] p ON pa.planoTrabalhoAtividadeId = p.planoTrabalhoAtividadeId
	                    INNER JOIN [ProgramaGestao].[ItemCatalogo] i ON pa.itemCatalogoId = i.itemCatalogoId 
                    WHERE p.planoTrabalhoId = @planoTrabalhoId
                            
                    SELECT  pa.planoTrabalhoAtividadeId
                            ,pa.planoTrabalhoAtividadeCriterioId
                            ,pa.criterioId
                            ,i.descricao as criterio
                    FROM [ProgramaGestao].[PlanoTrabalhoAtividadeCriterio] pa
                        INNER JOIN [ProgramaGestao].[PlanoTrabalhoAtividade] p ON pa.planoTrabalhoAtividadeId = p.planoTrabalhoAtividadeId
	                    INNER JOIN [dbo].[CatalogoDominio] i ON pa.criterioId = i.catalogoDominioId 
                    WHERE p.planoTrabalhoId = @planoTrabalhoId

                    SELECT pa.planoTrabalhoAtividadeAssuntoId
                          ,pa.planoTrabalhoAtividadeId
	                      ,pa.assuntoId
                    FROM [ProgramaGestao].[PlanoTrabalhoAtividade] p
                    LEFT OUTER JOIN [ProgramaGestao].[PlanoTrabalhoAtividadeAssunto] pa ON p.planoTrabalhoAtividadeId = pa.planoTrabalhoAtividadeId
                    WHERE p.planoTrabalhoId = @planoTrabalhoId
                ";
            }
        }


        public static string ObterMetasPlano
        {
            get
            {
                return @"
					SELECT  planoTrabalhoMetaId
                            ,planoTrabalhoId
                            ,meta
                            ,indicador
                            ,descricao
                    FROM [ProgramaGestao].[PlanoTrabalhoMeta] p
                    WHERE p.planoTrabalhoId = @planoTrabalhoId
                ";
            }
        }

        public static string ObterReunioesPlano
        {
            get
            {
                return @"
					SELECT  planoTrabalhoReuniaoId
                            ,planoTrabalhoId
                            ,data
                            ,titulo
                            ,descricao
                    FROM [ProgramaGestao].[PlanoTrabalhoReuniao] p
                    WHERE p.planoTrabalhoId = @planoTrabalhoId
                ";
            }
        }

        public static string ObterCustosPlano
        {
            get
            {
                return @"
					SELECT  planoTrabalhoCustoId
                            ,planoTrabalhoId
                            ,valor
                            ,descricao
                    FROM [ProgramaGestao].[PlanoTrabalhoCusto] p
                    WHERE p.planoTrabalhoId = @planoTrabalhoId
                ";
            }
        }

        public static string ObterHistoricoPlano
        {
            get
            {
                return @"
					SELECT  planoTrabalhoId
                            ,p.planoTrabalhoHistoricoId
                            ,situacaoId
							,cd.descricao situacao 
                            ,observacoes
                            ,ISNULL(pe.pesNome, responsavelOperacao) responsavelOperacao
                            ,dataOperacao
                    FROM [ProgramaGestao].[PlanoTrabalhoHistorico] p  
	                    INNER JOIN [dbo].[CatalogoDominio] cd ON p.situacaoId = cd.catalogoDominioId 
	                    LEFT OUTER JOIN [dbo].[Pessoa] pe ON p.responsavelOperacao = CAST(pe.pessoaId AS VARCHAR(12))
                    WHERE p.planoTrabalhoId = @planoTrabalhoId
                    ORDER BY dataOperacao DESC
                ";
            }
        }

        

        public static string ObterPorFiltro
        {
            get
            {
                return @"
					SELECT  planoTrabalhoId
                            ,p.unidadeId
                            ,u.undSiglaCompleta unidade
                            ,dataInicio
                            ,dataFim
                            ,tempoComparecimento
                            ,tempoFaseHabilitacao
                            ,situacaoId
							,cd.descricao situacao 
                    FROM [ProgramaGestao].[PlanoTrabalho] p
	                    INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON u.unidadeId = p.unidadeId    
	                    INNER JOIN [dbo].[CatalogoDominio] cd ON p.situacaoId = cd.catalogoDominioId 
                    WHERE   (@situacaoId IS NULL OR p.situacaoId = @situacaoId)
                            AND (@dataInicio IS NULL OR p.dataFim >= @dataInicio)
                            AND (@dataFim IS NULL OR p.dataInicio <= @dataFim)
                            #UNIDADES#

                    ORDER BY dataInicio DESC, dataFim DESC, u.undSiglaCompleta

                    OFFSET @Offset ROWS
                    FETCH NEXT @PageSize ROWS ONLY;

                    SELECT COUNT(*)
                    FROM [ProgramaGestao].[PlanoTrabalho] p
	                    INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON u.unidadeId = p.unidadeId    
	                    INNER JOIN [dbo].[CatalogoDominio] cd ON p.situacaoId = cd.catalogoDominioId 
                    WHERE   (@situacaoId IS NULL OR p.situacaoId = @situacaoId)
                            AND (@dataInicio IS NULL OR p.dataFim >= @dataInicio)
                            AND (@dataFim IS NULL OR p.dataInicio <= @dataFim)
                            #UNIDADES#
                ";
            }
        }



        public static string ObterCandidatosAtividade
        {
            get
            {
                return @"
                    SELECT  c.planoTrabalhoAtividadeCandidatoId
                            ,c.planoTrabalhoAtividadeId
		                    ,p.pessoaId
		                    ,p.pesNome nome
                            ,situacaoId
							,cd.descricao situacao 
                    FROM [ProgramaGestao].[PlanoTrabalhoAtividadeCandidato] c
                        INNER JOIN [dbo].[Pessoa] p ON p.pessoaId = c.pessoaId
	                    INNER JOIN [dbo].[CatalogoDominio] cd ON c.situacaoId = cd.catalogoDominioId 
                    WHERE c.planoTrabalhoAtividadeId = @planoTrabalhoAtividadeId
                    ORDER BY p.pesNome
                ";
            }
        }

        public static string ObterCandidatosPlano
        {
            get
            {
                return @"
                    SELECT  DISTINCT
                            c.planoTrabalhoAtividadeCandidatoId
                            ,c.planoTrabalhoAtividadeId
		                    ,p.pessoaId
		                    ,p.pesNome nome
                            ,c.situacaoId
							,cd.descricao situacao 
                            ,h.descricao
                    FROM [ProgramaGestao].[PlanoTrabalhoAtividadeCandidato] c
                        INNER JOIN [ProgramaGestao].[PlanoTrabalhoAtividade] pl ON c.planoTrabalhoAtividadeId = pl.planoTrabalhoAtividadeId
                        INNER JOIN [dbo].[Pessoa] p ON p.pessoaId = c.pessoaId
	                    INNER JOIN [dbo].[CatalogoDominio] cd ON c.situacaoId = cd.catalogoDominioId 
                        LEFT OUTER JOIN [ProgramaGestao].[PlanoTrabalhoAtividadeCandidatoHistorico] h ON c.planoTrabalhoAtividadeCandidatoId = h.planoTrabalhoAtividadeCandidatoId AND c.situacaoId = h.situacaoId
                    WHERE pl.planoTrabalhoId = @planoTrabalhoId
                    ORDER BY p.pesNome
                ";
            }
        }


        public static string ObterPorSituacao
        {
            get
            {
                return @"
					SELECT  planoTrabalhoId
                            ,p.unidadeId
                            ,u.undSiglaCompleta unidade
                            ,dataInicio
                            ,dataFim
                            ,tempoComparecimento
                            ,tempoFaseHabilitacao
                            ,situacaoId
							,cd.descricao situacao 
                            ,totalServidoresSetor                            
                    FROM [ProgramaGestao].[PlanoTrabalho] p
	                    INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON u.unidadeId = p.unidadeId   
	                    INNER JOIN [dbo].[CatalogoDominio] cd ON p.situacaoId = cd.catalogoDominioId 
                    WHERE p.unidadeId = (SELECT case when p1.tipoFuncaoId IS NOT NULL THEN u.unidadeIdPai ELSE u.unidadeId END 
					                     FROM [dbo].[Pessoa] p1 
						                    LEFT OUTER JOIN [dbo].[PessoaAlocacaoTemporaria] a ON a.pessoaId = p1.pessoaId AND a.dataFim IS NULL
	                                        INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON u.unidadeId = COALESCE(a.unidadeId, p1.unidadeId) 
						                 WHERE p1.pessoaId = @pessoaId)
                        AND situacaoId = @situacao
                ";
            }
        }

        public static string ObterTermoAceite
        {
            get
            {
                return @"
					SELECT  planoTrabalhoId
                            ,termoAceite
                    FROM [ProgramaGestao].[PlanoTrabalho] p
                    WHERE p.planoTrabalhoId = @planoTrabalhoId
                ";
            }
        }

        public static string ObterAtividadesCandidato
        {
            get
            {
                return @"
                    SELECT  c.planoTrabalhoAtividadeCandidatoId
                            ,c.planoTrabalhoAtividadeId
		                    ,p.pesNome nome
                            ,situacaoId
							,cd.descricao situacao 
                    FROM [ProgramaGestao].[PlanoTrabalhoAtividadeCandidato] c
                        INNER JOIN [dbo].[Pessoa] p ON p.pessoaId = c.pessoaId
	                    INNER JOIN [dbo].[CatalogoDominio] cd ON c.situacaoId = cd.catalogoDominioId 
                    WHERE p.pessoaId = @pessoaId
                  
                    ORDER BY p.pesNome
                ";
            }
        }

        public static string ObterModalidadePorPessoaPlano
        {
            get
            {
                return @"
                    SELECT c.pessoaId
                           ,pa.modalidadeExecucaoId 
	                       ,cd.descricao modalidadeExecucao                            
                    FROM [ProgramaGestao].[PlanoTrabalhoAtividadeCandidato] c
	                    INNER JOIN [ProgramaGestao].[PlanoTrabalhoAtividade] pa ON c.[planoTrabalhoAtividadeId] = pa.[planoTrabalhoAtividadeId]
	                    INNER JOIN [dbo].[CatalogoDominio] cd ON pa.modalidadeExecucaoId = cd.catalogoDominioId 
                    WHERE pa.[planoTrabalhoId] = @planoTrabalhoId
	                    AND c.[situacaoId] = @situacaoAprovada 
                ";
            }
        }

        public static string ObterObjetosPlano
        {
            get
            {
                return @"
                    SELECT pto.planoTrabalhoId
                           ,pto.planoTrabalhoObjetoId
                           ,o.objetoId
	                       ,o.descricao
	                       ,o.chave
                           ,o.tipo		
	                       ,CASE WHEN COUNT(papo.pactoAtividadePlanoObjetoId) > 0 THEN 1 ELSE 0 END AS associadoAtividadePlano
                    FROM [ProgramaGestao].[PlanoTrabalhoObjeto] pto
                    INNER JOIN [ProgramaGestao].[Objeto] o ON pto.objetoId = o.objetoId
                    LEFT OUTER JOIN [ProgramaGestao].[PactoAtividadePlanoObjeto] papo ON papo.planoTrabalhoObjetoId = pto.planoTrabalhoObjetoId
                    WHERE pto.planoTrabalhoId = @planoTrabalhoId
                    AND   o.ativo = 1
                    GROUP BY pto.planoTrabalhoId
                           ,pto.planoTrabalhoObjetoId
                           ,o.objetoId
	                       ,o.descricao
	                       ,o.chave
                           ,o.tipo
                    ORDER BY o.descricao;

                    SELECT pa.planoTrabalhoObjetoAssuntoId
                           ,pa.planoTrabalhoObjetoId
                           ,a.assuntoId
                           ,a.valor
                           ,a.hierarquia
                    FROM [ProgramaGestao].[PlanoTrabalhoObjeto] pto
						INNER JOIN [ProgramaGestao].[PlanoTrabalhoObjetoAssunto] pa ON pa.planoTrabalhoObjetoId = pto.planoTrabalhoObjetoId
						INNER JOIN [ProgramaGestao].[VW_AssuntoChaveCompleta] a ON pa.assuntoId = a.assuntoId
						INNER JOIN [ProgramaGestao].[Objeto] o ON pto.objetoId = o.objetoId
                    WHERE pto.planoTrabalhoId = @planoTrabalhoId;
                ";
            }
        }

        public static string ObterObjetoPlanoById
        {
            get
            {
                return @"
                    SELECT pto.planoTrabalhoId
                           ,pto.planoTrabalhoObjetoId
                           ,o.objetoId
	                       ,o.descricao
	                       ,o.chave
	                       ,o.tipo
                    FROM [ProgramaGestao].[PlanoTrabalhoObjeto] pto
                    INNER JOIN [ProgramaGestao].[Objeto] o ON pto.objetoId = o.objetoId
                    WHERE pto.planoTrabalhoObjetoId = @planoTrabalhoObjetoId;

                    SELECT pa.planoTrabalhoObjetoAssuntoId
                           ,pa.planoTrabalhoObjetoId
                           ,a.assuntoId
                           ,a.valor
                           ,a.hierarquia
                    FROM [ProgramaGestao].PlanoTrabalhoObjetoAssunto pa
                    INNER JOIN [ProgramaGestao].[VW_AssuntoChaveCompleta] a ON pa.assuntoId = a.assuntoId
                    WHERE pa.planoTrabalhoObjetoId = @planoTrabalhoObjetoId;

                    SELECT pc.planoTrabalhoCustoId
                           ,pc.planoTrabalhoId
	                       ,pc.valor
	                       ,pc.descricao
                    FROM [ProgramaGestao].[PlanoTrabalhoCusto] pc
                    WHERE pc.planoTrabalhoObjetoId = @planoTrabalhoObjetoId;

                    SELECT pr.planoTrabalhoReuniaoId
                           ,pr.planoTrabalhoId
	                       ,pr.titulo
	                       ,pr.data
	                       ,pr.descricao
                    FROM [ProgramaGestao].[PlanoTrabalhoReuniao] pr
                    WHERE pr.planoTrabalhoObjetoId = @planoTrabalhoObjetoId;
                ";
            }
        }

        public static string ObterObjetosPlanoAssociadosOuNaoAAtividadesDoPacto
        {
            get
            {
                return @"
                    SELECT po.planoTrabalhoObjetoId
                            ,o.objetoId
                            ,o.descricao
                    FROM [ProgramaGestao].[PlanoTrabalhoObjeto] po
                    INNER JOIN [ProgramaGestao].[Objeto] o ON po.objetoId = o.objetoId
                    WHERE po.planoTrabalhoId = @planoTrabalhoId;

                    SELECT po.planoTrabalhoObjetoId
                            ,a.[assuntoId]
                            ,a.[hierarquia] valor
                    FROM [ProgramaGestao].[PlanoTrabalhoObjetoAssunto] poa
						INNER JOIN [ProgramaGestao].[PlanoTrabalhoObjeto] po ON poa.planoTrabalhoObjetoId = po.planoTrabalhoObjetoId
						INNER JOIN [ProgramaGestao].[VW_AssuntoChaveCompleta] a ON poa.assuntoId = a.assuntoId
                    WHERE po.planoTrabalhoId = @planoTrabalhoId;

                    SELECT p.planoTrabalhoObjetoId
                    FROM [ProgramaGestao].[PactoAtividadePlanoObjeto] p
                    INNER JOIN [ProgramaGestao].[PlanoTrabalhoObjeto] po ON p.planoTrabalhoObjetoId = po.planoTrabalhoObjetoId
                    WHERE po.planoTrabalhoId = @planoTrabalhoId
                    AND   (@pactoTrabalhoAtividadeId IS NOT NULL AND p.pactoTrabalhoAtividadeId = @pactoTrabalhoAtividadeId);                
                ";
            }
        }
    }
}
