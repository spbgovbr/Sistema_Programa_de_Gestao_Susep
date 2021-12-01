namespace Susep.SISRH.Application.Queries.RawSql
{
    public static class CatalogoRawSqls
    {

        public static string ObterPorChave
        {
            get
            {
                return @"
                    SELECT  c.catalogoId
		                    ,c.unidadeId
		                    ,u.undSiglaCompleta sigla
		                    ,u.undDescricao nome
                    FROM [ProgramaGestao].[Catalogo] c
	                    INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON c.unidadeId = u.unidadeId
                    WHERE c.catalogoId = @catalogoId
                ";
            }
        }

        public static string ObterPorFiltro
        {
            get
            {
                return @"
                    SELECT  c.catalogoId
		                    ,c.unidadeId
		                    ,u.undSiglaCompleta sigla
		                    ,u.undDescricao nome
                    FROM [ProgramaGestao].[Catalogo] c
	                    INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON c.unidadeId = u.unidadeId
                    WHERE @unidadeId IS NULL OR c.unidadeId = @unidadeId

                    ORDER BY u.undSiglaCompleta

                    OFFSET @Offset ROWS
                    FETCH NEXT @PageSize ROWS ONLY;

                    SELECT COUNT(*)
                    FROM [ProgramaGestao].[Catalogo] c
	                    INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON c.unidadeId = u.unidadeId
                    WHERE @unidadeId IS NULL OR c.unidadeId = @unidadeId
                ";
            }
        }

        public static string ObterPorUnidade
        {
            get
            {
                return @"
                    SELECT  c.catalogoId
		                    ,c.unidadeId
		                    ,u.undSiglaCompleta unidadeSigla		                   
                    FROM [ProgramaGestao].[Catalogo] c
	                    INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] u ON c.unidadeId = u.unidadeId
                    WHERE c.unidadeId = @unidadeId
                ";
            }
        }

        public static string ObterItensPorUnidade
        {
            get
            {
                return @"
                    SELECT  i.itemCatalogoId 
							,i.titulo         
                            ,i.titulo + IIF(i.complexidade IS NULL OR i.complexidade = '', '', ' - ' + i.complexidade) + '  (pres.:' + cast(i.tempoPresencial as varchar(5)) + 'h / rem.:' + cast(i.tempoRemoto as varchar(5)) + 'h)' as tituloCompleto
                            ,i.complexidade 
							,i.calculoTempoId formaCalculoTempoItemCatalogoId
							,cd.descricao formaCalculoTempoItemCatalogo 
							,i.permiteRemoto permiteTrabalhoRemoto 
							,i.tempoPresencial tempoExecucaoPresencial
							,i.tempoRemoto tempoExecucaoRemoto   
							,i.descricao      
		                    ,a.assuntoId
		                    ,a.valor
                    FROM [ProgramaGestao].[ItemCatalogo] i
	                    INNER JOIN [ProgramaGestao].[CatalogoItemCatalogo] ci ON ci.itemCatalogoId = i.itemCatalogoId
	                    INNER JOIN [ProgramaGestao].[Catalogo] c ON ci.catalogoId = c.catalogoId
	                    INNER JOIN [dbo].[CatalogoDominio] cd ON i.calculoTempoId = cd.catalogoDominioId
	                    LEFT OUTER JOIN [ProgramaGestao].[ItemCatalogoAssunto] ia ON ia.itemCatalogoId = i.itemCatalogoId
	                    LEFT OUTER JOIN [ProgramaGestao].[Assunto] a ON a.assuntoId = ia.assuntoId AND a.ativo = 1
                    WHERE c.unidadeId = @unidadeId
                    ORDER BY i.titulo,i.complexidade 
                ";
            }
        }

    }
}
