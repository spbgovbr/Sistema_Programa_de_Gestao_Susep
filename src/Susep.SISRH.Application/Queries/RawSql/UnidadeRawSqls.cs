namespace Susep.SISRH.Application.Queries.RawSql
{
    public static class UnidadeRawSqls
    {

        public static string ObterAtivas
        {
            get
            {
                return @"
					SELECT DISTINCT u.unidadeId as Id
	                                ,u.undSiglaCompleta as Descricao   
                                    ,u.unidadeId
	                                ,u.undSiglaCompleta siglaCompleta
                                    ,u.unidadeIdPai     
                    FROM [dbo].[VW_UnidadeSiglaCompleta] u
                    WHERE situacaoUnidadeId = @situacaoAtiva
                    ORDER BY u.undSiglaCompleta
                ";
            }
        }

        public static string ObterComPlanoTrabalho
        {
            get
            {
                return @"
					SELECT DISTINCT u.unidadeId as Id
	                                ,u.undSiglaCompleta as Descricao   
                                    ,u.unidadeId
	                                ,u.undSiglaCompleta siglaCompleta
                                    ,u.unidadeIdPai   
                    FROM [dbo].[VW_UnidadeSiglaCompleta] u
                        INNER JOIN [ProgramaGestao].[PlanoTrabalho] p ON u.unidadeId = p.unidadeId                    
                    ORDER BY u.undSiglaCompleta
                ";
            }
        }
        
        public static string ObterSemCatalogoCadastrado
        {
            get
            {
                return @"
					SELECT DISTINCT u.unidadeId as Id
	                                ,u.undSiglaCompleta as Descricao   
                                    ,u.unidadeId
	                                ,u.undSiglaCompleta siglaCompleta
                                    ,u.unidadeIdPai         
                    FROM [dbo].[VW_UnidadeSiglaCompleta] u
                        LEFT OUTER JOIN [ProgramaGestao].[Catalogo] c ON u.unidadeId = c.unidadeId  
                    WHERE c.unidadeId IS NULL AND u.[situacaoUnidadeId] = 1
                    ORDER BY u.undSiglaCompleta
                ";
            }
        }

        public static string ObterComCatalogoCadastrado
        {
            get
            {
                return @"
					SELECT DISTINCT u.unidadeId as Id
	                                ,u.undSiglaCompleta as Descricao   
                                    ,u.unidadeId
	                                ,u.undSiglaCompleta siglaCompleta
                                    ,u.unidadeIdPai        
                    FROM [dbo].[VW_UnidadeSiglaCompleta] u
                        LEFT OUTER JOIN [ProgramaGestao].[Catalogo] c ON u.unidadeId = c.unidadeId  
                    WHERE c.unidadeId IS NOT NULL
                    ORDER BY u.undSiglaCompleta
                ";
            }
        }

        public static string ObterPorChave
        {
            get
            {
                return @"
                    SELECT  u.[unidadeId]
                        ,[undSigla]
                        ,[undDescricao] nome
                        ,[unidadeIdPai]
                        ,[tipoUnidadeId]
                        ,[situacaoUnidadeId]
                        ,[ufId]
                        ,[undNivel] Nivel
                        ,[tipoFuncaoUnidadeId]
                        ,[undSiglaCompleta] siglaCompleta
                        ,[Email]
                        ,quantidadeServidores
                    FROM [dbo].[VW_UnidadeSiglaCompleta] vu
                        INNER JOIN (SELECT
                                        u.[unidadeId]
                                        ,count(p.pessoaId) quantidadeServidores
                                    FROM [dbo].[VW_UnidadeSiglaCompleta] u
                                        LEFT OUTER JOIN [dbo].[Pessoa] p ON u.unidadeId = p.unidadeId
                                    WHERE u.[unidadeId] = @unidadeId
                                    GROUP BY u.[unidadeId]) u ON vu.unidadeId = U.unidadeId
                ";
            }
        }

        public static string ObterFeriados
        {
            get
            {
                return @"
                    SELECT  DATEFROMPARTS ( YEAR(@dataInicio), MONTH(ferData), DAY(ferData) ) data
		                    ,ferFixo fixo
		                    ,f.ufId ufId 
                    FROM [dbo].[Feriado] f
                    WHERE ((f.ferFixo = 1
		                    AND DATEFROMPARTS ( YEAR(@dataInicio), MONTH(ferData), DAY(ferData) ) >= DATEFROMPARTS ( YEAR(@dataInicio), MONTH(@dataInicio), DAY(@dataInicio) ) 
		                    AND DATEFROMPARTS ( YEAR(@dataFim), MONTH(ferData), DAY(ferData) ) <= DATEFROMPARTS ( YEAR(@dataFim), MONTH(@dataFim), DAY(@dataFim) ))
	                       OR 
	                       (f.ferFixo = 0 AND ferData >= @dataInicio AND ferData <= @dataFim))					  
	                    AND (f.ufId IS NULL 
		                     OR f.ufId = (SELECT ufId 
					                      FROM [dbo].[Unidade] 
					                      WHERE unidadeId = @unidadeId))
                ";
            }
        }

        public static string ObterModalidadesExecucaoPorUnidade
        {
            get
            {
                return @"
                    SELECT catalogoDominioId as id, descricao
                    FROM [ProgramaGestao].[UnidadeModalidadeExecucao] ue
                        INNER JOIN [dbo].[CatalogoDominio] cd ON cd.catalogoDominioId = ue.modalidadeExecucaoId
                    WHERE unidadeId = @unidadeId
                    ORDER BY descricao
                ";
            }
        }

        public static string ObterPessoasDadosComboPorUnidade
        {
            get
            {
                return @"
                    SELECT p.pessoaId as id, pesNome descricao
                    FROM [dbo].[Pessoa] p
			            LEFT OUTER JOIN [dbo].[PessoaAlocacaoTemporaria] a ON a.pessoaId = p.pessoaId AND a.dataFim IS NULL
	                    INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON u.unidadeId = COALESCE(a.unidadeId, p.unidadeId)
                    WHERE (COALESCE(a.unidadeId, p.unidadeId) = @unidadeId) OR (u.unidadeIdPai = @unidadeId and p.tipoFuncaoId IS NOT NULL) 
                    ORDER BY pesNome
                ";
            }
        }

        public static string ObterPessoasPorUnidade
        {
            get
            {
                return @"
                    SELECT pessoaId pessoaId, pesNome nome, pesEmail email, p.unidadeId, tipoFuncaoId
                    FROM [dbo].[Pessoa] p
	                    INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON p.unidadeId = u.unidadeId
                    WHERE (u.undSiglaCompleta like (SELECT undSiglaCompleta + '%' FROM [dbo].[VW_UnidadeSiglaCompleta] WHERE unidadeId = @unidadeId))
                        OR (u.unidadeIdPai = @unidadeId and p.tipoFuncaoId IS NOT NULL) 
                    ORDER BY pesNome
                ";
            }
        }
        public static string ObterChefesPorUnidade
        {
            get
            {
                return @"
                    SELECT pessoaId pessoaId, pesNome nome, pesEmail email
                    FROM [dbo].[Pessoa] p
	                    INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON p.unidadeId = u.unidadeId
                    WHERE p.tipoFuncaoId IS NOT NULL
	                    and u.undSiglaCompleta IN @siglas
                ";
            }
        }

        public static string ObterSubordinadasPorUnidade
        {
            get
            {
                return @"
                    SELECT unidadeId id
	                       ,undSiglaCompleta descricao
                    FROM [dbo].[VW_UnidadeSiglaCompleta] u
                        WHERE undSiglaCompleta LIKE (SELECT undSiglaCompleta + '%' FROM [dbo].[VW_UnidadeSiglaCompleta] us WHERE us.unidadeId = @unidadeId)
                    ORDER BY undSiglaCompleta
                ";
            }
        }

    }
}
