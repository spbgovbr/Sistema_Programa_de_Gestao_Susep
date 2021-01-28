namespace Susep.SISRH.Application.Queries.RawSql
{
    public static class APIExtracaoOrgaoCentralRawSqls
    {

        public static string ObterPlanosTrabalho
        {
            get
            {
                return @"
					
                    SELECT
                        pacto.pactoTrabalhoId PactoTrabalhoId 
                        ,pes.pesMatriculaSiape MatriculaSIAPE 
                        ,pes.pesCPF CPF 
                        ,pes.pesNome pessoa 
                        ,und.undCodigoSIORG CodigoUnidadeSIORGExercicio 
                        ,und.undSiglaCompleta NomeUnidadeSIORGExercicio 
                        ,CASE WHEN pacto.formaExecucaoId = 101 THEN 1 ELSE 2 END FormaExecucaoId 
                        ,(pes.CargaHoraria * 5) CargaHorariaSemanal 
                        ,pacto.dataInicio DataInicio 
                        ,pacto.dataFim DataFim 
                        ,pacto.tempoTotalDisponivel CargaHorariaTotal
                    FROM [ProgramaGestao].[PactoTrabalho] pacto
	                    INNER JOIN [dbo].[Pessoa] pes ON pes.pessoaId = pacto.pessoaId
	                    INNER JOIN [dbo].[VW_UnidadeSiglaCompleta] und on und.unidadeId = pes.unidadeId


                    SELECT
                        pitem.pactoTrabalhoId
                        ,item.itemCatalogoId ItemCatalogoId 
                        ,item.titulo Titulo 
                        ,item.complexidade Complexidade 
                        ,item.definicaoComplexidade DefinicaoComplexidade 
                        ,item.tempoPresencial TempoExecucaoPresencial 
                        ,item.tempoRemoto TempoExecucaoRemoto 
                        ,item.entregasEsperadas EntregasEsperadas 
                        ,pitem.quantidade QuantidadeEntregas 
                        ,pitem.quantidade QuantidadeEntregasRealizadas 
                        ,pitem.nota Nota 
                        ,pitem.justificativa Justificativa 
                    FROM [ProgramaGestao].[PactoTrabalhoAtividade] pitem
	                    INNER JOIN [ProgramaGestao].[ItemCatalogo] item ON pitem.itemCatalogoId = item.itemCatalogoId


                ";
            }
        }

    }
}
