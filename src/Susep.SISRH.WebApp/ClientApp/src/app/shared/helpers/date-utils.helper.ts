export class DateUtils {

    static formatar(data: Date, separador?: string): string {
        if (!separador || separador.length === 0) {
            separador = '/';
        }
        const dia = data.getDate();
        const mes = data.getMonth() + 1;
        const ano = data.getFullYear();
        return `${ dia < 10? '0'+dia: dia }${separador}${ mes < 10? '0'+mes: mes }${separador}${ ano }`;
    }

    static parse(dateValue: string): Date {
        let date = Date.parse(dateValue);
        if (!date || isNaN(date)) {
            date = this.getMilissegundosNoFormatoBrasileiro(dateValue);
            if (date === -1) return null;
        }
        return new Date(date);
    }

    static getMilissegundosNoFormatoBrasileiro(dateValue: string): number {
        if (!dateValue || dateValue.length === 0) return -1;
        const pattern = /\d{1,2}\/\d{1,2}\/\d{4}/.exec(dateValue);
        if (!pattern || pattern.length !== 1) return -1;
        let parts = dateValue.split(/\D+/g);
        if (!parts || parts.length !== 3) return -1;
        const dia = parseInt(parts[0]);
        const mes = parseInt(parts[1]);
        const ano = parseInt(parts[2]);
        if (dia < 1 || dia > 31) return -1;
        if (mes < 1 || mes > 12) return -1;
        if (ano.toString().length != 4) return -1;
        return new Date(ano, mes - 1, dia).getTime();
    }

}