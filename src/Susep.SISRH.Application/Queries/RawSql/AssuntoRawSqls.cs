namespace Susep.SISRH.Application.Queries.RawSql
{
    public static class AssuntoRawSqls
    {

        public static string ObterPorFiltro
        {
            get
            {
                return @"
					SELECT a.assuntoId,
                           a.valor,
                           a.hierarquia,
                           a.nivel,
                           a.ativo
                    FROM [ProgramaGestao].[VW_AssuntoChaveCompleta] a
                    WHERE (@valor IS NULL OR a.hierarquia LIKE '%' + @valor + '%')  
                    ORDER BY a.hierarquia

                    OFFSET @Offset ROWS
                    FETCH NEXT @PageSize ROWS ONLY;

                    SELECT COUNT(*)
                    FROM [ProgramaGestao].[VW_AssuntoChaveCompleta] a
                    WHERE (@valor IS NULL OR a.valor LIKE '%' + @valor + '%')  
                ";
            }
        }

        public static string ObterPorId
        {
            get
            {
                return @"
                    select a.assuntoId, a.valor, a.assuntoPaiId, a.ativo
                    from ProgramaGestao.Assunto a
                    where a.assuntoId = @id

                    UNION

                    select p.assuntoId, p.valor, p.assuntoPaiId, p.ativo
                    from ProgramaGestao.Assunto p
                    where p.assuntoId = (
	                    select a.assuntoPaiId
	                    from ProgramaGestao.Assunto a
	                    where a.assuntoId = @id
                    );                
                ";
            }
        }
        public static string ObterAtivos
        {
            get
            {
                return @"
                    SELECT a.assuntoId,
                           a.valor,
                           a.hierarquia,
                           a.nivel,
                           a.ativo
                    FROM [ProgramaGestao].[VW_AssuntoChaveCompleta] a
                    WHERE a.ativo = 1
                    ORDER BY a.hierarquia;                
                ";
            }
        }

        public static string ObterPorTexto
        {
            get
            {
                return @"
                    SELECT a.assuntoId,
                            a.valor,
                            a.hierarquia,
                            a.nivel,
                            a.ativo
                    FROM [ProgramaGestao].[VW_AssuntoChaveCompleta] a
                    WHERE a.ativo = 1
                    AND  (lower(a.valor) like '%' + lower(@texto) + '%' 
                    OR    lower(a.hierarquia) like '%' + lower(@texto) + '%')
                    ORDER BY a.hierarquia;                
                ";
            }
        }

        public static string ObterIdsDeTodosOsPais
        {
            get
            {
                return @"
                    WITH cte_assuntos_pais AS (

	                    -- Nível corrente
	                    SELECT 
		                    assuntoId, 
		                    assuntoPaiId 
	                    FROM ProgramaGestao.Assunto
	                    WHERE assuntoId = @assuntoId

	                    UNION ALL

	                    -- Todos os pais
	                    SELECT 
		                    pai.assuntoId, 
		                    pai.assuntoPaiId 
	                    FROM ProgramaGestao.Assunto pai
	                    JOIN cte_assuntos_pais corrente ON pai.assuntoId = corrente.assuntoPaiId

                    )
                    SELECT assuntoId
                    FROM cte_assuntos_pais
                    WHERE assuntoId <> @assuntoId;
                ";
            }
        }

        public static string ObterIdsDeTodosOsFilhos
        {
            get
            {
                return @"
                    WITH cte_assuntos_filhos AS (

	                    -- Nível corrente
	                    SELECT 
		                    assuntoId, 
		                    assuntoPaiId 
	                    FROM ProgramaGestao.Assunto
	                    WHERE assuntoId = @assuntoId

	                    UNION ALL

	                    -- Todos os filhos
	                    SELECT 
		                    filho.assuntoId, 
		                    filho.assuntoPaiId 
	                    FROM ProgramaGestao.Assunto filho
	                    JOIN cte_assuntos_filhos corrente ON filho.assuntoPaiId = corrente.assuntoId

                    )
                    SELECT assuntoId
                    FROM cte_assuntos_filhos
                    WHERE assuntoId <> @assuntoId;
                ";
            }
        }

        public static string ObterPorValor
        {
            get
            {
                return @"
                    SELECT COUNT(1)
                    FROM ProgramaGestao.Assunto a
                    WHERE UPPER(LTRIM(RTRIM(a.valor))) = UPPER(LTRIM(RTRIM(@valor)))
                    AND  (@assuntoId IS NULL OR a.assuntoId <> @assuntoId);                
                ";
            }
        }

    }
}