export interface IMenuItem {
  text: string,
  url?: string,
  isOpen?: boolean,
  subItems?: IMenuItem[]
}
