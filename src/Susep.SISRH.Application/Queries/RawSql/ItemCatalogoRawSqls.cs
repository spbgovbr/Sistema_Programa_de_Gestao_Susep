namespace Susep.SISRH.Application.Queries.RawSql
{
    public static class ItemCatalogoRawSqls
    {


        public static string ObterPorChave
        {
            get
            {
                return @"
					SELECT  i.itemCatalogoId 
							,i.titulo         
							,i.calculoTempoId formaCalculoTempoItemCatalogoId
							,cd.descricao formaCalculoTempoItemCatalogo 
							,i.permiteRemoto permiteTrabalhoRemoto 
							,i.tempoPresencial tempoExecucaoPresencial
							,i.tempoRemoto tempoExecucaoRemoto   
							,i.descricao   
                            ,i.complexidade   
                            ,i.definicaoComplexidade   
                            ,i.entregasEsperadas    
                            ,(SELECT CAST(COUNT(1) AS BIT) FROM [ProgramaGestao].[PactoTrabalhoAtividade] WHERE itemCatalogoId = i.itemCatalogoId) temPactoCadastrado
                            ,(SELECT CAST(COUNT(1) AS BIT) FROM [ProgramaGestao].[CatalogoItemCatalogo] WHERE itemCatalogoId = i.itemCatalogoId) temUnidadeAssociada
                    FROM [ProgramaGestao].[ItemCatalogo] i
	                    INNER JOIN [dbo].[CatalogoDominio] cd ON i.calculoTempoId = cd.catalogoDominioId
                    WHERE i.itemCatalogoId = @itemCatalogoId;

                    SELECT  a.assuntoId
		                    ,a.valor
		                    ,a.hierarquia
                    FROM [ProgramaGestao].[ItemCatalogoAssunto] ica
	                    LEFT OUTER JOIN [ProgramaGestao].[VW_AssuntoChaveCompleta] a ON ica.assuntoId = a.assuntoId
                    WHERE ica.itemCatalogoId = @itemCatalogoId;                
                ";
            }
        }


        public static string ObterPorUnidade
        {
            get
            {
                return @"
				SELECT  i.itemCatalogoId 
		                ,i.titulo         
		                ,i.calculoTempoId formaCalculoTempoItemCatalogoId
		                ,cd.descricao formaCalculoTempoItemCatalogo 
		                ,i.permiteRemoto permiteTrabalhoRemoto 
		                ,i.tempoPresencial tempoExecucaoPresencial
		                ,i.tempoRemoto tempoExecucaoRemoto   
		                ,i.descricao      
                        ,i.complexidade   
                        ,i.definicaoComplexidade   
                        ,i.entregasEsperadas  		
                FROM [ProgramaGestao].[ItemCatalogo] i
	                INNER JOIN [dbo].[CatalogoDominio] cd ON i.calculoTempoId = cd.catalogoDominioId
	                INNER JOIN [ProgramaGestao].[CatalogoItemCatalogo] cic ON i.itemCatalogoId = cic.itemCatalogoId
	                INNER JOIN [ProgramaGestao].[Catalogo] cat ON cic.catalogoId = cat.catalogoId
                WHERE cat.unidadeId = @unidadeId
                ORDER BY i.titulo, i.complexidade
                ";
            }
        }

        public static string ObterPorFiltro
        {
            get
            {
                return @"
					SELECT  i.itemCatalogoId 
							,i.titulo             
							,i.calculoTempoId formaCalculoTempoItemCatalogoId
		                    ,cd.descricao formaCalculoTempoItemCatalogo 
							,i.permiteRemoto permiteTrabalhoRemoto 
							,i.tempoPresencial tempoExecucaoPresencial
							,i.tempoRemoto tempoExecucaoRemoto   
							,i.descricao       
                            ,i.complexidade   
                            ,(SELECT CAST(COUNT(1) AS BIT) FROM [ProgramaGestao].[PactoTrabalhoAtividade] WHERE itemCatalogoId = i.itemCatalogoId) temPactoCadastrado
                            ,(SELECT CAST(COUNT(1) AS BIT) FROM [ProgramaGestao].[CatalogoItemCatalogo] WHERE itemCatalogoId = i.itemCatalogoId) temUnidadeAssociada
                    FROM [ProgramaGestao].[ItemCatalogo] i
	                    INNER JOIN [dbo].[CatalogoDominio] cd ON i.calculoTempoId = cd.catalogoDominioId
                    WHERE   (@titulo IS NULL OR i.titulo LIKE '%' + @titulo + '%')
                            AND (@formaCalculoTempoId IS NULL OR i.calculoTempoId = @formaCalculoTempoId)
                            AND (@permiteTrabalhoRemoto IS NULL OR i.permiteRemoto = @permiteTrabalhoRemoto)

                    ORDER BY i.titulo, i.complexidade

                    OFFSET @Offset ROWS
                    FETCH NEXT @PageSize ROWS ONLY;

                    SELECT COUNT(*)
                    FROM [ProgramaGestao].[ItemCatalogo] i
	                    INNER JOIN [dbo].[CatalogoDominio] cd ON i.calculoTempoId = cd.catalogoDominioId
                    WHERE   (@titulo IS NULL OR i.titulo LIKE '%' + @titulo + '%')
                            AND (@formaCalculoTempoId IS NULL OR i.calculoTempoId = @formaCalculoTempoId)
                            AND (@permiteTrabalhoRemoto IS NULL OR i.permiteRemoto = @permiteTrabalhoRemoto)
                ";
            }
        }



        public static string ObterTodos
        {
            get
            {
                return @"
					SELECT  i.itemCatalogoId 
							,i.titulo             
							,i.calculoTempoId formaCalculoTempoItemCatalogoId
							,cd.descricao formaCalculoTempoItemCatalogo 
							,i.permiteRemoto permiteTrabalhoRemoto 
							,i.tempoPresencial tempoExecucaoPresencial
							,i.tempoRemoto tempoExecucaoRemoto   
							,i.descricao        
                            ,i.complexidade   
                            ,i.definicaoComplexidade   
                            ,i.entregasEsperadas
                    FROM [ProgramaGestao].[ItemCatalogo] i
                    ORDER BY i.titulo, i.complexidade   
                ";
            }
        }
    }
}
