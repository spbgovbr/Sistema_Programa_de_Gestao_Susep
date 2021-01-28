CREATE VIEW [dbo].[VW_UnidadeSiglaCompleta]
as
WITH cte
AS
(
	SELECT	u.unidadeId, u.undNivel, u.undSigla as undSiglaCompleta, u.undSigla, u.Email
	FROM	Unidade as u
	WHERE	u.unidadeIdPai is null and u.situacaoUnidadeId = 1
	UNION ALL
	SELECT	u.unidadeId, u.undNivel, cast(cte.undSiglaCompleta+'/'+u.undSigla as varchar(50)), u.undSigla, u.Email
	FROM	Unidade as u
	INNER JOIN cte ON u.unidadeIdPai = cte.unidadeId
)
SELECT und.unidadeId, und.undSigla, und.undDescricao, und.unidadeIdPai, und.tipoUnidadeId,
und.situacaoUnidadeId, und.ufId, und.undNivel, und.tipoFuncaoUnidadeId, cte.undSiglaCompleta, und.Email
FROM cte 
INNER JOIN unidade und on cte.unidadeId = und.unidadeId