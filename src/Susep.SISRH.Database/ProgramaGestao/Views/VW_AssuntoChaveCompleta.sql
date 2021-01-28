
-- Criar view VW_AssuntoChaveCompleta
CREATE VIEW ProgramaGestao.VW_AssuntoChaveCompleta
AS

	WITH cte_assunto AS (

		SELECT 
			assuntoId, 
			chave, 
			valor, 
			assuntoPaiId, 
			ativo, 
			CAST(chave AS varchar(200)) AS hierarquia, 
			1 as nivel
		FROM ProgramaGestao.Assunto
		WHERE assuntoPaiId IS  NULL

		UNION ALL

		SELECT 
			filho.assuntoId, 
			filho.chave, 
			filho.valor, 
			filho.assuntoPaiId, 
			filho.ativo, 
			CAST(CONCAT(pai.hierarquia, '/', filho.chave) AS VARCHAR(200)) AS hierarquia, 
			pai.nivel + 1 AS nivel
		FROM ProgramaGestao.Assunto filho
		JOIN cte_assunto pai ON filho.assuntoPaiId = pai.assuntoId

	)
	SELECT *
	FROM cte_assunto;
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Exibe o nível hierarquico do assunto.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'VIEW', @level1name = N'VW_AssuntoChaveCompleta', @level2type = N'COLUMN', @level2name = N'nivel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Exibe as chaves de forma hierarquica.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'VIEW', @level1name = N'VW_AssuntoChaveCompleta', @level2type = N'COLUMN', @level2name = N'hierarquia';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Indica se o assunto encontra-se ativo', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'VIEW', @level1name = N'VW_AssuntoChaveCompleta', @level2type = N'COLUMN', @level2name = N'ativo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Assunto pai', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'VIEW', @level1name = N'VW_AssuntoChaveCompleta', @level2type = N'COLUMN', @level2name = N'assuntoPaiId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Campo valor', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'VIEW', @level1name = N'VW_AssuntoChaveCompleta', @level2type = N'COLUMN', @level2name = N'valor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Campo chave', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'VIEW', @level1name = N'VW_AssuntoChaveCompleta', @level2type = N'COLUMN', @level2name = N'chave';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'View para exibir dados hierarquicos da tabela Assunto.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'VIEW', @level1name = N'VW_AssuntoChaveCompleta';

