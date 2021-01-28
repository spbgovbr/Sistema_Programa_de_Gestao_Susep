import createNumberMask from 'text-mask-addons/dist/createNumberMask';

export class DecimalValuesHelper  {
  public toPtBr(value: number): string {
    if (value)
      return value.toString().replace(',', '').replace('.', ',');
    return null;
  }
  public fromPtBr(value: number): number {
    if (value)
      return Number(value.toString().replace('.', '').replace(',', '.'));
    return null;
  }
  public numberMask(intSize: number, decimalSize: number) {
    return createNumberMask({
      prefix: '',
      integerLimit: intSize,
      allowDecimal: decimalSize > 0,
      decimalLimit: decimalSize,
      decimalSymbol: ','
    });
  }
}
