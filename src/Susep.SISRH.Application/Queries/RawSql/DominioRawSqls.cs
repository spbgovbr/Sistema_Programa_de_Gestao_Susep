namespace Susep.SISRH.Application.Queries.RawSql
{
    public static class DominioRawSqls
    {

        public static string ObterDominios
        {
            get
            {
                return @"
                    SELECT catalogoDominioId as id, descricao
                    FROM [dbo].[CatalogoDominio]
                    WHERE classificacao = @classificacao 
                        AND ativo = 1
                    ORDER BY descricao
                    ";
            }
        }

        public static string ObterPorChave
        {
            get
            {
                return @"
                    SELECT catalogoDominioId as id, descricao
                    FROM [dbo].[CatalogoDominio]
                    WHERE catalogoDominioId = @id                        
                    ORDER BY descricao
                    ";
            }
        }
    }
}
