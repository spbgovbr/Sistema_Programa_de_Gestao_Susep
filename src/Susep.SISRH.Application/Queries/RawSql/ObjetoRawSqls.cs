namespace Susep.SISRH.Application.Queries.RawSql
{
    public static class ObjetoRawSqls
    {

        public static string ObterPorFiltro
        {
            get
            {
                return @"
					SELECT o.objetoId,
                           o.descricao,
                           o.tipo,
                           o.chave,
                           o.ativo
                    FROM [ProgramaGestao].[Objeto] o
                    WHERE (@chave IS NULL OR o.chave LIKE '%' + @chave + '%')  
                    AND   (@descricao IS NULL OR o.descricao LIKE '%' + @descricao + '%')  
                    AND   (@tipo IS NULL OR o.tipo = @tipo)  
                    ORDER BY o.descricao

                    OFFSET @Offset ROWS
                    FETCH NEXT @PageSize ROWS ONLY;

                    SELECT COUNT(*)
                    FROM [ProgramaGestao].[Objeto] o
                    WHERE (@chave IS NULL OR o.chave LIKE '%' + @chave + '%')  
                    AND   (@descricao IS NULL OR o.descricao LIKE '%' + @descricao + '%')  
                    AND   (@tipo IS NULL OR o.tipo = @tipo)  
                ";
            }
        }

        public static string ObterPorId
        {
            get
            {
                return @"
					SELECT o.objetoId,
                           o.descricao,
                           o.tipo,
                           o.chave,
                           o.ativo
                    FROM [ProgramaGestao].[Objeto] o
                    WHERE o.objetoId = @id
                ";
            }
        }

        public static string CountChaveDuplicada
        {
            get
            {
                return @"
					SELECT COUNT(1)
                    FROM [ProgramaGestao].[Objeto] o
                    WHERE (@objetoId IS NULL OR o.objetoId != @objetoId)
                    AND   @chave = o.chave
                ";
            }
        }

        public static string ObterPorTexto
        {
            get
            {
                return @"
                    SELECT o.objetoId
                           ,o.chave
                           ,o.descricao
                           ,o.tipo
                           ,o.ativo
                    FROM [ProgramaGestao].[Objeto] o
                    WHERE o.ativo = 1
                    AND   (lower(o.chave) like '%' + lower(@texto) + '%'
                    OR    lower(o.descricao) like '%' + lower(@texto) + '%')
                    ORDER BY o.chave, o.descricao
                ";
            }
        }
    }
}