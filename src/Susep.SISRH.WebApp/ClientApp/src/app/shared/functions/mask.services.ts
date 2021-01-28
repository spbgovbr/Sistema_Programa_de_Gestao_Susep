import { Injectable } from "@angular/core";

@Injectable()
export class MaskService {

    public removeMask(word: string) {
        return word.replace(/\./g, '').replace(/\//g, '').replace(/\-/g, '');
    }   

}