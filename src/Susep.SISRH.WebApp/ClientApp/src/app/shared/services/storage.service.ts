import { Injectable } from '@angular/core';

@Injectable()
export class StorageService {
  private storage: Storage;

  constructor() {
    this.storage = sessionStorage;
  }

  public retrieve(key: string): any {
    const item = this.storage.getItem(key);

    if (item && item !== 'undefined') {
      return JSON.parse(this.storage.getItem(key));
    }

    return;
  }

  public store(key: string, value: any) {
    if (value) {
      this.storage.setItem(key, JSON.stringify(value));      
    }
    else {
      this.storage.removeItem(key);
    }
  }

}
