export class EnumHelper {

    static toList(enumType: any): IItemEnum[] {
        return Object.keys(enumType)
            .map(k => { return { id: Number.parseInt(enumType[k]), descricao: k }});
    } 

}

export interface IItemEnum {
    id: number;
    descricao: string;
}
  