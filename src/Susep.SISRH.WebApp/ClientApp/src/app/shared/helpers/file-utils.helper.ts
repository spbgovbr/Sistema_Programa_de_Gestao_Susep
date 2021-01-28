import { Observable, Subscriber } from "rxjs";
import { map } from "rxjs/operators";

export class FileUtils {

	static arrayBufferToBase64(buffer: ArrayBuffer): string {
		let binary = '';
		let bytes = new Uint8Array(buffer);
		let len = bytes.byteLength;
		for (var i = 0; i < len; i++) {
			binary += String.fromCharCode(bytes[i]);
		}
		return window.btoa(binary);
	}  

	static fileToArrayBuffer(arquivo: File): Observable<ArrayBuffer> {
		return new Observable((observer: Subscriber<ArrayBuffer>): void => {
			let reader: FileReader = new FileReader();
			reader.onloadend = () => {
				let buffer = <ArrayBuffer>reader.result;
				observer.next(buffer);
				observer.complete();
			}
			reader.onerror = (error) => {
				observer.error(error);
			};
			reader.readAsArrayBuffer(arquivo);
		});
	
	}

	static fileToBase64(file: File): Observable<string> {
		return this.fileToArrayBuffer(file)
			.pipe(map(buffer => this.arrayBufferToBase64(buffer)));
	} 

}